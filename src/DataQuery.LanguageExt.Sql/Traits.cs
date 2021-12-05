namespace DataQuery.LanguageExt.Sql;

using System.Data;

public static partial class DataQuerySql
{
    /// <summary>
    /// Allows to get current open connection from the runtime
    /// </summary>
    public interface HasSqlConnection<RT>
        where RT : struct,
        HasSqlConnection<RT>
    {
        IDbConnection Connection { get; }
    }

    /// <summary>
    /// Allows to get current started transaction from the runtime
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    public interface HasSqlTransaction<RT>
        where RT : struct,
        HasSqlTransaction<RT>
    {
        Option<IDbTransaction> Transaction { get; }
    }

    /// <summary>
    /// Allows to use database IO
    /// </summary>
    public interface HasSqlDatabase<RT> :
        HasCancel<RT>,
        HasSqlConnection<RT>,
        HasSqlTransaction<RT>
        where RT : struct,
        HasSqlDatabase<RT>
    {
        Eff<RT, ISqlDatabaseIO> SqlDatabaseEff { get; }
    }
}