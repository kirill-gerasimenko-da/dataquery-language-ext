using LanguageExt;
using LanguageExt.Effects.Traits;
using Marten;
using Marten.Linq;

namespace DataQuery.LanguageExt.Marten
{
    using static Prelude;

    public static partial class DataQueryMarten
    {
        public static class MartenDatabase<RT>
            where RT : struct,
            HasCancel<RT>,
            HasMartenDatabase<RT>
        {
            public static Aff<RT, IMartenQueryable<T>> query<T>() =>
                from session in session<RT>()
                from queryable in default(RT).MartenDatabaseEff.Map(db => db.Query<T>(session))
                select queryable;
        }

        public static Eff<RT, IDocumentSession> session<RT>() where RT : struct, HasMartenDocumentSession<RT> =>
            Eff<RT, IDocumentSession>(rt => rt.Session);
    }
}