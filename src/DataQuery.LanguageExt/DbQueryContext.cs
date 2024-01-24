namespace DataQuery.LanguageExt;

using System.Data.Common;
using System.Threading;

public abstract class DbQueryContext
{
    public static Aff<DbQueryRuntime, CancellationToken> token() =>
        Eff<DbQueryRuntime, CancellationToken>(rt => rt.CancellationToken);

    public static Aff<DbQueryRuntime, DbConnection> connection() =>
        Eff<DbQueryRuntime, DbConnection>(rt => rt.Connection);

    public static Aff<DbQueryRuntime, Option<DbTransaction>> transaction() =>
        Eff<DbQueryRuntime, Option<DbTransaction>>(rt => rt.Transaction);
}
