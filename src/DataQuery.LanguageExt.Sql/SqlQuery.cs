namespace DataQuery.LanguageExt.Sql;

using System.Data;

public static partial class DataQuerySql
{
    /// <summary>
    /// Interface-marker 
    /// </summary>
    public interface ISqlQuery<T>
    {
        /// <summary>
        /// Returns query as async effect
        /// </summary>
        Aff<RT, T> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;
    }

    public abstract record SqlScalarQuery<T> : ISqlQuery<T>
    {
        public abstract Aff<RT, T> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;

        protected Aff<RT, Seq<T>> Query<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.query<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> Query<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.query<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, ISqlGridReader> QueryMultiple<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryMultiple(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, int> Execute<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.execute(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Unit> ExecuteAsUnit<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.execute(sql, param, cmdTimeout, cmdType).Bind(_ => unitAff);

        protected Aff<RT, T> ExecuteScalar<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.executeScalar<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> ExecuteScalar<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.executeScalar<V>(sql, param, cmdTimeout, cmdType);
    }


    public abstract record SqlQuery : SqlScalarQuery<Unit>;

    /// <summary>
    /// Base class for query, allows not to put generic constraints
    /// to the AsAff implementations, thus making code cleaner
    /// </summary>
    public abstract record SqlQuery<T> : ISqlQuery<Lst<T>>
    {
        public abstract Aff<RT, Lst<T>> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;

        protected Aff<RT, Seq<T>> Query<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.query<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> Query<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.query<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, ISqlGridReader> QueryMultiple<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryMultiple(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, int> Execute<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.execute(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Unit> ExecuteAsUnit<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.execute(sql, param, cmdTimeout, cmdType).Bind(_ => unitAff);

        protected Aff<RT, T> ExecuteScalar<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.executeScalar<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> ExecuteScalar<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.executeScalar<V>(sql, param, cmdTimeout, cmdType);
    }
}