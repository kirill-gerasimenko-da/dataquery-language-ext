namespace DataQuery.LanguageExt.Sql.NormNet;

using System.Data.Common;
using System.Threading;
using global::LanguageExt.Effects.Traits;

public static class DataQueryNormNet
{
    public abstract class QueryContext
    {
        public static CancellationToken Token => default(QueryRuntime).CancellationToken;
        public static DbConnection Connection => default(QueryRuntime).Connection;
    }

    public interface HasConnection<out RT> : HasCancel<RT>
        where RT : struct, HasConnection<RT>
    {
        public DbConnection Connection { get; }
    }

    public readonly struct QueryRuntime : HasConnection<QueryRuntime>
    {
        private QueryRuntime(QueryRuntimeEnv env) => Env = env;

        public static QueryRuntime New(DbConnection cnn, CancellationToken cancelToken) =>
            new(new QueryRuntimeEnv(new CancellationTokenSource(), cancelToken, cnn));

        private QueryRuntimeEnv Env { get; }

        public QueryRuntime LocalCancel => new(Env.LocalCancel);
        public CancellationToken CancellationToken => Env.Token;
        public CancellationTokenSource CancellationTokenSource => Env.Source;
        public DbConnection Connection => Env.Connection;
    }

    public readonly struct QueryRuntimeEnv
    {
        public readonly CancellationTokenSource Source;
        public readonly CancellationToken Token;
        public readonly DbConnection Connection;

        public QueryRuntimeEnv(
            CancellationTokenSource source,
            CancellationToken token,
            DbConnection connection)
        {
            Source = source;
            Token = token;
            Connection = connection;
        }

        private QueryRuntimeEnv(CancellationTokenSource source, DbConnection connection)
            : this(source, source.Token, connection)
        { }

        public QueryRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Connection);
    }
}