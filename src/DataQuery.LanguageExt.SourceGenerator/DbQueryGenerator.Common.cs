namespace DataQuery.LanguageExt.SourceGenerator;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1036:Specify analyzer banned API enforcement setting")]
public class DbCommonGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider,
            static (spc, _) =>
            {
                spc.AddSource("DataQuery.LanguageExt.Common.g.cs", SourceText.From(Content, Encoding.UTF8));
            });
    }

    static readonly string Content = @"
namespace DataQuery.LanguageExt;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using global::LanguageExt;
using static global::LanguageExt.Prelude;

public static class ServiceCollectionExtensions 
{
    /// <summary>
    /// Registers all db queries in the DI.
    /// </summary>
    public static IServiceCollection AddAllFunctions(this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        bool IsNormQuery(ICustomAttributeProvider t) =>
            toSeq(t.GetCustomAttributes(typeof(NormNet.DbQueryAttribute), false))
                .Cast<NormNet.DbQueryAttribute>()
                .HeadOrNone()
                .IsSome;
        bool IsSystemDataQuery(ICustomAttributeProvider t) =>
            toSeq(t.GetCustomAttributes(typeof(SystemData.DbQueryAttribute), false))
                .Cast<SystemData.DbQueryAttribute>()
                .HeadOrNone()
                .IsSome;
        var types = toSeq(assemblies).Bind(a => toSeq(a.GetTypes()));
        var queryTypes = types.Filter(t => IsNormQuery(t) || IsSystemDataQuery(t));
        if (queryTypes == Empty)
            return services;
        var extensionsType = Assembly.GetCallingAssembly()
            .GetType(""TheUtils.DependencyInjection.ServiceCollectionFunctionExtensions"");
        queryTypes
            .Map(t => extensionsType?.GetMethod($""Add{t.Name}Query""))
            .Filter(notnull)
            .Iter(m => m.Invoke(null, new object[] { services, lifetime }));
        return services;
    }
}";

}