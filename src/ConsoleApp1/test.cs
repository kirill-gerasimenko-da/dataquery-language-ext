//
// #pragma warning disable CS0105
//
// using Norm;
// using System.Threading;
// using System.Threading.Tasks;
// using LanguageExt;
// using LanguageExt.Common;
// using static LanguageExt.Prelude;
// using static DataQuery.LanguageExt.Sql.NormNet.DataQueryNormNet.QueryContext;
// using TheUtils;
// using System.Runtime.CompilerServices;
//
// // TYPES: string, int, System.Data.Common.DbConnection, Option<System.Data.Common.DbTransaction>, CancellationToken
//
// namespace ConsoleApp1
// {
//     using Unit = LanguageExt.Unit;
//
//
//     public delegate Aff
//     <
//         DataQuery.LanguageExt.Sql.NormNet.DataQueryNormNet.QueryRuntime,
//         Unit
//     > GetUserQuery(string y, int value, Option<System.Data.Common.DbTransaction> tran);
//
// }
//
// namespace TheUtils.DependencyInjection
// {
//     using Unit = LanguageExt.Unit;
//     using ConsoleApp1;
//     using Microsoft.Extensions.DependencyInjection;
//     using System;
//     using System.Threading;
//     using System.Threading.Tasks;
//
//     public static partial class ServiceCollectionFunctionExtensions
//     {
//         public static IServiceCollection AddGetUserQuery
//         (
//             this IServiceCollection services,
//             ServiceLifetime lifetime
//         )
//         {
//             services.Add(new(
//                 serviceType: typeof(GetUser),
//                 implementationType: typeof(GetUser),
//                 lifetime));
//
//             services.Add(new(
//                 serviceType: typeof(GetUserQuery),
//                 factory: ___x => new GetUserQuery(
//                     (y, value) => (
//                          from ___conn in DataQuery.LanguageExt.Sql.NormNet.DataQueryNormNet.QueryContext.connection()
//                          from ___token in DataQuery.LanguageExt.Sql.NormNet.DataQueryNormNet.QueryContext.token()
//                          from ___trans in DataQuery.LanguageExt.Sql.NormNet.DataQueryNormNet.QueryContext.transaction()
//                          select (___conn, ___token)
//                         ).MapAsync(async ___y =>
//                             await ___x.GetRequiredService<GetUser>()
//                                 .Invoke(y, value, ___y.___conn, tran, ___y.___token))),
//                 lifetime));
//
//             return services;
//         }
//     }
// }