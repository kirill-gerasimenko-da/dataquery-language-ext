using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

namespace DataQuery.LanguageExt.Marten
{
    public static partial class DataQueryMarten
    {
        public interface IDocDatabase<RT>
            where RT : struct, HasMartenDatabase<RT>
        {
            Task<Fin<T>> Run<T>(IDocQuery<T> query, CancellationToken cancelToken);
            Task<Fin<T>> Run<T>(IDocQuery<T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);
            Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken);
            Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, IsolationLevel isolationLevel, CancellationToken cancelToken);
        }

        public abstract class DocDatabaseBase<RT> : IDocDatabase<RT>
            where RT : struct, HasMartenDatabase<RT>
        {
            private readonly IDocQueryRunner<RT> _runner;

            protected DocDatabaseBase(IDocQueryRunner<RT> runner) => _runner = runner;

            public async Task<Fin<T>> Run<T>(IDocQuery<T> query, CancellationToken cancelToken) =>
                await Run(query, IsolationLevel.ReadCommitted, cancelToken);

            public async Task<Fin<T>> Run<T>(
                IDocQuery<T> query,
                IsolationLevel isolationLevel,
                CancellationToken cancelToken)
                =>
                    await _runner.AsAff(query.AsAff<RT>(), isolationLevel, cancelToken).Run();

            public async Task<Fin<T>> Run<T>(Aff<RT, T> queryAff, CancellationToken cancelToken) =>
                await Run(queryAff, IsolationLevel.ReadCommitted, cancelToken);

            public async Task<Fin<T>> Run<T>(
                Aff<RT, T> queryAff,
                IsolationLevel isolationLevel,
                CancellationToken cancelToken)
                =>
                    await _runner.AsAff(queryAff, isolationLevel, cancelToken).Run();
        }


        public interface IDocDatabase : IDocDatabase<MartenDatabaseRuntime>
        { }

        public class DocDatabase : DocDatabaseBase<MartenDatabaseRuntime>, IDocDatabase
        {
            public DocDatabase(IDocQueryRunner<MartenDatabaseRuntime> runner) : base(runner)
            { }
        }
    }
}