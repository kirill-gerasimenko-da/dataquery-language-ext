namespace ConsoleApp1;

using System.Data.Common;
using DataQuery.LanguageExt.SystemData;
using Norm;
using TheUtils;

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
            .WithTransaction(tran.IfNoneDefault())
            .WithCancellationToken(token)
            .ReadAsync<int>("Select @input", new {input = value})
            .SingleAsync(cancellationToken: token);
}