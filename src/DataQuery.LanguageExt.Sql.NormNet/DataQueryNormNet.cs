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

        public static Aff<QueryRuntime, Option<DbTransaction>> transaction() =>
            Eff<QueryRuntime, Option<DbTransaction>>(rt => rt.Transaction);
    }

    public interface HasConnection<out RT> : HasCancel<RT>
        where RT : struct, HasConnection<RT>
    {
        DbConnection Connection { get; }
        Option<DbTransaction> Transaction { get; }
    }

    public readonly struct QueryRuntime : HasConnection<QueryRuntime>
    {
        private QueryRuntime(QueryRuntimeEnv env) => Env = env;

        public static QueryRuntime New(DbConnection cnn, Option<DbTransaction> tran, CancellationToken cancelToken) =>
            new(new QueryRuntimeEnv(new CancellationTokenSource(), cancelToken, cnn, tran));

        private QueryRuntimeEnv Env { get; }

        public QueryRuntime LocalCancel => new(Env.LocalCancel);
        public CancellationToken CancellationToken => Env.Token;
        public CancellationTokenSource CancellationTokenSource => Env.Source;
        public DbConnection Connection => Env.Connection;
        public Option<DbTransaction> Transaction => Env.Transaction;
    }

    public readonly struct QueryRuntimeEnv
    {
        public readonly CancellationTokenSource Source;
        public readonly CancellationToken Token;
        public readonly DbConnection Connection;
        public readonly Option<DbTransaction> Transaction;

        public QueryRuntimeEnv(
            CancellationTokenSource source,
            CancellationToken token,
            DbConnection connection,
            Option<DbTransaction> transaction)
        {
            Source = source;
            Token = token;
            Connection = connection;
            Transaction = transaction;
        }

        private QueryRuntimeEnv(
            CancellationTokenSource source,
            DbConnection connection,
            Option<DbTransaction> transaction)
            : this(source, source.Token, connection, transaction)
        { }

        public QueryRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Connection, None);
    }
}