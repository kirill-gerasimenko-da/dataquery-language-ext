namespace DataQuery.LanguageExt.Sql;

using System.Data;
using System.Data.Common;
using System.Threading;

public static partial class DataQuerySql
{
    public interface ISqlQueryRunner<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        /// <summary>
        /// Creates an effect, which runs the query with DatabaseRuntime.
        ///
        /// <p>If isolation level is supplied - new transaction is started with
        /// the supplied level and it is passed to the query in the runtime context.</p>
        ///
        /// <p>If no isolation level is supplied - no transaction is started.</p> 
        /// </summary>
        Aff<T> AsAff<T>(
            Aff<RT, T> query,
            Option<IsolationLevel> isolationLevel,
            CancellationToken cancelToken);
    }

    public delegate RT SqlDatabaseRuntimeFactory<out RT>(
        IDbConnection connection,
        Option<IDbTransaction> transaction,
        CancellationToken cancelToken) where RT : struct, HasSqlDatabase<RT>;

    public delegate DbConnection SqlConnectionFactory();

    public abstract class SqlQueryRunner<RT> : ISqlQueryRunner<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        readonly SqlConnectionFactory _sqlConnectionFactory;
        readonly SqlDatabaseRuntimeFactory<RT> _runtimeFactory;

        protected SqlQueryRunner(
            SqlConnectionFactory sqlConnectionFactory,
            SqlDatabaseRuntimeFactory<RT> runtimeFactory)
        {
            _runtimeFactory = runtimeFactory;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public Aff<T> AsAff<T>(
            Aff<RT, T> query,
            Option<IsolationLevel> isolationLevel,
            CancellationToken cancelToken) => AffMaybe(async () =>
        {
            await using var cnn = _sqlConnectionFactory();
            await cnn.OpenAsync(cancelToken);

            Option<DbTransaction> tran = isolationLevel.Case switch
            {
                IsolationLevel level => await cnn.BeginTransactionAsync(level, cancelToken),
                _ => None
            };

            var runtime = _runtimeFactory(cnn, tran.Map(t => (IDbTransaction) t), cancelToken);

            var finalQuery = query.MapFailAsync(async error =>
            {
                var _ = await tran.MapAsync(async t => await t.RollbackAsync(cancelToken));
                return error;
            });

            var result = await finalQuery.Run(runtime);
            if (!result.IsSucc)
                return result;

            var _ = await tran.MapAsync(async t => await t.CommitAsync(cancelToken));

            return result;
        });
    }
}