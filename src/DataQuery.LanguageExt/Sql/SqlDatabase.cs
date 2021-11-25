using System.Data;

namespace DataQuery.LanguageExt.Sql;

public static partial class DataQuerySql
{
    public static class SqlDb<RT>
        where RT : struct,
        HasSqlDatabase<RT>
    {
        public static Aff<RT, Seq<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryAsync<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, ISqlGridReader> queryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultipleAsync(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, int> execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteAsync(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<RT, T> executeScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection<RT>()
                from trn in transaction<RT>()
                from result in default(RT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteScalarAsync<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;
    }

    public static Eff<RT, IDbConnection> connection<RT>() where RT : struct, HasSqlConnection<RT> =>
        Eff<RT, IDbConnection>(rt => rt.Connection);

    public static Eff<RT, IDbTransaction> transaction<RT>() where RT : struct, HasSqlTransaction<RT> =>
        Eff<RT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}