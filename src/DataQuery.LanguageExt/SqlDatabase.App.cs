using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    public static partial class DataQuery
    {
        /// <summary>
        /// Application level abstraction for running SQL queries.
        /// 
        /// Main purpose is to make consumer more clean, without
        /// a need to know about SqlQueryRunner and SqlDatabaseRuntime etc.
        /// </summary>
        public interface ISqlDatabase
        {
            /// <summary>
            /// Runs the supplied query, handles errors and returns
            /// Fin<T>, which encodes successful and failed cases
            /// </summary>
            Task<Fin<T>> RunQuery<T>(ISqlQuery<T> query, CancellationToken cancelToken);

            /// <summary>
            /// Runs the supplied query, with specified transaction isolation
            /// level, handles errors and returns Fin<T>, which encodes successful
            /// and failed cases
            /// </summary>
            Task<Fin<T>> RunQuery<T>(ISqlQuery<T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);
        }

        public class SqlDatabase : ISqlDatabase
        {
            private readonly ISqlQueryRunner<SqlDatabaseRuntime> _runner;

            public SqlDatabase(ISqlQueryRunner<SqlDatabaseRuntime> runner) => _runner = runner;

            public async Task<Fin<T>> RunQuery<T>(ISqlQuery<T> query, CancellationToken cancelToken) =>
                await _runner.AsAff(query.AsAff<SqlDatabaseRuntime>(), IsolationLevel.Unspecified, cancelToken).Run();

            public async Task<Fin<T>> RunQuery<T>(
                ISqlQuery<T> query,
                IsolationLevel isolationLevel,
                CancellationToken cancelToken)
                =>
                    await _runner.AsAff(query.AsAff<SqlDatabaseRuntime>(), isolationLevel, cancelToken).Run();
        }
    }
}