using Marten.Linq;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    /// <summary>
    /// Interface-marker 
    /// </summary>
    public interface IMartenQuery<T>
    {
        /// <summary>
        /// Returns query as async effect
        /// </summary>
        Aff<RT, T> AsAff<RT>() where RT : struct, HasMartenDatabase<RT>;
    }

    public abstract record MartenScalarQuery<T> : IMartenQuery<T>
    {
        public abstract Aff<RT, T> AsAff<RT>() where RT : struct, HasMartenDatabase<RT>;

        protected Aff<RT, IMartenQueryable<T>> Query<RT>() where RT : struct, HasMartenDatabase<RT> =>
            MartenDatabase<RT>.query<T>();

        protected Aff<RT, IMartenQueryable<V>> Query<RT, V>() where RT : struct, HasMartenDatabase<RT> =>
            MartenDatabase<RT>.query<V>();
    }

    public abstract record MartenQuery : MartenScalarQuery<Unit>;

    public abstract record MartenQuery<T> : IMartenQuery<Lst<T>>
    {
        public abstract Aff<RT, Lst<T>> AsAff<RT>() where RT : struct, HasMartenDatabase<RT>;

        protected Aff<RT, IMartenQueryable<T>> Query<RT>() where RT : struct, HasMartenDatabase<RT> =>
            MartenDatabase<RT>.query<T>();

        protected Aff<RT, IMartenQueryable<V>> Query<RT, V>() where RT : struct, HasMartenDatabase<RT> =>
            MartenDatabase<RT>.query<V>();
    }
}