using System.Data;
using Dapper;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    using static Prelude;
    using static SqlMapper;

    public static partial class DataQuery
    {
        public static class Database<RT>
            where RT : struct,
            HasDatabase<RT>
        {
            public static Aff<RT, Seq<TResult>> SqlQuery<TResult>(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                =>
                    from cnn in connection<RT>()
                    from trn in transaction<RT>()
                    from result in default(RT).DatabaseEff.MapAsync(dapper => dapper.QueryAsync<TResult>(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                    select result;

            public static Aff<RT, GridReader> SqlQueryMultiple(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                =>
                    from cnn in connection<RT>()
                    from trn in transaction<RT>()
                    from result in default(RT).DatabaseEff.MapAsync(dapper => dapper.QueryMultipleAsync(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                    select result;

            public static Aff<RT, int> SqlExecute(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                =>
                    from cnn in connection<RT>()
                    from trn in transaction<RT>()
                    from result in default(RT).DatabaseEff.MapAsync(dapper => dapper.ExecuteAsync(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                    select result;

            public static Aff<RT, T> SqlExecuteScalar<T>(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                =>
                    from cnn in connection<RT>()
                    from trn in transaction<RT>()
                    from result in default(RT).DatabaseEff.MapAsync(dapper => dapper.ExecuteScalarAsync<T>(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                    select result;

            public static Aff<RT, IDataReader> SqlExecuteReader(
                string sql, object param = null, int? cmdTimeout = null, CommandType? cmdType = null)
                =>
                    from cnn in connection<RT>()
                    from trn in transaction<RT>()
                    from result in default(RT).DatabaseEff.MapAsync(dapper => dapper.ExecuteReaderAsync(
                        cnn, sql, param, trn, cmdTimeout, cmdType))
                    select result;
        }

        static Eff<RT, IDbConnection> connection<RT>() where RT : struct, HasConnection<RT> =>
            Eff<RT, IDbConnection>(rt => rt.Connection);

        static Eff<RT, IDbTransaction> transaction<RT>() where RT : struct, HasTransaction<RT> =>
            Eff<RT, IDbTransaction>(rt => rt.Transaction.IfNoneUnsafe((IDbTransaction)null));
    }
}