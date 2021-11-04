using LanguageExt;
using LanguageExt.Pipes;

namespace Dataquery.LanguageExt
{
    using static Prelude;

    public static partial class DataQuery
    {
        /// <summary>
        /// Interface-marker 
        /// </summary>
        public interface IQuery<TResult>
        { }

        public interface IQueryHandler<TQuery, TResult>
        {
            /// <summary>
            /// Returns query async effect
            /// </summary>
            Aff<RT, TResult> asQuery<RT>(TQuery query) where RT : struct, HasDatabase<RT>;

            /// <summary>
            /// Returns query async effect as pipe
            /// </summary>
            Pipe<RT, TQuery, TResult, Unit> asPipe<RT>() where RT : struct, HasDatabase<RT>;

            /// <summary>
            /// Returns query async effect as producer 
            /// </summary>
            Producer<RT, TResult, Unit> asProducer<RT>(TQuery query) where RT : struct, HasDatabase<RT>;

            /// <summary>
            /// Returns query async effect as consumer 
            /// </summary>
            Consumer<RT, TQuery, TResult> asConsumer<RT>() where RT : struct, HasDatabase<RT>;
        }

        /// <summary>
        /// Base class for query handlers, with implemented Pipes methods 
        /// </summary>
        public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            public abstract Aff<RT, TResult> asQuery<RT>(TQuery query)
                where RT : struct, HasDatabase<RT>;

            public Pipe<RT, TQuery, TResult, Unit> asPipe<RT>()
                where RT : struct, HasDatabase<RT>
                =>
                    from input in Proxy.awaiting<TQuery>()
                    from result in asQuery<RT>(input)
                    from _ in Proxy.yield(result)
                    select unit;

            public Producer<RT, TResult, Unit> asProducer<RT>(TQuery query)
                where RT : struct, HasDatabase<RT>
                =>
                    from result in asQuery<RT>(query)
                    from _ in Proxy.yield(result)
                    select unit;

            public Consumer<RT, TQuery, TResult> asConsumer<RT>()
                where RT : struct, HasDatabase<RT>
                =>
                    from input in Proxy.awaiting<TQuery>()
                    from result in asQuery<RT>(input)
                    select result;
        }
    }
}