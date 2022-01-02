namespace DataQuery.LanguageExt.Sql;

using System;
using System.Collections.Generic;
using static Dapper.SqlMapper;

public static partial class DataQuerySql
{
    public interface ISqlGridReader : IDisposable
    {
        Aff<IEnumerable<T>> Read<T>();
        Aff<Seq<T>> ReadAll<T>();
        Aff<T> ReadFirst<T>();
        Aff<T> ReadSingle<T>();
        Aff<Option<T>> TryReadFirst<T>();
        Aff<Option<T>> TryReadSingle<T>();
    }

    public class SqlGridReader : ISqlGridReader
    {
        readonly GridReader _reader;

        public SqlGridReader(GridReader gridReader) => _reader = gridReader;

        public void Dispose() => _reader.Dispose();

        public Aff<IEnumerable<T>> Read<T>() =>
            Aff(async () => await _reader.ReadAsync<T>(buffered: false));

        public Aff<Seq<T>> ReadAll<T>() =>
            Aff(async () => await _reader.ReadAsync<T>(buffered: true))
                .Map(x => x.ToSeq().Strict());

        public Aff<T> ReadFirst<T>() =>
            Aff(async () => await _reader.ReadFirstAsync<T>());

        public Aff<T> ReadSingle<T>() =>
            Aff(async () => await _reader.ReadSingleAsync<T>());

        public Aff<Option<T>> TryReadFirst<T>() => Aff(async () =>
        {
            var results = await _reader.ReadAsync<T>(buffered: false);

            using var enumerator = results.GetEnumerator();

            return enumerator.MoveNext() ? Some(enumerator.Current) : None;
        });

        public Aff<Option<T>> TryReadSingle<T>() => Aff(async () =>
        {
            var results = await _reader.ReadAsync<T>(buffered: false);

            using var enumerator = results.GetEnumerator();

            if (!enumerator.MoveNext())
                return None;

            var result = Some(enumerator.Current);

            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return result;
        });
    }
}