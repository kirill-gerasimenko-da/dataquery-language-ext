using System.Data;
using LanguageExt;

namespace DataQuery.LanguageExt.Sql
{
    public static partial class DataQuerySql
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
        public abstract record SqlQuery<T> : ISqlQuery<T>
        {
            public abstract Aff<RT, T> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;

            protected Aff<RT, Seq<T>> Query<RT>(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                where RT : struct, HasSqlDatabase<RT>
                =>
                    SqlDatabase<RT>.query<T>(sql, param, cmdTimeout, cmdType);

            protected Aff<RT, ISqlGridReader> QueryMultiple<RT>(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                where RT : struct, HasSqlDatabase<RT>
                =>
                    SqlDatabase<RT>.queryMultiple(sql, param, cmdTimeout, cmdType);

            protected Aff<RT, int> Execute<RT>(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                where RT : struct, HasSqlDatabase<RT>
                =>
                    SqlDatabase<RT>.execute(sql, param, cmdTimeout, cmdType);

            protected Aff<RT, T> ExecuteScalar<RT>(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                where RT : struct, HasSqlDatabase<RT>
                =>
                    SqlDatabase<RT>.executeScalar<T>(sql, param, cmdTimeout, cmdType);
        }
    }
}