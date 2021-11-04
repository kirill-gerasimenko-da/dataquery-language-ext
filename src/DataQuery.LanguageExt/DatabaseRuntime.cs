using System.Data;
using System.Threading;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    using static Prelude;

    public static partial class DataQuery
    {
        /// <summary>
        /// Default database runtime, cancellable, allows access to the
        /// current connection, transaction and Dapper IO 
        /// </summary>
        public readonly struct DatabaseRuntime : HasDatabase<DatabaseRuntime>
        {
            private DatabaseRuntime(DatabaseRuntimeEnv env) => Env = env;

            public static DatabaseRuntime New(
                IDbConnection cnn,
                Option<IDbTransaction> trn,
                CancellationToken cancelToken)
                =>
                    new(new DatabaseRuntimeEnv(new CancellationTokenSource(), cancelToken, cnn, trn));

            public DatabaseRuntimeEnv Env { get; }
            public DatabaseRuntime LocalCancel => new(Env.LocalCancel);
            public CancellationToken CancellationToken => Env.Token;
            public CancellationTokenSource CancellationTokenSource => Env.Source;
            public IDbConnection Connection => Env.Connection;
            public Option<IDbTransaction> Transaction => Env.Transaction;

            public Eff<DatabaseRuntime, IDatabaseIO> DatabaseEff => SuccessEff(LiveDatabaseIO.Default);
        }

        public readonly struct DatabaseRuntimeEnv
        {
            public readonly CancellationTokenSource Source;
            public readonly CancellationToken Token;
            public readonly IDbConnection Connection;
            public readonly Option<IDbTransaction> Transaction;

            public DatabaseRuntimeEnv(
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

            private DatabaseRuntimeEnv(
                CancellationTokenSource source,
                IDbConnection connection,
                Option<IDbTransaction> transaction)
                : this(source, source.Token, connection, transaction)
            { }

            public DatabaseRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Connection, Transaction);
        }
    }
}