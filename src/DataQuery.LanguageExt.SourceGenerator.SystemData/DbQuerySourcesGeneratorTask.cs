namespace DataQuery.LanguageExt.SourceGenerator.SystemData;

using System.Collections.Generic;
using System.Linq;

public static class DbQuerySourcesGeneratorTask
{
    public class DataQueryTask
    {
        public string FuncName { get; set; }
        public string NamespaceName { get; set; }
        public string ParentClassName { get; set; }
        public bool ParentClassIsStatic { get; set; }
        public string ReturnSubTypeName { get; set; }

        public List<InputParameter> Parameters { get; set; } = new();
    }

    public static string GenerateAff(DataQueryTask meta)
    {
        var outerClassBegin =
            meta.ParentClassName != null
                ? $@"public {(meta.ParentClassIsStatic ? "static" : "")} partial class {meta.ParentClassName}
    {{
"
                : "";

        var outerClassEnd = meta.ParentClassName != null ? "}" : "";

        var parentClassPrefix = meta.ParentClassName != null ? $"{meta.ParentClassName}." : "";

        var inputParams = string.Join(
            ", ",
            meta.Parameters.Where(p =>
                p.TypeName != "System.Data.Common.DbConnection"
                && p.TypeName != "Option<System.Data.Common.DbTransaction>"
                && p.TypeName != "CancellationToken"
            )
                .Select(p =>
                    $"{p.TypeName} {char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)}"
                )
        );

        var inputTypes = string.Join(", ", meta.Parameters.Select(p => p.TypeName));

        var inputAsLambdaParams = string.Join(
            ", ",
            meta.Parameters.Where(p =>
                p.TypeName != "System.Data.Common.DbConnection"
                && p.TypeName != "Option<System.Data.Common.DbTransaction>"
                && p.TypeName != "CancellationToken"
            )
                .Select(p => $"{char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)}")
        );

        var inputAsInvokeParams = string.Join(
            ", ",
            meta.Parameters.Select(p =>
            {
                if (p.TypeName == "System.Data.Common.DbConnection")
                    return "___y.___conn";
                if (p.TypeName == "CancellationToken")
                    return "___y.___token";
                if (p.TypeName == "Option<System.Data.Common.DbTransaction>")
                    return "___y.___trans";

                return $"{char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)}";
            })
        );

        return @$"
#pragma warning disable CS0105

using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using System.Runtime.CompilerServices;

namespace {meta.NamespaceName}
{{
    using Unit = LanguageExt.Unit;

    {outerClassBegin}
    public delegate Aff
    <
        DataQuery.LanguageExt.DbQueryRuntime,
        {meta.ReturnSubTypeName}
    > {meta.FuncName}Query({inputParams});
    {outerClassEnd}
}}

namespace TheUtils.DependencyInjection
{{
    using Unit = LanguageExt.Unit;
    using {meta.NamespaceName};
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static partial class ServiceCollectionFunctionExtensions
    {{
        public static IServiceCollection Add{meta.FuncName}Query
        (
            this IServiceCollection services,
            ServiceLifetime lifetime
        )
        {{
            services.Add(new(
                serviceType: typeof({parentClassPrefix}{meta.FuncName}),
                implementationType: typeof({parentClassPrefix}{meta.FuncName}),
                lifetime));

            services.Add(new(
                serviceType: typeof({parentClassPrefix}{meta.FuncName}Query),
                factory: ___x => new {parentClassPrefix}{meta.FuncName}Query(
                    ({inputAsLambdaParams}) => (
                         from ___conn in DataQuery.LanguageExt.DbQueryContext.connection()
                         from ___token in DataQuery.LanguageExt.DbQueryContext.token()
                         from ___trans in DataQuery.LanguageExt.DbQueryContext.transaction()
                         select (___conn, ___token, ___trans)
                        ).MapAsync(async ___y =>
                            await ___x.GetRequiredService<{parentClassPrefix}{meta.FuncName}>()
                                .Invoke({inputAsInvokeParams}))),
                lifetime));

            return services;
        }}       
    }}
}}
";
    }
}