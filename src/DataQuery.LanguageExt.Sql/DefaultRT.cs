namespace DataQuery.LanguageExt.Sql;

using System.Data;
using System.Threading;

public static partial class DataQuerySql
{
    /// <summary>
    /// Default database runtime, cancellable, allows access to the
    /// current connection, transaction and Dapper (via database IO) 
    /// </summary>
    public readonly struct DefaultRT : HasSqlDatabase<DefaultRT>
    {
        private DefaultRT(DefaultRtEnv env) => Env = env;

        public static DefaultRT New(
            IDbConnection cnn,
            Option<IDbTransaction> trn,
            CancellationToken cancelToken)
            =>
                new(new DefaultRtEnv(new CancellationTokenSource(), cancelToken, cnn, trn));

        public DefaultRtEnv Env { get; }
        public DefaultRT LocalCancel => new(Env.LocalCancel);
        public CancellationToken CancellationToken => Env.Token;
        public CancellationTokenSource CancellationTokenSource => Env.Source;
        public IDbConnection Connection => Env.Connection;
        public Option<IDbTransaction> Transaction => Env.Transaction;

        public Eff<DefaultRT, ISqlDatabaseIO> SqlDatabaseEff => SuccessEff(LiveSqlDatabaseIO.Default);
    }

    public readonly struct DefaultRtEnv
    {
        public readonly CancellationTokenSource Source;
        public readonly CancellationToken Token;
        public readonly IDbConnection Connection;
        public readonly Option<IDbTransaction> Transaction;

        public DefaultRtEnv(
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

        private DefaultRtEnv(
            CancellationTokenSource source,
            IDbConnection connection,
            Option<IDbTransaction> transaction)
            : this(source, source.Token, connection, transaction)
        { }

        public DefaultRtEnv LocalCancel => new(new CancellationTokenSource(), Connection, Transaction);
    }
}