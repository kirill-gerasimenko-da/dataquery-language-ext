using System;
using System.Data.SqlClient;
using Npgsql;

namespace DataQuery.LanguageExt.Sql
{
    public static partial class DataQuerySql
    {
        public enum DriverType
        {
            AdoNet = 1,
            Npgsql = 2
        }

        public static ISqlDatabase CreateSqlDatabase(
            string connectionString,
            DriverType driverType = DriverType.Npgsql)
            =>
                new SqlDatabase(new SqlQueryRunner<SqlDatabaseRuntime>(
                    toConnection(driverType, connectionString),
                    SqlDatabaseRuntime.New));

        static SqlConnectionFactory toConnection(DriverType driverType, string connectionString) => driverType switch
        {
            DriverType.Npgsql => () => new NpgsqlConnection(connectionString),
            DriverType.AdoNet => () => new SqlConnection(connectionString),
            _ => throw new ArgumentOutOfRangeException(nameof(driverType), driverType, null)
        };
    }
}