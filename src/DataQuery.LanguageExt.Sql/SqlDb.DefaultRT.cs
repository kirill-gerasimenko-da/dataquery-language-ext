namespace DataQuery.LanguageExt.Sql;

using System.Data;

public static partial class DataQuerySql
{
    public static class SqlDb
    {
        public static Aff<DefaultRT, Seq<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(DefaultRT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryAsync<T>(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<DefaultRT, ISqlGridReader> queryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(DefaultRT).SqlDatabaseEff.MapAsync(dapper => dapper.QueryMultipleAsync(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<DefaultRT, int> execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(DefaultRT).SqlDatabaseEff.MapAsync(dapper => dapper.ExecuteAsync(
                    cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;

        public static Aff<DefaultRT, T> executeScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                from cnn in connection()
                from trn in transaction()
                from result in default(DefaultRT).SqlDatabaseEff.MapAsync(dapper =>
                    dapper.ExecuteScalarAsync<T>(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                select result;
    }

    public static Eff<DefaultRT, IDbConnection> connection() =>
        Eff<DefaultRT, IDbConnection>(rt => rt.Connection);

    public static Eff<DefaultRT, IDbTransaction> transaction() =>
        Eff<DefaultRT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}