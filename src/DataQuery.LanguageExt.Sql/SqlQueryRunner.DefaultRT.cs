namespace DataQuery.LanguageExt.Sql;

public static partial class DataQuerySql
{
    public interface ISqlQueryRunner : ISqlQueryRunner<DefaultRT>
    { }

    public class SqlQueryRunner : SqlQueryRunner<DefaultRT>, ISqlQueryRunner
    {
        public SqlQueryRunner(
            SqlConnectionFactory sqlConnectionFactory,
            SqlDatabaseRuntimeFactory<DefaultRT> runtimeFactory)
            : base(sqlConnectionFactory, runtimeFactory)
        { }
    }
}