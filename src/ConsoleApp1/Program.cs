// See https://aka.ms/new-console-template for more information

global using LanguageExt;
global using DataQuery.LanguageExt;
using System.Data;
using System.Reflection;
using ConsoleApp1;
using DataQuery.LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using static DataQuery.LanguageExt.NormNet.DbQuery;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(services => services.AddAllFunctions(new[] {Assembly.GetExecutingAssembly()}));

var app = builder.Build();

var getUserQuery = app.Services.GetService<GetUserQuery>();
var getUserCombinedQuery = app.Services.GetService<GetUsersCombinedQuery>();
var getUserNormQuery = app.Services.GetService<GetUserNormQuery>();
var getSd = app.Services.GetService<GetUsersCombinedSystemDQuery>();
var getDapper = app.Services.GetService<GetUserDapperQuery>();

await using var conn =
    new NpgsqlConnection(
        "Port=5432;Host=localhost;Username=postgres;Password=gv9f0IeQ6B;Database=postgres;Pooling=True;SSL Mode=Disable;Include Error Detail=True");

var qqq =
    from _1 in getUserQuery("", 500)
    from _2 in getUserNormQuery(600)
    from _3 in query((norm, token) => norm
        .ReadAsync<int>("select 200")
        .SingleAsync(token))
    from _4 in getUserCombinedQuery(100, 200)
    from _5 in getSd(2, 8)
    from _6 in getDapper(1000)
    select _1 + _2 + _3 + _4 + _6;

var results = await qqq.Run(conn, default);

await getUserQuery("", 500).Run(conn, IsolationLevel.ReadCommitted, default);

Console.WriteLine(results);


await app.RunAsync();