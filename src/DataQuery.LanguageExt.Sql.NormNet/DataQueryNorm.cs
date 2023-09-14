namespace DataQuery.LanguageExt.Sql.NormNet;

using System;
using System.Data.Common;
using System.Threading;
using global::LanguageExt.Effects.Traits;

public static class DataQueryNorm
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataQueryAttribute : Attribute
    { }

    public abstract class DataQuery
    {
        public static CancellationToken Token => default(DatabaseQueryRuntime).CancellationToken;
        public static DbConnection Connection => default(DatabaseQueryRuntime).Connection;
    }

    public interface HasDbConnection<out RT> : HasCancel<RT>
        where RT : struct, HasDbConnection<RT>
    {
        public DbConnection Connection { get; }
    }

    public readonly struct DatabaseQueryRuntime : HasDbConnection<DatabaseQueryRuntime>
    {
        private DatabaseQueryRuntime(DatabaseQueryRuntimeEnv env) => Env = env;

        public static DatabaseQueryRuntime New(DbConnection cnn, CancellationToken cancelToken) =>
            new(new DatabaseQueryRuntimeEnv(new CancellationTokenSource(), cancelToken, cnn));

        private DatabaseQueryRuntimeEnv Env { get; }

        public DatabaseQueryRuntime LocalCancel => new(Env.LocalCancel);
        public CancellationToken CancellationToken => Env.Token;
        public CancellationTokenSource CancellationTokenSource => Env.Source;
        public DbConnection Connection => Env.Connection;
    }

    public readonly struct DatabaseQueryRuntimeEnv
    {
        public readonly CancellationTokenSource Source;
        public readonly CancellationToken Token;
        public readonly DbConnection Connection;

        public DatabaseQueryRuntimeEnv(
            CancellationTokenSource source,
            CancellationToken token,
            DbConnection connection)
        {
            Source = source;
            Token = token;
            Connection = connection;
        }

        private DatabaseQueryRuntimeEnv(CancellationTokenSource source, DbConnection connection)
            : this(source, source.Token, connection)
        { }

        public DatabaseQueryRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Connection);
    }
}