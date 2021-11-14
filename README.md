# SQL data queries using Language Ext Aff monad

There are couple of goals of this library: 

- treat SQL query as a data, so that it could be passed and returned as a value, asserted (for example in the unit tests etc.)
- make composition of the queries easy and reuse the same query abstraction for the composite queries
- have automatic error handling and propagation
- hide, but still make available on-demand, SQL execution runtime context, like opened connection|transaction and cancellation token, so that the code is clean

# Details

Each SQL query is inherited from a record `SqlQuery<TResult>`. So it is an immutable data structure, holding an input to the SQL query, and by calling `AsAff` method on it - we could have it as an actual SQL query side effect, encoded in Aff monad. Data and code is mixed here, but it's so much more convenient this way, than having query object to be separate from the query code. 

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
