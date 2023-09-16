namespace ConsoleApp1;

using System.Data.Common;
using Norm;

[DbQuery]
public class GetUser
{
    public async Task<int> Invoke
    (
        string y,
        int value,
        DbConnection conn,
        Option<DbTransaction> tran,
        CancellationToken token
    ) =>
        await conn
            .WithTransaction(tran)
            .WithCancellationToken(token)
            .ReadAsync<int>("Select 42")
            .SingleAsync(cancellationToken: token);
}