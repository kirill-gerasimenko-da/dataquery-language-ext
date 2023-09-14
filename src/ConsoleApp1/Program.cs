// See https://aka.ms/new-console-template for more information

global using static LanguageExt.Prelude;
global using LanguageExt;
global using DataQuery.LanguageExt.Sql.NormNet;
global using static DataQuery.LanguageExt.Sql.NormNet.DataQueryNormNet;
using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheUtils.DependencyInjection;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(services =>
{
    services.AddGetUserFunction(ServiceLifetime.Singleton);
});

var app = builder.Build();

var getUser = app.Services.GetService<GetUserUnsafe>();

await getUser();

await app.RunAsync();