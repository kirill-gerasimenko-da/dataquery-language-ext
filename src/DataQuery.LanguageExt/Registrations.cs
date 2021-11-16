using System;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SimpleInjector;

namespace Dataquery.LanguageExt
{
    public static partial class DataQuerySql
    {
        public enum DriverType
        {
            AdoNet = 1,
            Npgsql = 2
        }

        /// <summary>
        /// Registers sql database abstraction as singleton. Depending on driver type
        /// it would use PostgreSQL SQL driver or ADO NET.  
        /// </summary>
        public static void AddSqlDatabase(
            this IServiceCollection services,
            string connectionString,
            DriverType driverType = DriverType.Npgsql)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            services.AddSingleton<ISqlQueryRunner<SqlDatabaseRuntime>, SqlQueryRunner<SqlDatabaseRuntime>>(_ =>
                new SqlQueryRunner<SqlDatabaseRuntime>(
                    toConnection(driverType, connectionString), SqlDatabaseRuntime.New));

            services.AddSingleton<ISqlDatabase, SqlDatabase>();
        }

        /// <summary>
        /// Registers sql database abstraction as singleton. Depending on driver type
        /// it would use PostgreSQL SQL driver or ADO NET.  
        /// </summary>
        public static void RegisterSqlDatabase(
            this Container container,
            string connectionString,
            DriverType driverType = DriverType.Npgsql)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            container.RegisterSingleton<ISqlQueryRunner<SqlDatabaseRuntime>>(() =>
                new SqlQueryRunner<SqlDatabaseRuntime>(
                    toConnection(driverType, connectionString), SqlDatabaseRuntime.New));

            container.RegisterSingleton<ISqlDatabase, SqlDatabase>();
        }

        /// <summary>
        /// Factory of SQL database, if one doesn't have|need DI container
        /// </summary>
        public static ISqlDatabase
            CreateSqlDatabase(string connectionString, DriverType driverType = DriverType.Npgsql) =>
            new SqlDatabase(
                new SqlQueryRunner<SqlDatabaseRuntime>(
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