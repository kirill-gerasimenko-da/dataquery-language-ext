namespace DataQuery.LanguageExt.SourceGenerator;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1036:Specify analyzer banned API enforcement setting")]
public class DbNormNetQueryCommonGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider,
            static (spc, _) =>
            {
                spc.AddSource("DataQuery.LanguageExt.NormNet.Common.g.cs", SourceText.From(Content, Encoding.UTF8));
            });
    }

    static readonly string Content = @"
namespace DataQuery.LanguageExt.NormNet;

using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Norm;

public static class DbQuery
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
}
";

}