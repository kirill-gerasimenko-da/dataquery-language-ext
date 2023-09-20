// See https://aka.ms/new-console-template for more information

global using LanguageExt;
global using DataQuery.LanguageExt;
using System.Data;
using ConsoleApp1;
using static DataQuery.LanguageExt.DbQuery;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Norm;
using Npgsql;
using TheUtils;
using TheUtils.DependencyInjection;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(services => { services.AddGetUserQuery().AddGetUserNormQuery(); });

var app = builder.Build();

var getUserQuery = app.Services.GetService<GetUserQuery>();
var getUserNormQuery = app.Services.GetService<GetUserNormQuery>();

await using var conn =
    new NpgsqlConnection(
        "Port=5432;Host=localhost;Username=postgres;Password=gv9f0IeQ6B;Database=postgres;Pooling=True;SSL Mode=Disable;Include Error Detail=True");

var qqq =
    from _1 in getUserQuery("", 500)
    from _2 in getUserNormQuery(600)
    from _3 in query(norm => norm
        .ReadAsync<int>("select 200")
        .SingleAsync(token))
    select _1 + _2 + _3;

var results = await qqq.Run(conn, default);

await getUserQuery("", 500).Run(conn, IsolationLevel.ReadCommitted, default);

Console.WriteLine(results);

await app.RunAsync();