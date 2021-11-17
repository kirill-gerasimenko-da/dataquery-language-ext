using Marten;
using Marten.Linq;

namespace DataQuery.LanguageExt.Marten
{
    public static partial class DataQueryMarten
    {
        public interface IMartenDatabaseIO
        {
            IMartenQueryable<T> Query<T>(IDocumentSession session); 
        }

        public struct LiveMartenDatabaseIO : IMartenDatabaseIO
        {
            public static readonly IMartenDatabaseIO Default = new LiveMartenDatabaseIO();

            public IMartenQueryable<T> Query<T>(IDocumentSession session) => session.Query<T>();
        }
    }
}