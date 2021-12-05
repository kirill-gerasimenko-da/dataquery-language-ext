namespace DataQuery.LanguageExt.Sql;

using System.Data;

public static partial class DataQuerySql
{
    public static class SqlDb
    {
        public static Aff<SqlDatabaseRuntime, Seq<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(SqlDatabaseRuntime).SqlDatabaseEff.MapAsync(dapper => dapper.QueryAsync<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<SqlDatabaseRuntime, ISqlGridReader> queryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(SqlDatabaseRuntime).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultipleAsync(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<SqlDatabaseRuntime, int> execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(SqlDatabaseRuntime).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteAsync(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<SqlDatabaseRuntime, T> executeScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(SqlDatabaseRuntime).SqlDatabaseEff.MapAsync(dapper =>
                    dapper.ExecuteScalarAsync<T>(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;
    }

    public static Eff<SqlDatabaseRuntime, IDbConnection> connection() =>
        Eff<SqlDatabaseRuntime, IDbConnection>(rt => rt.Connection);

    public static Eff<SqlDatabaseRuntime, IDbTransaction> transaction() =>
        Eff<SqlDatabaseRuntime, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}