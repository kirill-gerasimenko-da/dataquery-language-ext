// ReSharper disable MemberCanBePrivate.Global

namespace DataQuery.LanguageExt;

using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

public static class DbQueryRuntimeExtensions
{
    /// <summary>
    /// Runs the query using the specified connection. No transaction is created.
    /// </summary>
    public static async ValueTask<T> Run<T>(this Aff<DbQueryRuntime, T> query,
        DbConnection connection,
        CancellationToken token)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        var runtime = DbQueryRuntime.New(connection, None, token);
        var result = (await query.Run(runtime)).ThrowIfFail();

        return result;
    }

    /// <summary>
    /// Runs the query using the specified connection. No transaction is created.
    /// </summary>
    public static async ValueTask<T> Run<T>(this DbConnection connection,
        Aff<DbQueryRuntime, T> query,
        CancellationToken token
    )
        => await query.Run(connection, token);

    /// <summary>
    /// Runs the query using the specified connection and creating the transaction
    /// with specified isolation level. Created transaction is scoped
    /// to the method. Specified connection is not manipulated.
    /// </summary>
    public static async ValueTask<T> Run<T>(this Aff<DbQueryRuntime, T> query,
        DbConnection connection,
        IsolationLevel isolationLevel,
        CancellationToken token)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        if (!Enum.IsDefined(typeof(IsolationLevel), isolationLevel))
            throw new ArgumentOutOfRangeException(nameof(isolationLevel));

        await using var transaction = await connection.BeginTransactionAsync(isolationLevel, token);

        var runtime = DbQueryRuntime.New(connection, transaction, token);
        var result = (await query.Run(runtime)).ThrowIfFail();

        await transaction.CommitAsync(token);

        return result;
    }

    /// <summary>
    /// Runs the query using the specified connection and creating the transaction
    /// with specified isolation level. Created transaction is scoped
    /// to the method. Specified connection is not manipulated.
    /// </summary>
    public static async ValueTask<T> Run<T>(this DbConnection connection,
        Aff<DbQueryRuntime, T> query,
        IsolationLevel isolationLevel,
        CancellationToken token
    )
        => await query.Run(connection, isolationLevel, token);

    /// <summary>
    /// Runs the query using the specified connection and transaction objects.
    /// Specified connection and transaction are not manipulated.
    /// </summary>
    public static async ValueTask<T> Run<T>(this Aff<DbQueryRuntime, T> query,
        DbConnection connection,
        DbTransaction transaction,
        CancellationToken token)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        var runtime = DbQueryRuntime.New(connection, transaction, token);
        var result = await query.Run(runtime);

        return result.ThrowIfFail();
    }

    /// <summary>
    /// Runs the query using the specified connection and transaction objects.
    /// Specified connection and transaction are not manipulated.
    /// </summary>
    public static async ValueTask<T> Run<T>(this DbConnection connection,
        DbTransaction transaction,
        Aff<DbQueryRuntime, T> query,
        CancellationToken token
    )
        => await query.Run(connection, transaction, token);
}