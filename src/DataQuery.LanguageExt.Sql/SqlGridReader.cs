namespace DataQuery.LanguageExt.Sql;

using System;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

public static partial class DataQuerySql
{
    public interface ISqlGridReader : IDisposable
    {
        ValueTask<Seq<T>> Read<T>();
        ValueTask<T> ReadFirst<T>();
        ValueTask<T> ReadSingle<T>();
        ValueTask<Option<T>> TryReadFirst<T>();
        ValueTask<Option<T>> TryReadSingle<T>();

        ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TReturn>(
            Func<TFirst, TSecond, TReturn> func,
            string splitOn = "id");

        ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TReturn>(
            Func<TFirst, TSecond, TThird, TReturn> func,
            string splitOn = "id");

        ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TReturn> func,
            string splitOn = "id");

        ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func,
            string splitOn = "id");

        ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> func,
            string splitOn = "id");

        ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> func,
            string splitOn = "id");

        ValueTask<Seq<TReturn>> Read<TReturn>(
            Type[] types,
            Func<object[], TReturn> map,
            string splitOn = "id");
    }

    public class SqlGridReader : ISqlGridReader
    {
        readonly GridReader _reader;

        public SqlGridReader(GridReader gridReader) => _reader = gridReader;

        public void Dispose() => _reader.Dispose();

        public async ValueTask<Seq<T>> Read<T>() => toSeq(await _reader.ReadAsync<T>(buffered: true));
        public async ValueTask<T> ReadFirst<T>() => await _reader.ReadFirstAsync<T>();
        public async ValueTask<T> ReadSingle<T>() => await _reader.ReadSingleAsync<T>();
        public async ValueTask<Option<T>> TryReadFirst<T>() => Optional(await _reader.ReadFirstOrDefaultAsync<T>());
        public async ValueTask<Option<T>> TryReadSingle<T>() => Optional(await _reader.ReadSingleOrDefaultAsync<T>());

        public ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TReturn>(
            Func<TFirst, TSecond, TReturn> func,
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(func, splitOn)));

        public ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TReturn>(
            Func<TFirst, TSecond, TThird, TReturn> func,
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(func, splitOn)));

        public ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TReturn> func,
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(func, splitOn)));

        public  ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, 
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(func, splitOn)));

        public ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> func,
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(func, splitOn)));

        public ValueTask<Seq<TReturn>> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> func,
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(func, splitOn)));

        public ValueTask<Seq<TReturn>> Read<TReturn>(
            Type[] types,
            Func<object[], TReturn> map,
            string splitOn = "id")
            =>
                ValueTask.FromResult(toSeq(_reader.Read(types, map, splitOn)));
    }
}