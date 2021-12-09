# SQL data queries using Language Ext Aff monad

This is an experiment to find out if building an abstraction over the database query itself as opposed to the repository
approach, with addition of some functional goodies from brilliant `Language.Ext` library, could be really viable.

## The library emphasizes the following

- SQL query as data

- composition (both ad-hoc and composite queries)

- implicit SQL execution runtime context (like currently opened connection|transaction, cancellation token etc.), but
  with ability to get the context where it's needed

- error handling

### Examples

Scalar query (getting single value):

``` c#
public record GetBenefitsCountForType(BenefitType Type) : SqlScalarQuery<uint>
{
    public override Aff<RT, uint> AsAff<RT>() => ExecuteScalar<RT>(@"
        SELECT 
            COUNT(*)
        FROM 
            "benefit" b 
        WHERE
            b.type = @benefitType",
        new
        {
            benefitType = (int)Type
        });
}
```

Another example - find entity by id, returning `Option<T>`:


``` c#
public record FindCustomer(CustomerId Id) : SqlScalarQuery<Option<Customer>>
{
    public override Aff<RT, Option<Customer>> AsAff<RT>() =>
        from customers in Query<RT, Customer>(@"
            SELECT * FROM customer WHERE id = @customerId",
            new
            {
                customerId = Id
            })
        select customers.HeadOrNone();
}
```

The same could be achieved using errors mechanics from LanguageExt:

``` c#
public record FindCustomer(CustomerId Id) : SqlScalarQuery<Option<Customer>>
{
    static Error noCustomersFound = Error.New(1000, "no customers found");

    public override Aff<RT, Option<Customer>> AsAff<RT>() =>
    (
        from customers in Query<RT, Customer>(@"
            SELECT * FROM customer WHERE id = @customerId",
            new
            {
                customerId = Id
            })
        from _ in guard(customers != Empty, noCustomersFound)
        select Some(customers.Head)
    ) | @catch(noCustomersFound, Option<Customer>.None); 
}
```

In the above query, it might look like overkill, but in some circumstances it's the way to short-circuit the query processing (and even rollback the transaction if necessary).

