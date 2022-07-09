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
        /// Runs the supplied query in a new transaction with IsolationLevel.ReadCommitted.
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<T> query, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query in a new transaction with IsolationLevel.ReadCommitted.
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query in a new transaction with IsolationLevel.ReadCommitted.
        /// </summary>
        Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query in a new transaction with specified transaction isolation level.
        /// If no isolation level is provided - the the transaction is not created.
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<T> query, Option<IsolationLevel> isolationLevel, CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query in a new transaction with specified transaction isolation level.
        /// If no isolation level is provided - the the transaction is not created.
        /// </summary>
        Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query, Option<IsolationLevel> isolationLevel,
            CancellationToken cancelToken);

        /// <summary>
        /// Runs the supplied query in a new transaction with specified transaction isolation level.
        /// If no isolation level is provided - the the transaction is not created.
        /// </summary>
        Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, Option<IsolationLevel> isolationLevel, CancellationToken cancelToken);
    }

    public abstract class SqlDatabaseBase<RT> : ISqlDatabase<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        protected readonly ISqlQueryRunner<RT> _runner;

        protected SqlDatabaseBase(ISqlQueryRunner<RT> runner) => _runner = runner;

        public async Task<Fin<T>> Run<T>(ISqlQuery<T> query, CancellationToken cancelToken) =>
            await _runner.AsAff(query.AsAff<RT>(), IsolationLevel.ReadCommitted, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query, CancellationToken cancelToken) =>
            await _runner.AsAff(query.AsAff(), IsolationLevel.ReadCommitted, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken) =>
            await _runner.AsAff(queryAff, IsolationLevel.ReadCommitted, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(ISqlQuery<RT, T> query,
            Option<IsolationLevel> isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(query.AsAff(), isolationLevel, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(ISqlQuery<T> query,
            Option<IsolationLevel> isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(query.AsAff<RT>(), isolationLevel, cancelToken).Run();

        public async Task<Fin<T>> Run<T>(Aff<RT, T> queryAff,
            Option<IsolationLevel> isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(queryAff, isolationLevel, cancelToken).Run();
    }

    /// <summary>
    /// Runs the supplied query in a new transaction with IsolationLevel.ReadCommitted.
    /// </summary>
    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        ISqlQuery<DefaultRT, T> query,
        CancellationToken cancelToken)
        =>
            (await database.Run(query, cancelToken)).ThrowIfFail();

    /// <summary>
    /// Runs the supplied query in a new transaction with IsolationLevel.ReadCommitted.
    /// </summary>
    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        Aff<DefaultRT, T> queryAff,
        CancellationToken cancelToken)
        =>
            (await database.Run(queryAff, cancelToken)).ThrowIfFail();

    /// <summary>
    /// Runs the supplied query in a new transaction if isolation level is specified,
    /// otherwise - without it. 
    /// </summary>
    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        ISqlQuery<DefaultRT, T> query,
        Option<IsolationLevel> isolationLevel,
        CancellationToken cancelToken)
        =>
            (await database.Run(query, isolationLevel, cancelToken)).ThrowIfFail();

    /// <summary>
    /// Runs the supplied query in a new transaction if isolation level is specified,
    /// otherwise - without it.
    /// </summary>
    public static async Task<T> RunOrFail<T>(
        this ISqlDatabase database,
        Aff<DefaultRT, T> queryAff,
        Option<IsolationLevel> isolationLevel,
        CancellationToken cancelToken)
        =>
            (await database.Run(queryAff, isolationLevel, cancelToken)).ThrowIfFail();
}