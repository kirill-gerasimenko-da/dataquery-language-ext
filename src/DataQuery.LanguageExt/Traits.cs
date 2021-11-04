using System.Data;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace Dataquery.LanguageExt
{
    public static partial class DataQuery
    {
        /// <summary>
        /// Allows to get Dapper IO effect from the runtime
        /// </summary>
        public interface HasDapper<RT>
            where RT : struct,
            HasDapper<RT>
        {
            Eff<RT, IDatabaseIO> DatabaseEff { get; }
        }

        /// <summary>
        /// Allows to get current open connection from the runtime
        /// </summary>
        public interface HasConnection<RT>
            where RT : struct,
            HasConnection<RT>
        {
            IDbConnection Connection { get; }
        }

        /// <summary>
        /// Allows to get current started transaction from the runtime
        /// </summary>
        /// <typeparam name="RT"></typeparam>
        public interface HasTransaction<RT>
            where RT : struct,
            HasTransaction<RT>
        {
            Option<IDbTransaction> Transaction { get; }
        }

        /// <summary>
        /// All database traits combined
        /// </summary>
        public interface HasDatabase<RT> :
            HasCancel<RT>,
            HasDapper<RT>,
            HasConnection<RT>,
            HasTransaction<RT>
            where RT : struct,
            HasDatabase<RT>
        { }
    }
}