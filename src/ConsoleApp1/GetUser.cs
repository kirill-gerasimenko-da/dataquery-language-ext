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
    ) => await conn
        .WithTransaction(tran.IfNoneDefault())
        .WithCancellationToken(token)
        .ReadAsync<int>("Select @input + 1", new {input = value})
        .SingleAsync(cancellationToken: token);
}

[DataQuery.LanguageExt.NormNet.DbQuery]
public class GetUserNorm
{
    public async Task<int> Invoke
    (
        int value,
        Norm norm,
        CancellationToken token
    ) => await norm
        .ReadAsync<int>("Select @input + 2", new {input = value})
        .SingleAsync(token);
}