# SQL data queries using Language Ext Aff monad

This is an experiment to find out if building an abstraction over the database query itself as opposed to the repository approach, with addition of some functional goodies from brilliant `Language.Ext` library, could be really viable.

## The library emphasizes the following

- SQL query as data 

- composition (both ad-hoc and composite queries)

- implicit SQL execution runtime context (like currently opened connection|transaction, cancellation token etc.), but with ability to get the context where it's needed

- error handling

### SQL query as data

Each SQL query is inherited from a C# record `SqlQuery<TResult>`. It is an immutable data structure, holding an input to the SQL query and has encoded type of the returned value. It can be persisted, sent over the wire, accepted as argument and returned from the function.

Typical query looks like the following

```csharp

public record GetCustomerByIdQuery(CustomerId Id) : SqlQuery<Customer>
{
    public override Aff<RT, Customer> AsAff<RT>() => SqlDatabase<RT>
        .query<Customer>(@"
            SELECT id, fist_name, last_name, status
            FROM customer
            WHERE deleted_when IS NULL AND id = @id
            ", 
            new 
            {
               id = Id.Value
            });
}

```

One could create an instance of the query, just like any other object

```csharp
var customerId = CustomerId.New(Guid.Parse("8bc750f0-5c0b-4a94-97f4-1a6ff2b6857f"));
var getCustomer = new GetCustomerByIdQuery(customerId);
```
