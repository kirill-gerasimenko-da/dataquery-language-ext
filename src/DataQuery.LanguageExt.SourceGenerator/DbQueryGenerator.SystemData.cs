namespace DataQuery.LanguageExt.SourceGenerator;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1036:Specify analyzer banned API enforcement setting")]
public class DbSystemDataQueryCommonGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider,
            static (spc, _) =>
            {
                spc.AddSource("DataQuery.LanguageExt.SystemData.Common.g.cs", SourceText.From(Content, Encoding.UTF8));
            });
    }

    static readonly string Content = @"
namespace DataQuery.LanguageExt.SystemData;

using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Norm;
using global::LanguageExt;
using static global::LanguageExt.Prelude;

public static class DbQuery
{
    public static Aff<DbQueryRuntime, T> query<T>(Func<DbConnection, CancellationToken, ValueTask<T>> query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        return Aff<DbQueryRuntime, T>(async rt => await query(rt.Connection, rt.CancellationToken));
    }

    public static Aff<DbQueryRuntime, T> query<T>(
        Func<
            DbConnection,
            Option<DbTransaction>,
            CancellationToken,
            ValueTask<T>
        > query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        return Aff<DbQueryRuntime, T>(async rt => await query(rt.Connection, rt.Transaction, rt.CancellationToken));
    }

    public static Aff<DbQueryRuntime, T> transform<T>(Func<Aff<DbQueryRuntime, T>> aff) =>
        Eff(aff).Bind(identity);
}
";

}