namespace DataQuery.LanguageExt.Sql;

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Npgsql;

public static partial class DataQuerySql
{
    public interface DriverType
    {
        record AdoNet : DriverType;
        record Npgsql(Action<NpgsqlCommand> onCommandCreated = null) : DriverType;
    }

    /// <summary>
    /// Creates new SQL database client. Supports Npgsql and AdoNet drivers.
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    /// <param name="driverType">Driver type see <see cref="DriverType"/>. If not provided - defaults to Npgsql.</param>
    /// <returns></returns>
    public static ISqlDatabase CreateSqlDatabaseClient
    (
        string connectionString,
        DriverType driverType = null
    ) =>
        new SqlDatabase(new SqlQueryRunner(
            toConnection(driverType, connectionString), DefaultRT.New));

    static SqlConnectionFactory toConnection(DriverType driverType, string connectionString) => driverType switch
    {
        DriverType.Npgsql {onCommandCreated: { } cmd} =>
            () => new NpgsqlCustomConnection(new NpgsqlConnection(connectionString), cmd),

        DriverType.Npgsql {onCommandCreated: null} =>
            () => new NpgsqlConnection(connectionString),

        DriverType.AdoNet =>
            () => new SqlConnection(connectionString),

        null =>
            () => new NpgsqlConnection(connectionString),

        _ => throw new ArgumentOutOfRangeException(nameof(driverType), driverType, null)
    };

    class NpgsqlCustomConnection : DbConnection, ICloneable
    {
        readonly NpgsqlConnection _connection;
        readonly Action<NpgsqlCommand> _customizeCommand;

        public NpgsqlCustomConnection(NpgsqlConnection connection, Action<NpgsqlCommand> customizeCommand)
        {
            _connection = connection;
            _customizeCommand = customizeCommand;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) =>
            _connection.BeginTransaction(isolationLevel);

        public override void ChangeDatabase(string databaseName) =>
            _connection.ChangeDatabase(databaseName);

        public override void Close() => _connection.Close();

        public override void Open() => _connection.Open();

        public override string ConnectionString
        {
            get => _connection.ConnectionString;
            set => _connection.ConnectionString = value;
        }

        public override string Database => _connection.Database;

        public override ConnectionState State => _connection.State;

        public override string DataSource => _connection.DataSource;

        public override string ServerVersion => _connection.ServerVersion;

        protected override DbCommand CreateDbCommand()
        {
            var cmd = _connection.CreateCommand();

            _customizeCommand(cmd);

            return cmd;
        }

        public object Clone() => ((ICloneable) _connection).Clone();
    }
}