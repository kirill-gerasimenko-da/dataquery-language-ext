# Composable SQL queries
![Nuget](https://img.shields.io/nuget/v/DataQuery.LanguageExt)

This library is a wrapper around [Dapper](https://dapperlib.github.io/Dapper/) to access relation databases ([Npgsql](https://www.npgsql.org/) driver for `PostgreSQL` and `ADO.NET` driver for other databases). Most operations supported are calling `Dapper` directly and have the same set of parameters. The project is heavily based on [Language.Ext](https://github.com/louthy/language-ext) - an amazing functional C# extensions library.

## Goals

- SQL query as data
- composition (both ad-hoc and composite queries)
- implicit SQL execution runtime context: connection, transaction and cancellation token are passed to each query implicitly, but it is possible to get these from the context if required 
- error handling

## Usage

### Ad-hoc queries

Create database client first:

```csharp
using static DataQuerySql;

// create database client with Npgsql driver by default
var postgresDatabase = CreateSqlDatabaseClient("connection-string");

// create database client with ADO.NET driver
var sqlDatabase = CreateSqlDatabaseClient("connection-string", DriverType.AdoNet);
```

Below examples show how ad-hoc queries might be constructed and executed without a lot of ceremonies: 

```csharp
using static DataQuerySql;

// get count of users
var usersCount = await database.RunOrFail(Query<int>(@"select count(*) from users"), cancelToken);

// create ad-hoc query object for inserting new user
var insertQuery = ExecuteScalar<int>(@"
    INSERT INTO users (name, email, zip_code)
    VALUES (@name, @email, @zip_code)
    RETURNING id", new
    {
        name = "John Smith",
        email = "jsmith@email.com",
        zip_code = "73000"
    });

var userId = await database.RunOrFail(insertQuery, cancelToken);
```

The next examples show how to compose more than one queries together into a compound query, which still has the same qualities as the ones it comprises of:

```csharp
using static DataQuerySql;

var insertUser = ExecuteScalar<int>("...", new {...});
var insertUserAuditLog = ExecuteScalar<int>(@"...", new {...});
    
// create ad-hoc compound query using LINQ syntax, which has the usual semantics:
// for example if insertUser query fails - other queries in the chain won't even
// start and the error will be propagated (and transaction rolled back)
var insertUserWithAudit =
    from userId in insertUser
    from logId in insertUserAuditLog
    from appointmentId in QuerySingle<long>(@"SELECT id FROM appointment...", new { user_id = userId });
    select (userId, logId, appointmentId);
    
// run the compound query: the connection and transaction will be shared for all 
// queries, if anything fails - the transaction is automatically reverted
var (userId, logId, _) = await database.RunOrFail(insertUserWithAudit, cancelToken);    

// or run inline with addition of new queries, passing values from the
// previous query execution results
var _  = await database.RunOrFail(
    from insertResult in insertUserWithAudit
    from _ in ExecuteScalar(@"INSERT ...", new { id = insertResult.appointmentId })
    select new { id = insertResult.userId, appId = insertResult.appointmentId },
    cancelToken); 
```

From examples above it is clear that ad-hoc queries could be combined in a different ways. This allows to split complex queries and/or add additional queries on top of existing ones. 

In addition we could create query that can be stopped conditionally, like this: 

```csharp
// queries from previous examples 
from userId in insertUser
from logId in insertUserAuditLog
from _ in guard(logId % 2 == 0, Error.New("Can't allow this!"))
from appointmentId in QuerySingle<long>(@"SELECT id FROM appointment...", new {...});
select (userId, logId, appointmentId);
```

### Record-based queries

In addition to ad-hoc queries it is possible to use C# records to define a query:

``` csharp
using static DataQuerySql;

// query without parameters
public record GetCompaniesCount : SqlQuery<int>
{
    public override Aff<DefaultRT, int> AsAff() => 
        ExecuteScalar<int>(@"SELECT COUNT(*) from company");
}

// query is a record inherited SqlQuery and implementing AsAff method
public record FindCompany(int CompanyId) : SqlQuery<Option<Company>>
{
    public override Aff<DefaultRT, Option<Company>> AsAff() =>
        TryQueryFirst<Company>("SELECT * FROM company WHERE id = @id", new { id = CompanyId });
}

// query is an immutable data record and can be used as a regular 
// object - passed and returned from funcitons, serialized etc.
public record InsertCompany(string Name, DateTime CreatedWhen) : SqlQuery<int>
{
    public override Aff<DefaultRT, int> AsAff() => ExecuteScalar<int>(@"
        INSERT INTO company (...) VALUES (...) RETURNING id", new
        {
            name = Name,
            created_when = CreatedWhen
        });
}
```

Composition with ad-hoc queries is possible, along regular compostion on a language level:

``` csharp
using static DataQuerySql;

public record FindCompany(int CompanyId) : SqlQuery<Option<Company>> {...}
public record InsertCompany(string Name, DateTime CreatedWhen) : SqlQuery<int> {...}

// compose queries as ad-hoc, because AsAff method converts query to ad-hoc one
public record AddNewCompany(string Name, DateTime CreatedWhen) : SqlQuery<Company>
{
    public override Aff<DefaultRT, Company> AsAff() =>
        from companyId in new InsertCompany(Name, CreatedWhen).AsAff()
        from company in new FindCompany(companyId).AsAff()
        select (Company) company;
}

// or the same query as above, but using fluent syntax
public record AddNewCompany(string Name, DateTime CreatedWhen) : SqlQuery<Company>
{
    public override Aff<DefaultRT, Company> AsAff()
    {
        var insertCompany = new InsertCompany(Name, CreatedWhen).AsAff();
        var findCompany = (int companyId) => new FindCompany(companyId).AsAff();
        
        return insertCompany
            .Bind(companyId => findCompany(companyId))
            .Map(company => (Company)company);
    }
}

// running the record-based queries is exactly the same
await database.RunOrFail(new AddNewCompany("ABC", DateTime.Now), cancelToken);

// and there is support for composition with ad-hoc queries 
var newCompany = await database.RunOrFail(
    from company in new AddNewCompany("ABC", DateTime.Now).AsAff()
    from logId in ExecuteScalar<int>("INSERT ...")
    select company, 
    cancelToken);
```

Partial application is possible as well, allowing to bake in query parameters with simple inheritance. 
This could be useful to create a query (simple or compound) with an open set of parameters, where the combination of these parameters is forming specifics. For example: 

``` csharp
// base query object, opened to different combinations of parameters
public abstract record AddCompanyWithEmployeesBase
(
    string Name,
    DateTime CreatedWhen,
    CompanyStatus Status,
    Seq<Employee> Employees
) : SqlQuery<Company>
{
    public override Aff<DefaultRT, Company> AsAff()
    {
        // insert new company
        // insert employees
        // do something else what's necessary - combine queries etc.
    }
}

// add default company, which means default name, status Default and empty list of employees
public record AddDefaultCompany(DateTime CreatedWhen) :
    AddCompanyWithEmployeesBase("DEFAULT_COMPANY_NAME", CreatedWhen, CompanyStatus.Default, Empty);
    
// add managed company with proper status and single employee
public record AddManagedCompany(string Name, DateTime CreatedWhen, Employee Manager) :
    AddCompanyWithEmployeesBase(Name, CreatedWhen, CompanyStatus.Managed, Seq1(Manager));
```

### API

TBD
