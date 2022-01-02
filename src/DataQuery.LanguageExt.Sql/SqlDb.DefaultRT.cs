namespace DataQuery.LanguageExt.Sql;

using System.Collections.Generic;
using System.Data;

public static partial class DataQuerySql
{
    public static class SqlDb
    {
        public static Aff<DefaultRT, IEnumerable<T>> query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.query<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> queryAll<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryAll<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> queryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> querySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.querySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Option<T>> tryQueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.tryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Option<T>> tryQuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.tryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, ISqlGridReader> queryMultiple(
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

        public static Aff<DefaultRT, IDataReader> executeReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.executeReader(sql, param, cmdTimeout, cmdType);
    }

    public static Eff<DefaultRT, IDbConnection> connection() =>
        Eff<DefaultRT, IDbConnection>(rt => rt.Connection);

    public static Eff<DefaultRT, IDbTransaction> transaction() =>
        Eff<DefaultRT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}