// ReSharper disable MemberCanBePrivate.Global

namespace DataQuery.LanguageExt;

using System.Data.Common;
using System.Threading;

public readonly struct DbQueryRuntimeEnv
{
    public readonly CancellationTokenSource Source;
    public readonly CancellationToken Token;
    public readonly DbConnection Connection;
    public readonly Option<DbTransaction> Transaction;

    public DbQueryRuntimeEnv(
        CancellationTokenSource source,
        CancellationToken token,
        DbConnection connection,
        Option<DbTransaction> transaction
    )
    {
        Source = source;
        Token = token;
        Connection = connection;
        Transaction = transaction;
    }

    private DbQueryRuntimeEnv(
        CancellationTokenSource source,
        DbConnection connection,
        Option<DbTransaction> transaction
    )
        : this(source, source.Token, connection, transaction) { }

    public DbQueryRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Connection, None);
}
