// See https://aka.ms/new-console-template for more information

global using LanguageExt;
global using DataQuery.LanguageExt;
using System.Data;
using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using TheUtils.DependencyInjection;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(services => { services.AddGetUserQuery().AddGetUserNormQuery(); });

var app = builder.Build();

var query = app.Services.GetService<GetUserQuery>();
var queryNorm = app.Services.GetService<GetUserNormQuery>();

await using var conn =
    new NpgsqlConnection(
        "Port=5432;Host=localhost;Username=postgres;Password=gv9f0IeQ6B;Database=postgres;Pooling=True;SSL Mode=Disable;Include Error Detail=True");

var qqq =
    from _1 in  query("", 500)
    from _2 in queryNorm(600)
    select _1 + _2;

await query("", 500).Run(conn, IsolationLevel.ReadCommitted, default);

var results = await qqq.Run(conn, default);

Console.WriteLine(results);

await app.RunAsync();