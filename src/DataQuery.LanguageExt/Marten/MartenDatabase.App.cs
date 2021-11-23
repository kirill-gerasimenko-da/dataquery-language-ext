using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public interface IMartenDatabase<RT>
        where RT : struct, HasMartenDatabase<RT>
    {
        ValueTask<Fin<T>> Run<T>(IMartenQuery<T> query, CancellationToken cancelToken);
        ValueTask<Fin<T>> Run<T>(IMartenQuery<T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);
        ValueTask<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken);
        ValueTask<Fin<T>> Run<T>(Aff<RT, T> queryAff, IsolationLevel isolationLevel, CancellationToken cancelToken);
    }

    public abstract class MartenDatabaseBase<RT> : IMartenDatabase<RT>
        where RT : struct, HasMartenDatabase<RT>
    {
        private readonly IMartenQueryRunner<RT> _runner;

        protected MartenDatabaseBase(IMartenQueryRunner<RT> runner) => _runner = runner;

        public async ValueTask<Fin<T>> Run<T>(IMartenQuery<T> query, CancellationToken cancelToken) =>
            await Run(query, IsolationLevel.ReadCommitted, cancelToken);

        public async ValueTask<Fin<T>> Run<T>(IMartenQuery<T> query,
            IsolationLevel isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(query.AsAff<RT>(), isolationLevel, cancelToken).Run();

        public async ValueTask<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken) =>
            await Run(queryAff, IsolationLevel.ReadCommitted, cancelToken);

        public async ValueTask<Fin<T>> Run<T>(Aff<RT, T> queryAff,
            IsolationLevel isolationLevel,
            CancellationToken cancelToken)
            =>
                await _runner.AsAff(queryAff, isolationLevel, cancelToken).Run();
    }

    public interface IMartenDatabase : IMartenDatabase<MartenDatabaseRuntime>
    { }

    public class MartenDatabaseDatabase : MartenDatabaseBase<MartenDatabaseRuntime>, IMartenDatabase
    {
        public MartenDatabaseDatabase(IMartenQueryRunner<MartenDatabaseRuntime> runner) : base(runner)
        { }
    }
}