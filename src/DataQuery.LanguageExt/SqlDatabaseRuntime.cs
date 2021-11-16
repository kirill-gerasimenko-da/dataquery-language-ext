using System.Data;
using System.Threading;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    using static Prelude;

    public static partial class DataQuerySql
    {
        /// <summary>
        /// Default database runtime, cancellable, allows access to the
        /// current connection, transaction and Dapper (via database IO) 
        /// </summary>
        public readonly struct SqlDatabaseRuntime : HasSqlDatabase<SqlDatabaseRuntime>
        {
            private SqlDatabaseRuntime(SqlDatabaseRuntimeEnv env) => Env = env;

            public static SqlDatabaseRuntime New(
                IDbConnection cnn,
                Option<IDbTransaction> trn,
                CancellationToken cancelToken)
                =>
                    new(new SqlDatabaseRuntimeEnv(new CancellationTokenSource(), cancelToken, cnn, trn));

            public SqlDatabaseRuntimeEnv Env { get; }
            public SqlDatabaseRuntime LocalCancel => new(Env.LocalCancel);
            public CancellationToken CancellationToken => Env.Token;
            public CancellationTokenSource CancellationTokenSource => Env.Source;
            public IDbConnection Connection => Env.Connection;
            public Option<IDbTransaction> Transaction => Env.Transaction;

            public Eff<SqlDatabaseRuntime, ISqlDatabaseIO> SqlDatabaseEff => SuccessEff(LiveSqlDatabaseIO.Default);
        }

        public readonly struct SqlDatabaseRuntimeEnv
        {
            public readonly CancellationTokenSource Source;
            public readonly CancellationToken Token;
            public readonly IDbConnection Connection;
            public readonly Option<IDbTransaction> Transaction;

            public SqlDatabaseRuntimeEnv(
                CancellationTokenSource source,
                CancellationToken token,
                IDbConnection connection,
                Option<IDbTransaction> transaction)
            {
                Source = source;
                Token = token;
                Connection = connection;
                Transaction = transaction;
            }

            private SqlDatabaseRuntimeEnv(
                CancellationTokenSource source,
                IDbConnection connection,
                Option<IDbTransaction> transaction)
                : this(source, source.Token, connection, transaction)
            { }

            public SqlDatabaseRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Connection, Transaction);
        }
    }
}