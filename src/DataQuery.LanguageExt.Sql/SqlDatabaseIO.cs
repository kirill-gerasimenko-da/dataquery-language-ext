using System.Collections.Generic;
using System.Threading;

namespace DataQuery.LanguageExt.Sql;

using System.Data;
using System.Threading.Tasks;
using Dapper;

public static partial class DataQuerySql
{
    public interface ISqlDatabaseIO
    {
        ValueTask<IEnumerable<T>> QueryAsync<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            bool buffered,
            CancellationToken cancelToken);

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

        public async ValueTask<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            bool buffered,
            CancellationToken cancelToken)
            =>
                await cnn.QueryAsync<T>(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    buffered ? CommandFlags.Buffered : CommandFlags.None,
                    cancelToken));

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