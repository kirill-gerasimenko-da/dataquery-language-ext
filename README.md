# SQL queries on steroids

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
var  = await database.RunOrFail(
    from insertResult in insertUserWithAudit
    from _ in ExecuteScalar(@"INSERT ...", new { id = insertResult.appointmentId })
    select (userId, logId),
    cancelToken); 
```

From examples above it is clear that ad-hoc queries could be combined in a different ways. This allows to split complex queries and/or add additional queries on top of existing ones - PROFIT. 

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

TBD
