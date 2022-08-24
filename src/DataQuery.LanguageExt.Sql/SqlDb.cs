namespace DataQuery.LanguageExt.Sql;

using System;
using System.Data;
using System.Threading.Tasks;
using static Prelude;

public static partial class DataQuerySql
{
    public static class SqlDb<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        public static Aff<RT, Seq<T>> Query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.Query<T>(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, true, token))
                select toSeq(result);

        public static Aff<RT, Seq<T>> QueryMultiMap<T>(
            string sql,
            Type[] types,
            Func<object[], T> map,
            object param = null,
            string splitOn = "id",
            int? cmdTimeout = null,
            CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultiMap(
                    cnn, sql, types, map, param, trn.IfNoneUnsafe((IDbTransaction) null),
                    true, splitOn, cmdTimeout, cmdType, token))
                select toSeq(result);

        public static Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, TFifth, T
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, T> map,
            object param = null, string splitOn = "id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                QueryMultiMap(
                    sql,
                    new[] {typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth)},
                    args => map((TFirst) args[0], (TSecond) args[1], (TThird) args[2], (TFourth) args[3],
                        (TFifth) args[4]),
                    param,
                    splitOn,
                    cmdTimeout,
                    cmdType);

        public static Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, TFourth, T
        >
        (string sql, Func<TFirst, TSecond, TThird, TFourth, T> map,
            object param = null, string splitOn = "id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                QueryMultiMap(
                    sql,
                    new[] {typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth)},
                    args => map((TFirst) args[0], (TSecond) args[1], (TThird) args[2], (TFourth) args[3]),
                    param,
                    splitOn,
                    cmdTimeout,
                    cmdType);

        public static Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, TThird, T
        >
        (string sql, Func<TFirst, TSecond, TThird, T> map,
            object param = null, string splitOn = "id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                QueryMultiMap(
                    sql,
                    new[] {typeof(TFirst), typeof(TSecond), typeof(TThird)},
                    args => map((TFirst) args[0], (TSecond) args[1], (TThird) args[2]),
                    param,
                    splitOn,
                    cmdTimeout,
                    cmdType);

        public static Aff<RT, Seq<T>> QueryMultiMap
        <
            TFirst, TSecond, T
        >
        (string sql, Func<TFirst, TSecond, T> map,
            object param = null, string splitOn = "id",
            int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                QueryMultiMap(
                    sql,
                    new[] {typeof(TFirst), typeof(TSecond)},
                    args => map((TFirst) args[0], (TSecond) args[1]),
                    param,
                    splitOn,
                    cmdTimeout,
                    cmdType);

        public static Aff<RT, T> QueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryFirst<T>(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, T> QuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QuerySingle<T>(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, Option<T>> TryQueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.TryQueryFirst<T>(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, Option<T>> TryQuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.TryQuerySingle<T>(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, ISqlGridReader> QueryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultiple(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, int> Execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.Execute(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, T> ExecuteScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteScalar<T>(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, IDataReader> ExecuteReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteReader(
                    cnn, sql, param, trn.IfNoneUnsafe((IDbTransaction) null), cmdTimeout, cmdType, token))
                select result;
    }

    public static Eff<RT, IDbConnection> connection<RT>() where RT : struct, HasSqlConnection<RT> =>
        Eff<RT, IDbConnection>(rt => rt.Connection);

    public static Eff<RT, Option<IDbTransaction>> transaction<RT>() where RT : struct, HasSqlTransaction<RT> =>
        Eff<RT, Option<IDbTransaction>>(rt => rt.Transaction);

    public static Aff<RT, R> UseReader<RT, R>(this Aff<RT, ISqlGridReader> Acq, Func<ISqlGridReader, ValueTask<R>> Use)
        where RT : struct, HasCancel<RT>
        =>
            use(Acq, reader => Aff(async () => await Use(reader)));
}