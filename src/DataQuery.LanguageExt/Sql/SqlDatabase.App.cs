using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DataQuery.LanguageExt.Sql;

public static partial class DataQuerySql
{
    /// <summary>
    /// Application level abstraction for running SQL queries.
    /// 
    /// Main purpose is to make consumer more clean, without
    /// a need to know about SqlQueryRunner and SqlDatabaseRuntime etc.
    /// </summary>
    public interface ISqlDatabase<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        /// <summary>
        /// Runs the supplied query, handles errors and returns
        /// Fin<T>, which encodes successful and failed cases
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<T> query, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query, with specified transaction isolation
        /// level, handles errors and returns Fin<T>, which encodes successful
        /// and failed cases
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query aff, handles errors and returns
        /// Fin<T>, which encodes successful and failed cases
        /// </summary>
        Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query aff, with specified transaction isolation
        /// level, handles errors and returns Fin<T>, which encodes successful
        /// and failed cases
        /// </summary>
        Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, IsolationLevel isolationLevel, CancellationToken cancelToken);
    }

    public abstract class SqlDatabaseBase<RT> : ISqlDatabase<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        private readonly ISqlQueryRunner<RT> _runner;

        protected SqlDatabaseBase(ISqlQueryRunner<RT> runner) => _runner = runner;

        public async Task<Fin<T>> Run<T>(ISqlQuery<T> query, CancellationToken cancelToken) =>
            await _runner.AsAff(query.AsAff<RT>(), IsolationLevel.Unspecified, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(
            ISqlQuery<T> query,
            IsolationLevel isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(query.AsAff<RT>(), isolationLevel, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken) =>
            await _runner.AsAff(queryAff, IsolationLevel.Unspecified, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(
            Aff<RT, T> queryAff,
            IsolationLevel isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(queryAff, isolationLevel, cancelToken).Run();
    }

    public interface ISqlDatabase : ISqlDatabase<SqlDatabaseRuntime>
    { }

    public class SqlDatabase : SqlDatabaseBase<SqlDatabaseRuntime>, ISqlDatabase
    {
        public SqlDatabase(ISqlQueryRunner<SqlDatabaseRuntime> runner) : base(runner)
        { }
    }
}