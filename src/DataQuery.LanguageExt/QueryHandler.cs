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
            Aff<RT, TResult> AsAff<RT>(TQuery query) where RT : struct, HasDatabase<RT>;

            /// <summary>
            /// Returns query async effect as pipe
            /// </summary>
            Pipe<RT, TQuery, TResult, Unit> AsPipe<RT>() where RT : struct, HasDatabase<RT>;

            /// <summary>
            /// Returns query async effect as producer 
            /// </summary>
            Producer<RT, TResult, Unit> AsProducer<RT>(TQuery query) where RT : struct, HasDatabase<RT>;

            /// <summary>
            /// Returns query async effect as consumer 
            /// </summary>
            Consumer<RT, TQuery, TResult> AsConsumer<RT>() where RT : struct, HasDatabase<RT>;
        }

        /// <summary>
        /// Base class for query handlers, with implemented Pipes methods 
        /// </summary>
        public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            public abstract Aff<RT, TResult> AsAff<RT>(TQuery query)
                where RT : struct, HasDatabase<RT>;

            public Pipe<RT, TQuery, TResult, Unit> AsPipe<RT>()
                where RT : struct, HasDatabase<RT>
                =>
                    from input in Proxy.awaiting<TQuery>()
                    from result in AsAff<RT>(input)
                    from _ in Proxy.yield(result)
                    select unit;

            public Producer<RT, TResult, Unit> AsProducer<RT>(TQuery query)
                where RT : struct, HasDatabase<RT>
                =>
                    from result in AsAff<RT>(query)
                    from _ in Proxy.yield(result)
                    select unit;

            public Consumer<RT, TQuery, TResult> AsConsumer<RT>()
                where RT : struct, HasDatabase<RT>
                =>
                    from input in Proxy.awaiting<TQuery>()
                    from result in AsAff<RT>(input)
                    select result;
        }
    }
}