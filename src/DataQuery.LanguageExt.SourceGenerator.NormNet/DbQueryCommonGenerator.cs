namespace DataQuery.LanguageExt.SourceGenerator.NormNet;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
[SuppressMessage(
    "MicrosoftCodeAnalysisCorrectness",
    "RS1036:Specify analyzer banned API enforcement setting"
)]
public class DbQueryCommonGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.CompilationProvider,
            static (spc, _) =>
            {
                spc.AddSource(
                    "DataQuery.LanguageExt.NormNet.Common.g.cs",
                    SourceText.From(ContentInlines, Encoding.UTF8)
                );
                spc.AddSource(
                    "DataQuery.LanguageExt.NormNet.Common.g.cs",
                    SourceText.From(ContentCommon, Encoding.UTF8)
                );
            }
        );
    }


    static readonly string ContentCommon = @"<auto-generated />
#pragma warning disable

using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using System.Runtime.CompilerServices;

namespace TheUtils.DependencyInjection
{
    using Unit = LanguageExt.Unit;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static partial class ServiceCollectionFunctionExtensions
    {
        public static async ValueTask<Unit> ToUnit(this ValueTask<Unit> source)
        {
            await source.ConfigureAwait(false);
            return Prelude.unit;
        }
    }
}
";


    static readonly string ContentInlines =
        @"<auto-generated />
#pragma warning disable
namespace DataQuery.LanguageExt.NormNet;

using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Norm;
using global::LanguageExt;
using static global::LanguageExt.Prelude;

public static class DbQueryInline
{
    public static Aff<DbQueryRuntime, T> query<T>(Func<Norm, CancellationToken, ValueTask<T>> query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        return Aff<DbQueryRuntime, T>(async rt => await query(
            rt.Connection
                .WithTransaction(rt.Transaction.IfNoneUnsafe((DbTransaction) null))
                .WithCancellationToken(rt.CancellationToken),
            rt.CancellationToken));
    }

    public static Aff<DbQueryRuntime, T> transform<T>(Func<Aff<DbQueryRuntime, T>> aff) =>
        Eff(aff).Bind(identity);
}
";
}