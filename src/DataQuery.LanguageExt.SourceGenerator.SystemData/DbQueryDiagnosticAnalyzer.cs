namespace DataQuery.LanguageExt.SourceGenerator.SystemData;

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DbQueryDiagnosticAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var classDeclarationSyntax = context.Node
            is ClassDeclarationSyntax { AttributeLists.Count: > 0 } node
            ? node
            : null;

        if (classDeclarationSyntax == null)
            return;

        var hasFunctionAttribute = false;

        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (
                context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol
                is not IMethodSymbol attributeSymbol
            )
                continue;

            var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            var fullName = attributeContainingTypeSymbol.ToDisplayString();

            if (fullName == "DataQuery.LanguageExt.SystemData.DatabaseQueryAttribute")
            {
                hasFunctionAttribute = true;
                break;
            }
        }

        if (!hasFunctionAttribute)
            return;

        var semanticModel = context.SemanticModel;
        if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not { } classSymbol)
            return;

        var hasInvokeMethod = false;

        var members = classSymbol.GetMembers();
        foreach (var m in members)
        {
            if (
                m is IMethodSymbol msr
                && msr.MethodKind == MethodKind.Ordinary
                && msr.IsStatic == false
                && msr.DeclaredAccessibility == Accessibility.Public
            )
            {
                if (msr.Name == "Invoke")
                {
                    var hasNormParameter = msr.Parameters.Any(dbConn =>
                        dbConn.Type.ToDisplayString() == "System.Data.Common.DbConnection"
                    );
                    var hasValidReturnType =
                        msr.ReturnType.MetadataName
                            is "ValueTask`1"
                                or "ValueTask"
                                or "Task`1"
                                or "Task";

                    var hasAffWithRtReturnType = msr.ReturnType.MetadataName == "Aff`2";

                    var nts = msr.ReturnType as INamedTypeSymbol;
                    var isRt =
                        nts?.TypeArguments.First().ToDisplayString()
                        == "DataQuery.LanguageExt.DbQueryRuntime";

                    if (
                        (hasNormParameter && hasValidReturnType) || (hasAffWithRtReturnType && isRt)
                    )
                        hasInvokeMethod = true;

                    break;
                }
            }
        }

        if (!hasInvokeMethod)
            context.ReportDiagnostic(
                Diagnostic.Create(
                    DbQueryGenerator.NoInvokeMethodFound,
                    classSymbol.Locations.FirstOrDefault(),
                    classSymbol.Name
                )
            );
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DbQueryGenerator.NoInvokeMethodFound);
}
