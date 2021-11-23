using System;
using Marten;
using Marten.Linq;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public interface IMartenDatabaseIO
    {
        IMartenQueryable<T> Query<T>(IDocumentSession session);

        Unit Store<T>(IDocumentSession session, Seq<T> entities);
        Unit Store<T>(IDocumentSession session, T entity, Guid version);
    }

    public struct LiveMartenDatabaseIO : IMartenDatabaseIO
    {
        public static readonly IMartenDatabaseIO Default = new LiveMartenDatabaseIO();

        public IMartenQueryable<T> Query<T>(IDocumentSession session) => session.Query<T>();

        public Unit Store<T>(IDocumentSession session, Seq<T> entities)
        {
            session.Store(entities.AsEnumerable());
            return unit;
        }

        public Unit Store<T>(IDocumentSession session, T entity, Guid version)
        {
            session.Store(entity, version);
            return unit;
        }
    }
}