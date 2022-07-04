namespace DataQuery.LanguageExt.Sql;

using System.Data;
using static Prelude;

public static partial class DataQuerySql
{
    public static class SqlDb<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        public static Aff<RT, Seq<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.Query<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, true, token))
                select toSeq(result);

        public static Aff<RT, T> queryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryFirst<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, T> querySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QuerySingle<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, Option<T>> tryQueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.TryQueryFirst<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, Option<T>> tryQuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.TryQuerySingle<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, ISqlGridReader> queryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultiple(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, int> execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.Execute(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, T> executeScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteScalar<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;

        public static Aff<RT, IDataReader> executeReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteReader(
                    cnn, sql, param, trn, cmdTimeout, cmdType, token))
                select result;
    }

    public static Eff<RT, IDbConnection> connection<RT>() where RT : struct, HasSqlConnection<RT> =>
        Eff<RT, IDbConnection>(rt => rt.Connection);

    public static Eff<RT, IDbTransaction> transaction<RT>() where RT : struct, HasSqlTransaction<RT> =>
        Eff<RT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}