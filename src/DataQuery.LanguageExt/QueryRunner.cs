using System.Runtime.Serialization;
using System.Threading;
using LanguageExt;
using LanguageExt.TypeClasses;
using Npgsql;

namespace Dataquery.LanguageExt
{
    using static Prelude;

    public static partial class DataQuery
    {
        public interface IQueryRunner
        {
            /// <summary>
            /// Creates an effect, which runs the query with DatabaseRuntime.
            /// </summary>
            Aff<TResult> RunAff<TResult>(Aff<DatabaseRuntime, TResult> query, CancellationToken cancelToken);
        }

        public class QueryRunner : IQueryRunner
        {
            private readonly ConnectionString _connectionString;

            public QueryRunner(ConnectionString connectionString) => _connectionString = connectionString;

            public Aff<TResult> RunAff<TResult>(Aff<DatabaseRuntime, TResult> query, CancellationToken cancelToken) =>
                AffMaybe(async () =>
                {
                    await using var cnn = new NpgsqlConnection(_connectionString.Value);
                    await cnn.OpenAsync(cancelToken);

                    var trn = await cnn.BeginTransactionAsync(cancelToken);
                    var runtime = DatabaseRuntime.New(cnn, trn, cancelToken);

                    var finalQuery = query.MapFailAsync(async error =>
                    {
                        await trn.RollbackAsync(cancelToken);
                        return error;
                    });

                    var result = await finalQuery.Run(runtime);
                    if (result.IsSucc)
                        await trn.CommitAsync(cancelToken);

                    return result;
                });
        }

        /// <summary>
        /// Connection string, should be non empty string, otherwise fails
        /// </summary>
        public class ConnectionString : NewType<ConnectionString, string, ConnectionString.NotEmptyString>
        {
            public struct NotEmptyString : Pred<string>
            {
                public bool True(string value) => !string.IsNullOrEmpty(value);
            }

            public ConnectionString(string value) : base(value)
            { }

            public ConnectionString(SerializationInfo info, StreamingContext context) : base(info, context)
            { }
        }
    }
}