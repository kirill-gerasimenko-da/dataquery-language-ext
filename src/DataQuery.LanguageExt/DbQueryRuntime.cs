namespace DataQuery.LanguageExt;

using System.Data.Common;
using System.Threading;

public readonly struct DbQueryRuntime : HasDbConnection<DbQueryRuntime>
{
    private DbQueryRuntime(DbQueryRuntimeEnv env) => Env = env;

    public static DbQueryRuntime New(DbConnection cnn, Option<DbTransaction> tran, CancellationToken cancelToken) =>
        new(new DbQueryRuntimeEnv(new CancellationTokenSource(), cancelToken, cnn, tran));

    private DbQueryRuntimeEnv Env { get; }

    public DbQueryRuntime LocalCancel => new(Env.LocalCancel);
    public CancellationToken CancellationToken => Env.Token;
    public CancellationTokenSource CancellationTokenSource => Env.Source;
    public DbConnection Connection => Env.Connection;
    public Option<DbTransaction> Transaction => Env.Transaction;
}