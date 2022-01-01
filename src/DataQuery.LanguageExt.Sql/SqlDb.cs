using System.Data;

namespace DataQuery.LanguageExt.Sql;

using static Dapper.SqlMapper;
using static Prelude;

public static partial class DataQuerySql
{
    public static class SqlDb<RT>
        where RT : struct, HasSqlDatabase<RT>
    {
        public static Aff<RT, Seq<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null, bool buffered = true)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from token in cancelToken<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.Query<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType, buffered, token))
                select toSeq(result).Strict();

        public static Aff<RT, Option<T>> queryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryFirst<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, T> querySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QuerySingle<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, GridReader> queryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultiple(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, int> execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.Execute(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, T> executeScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteScalar<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, IDataReader> executeReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteReader(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;
    }

    public static Eff<RT, IDbConnection> connection<RT>() where RT : struct, HasSqlConnection<RT> =>
        Eff<RT, IDbConnection>(rt => rt.Connection);

    public static Eff<RT, IDbTransaction> transaction<RT>() where RT : struct, HasSqlTransaction<RT> =>
        Eff<RT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}