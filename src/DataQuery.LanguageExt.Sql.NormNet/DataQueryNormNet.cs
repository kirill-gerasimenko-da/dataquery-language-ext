// ReSharper disable MemberCanBePrivate.Global
namespace DataQuery.LanguageExt.Sql.NormNet;

using System.Data.Common;
using System.Threading;
using global::LanguageExt;
using global::LanguageExt.Effects.Traits;
using static global::LanguageExt.Prelude;

public static class DataQueryNormNet
{
    public abstract class QueryContext
    {
        public static Aff<QueryRuntime, CancellationToken> token() =>
            Eff<QueryRuntime, CancellationToken>(rt => rt.CancellationToken);

        public static Aff<QueryRuntime, DbConnection> connection() =>
            Eff<QueryRuntime, DbConnection>(rt => rt.Connection);
    }

    public interface HasConnection<out RT> : HasCancel<RT>
        where RT : struct, HasConnection<RT>
    {
        DbConnection Connection { get; }
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