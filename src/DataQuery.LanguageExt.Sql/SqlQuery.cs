namespace DataQuery.LanguageExt.Sql;

using System.Collections.Generic;
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

    public abstract record SqlQueryRT : SqlQueryBaseRT<Unit>, ISqlQuery<Unit>
    {
        public abstract Aff<RT, Unit> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;
    }

    public abstract record SqlQueryRT<T> : SqlQueryBaseRT<T>, ISqlQuery<T>
    {
        public abstract Aff<RT, T> AsAff<RT>() where RT : struct, HasSqlDatabase<RT>;
    }

    public abstract record SqlQueryBaseRT<T>
    {
        protected Aff<RT, Seq<T>> Query<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.query<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryAll<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.query<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QueryFirst<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QueryFirst<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.queryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QuerySingle<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.querySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QuerySingle<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.querySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQueryFirst<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.tryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQueryFirst<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.tryQueryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQuerySingle<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.tryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQuerySingle<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.tryQuerySingle<V>(sql, param, cmdTimeout, cmdType);

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

        protected Aff<RT, IDataReader> ExecuteReader<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.executeReader(sql, param, cmdTimeout, cmdType);
    }
}