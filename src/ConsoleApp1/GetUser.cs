namespace ConsoleApp1;

using System.Data.Common;
using System.Text;
using Dapper;
using DataQuery.LanguageExt.NormNet;
using Norm;
using TheUtils;
using static DataQuery.LanguageExt.NormNet.DbQueryInline;

// [DataQuery.LanguageExt.SystemData.DbQuery]
// public class GetUser
// {
//     public async Task<int> Invoke(
//         string y,
//         int value,
//         DbConnection conn,
//         Option<DbTransaction> tran,
//         CancellationToken token
//     ) =>
//         await conn.WithTransaction(tran.IfNoneDefault())
//             .WithCancellationToken(token)
//             .ReadAsync<int>("Select @input + 1", new { input = value })
//             .SingleAsync(cancellationToken: token);
// }
//
// [DataQuery.LanguageExt.SystemData.DbQuery]
// public class GetUserDapper
// {
//     public async Task<int> Invoke(int value, DbConnection conn, Option<DbTransaction> tran) =>
//         await conn.QuerySingleAsync<int>(
//             "Select @input",
//             new { input = value },
//             transaction: tran.IfNoneDefault()
//         );
// }
//
// [DataQuery.LanguageExt.SystemData.DbQuery]
// public class GetUsersCombinedSystemD
// {
//     readonly GetUserQuery _getUser;
//     readonly GetUserNormQuery _getUserNorm;
//
//     public GetUsersCombinedSystemD(GetUserQuery getUser, GetUserNormQuery getUserNorm)
//     {
//         _getUser = getUser;
//         _getUserNorm = getUserNorm;
//     }
//
//     public Aff<DbQueryRuntime, int> Invoke(int x1, int x2) =>
//         from _1 in _getUser("", x1)
//         from _2 in _getUserNorm(x2)
//         from _3 in query((norm, token) => norm.ReadAsync<int>("select 200").SingleAsync(token))
//         select _1 + _2;
// }
//
// [DbQuery]
// public class GetUserNorm
// {
//     public async Task<int> Invoke(int value, Norm norm, CancellationToken token) =>
//         await norm.ReadAsync<int>("Select @input + 2", new { input = value }).SingleAsync(token);
// }
//
// [DbQuery]
// public class GetUsersCombined
// {
//     readonly GetUserQuery _getUser;
//     readonly GetUserNormQuery _getUserNorm;
//
//     public GetUsersCombined(GetUserQuery getUser, GetUserNormQuery getUserNorm)
//     {
//         _getUser = getUser;
//         _getUserNorm = getUserNorm;
//     }
//
//     public Aff<DbQueryRuntime, int> Invoke(int x1, int x2) =>
//         from _1 in _getUser("", x1)
//         from _2 in _getUserNorm(x2)
//         from _3 in query((norm, token) => norm.ReadAsync<int>("select 200").SingleAsync(token))
//         select _1 + _2;
// }


public enum TENUM
{
    val = 1
}

[DatabaseQuery]
public class GetUserNorm
{
     // GetUserNormQuery(
     //     Option<int> x /*default*/,
     //     string test /*hello/value-as-string*/,
     //     ConsoleApp1.GetUserNorm o /*default*/,
     //     object obj /*default*/,
     //     double zz /*0/value-as-string*/,
     //     int yy /*400/value-as-string*/,
     //     int hh /*0/value-as-string*/,
     //     ConsoleApp1.TENUM tenum /*0/value-as-string*/);
     //
//     public Aff<DbQueryRuntime, Unit> Invoke(Norm norm, CancellationToken token,
    public Task Invoke(Norm norm, CancellationToken token,
        string uu = default,
        string uv = null,
        Option<int> x = default,
        Encoder enc = default,
        string test = "hello",
        GetUserNorm o = default,
        object obj = null,
        double zz = default,
        int yy = 400,
        int hh = default,
        TENUM tenum = default)
    {

        var y = (string s = "")=> {};

        GetUserNormQuery q = default;
        q();

        return default;
    }

}