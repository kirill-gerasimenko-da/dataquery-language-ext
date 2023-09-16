namespace ConsoleApp1;

using System.Data.Common;
using Norm;
using static DataQueryNormNet.QueryContext;

[DbQuery]
public class GetUser
{
    // public Aff<QueryRuntime, Unit> Invoke() => connection().MapAsync(Invoke2);

    public async Task<Unit> Invoke(string y, int value, DbConnection conn, Option<DbTransaction> tran, CancellationToken token)
    {
        conn.WithCancellationToken(token);

        // conn.WithTransaction()

        // await connection.ReadAsync("").SingleAsync();
        return unit;
    }
}