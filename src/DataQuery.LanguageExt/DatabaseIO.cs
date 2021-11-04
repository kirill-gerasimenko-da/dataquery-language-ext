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
        public interface IDatabaseIO
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

            ValueTask<IDataReader> ExecuteReaderAsync(
                IDbConnection cnn,
                string sql,
                object param,
                IDbTransaction transaction,
                int? commandTimeout,
                CommandType? commandType);

            ValueTask<GridReader> QueryMultipleAsync(
                IDbConnection cnn,
                string sql,
                object param,
                IDbTransaction transaction,
                int? commandTimeout,
                CommandType? commandType);
        }

        public struct LiveDatabaseIO : IDatabaseIO
        {
            public static readonly IDatabaseIO Default = new LiveDatabaseIO();

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

            public async ValueTask<IDataReader> ExecuteReaderAsync(
                IDbConnection cnn, string sql, object param,
                IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
                =>
                    await cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);

            public async ValueTask<GridReader> QueryMultipleAsync(
                IDbConnection cnn, string sql, object param,
                IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
                =>
                    await cnn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }
}