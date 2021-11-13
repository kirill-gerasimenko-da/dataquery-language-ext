using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace Dataquery.LanguageExt
{
    using static DataQuery;

    public static class DataQueryRegistration
    {
        public static void AddPostgreSqlDataQuery(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddSingleton
            <
                ISqlQueryRunner<SqlDatabaseRuntime>,
                PostgreSqlQueryRunner<SqlDatabaseRuntime>
            >(_ => new PostgreSqlQueryRunner<SqlDatabaseRuntime>(
                ConnectionString.New(connectionString),
                SqlDatabaseRuntime.New));

            services.AddSingleton<ISqlDatabase, SqlDatabase>();
        }

        public static void AddPostgreSqlDataQuery(
            this Container container,
            string connectionString)
        {
            container.RegisterSingleton<ISqlQueryRunner<SqlDatabaseRuntime>>(() =>
                new PostgreSqlQueryRunner<SqlDatabaseRuntime>(
                    ConnectionString.New(connectionString),
                    SqlDatabaseRuntime.New));

            container.RegisterSingleton<ISqlDatabase, SqlDatabase>();
        }
    }
}