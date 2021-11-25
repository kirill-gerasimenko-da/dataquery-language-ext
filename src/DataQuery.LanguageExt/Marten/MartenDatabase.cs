using System;
using System.Linq;
using System.Threading;
using Marten;
using Marten.Linq;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public static class MartenDb<RT>
        where RT : struct,
        HasMartenDatabase<RT>
    {
        public static Aff<RT, IMartenQueryable<T>> query<T>() =>
            from session in session<RT>()
            from queryable in default(RT).MartenDatabaseEff.Map(db => db.Query<T>(session))
            select queryable;

        public static Aff<RT, Unit> store<T>(Seq<T> entities) =>
            from session in session<RT>()
            from _ in default(RT).MartenDatabaseEff.Map(db => db.Store(session, entities))
            select unit;

        public static Aff<RT, Unit> store<T>(T entity) =>
            from session in session<RT>()
            from _ in default(RT).MartenDatabaseEff.Map(db => db.Store(session, Seq1(entity)))
            select unit;

        public static Aff<RT, Unit> store<T>(T entity, Guid version) =>
            from session in session<RT>()
            from _ in default(RT).MartenDatabaseEff.Map(db => db.Store(session, entity, version))
            select unit;

        public static Aff<RT, Seq<T>> toSeq<T>(IQueryable<T> query) =>
            from cancelToken in cancelToken<RT>()
            from result in Aff(async () => Prelude.toSeq(await query.ToListAsync(cancelToken)).Strict())
            select result;

        public static Aff<RT, Option<T>> headOrNone<T>(IQueryable<T> query) =>
            from cancelToken in cancelToken<RT>()
            from result in Aff(async () => Optional(await query.FirstOrDefaultAsync(cancelToken)))
            select result;

        public static Aff<RT, bool> any<T>(IQueryable<T> query) =>
            from cancelToken in cancelToken<RT>()
            from result in Aff(async () => await query.AnyAsync(cancelToken))
            select result;
    }

    public static Eff<RT, IDocumentSession> session<RT>() where RT : struct, HasMartenDatabase<RT> =>
        Eff<RT, IDocumentSession>(rt => rt.MartenSession);

    public static Aff<Seq<T>> ToSeq<T>(this IQueryable<T> query, CancellationToken cancelToken)
        => Aff(async () => toSeq(await query.ToListAsync(cancelToken)).Strict());

    public static Aff<Option<T>> HeadOrNone<T>(this IQueryable<T> query, CancellationToken cancelToken)
        => Aff(async () => Optional(await query.FirstOrDefaultAsync(cancelToken)));

    public static Aff<bool> Any<T>(this IQueryable<T> query, CancellationToken cancelToken)
        => Aff(async () => await query.AnyAsync(cancelToken));
}