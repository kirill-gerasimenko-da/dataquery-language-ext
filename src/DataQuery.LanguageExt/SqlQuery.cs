using LanguageExt;

namespace Dataquery.LanguageExt
{
    public static partial class DataQuery
    {
        /// <summary>
        /// Interface-marker 
        /// </summary>
        public interface ISqlQuery<TResult>
        {
            /// <summary>
            /// Returns query as async effect
            /// </summary>
            Aff<RT, TResult> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;
        }

        /// <summary>
        /// Base class for query, allows not to put generic constraints
        /// to the AsAff implementations, thus making code cleaner
        /// </summary>
        public abstract record SqlQuery<TResult> : ISqlQuery<TResult>
        {
            public abstract Aff<RT, TResult> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;
        }
    }
}