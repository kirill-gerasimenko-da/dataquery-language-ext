namespace DataQuery.LanguageExt.Sql;

using System.Data;
using System.Threading;
using System.Threading.Tasks;

public static partial class DataQuerySql
{
    /// <summary>
    /// Application level abstraction for running SQL queries.
    /// </summary>
    public interface ISqlDatabase<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        /// <summary>
        /// Runs the supplied query
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<T> query, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query, with specified transaction isolation
        /// level
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query, with specified transaction isolation
        /// level
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query aff, handles errors
        /// </summary>
        Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query aff, with specified transaction isolation level
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

        public async Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query, CancellationToken cancelToken) =>
            await _runner.AsAff(query.AsAff(), IsolationLevel.Unspecified, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(
            ISqlQuery<RT, T> query,
            IsolationLevel isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(query.AsAff(), isolationLevel, cancelToken).Run();

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

    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        ISqlQuery<DefaultRT, T> query,
        CancellationToken cancelToken)
        =>
            (await database.Run(query, cancelToken)).ThrowIfFail();

    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        ISqlQuery<DefaultRT, T> query,
        IsolationLevel isolationLevel,
        CancellationToken cancelToken)
        =>
            (await database.Run(query, isolationLevel, cancelToken)).ThrowIfFail();

    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        Aff<DefaultRT, T> queryAff,
        CancellationToken cancelToken)
        =>
            (await database.Run(queryAff, cancelToken)).ThrowIfFail();

    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        Aff<DefaultRT, T> queryAff,
        IsolationLevel isolationLevel,
        CancellationToken cancelToken)
        =>
            (await database.Run(queryAff, isolationLevel, cancelToken)).ThrowIfFail();
}