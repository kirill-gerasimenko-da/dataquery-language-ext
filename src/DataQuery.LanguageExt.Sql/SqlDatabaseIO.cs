namespace DataQuery.LanguageExt.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

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

        ValueTask<IEnumerable<T>> QueryMultiMap<T>(
            IDbConnection cnn,
            string sql,
            Type[] types,
            Func<object[], T> map,
            object param,
            IDbTransaction transaction,
            bool buffered,
            string splitOn,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<T> QueryFirst<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<T> QuerySingle<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<Option<T>> TryQueryFirst<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<Option<T>> TryQuerySingle<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<ISqlGridReader> QueryMultiple(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<int> Execute(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<T> ExecuteScalar<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);

        ValueTask<IDataReader> ExecuteReader(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken);
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
                    cancellationToken: cancelToken));

        public async ValueTask<IEnumerable<T>> QueryMultiMap<T>(
            IDbConnection cnn,
            string sql,
            Type[] types,
            Func<object[], T> map,
            object param,
            IDbTransaction transaction,
            bool buffered,
            string splitOn,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken _)
            =>
                await cnn.QueryAsync(
                    sql,
                    types,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType);

        public async ValueTask<T> QueryFirst<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
            =>
                await cnn.QueryFirstAsync<T>(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: cancelToken));

        public async ValueTask<T> QuerySingle<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
            =>
                await cnn.QuerySingleAsync<T>(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: cancelToken));

        public async ValueTask<Option<T>> TryQueryFirst<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
        {
            var entities = await cnn.QueryAsync<T>(new CommandDefinition(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType,
                flags: CommandFlags.None, // disable buffering
                cancellationToken: cancelToken));

            foreach (var entity in entities)
                return entity;

            return None;
        }

        public async ValueTask<Option<T>> TryQuerySingle<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
        {
            var entities = await cnn.QueryAsync<T>(new CommandDefinition(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType,
                flags: CommandFlags.None, // disable buffering
                cancellationToken: cancelToken));

            Option<T> result = default;

            var i = 0;
            foreach (var entity in entities)
            {
                if (i++ == 0)
                {
                    result = entity;
                    continue;
                }

                throw new InvalidOperationException("There are multiple results available when single expected");
            }

            return result;
        }

        public async ValueTask<int> Execute(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
            =>
                await cnn.ExecuteAsync(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: cancelToken));

        public async ValueTask<T> ExecuteScalar<T>(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
            =>
                await cnn.ExecuteScalarAsync<T>(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: cancelToken));

        public async ValueTask<IDataReader> ExecuteReader(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
            =>
                await cnn.ExecuteReaderAsync(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: cancelToken));

        public async ValueTask<ISqlGridReader> QueryMultiple(
            IDbConnection cnn,
            string sql,
            object param,
            IDbTransaction transaction,
            int? commandTimeout,
            CommandType? commandType,
            CancellationToken cancelToken)
            =>
                new SqlGridReader(await cnn.QueryMultipleAsync(new CommandDefinition(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: cancelToken)));
    }
}
