using LanguageExt;
using Marten.Linq;

namespace DataQuery.LanguageExt.Marten
{
    public static partial class DataQueryMarten
    {
        /// <summary>
        /// Interface-marker 
        /// </summary>
        public interface IDocQuery<T>
        {
            /// <summary>
            /// Returns query as async effect
            /// </summary>
            Aff<RT, T> AsAff<RT>() where RT : struct, HasMartenDatabase<RT>;
        }

        public abstract record DocScalarQuery<T> : IDocQuery<T>
        {
            public abstract Aff<RT, T> AsAff<RT>() where RT : struct, HasMartenDatabase<RT>;

            protected Aff<RT, IMartenQueryable<T>> Query<RT>() where RT : struct, HasMartenDatabase<RT> =>
                MartenDatabase<RT>.query<T>();

            protected Aff<RT, IMartenQueryable<V>> Query<RT, V>() where RT : struct, HasMartenDatabase<RT> =>
                MartenDatabase<RT>.query<V>();
        }

        public abstract record DocQuery : DocScalarQuery<Unit>;

        public abstract record DocQuery<T> : IDocQuery<Lst<T>>
        {
            public abstract Aff<RT, Lst<T>> AsAff<RT>() where RT : struct, HasMartenDatabase<RT>;

            protected Aff<RT, IMartenQueryable<T>> Query<RT>() where RT : struct, HasMartenDatabase<RT> =>
                MartenDatabase<RT>.query<T>();

            protected Aff<RT, IMartenQueryable<V>> Query<RT, V>() where RT : struct, HasMartenDatabase<RT> =>
                MartenDatabase<RT>.query<V>();
        }
    }
}