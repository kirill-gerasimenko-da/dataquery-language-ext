namespace DataQuery.LanguageExt.Sql;

using System;
using System.Data.SqlClient;
using Npgsql;

public static partial class DataQuerySql
{
    public enum DriverType
    {
        AdoNet = 1,
        Npgsql = 2
    }

    /// <summary>
    /// Creates new SQL database client. Supports Npgsql and AdoNet drivers.
    /// </summary>
    public static ISqlDatabase CreateSqlDatabaseClient
    (
        string connectionString,
        DriverType driverType = DriverType.Npgsql
    ) =>
        new SqlDatabase(new SqlQueryRunner(toConnection(driverType, connectionString), DefaultRT.New));

    static SqlConnectionFactory toConnection(DriverType driverType, string connectionString) => driverType switch
    {
        DriverType.Npgsql => () => new NpgsqlConnection(connectionString),
        DriverType.AdoNet => () => new SqlConnection(connectionString),
        _ => throw new ArgumentOutOfRangeException(nameof(driverType), driverType, null)
    };
}