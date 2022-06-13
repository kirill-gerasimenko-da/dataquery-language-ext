namespace DataQuery.LanguageExt.Sql;

using System.Data;
using System.Threading;

public static partial class DataQuerySql
{
    public interface ISqlDatabase : ISqlDatabase<DefaultRT>
    {
        Aff<T> RunAsAff<T>(Aff<DefaultRT, T> query, CancellationToken cancelToken);
        Aff<T> RunAsAff<T>(CancellationToken cancelToken, Aff<DefaultRT, T> query);
        Aff<T> RunAsAff<T>(Aff<DefaultRT, T> query, IsolationLevel isolationLevel, CancellationToken cancelToken);
    }

    public class SqlDatabase : SqlDatabaseBase<DefaultRT>, ISqlDatabase
    {
        public SqlDatabase(ISqlQueryRunner<DefaultRT> runner) : base(runner)
        { }

        public Aff<T> RunAsAff<T>(Aff<DefaultRT, T> query, CancellationToken cancelToken) =>
            _runner.AsAff(query, IsolationLevel.Unspecified, cancelToken);

        public Aff<T> RunAsAff<T>(CancellationToken cancelToken, Aff<DefaultRT, T> query) =>
            _runner.AsAff(query, IsolationLevel.Unspecified, cancelToken);

        public Aff<T> RunAsAff<T>(Aff<DefaultRT, T> query, IsolationLevel isolationLevel, CancellationToken cancelToken) => 
            _runner.AsAff(query, isolationLevel, cancelToken);
    }
}