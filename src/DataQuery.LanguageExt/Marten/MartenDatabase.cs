using System;
using LanguageExt;
using Marten;
using Marten.Linq;

namespace DataQuery.LanguageExt.Marten
{
    using static Prelude;

    public static partial class DataQueryMarten
    {
        public static class MartenDatabase<RT>
            where RT : struct,
            HasMartenDatabase<RT>
        {
            public static Aff<RT, IMartenQueryable<T>> query<T>() =>
                from session in session<RT>()
                from queryable in default(RT).MartenDatabaseEff.Map(db => db.Query<T>(session))
                select queryable;

            public static Aff<RT, Unit> store<T>(Seq<T> entities) =>
                from session in session<RT>()
                from queryable in default(RT).MartenDatabaseEff.Map(db => db.Store<T>(session, entities))
                select queryable;

            public static Aff<RT, Unit> store<T>(T entity, Guid version) =>
                from session in session<RT>()
                from queryable in default(RT).MartenDatabaseEff.Map(db => db.Store<T>(session, entity, version))
                select queryable;
        }

        public static Eff<RT, IDocumentSession> session<RT>() where RT : struct, HasMartenDocumentSession<RT> =>
            Eff<RT, IDocumentSession>(rt => rt.Session);
    }
}