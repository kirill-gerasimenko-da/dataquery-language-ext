using System;
using Dapper;
using LanguageExt;

namespace DataQuery.LanguageExt.Sql
{
    using static Prelude;
    using static SqlMapper;

    public static partial class DataQuerySql
    {
        public interface ISqlGridReader : IDisposable
        {
            Aff<Seq<T>> Read<T>(bool buffered = true);
        }

        public class SqlGridReader : ISqlGridReader
        {
            readonly GridReader _reader;

            public SqlGridReader(GridReader gridReader) => _reader = gridReader;

            public void Dispose() => _reader.Dispose();

            public Aff<Seq<T>> Read<T>(bool buffered) => Aff(async () => toSeq(await _reader.ReadAsync<T>(buffered)));
        }
    }
}