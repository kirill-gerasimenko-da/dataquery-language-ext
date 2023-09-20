// See https://aka.ms/new-console-template for more information

global using LanguageExt;
global using DataQuery.LanguageExt;
using System.Data;
using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using TheUtils.DependencyInjection;
using static DataQuery.LanguageExt.NormNet.DbQuery;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(services => { services
    .AddGetUserQuery()
    .AddGetUserNormQuery()
    .AddGetUsersCombinedQuery(); });

var app = builder.Build();

var getUserQuery = app.Services.GetService<GetUserQuery>();
var getUserCombinedQuery = app.Services.GetService<GetUsersCombinedQuery>();
var getUserNormQuery = app.Services.GetService<GetUserNormQuery>();

await using var conn =
    new NpgsqlConnection(
        "Port=5432;Host=localhost;Username=postgres;Password=gv9f0IeQ6B;Database=postgres;Pooling=True;SSL Mode=Disable;Include Error Detail=True");

var qqq =
    from _1 in getUserQuery("", 500)
    from _2 in getUserNormQuery(600)
    from _3 in query((norm, token) => norm
        .ReadAsync<int>("select 200")
        .SingleAsync(token))
    from _4 in getUserCombinedQuery()
    select _1 + _2 + _3 + _4;

var results = await qqq.Run(conn, default);

await getUserQuery("", 500).Run(conn, IsolationLevel.ReadCommitted, default);

Console.WriteLine(results);


await app.RunAsync();