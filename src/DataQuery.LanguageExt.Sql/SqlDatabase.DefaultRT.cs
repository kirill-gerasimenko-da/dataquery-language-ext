namespace DataQuery.LanguageExt.Sql;

public static partial class DataQuerySql
{
    public interface ISqlDatabase : ISqlDatabase<DefaultRT>
    { }

    public class SqlDatabase : SqlDatabaseBase<DefaultRT>, ISqlDatabase
    {
        public SqlDatabase(ISqlQueryRunner<DefaultRT> runner) : base(runner)
        { }
    }
}