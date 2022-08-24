namespace DataQuery.LanguageExt.Sql;

using System;
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
        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            RT, TFirst, TSecond, TThird, TFourth, TFifth
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT> =>
            SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            RT, TFirst, TSecond, TThird, TFourth, TFifth, V
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            RT, TFirst, TSecond, TThird, TFourth
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(
                    sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            RT, TFirst, TSecond, TThird, TFourth, V
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            RT, TFirst, TSecond, TThird
        >
        (string sql, Func<TFirst, TSecond, TThird, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            RT, TFirst, TSecond, TThird, V
        >
        (string sql, Func<TFirst, TSecond, TThird, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            RT, TFirst, TSecond
        >
        (string sql, Func<TFirst, TSecond, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            RT, TFirst, TSecond, V
        >
        (string sql, Func<TFirst, TSecond, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> Query<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.Query<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> Query<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.Query<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QueryFirst<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QueryFirst<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QuerySingle<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QuerySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QuerySingle<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QuerySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQueryFirst<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.TryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQueryFirst<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.TryQueryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQuerySingle<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.TryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQuerySingle<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.TryQuerySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, ISqlGridReader> QueryMultiple<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.QueryMultiple(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, int> Execute<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.Execute(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Unit> ExecuteAsUnit<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.Execute(sql, param, cmdTimeout, cmdType).Bind(_ => unitAff);

        protected Aff<RT, T> ExecuteScalar<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.ExecuteScalar<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> ExecuteScalar<RT, V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.ExecuteScalar<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, IDataReader> ExecuteReader<RT>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            where RT : struct, HasSqlDatabase<RT>
            =>
                SqlDb<RT>.ExecuteReader(sql, param, cmdTimeout, cmdType);
    }
}