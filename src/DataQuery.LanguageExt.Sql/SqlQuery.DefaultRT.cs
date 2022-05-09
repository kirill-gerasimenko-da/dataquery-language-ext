namespace DataQuery.LanguageExt.Sql;

using System.Collections.Generic;
using System.Data;

public static partial class DataQuerySql
{
    /// <summary>
    /// Interface-marker 
    /// </summary>
    public interface ISqlQuery<RT, T> where RT : struct, HasSqlDatabase<RT>
    {
        /// <summary>
        /// Returns query as async effect
        /// </summary>
        Aff<RT, T> AsAff();
    }

    public abstract record SqlQuery : SqlQueryBase<DefaultRT, Unit>, ISqlQuery<DefaultRT, Unit>
    {
        public abstract Aff<DefaultRT, Unit> AsAff();
    }

    public abstract record SqlQuery<T> : SqlQueryBase<DefaultRT, T>, ISqlQuery<DefaultRT, T>
    {
        public abstract Aff<DefaultRT, T> AsAff();
    }

    public abstract record SqlQueryBase<RT, T> where RT : struct, HasSqlDatabase<RT>
    {
        protected Aff<RT, IEnumerable<T>> Query(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.query<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, IEnumerable<V>> Query<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.query<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryAll(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.queryAll<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryAll<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.queryAll<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QueryFirst(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.queryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QueryFirst<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.queryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QuerySingle(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.querySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QuerySingle<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.querySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQueryFirst(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.tryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQueryFirst<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.tryQueryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQuerySingle(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.tryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQuerySingle<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.tryQuerySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, ISqlGridReader> QueryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.queryMultiple(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, int> Execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.execute(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Unit> ExecuteAsUnit(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.execute(sql, param, cmdTimeout, cmdType).Bind(_ => unitAff);

        protected Aff<RT, T> ExecuteScalar(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.executeScalar<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> ExecuteScalar<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.executeScalar<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, IDataReader> ExecuteReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.executeReader(sql, param, cmdTimeout, cmdType);
    }
}