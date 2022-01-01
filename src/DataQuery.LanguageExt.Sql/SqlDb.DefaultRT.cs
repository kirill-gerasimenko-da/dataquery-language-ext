namespace DataQuery.LanguageExt.Sql;

using System.Data;
using Dapper;

public static partial class DataQuerySql
{
    public static class SqlDb
    {
        public static Aff<DefaultRT, Seq<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null, bool buffered = true)
            =>
                SqlDb<DefaultRT>.query<T>(sql, param, cmdTimeout, cmdType, buffered);

        public static Aff<DefaultRT, Option<T>> queryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> querySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.querySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, SqlMapper.GridReader> queryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryMultiple(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, int> execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.execute(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> executeScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.executeScalar<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, IDataReader> executeReader<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.executeReader(sql, param, cmdTimeout, cmdType);
    }

    public static Eff<DefaultRT, IDbConnection> connection() =>
        Eff<DefaultRT, IDbConnection>(rt => rt.Connection);

    public static Eff<DefaultRT, IDbTransaction> transaction() =>
        Eff<DefaultRT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}