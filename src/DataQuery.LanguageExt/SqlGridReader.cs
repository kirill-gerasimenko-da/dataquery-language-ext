using System;
using Dapper;
using LanguageExt;

namespace Dataquery.LanguageExt
{
    using static Prelude;
    using static SqlMapper;

    public interface ISqlGridReader : IDisposable
    {
        Aff<Seq<T>> Read<T>();
    }

    public class SqlGridReader : ISqlGridReader
    {
        readonly GridReader _reader;

        public SqlGridReader(GridReader gridReader) => _reader = gridReader;

        public void Dispose() => _reader.Dispose();

        public Aff<Seq<T>> Read<T>() => Aff(async () => toSeq(await _reader.ReadAsync<T>()));
    }
}