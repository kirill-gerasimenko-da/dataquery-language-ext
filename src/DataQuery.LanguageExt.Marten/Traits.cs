namespace DataQuery.LanguageExt.Marten;

public static partial class DataQueryMarten
{
    public interface HasMartenDatabase<RT> :
        HasCancel<RT>
        where RT : struct,
        HasMartenDatabase<RT>
    {
        IDocumentSession MartenSession { get; }
        
        Eff<RT, IMartenDatabaseIO> MartenDatabaseEff { get; }
    }
}