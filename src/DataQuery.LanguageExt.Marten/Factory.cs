namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public static IMartenDatabase CreateMartenDatabase(IDocumentStore store)
        => new MartenDatabaseDatabase(new MartenQueryRunner(store, MartenDatabaseRuntime.New));

    public static IMartenQueryRunner CreateMartenQueryRunner(IDocumentStore store)
        => new MartenQueryRunner(store, MartenDatabaseRuntime.New);
}