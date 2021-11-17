using LanguageExt;
using LanguageExt.Effects.Traits;
using Marten;

namespace DataQuery.LanguageExt.Marten
{
    public static partial class DataQueryMarten
    {
        public interface HasMartenDocumentSession<RT>
            where RT : struct, 
            HasMartenDocumentSession<RT>
        {
            IDocumentSession Session { get; }
        }
        
        public interface HasMartenDatabase<RT> :
            HasCancel<RT>,
            HasMartenDocumentSession<RT>
            where RT : struct,
            HasMartenDatabase<RT>
        {
            Eff<RT, IMartenDatabaseIO> MartenDatabaseEff { get; }
        }
    }
}