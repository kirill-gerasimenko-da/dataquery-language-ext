using Marten;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public static IMartenDatabase CreateMartenDatabase(IDocumentStore store)
        => new MartenDatabaseDatabase(new MartenQueryRunner<MartenDatabaseRuntime>(
            store, MartenDatabaseRuntime.New));
}