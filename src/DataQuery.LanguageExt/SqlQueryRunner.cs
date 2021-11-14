using System.Data;
using System.Data.Common;
using System.Threading;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    using static Prelude;

    public static partial class DataQuery
    {
        public interface ISqlQueryRunner<RT>
            where RT : struct, HasSqlDatabase<RT>
        {
            /// <summary>
            /// Creates an effect, which runs the query with DatabaseRuntime.
            /// </summary>
            Aff<TResult> AsAff<TResult>(
                Aff<RT, TResult> query,
                IsolationLevel isolationLevel,
                CancellationToken cancelToken);
        }

        public delegate RT SqlDatabaseRuntimeFactory<out RT>(
            IDbConnection connection,
            Option<IDbTransaction> transaction,
            CancellationToken cancelToken) where RT : struct, HasSqlDatabase<RT>;

        public delegate DbConnection SqlConnectionFactory();

        public class SqlQueryRunner<RT> : ISqlQueryRunner<RT>
            where RT : struct, HasSqlDatabase<RT>
        {
            readonly SqlConnectionFactory _sqlConnectionFactory;
            readonly SqlDatabaseRuntimeFactory<RT> _runtimeFactory;

            public SqlQueryRunner(
                SqlConnectionFactory sqlConnectionFactory,
                SqlDatabaseRuntimeFactory<RT> runtimeFactory)
            {
                _runtimeFactory = runtimeFactory;
                _sqlConnectionFactory = sqlConnectionFactory;
            }

            public Aff<TResult> AsAff<TResult>(
                Aff<RT, TResult> query,
                IsolationLevel isolationLevel,
                CancellationToken cancelToken)
                =>
                    AffMaybe(async () =>
                    {
                        await using var cnn = _sqlConnectionFactory();
                        await cnn.OpenAsync(cancelToken);

                        var trn = await cnn.BeginTransactionAsync(isolationLevel, cancelToken);
                        var runtime = _runtimeFactory(cnn, trn, cancelToken);

                        var finalQuery = query.MapFailAsync(async error =>
                        {
                            await trn.RollbackAsync(cancelToken);
                            return error;
                        });

                        var result = await finalQuery.Run(runtime);
                        if (result.IsSucc)
                            await trn.CommitAsync(cancelToken);

                        return result;
                    });
        }
    }
}