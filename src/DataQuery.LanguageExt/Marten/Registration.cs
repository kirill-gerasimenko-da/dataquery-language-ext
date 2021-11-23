using Marten;

namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public static IDocDatabase CreateDocDatabase(IDocumentStore store)
        => new DocDatabase(new DocQueryRunner<MartenDatabaseRuntime>(store, MartenDatabaseRuntime.New));
}