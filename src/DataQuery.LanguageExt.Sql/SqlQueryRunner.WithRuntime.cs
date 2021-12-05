namespace DataQuery.LanguageExt.Sql;

public static partial class DataQuerySql
{
    public interface ISqlQueryRunner : ISqlQueryRunner<SqlDatabaseRuntime>
    { }

    public class SqlQueryRunner : SqlQueryRunner<SqlDatabaseRuntime>, ISqlQueryRunner
    {
        public SqlQueryRunner(
            SqlConnectionFactory sqlConnectionFactory,
            SqlDatabaseRuntimeFactory<SqlDatabaseRuntime> runtimeFactory)
            : base(sqlConnectionFactory, runtimeFactory)
        { }
    }
}