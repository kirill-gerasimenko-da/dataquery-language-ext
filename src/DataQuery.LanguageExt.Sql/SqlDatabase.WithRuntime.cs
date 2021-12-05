namespace DataQuery.LanguageExt.Sql;

public static partial class DataQuerySql
{
    public interface ISqlDatabase : ISqlDatabase<SqlDatabaseRuntime>
    { }

    public class SqlDatabase : SqlDatabaseBase<SqlDatabaseRuntime>, ISqlDatabase
    {
        public SqlDatabase(ISqlQueryRunner<SqlDatabaseRuntime> runner) : base(runner)
        { }
    }
}