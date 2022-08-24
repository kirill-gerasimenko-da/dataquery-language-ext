namespace DataQuery.LanguageExt.Sql;

using System;
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
        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, TFifth
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, TFifth, V
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, V
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird
        >
        (string sql, Func<TFirst, TSecond, TThird, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            TFirst, TSecond, TThird, V
        >
        (string sql, Func<TFirst, TSecond, TThird, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond
        >
        (string sql, Func<TFirst, TSecond, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> QueryMultiMap
        <
            TFirst, TSecond, V
        >
        (string sql, Func<TFirst, TSecond, V> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiMap(sql, map, param, splitOn, cmdTimeout, cmdType);

        protected Aff<RT, Seq<T>> Query(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.Query<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Seq<V>> Query<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.Query<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QueryFirst(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QueryFirst<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, T> QuerySingle(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QuerySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> QuerySingle<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QuerySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQueryFirst(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.TryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQueryFirst<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.TryQueryFirst<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<T>> TryQuerySingle(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.TryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Option<V>> TryQuerySingle<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.TryQuerySingle<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, ISqlGridReader> QueryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.QueryMultiple(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, int> Execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.Execute(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, Unit> ExecuteAsUnit(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.Execute(sql, param, cmdTimeout, cmdType).Bind(_ => unitAff);

        protected Aff<RT, T> ExecuteScalar(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.ExecuteScalar<T>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, V> ExecuteScalar<V>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.ExecuteScalar<V>(sql, param, cmdTimeout, cmdType);

        protected Aff<RT, IDataReader> ExecuteReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<RT>.ExecuteReader(sql, param, cmdTimeout, cmdType);
    }
}