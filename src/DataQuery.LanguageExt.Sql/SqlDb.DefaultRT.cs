namespace DataQuery.LanguageExt.Sql;

using System.Collections.Generic;
using System.Data;

public static partial class DataQuerySql
{
    public static class SqlDb
    {
        public static Aff<DefaultRT, IEnumerable<T>> Query<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.query<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Seq<T>> QueryAll<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryAll<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> QueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> QuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.querySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Option<T>> TryQueryFirst<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.tryQueryFirst<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, Option<T>> TryQuerySingle<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.tryQuerySingle<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, ISqlGridReader> QueryMultiple(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.queryMultiple(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, int> Execute(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.execute(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, T> ExecuteScalar<T>(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.executeScalar<T>(sql, param, cmdTimeout, cmdType);

        public static Aff<DefaultRT, IDataReader> ExecuteReader(
            string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
            =>
                SqlDb<DefaultRT>.executeReader(sql, param, cmdTimeout, cmdType);
    }

    public static Eff<DefaultRT, IDbConnection> Connection() =>
        Eff<DefaultRT, IDbConnection>(rt => rt.Connection);

    public static Eff<DefaultRT, IDbTransaction> Transaction() =>
        Eff<DefaultRT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
}