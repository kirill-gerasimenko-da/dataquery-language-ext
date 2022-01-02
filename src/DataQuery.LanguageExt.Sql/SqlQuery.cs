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

    public abstract record SqlQueryBase<T> : ISqlQuery<T>
    {
        public abstract Aff<RT, T> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;

        protected Aff<RT, Seq<T>> Query<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryAll<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> Query<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryAll<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> QueryFirst<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> QueryFirst<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> QuerySingle<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.querySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> QuerySingle<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.querySingle<V>(sql, param, cmdTimeout, cmdType);

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

    public abstract record SqlQueryOption<T> : SqlQueryBase<Option<T>>;
    public abstract record SqlQueryUnit : SqlQueryBase<Unit>;
    public abstract record SqlQuerySeq<T> : SqlQueryBase<Seq<T>>;
    public abstract record SqlQuery<T> : SqlQueryBase<T>;
}