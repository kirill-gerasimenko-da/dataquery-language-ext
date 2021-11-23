using System.Data;
using System.Threading;
using Marten;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public interface IMartenQueryRunner<RT>
        where RT : struct, HasMartenDatabase<RT>
    {
        /// <summary>
        /// Creates an effect, which runs the query with DatabaseRuntime.
        /// </summary>
        Aff<T> AsAff<T>(Aff<RT, T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);
    }

    public delegate RT MartenDatabaseRuntimeFactory<out RT>(IDocumentSession session, CancellationToken cancelToken)
        where RT : struct, HasMartenDatabase<RT>;

    public class MartenQueryRunner<RT> : IMartenQueryRunner<RT>
        where RT : struct, HasMartenDatabase<RT>
    {
        private readonly IDocumentStore _store;
        readonly MartenDatabaseRuntimeFactory<RT> _runtimeFactory;

        public MartenQueryRunner(IDocumentStore store, MartenDatabaseRuntimeFactory<RT> runtimeFactory)
        {
            _store = store;
            _runtimeFactory = runtimeFactory;
        }

        public Aff<T> AsAff<T>(Aff<RT, T> query, IsolationLevel isolationLevel, CancellationToken cancelToken) =>
            AffMaybe(async () =>
            {
                await using var session = _store.LightweightSession(isolationLevel);
                return await query.Run(_runtimeFactory(session, cancelToken));
            });
    }
}