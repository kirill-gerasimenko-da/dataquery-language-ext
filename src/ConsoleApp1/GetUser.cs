namespace ConsoleApp1;

using System.Data.Common;

[DbQuery]
public class GetUser
{
    // public Aff<QueryRuntime, Unit> Invoke() => connection().MapAsync(Invoke2);

    public async Task<Unit> Invoke(DbConnection conn)
    {
        // await connection.ReadAsync("").SingleAsync();
        return unit;
    }
}