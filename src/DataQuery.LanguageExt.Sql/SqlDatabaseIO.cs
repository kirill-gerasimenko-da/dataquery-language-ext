using System.Collections.Generic;
using System.Threading;

namespace DataQuery.LanguageExt.Sql;

using System.Data;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;

public static partial class DataQuerySql
{
    public interface ISqlDatabaseIO
    {
        ValueTask<IEnumerable<T>> Query<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            bool buffered,
            CancellationToken cancelToken);

        ValueTask<Option<T>> QueryFirst<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType);

        ValueTask<T> QuerySingle<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType);

        ValueTask<GridReader> QueryMultiple(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType);

        ValueTask<int> Execute(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType);

        ValueTask<T> ExecuteScalar<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType);

        ValueTask<IDataReader> ExecuteReader(
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

        public async ValueTask<IEnumerable<T>> Query<T>(IDbConnection cnn,
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

        public async ValueTask<Option<T>> QueryFirst<T>(
            IDbConnection cnn, string sql, object param,
            IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
        {
            using var reader = await cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);

            if (!reader.Read())
                return None;

            return reader.GetRowParser<T>()(reader);
        }

        public async ValueTask<T> QuerySingle<T>(
            IDbConnection cnn, string sql,
            object param, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
            =>
                await cnn.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);

        public async ValueTask<int> Execute(
            IDbConnection cnn, string sql, object param,
            IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
            =>
                await cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);

        public async ValueTask<T> ExecuteScalar<T>(
            IDbConnection cnn, string sql, object param,
            IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
            =>
                await cnn.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);

        public async ValueTask<GridReader> QueryMultiple(
            IDbConnection cnn, string sql, object param,
            IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
            =>
                await cnn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);

        public async ValueTask<IDataReader> ExecuteReader(
            IDbConnection cnn, string sql, object param,
            IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
            =>
                await cnn.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
    }
}