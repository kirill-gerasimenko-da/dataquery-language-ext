namespace DataQuery.LanguageExt.Sql;

using System;
using System.Data;

public static partial class DataQuerySql
{
    public static class SqlDb
    {
        public static Aff<DefaultRT, Seq<T>> Query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.Query<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> QueryMultiMap<T>(
            string sql,
            Type[] types,
            Func<object[], T> map,
            object param = null,
            string splitOn = "Id",
            int? cmdTimeout = null,
            CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryMultiMap(sql, types, map, param, splitOn, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, TFifth, T
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryMultiMap(
                    sql, map, param, splitOn, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, T
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryMultiMap(
                    sql, map, param, splitOn, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, T
        >
        (string sql, Func<TFirst, TSecond, TThird, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryMultiMap(
                    sql, map, param, splitOn, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, T
        >
        (string sql, Func<TFirst, TSecond, T> map,
            object param = null, string splitOn = "Id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryMultiMap(
                    sql, map, param, splitOn, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> QueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> QuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QuerySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Option<T>> TryQueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.TryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Option<T>> TryQuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.TryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, ISqlGridReader> QueryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.QueryMultiple(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, int> Execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.Execute(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> ExecuteScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.ExecuteScalar<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, IDataReader> ExecuteReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.ExecuteReader(sql, param, cmdTimeout, cmdType);
    }

    public static Eff<DefaultRT, IDbConnection> connection() =>
        Eff<DefaultRT, IDbConnection>(rt => rt.Connection);

    public static Eff<DefaultRT, Option<IDbTransaction>> transaction() =>
        Eff<DefaultRT, Option<IDbTransaction>>(rt => rt.Transaction);
}