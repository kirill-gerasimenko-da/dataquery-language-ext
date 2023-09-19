namespace DataQuery.LanguageExt;

using System.Data.Common;

public interface HasDbConnection<out RT> : HasCancel<RT>
    where RT : struct, HasDbConnection<RT>
{
    DbConnection Connection { get; }
    Option<DbTransaction> Transaction { get; }
}