using System.Data;
using System.Threading.Tasks;
using Dapper;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    using static Prelude;
    using static SqlMapper;

    public static partial class DataQuery
    {
        public interface ISqlDatabaseIO
        {
            ValueTask<Seq<T>> QueryAsync<T>(
                IDbConnection cnn,
                string sql,
                object param,
                IDbTransaction transaction,
                int? commandTimeout,
                CommandType? commandType);

            ValueTask<int> ExecuteAsync(
                IDbConnection cnn,
                string sql,
                object param,
                IDbTransaction transaction,
                int? commandTimeout,
                CommandType? commandType);

            ValueTask<T> ExecuteScalarAsync<T>(
                IDbConnection cnn,
                string sql,
                object param,
                IDbTransaction transaction,
                int? commandTimeout,
                CommandType? commandType);

            ValueTask<ISqlGridReader> QueryMultipleAsync(
                IDbConnection cnn,
                string sql,
                object param,
                IDbTransaction transaction,
                int? commandTimeout,
                CommandType? commandType);
        }

        public struct LiveSqlDatabaseIO : ISqlDatabaseIO
        {
            public static readonly ISqlDatabaseIO Default = new LiveSqlDatabaseIO();

            public async ValueTask<Seq<T>> QueryAsync<T>(
                IDbConnection cnn, string sql, object param,
                IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
                =>
                    toSeq(await cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType));

            public async ValueTask<int> ExecuteAsync(
                IDbConnection cnn, string sql, object param,
                IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
                =>
                    await cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);

            public async ValueTask<T> ExecuteScalarAsync<T>(
                IDbConnection cnn, string sql, object param,
                IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
                =>
                    await cnn.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);

            public async ValueTask<ISqlGridReader> QueryMultipleAsync(
                IDbConnection cnn, string sql, object param,
                IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
                =>
                    new SqlGridReader(await cnn.QueryMultipleAsync(
                        sql, param, transaction, commandTimeout, commandType));
        }
    }
}