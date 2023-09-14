namespace DataQuery.LanguageExt.Sql.NormNet.SourceGenerator.DataQuery;

using System;

public static class FunctionSourcesGenerator
{
    public static string GenerateDelegates(FuncMetadata meta) => meta switch
    {
        { ReturnIsTask: true } => FunctionSourcesGeneratorTask.GenerateAff(
            new FunctionSourcesGeneratorTask.FuncTask
            {
                FuncName = meta.FuncName,
                Parameters = meta.Parameters,
                NamespaceName = meta.NamespaceName,
                ParentClassName = meta.ParentClassName,
                ParentClassIsStatic = meta.ParentClassIsStatic,
                ReturnSubTypeName = meta.ReturnSubTypeName
            }
        ),
        { ReturnIsAff: true } => FunctionSourcesGeneratorAff.GenerateAff(
            new FunctionSourcesGeneratorAff.FuncAff
            {
                FuncName = meta.FuncName,
                Parameters = meta.Parameters,
                NamespaceName = meta.NamespaceName,
                ParentClassName = meta.ParentClassName,
                ParentClassIsStatic = meta.ParentClassIsStatic,
                ReturnSubTypeName = meta.ReturnSubTypeName
            }),
        { ReturnIsEff: true } => FunctionSourcesGeneratorEff.GenerateAff(
            new FunctionSourcesGeneratorEff.FuncEff
            {
                FuncName = meta.FuncName,
                Parameters = meta.Parameters,
                NamespaceName = meta.NamespaceName,
                ParentClassName = meta.ParentClassName,
                ParentClassIsStatic = meta.ParentClassIsStatic,
                ReturnSubTypeName = meta.ReturnSubTypeName
            }),
        _ => throw new ArgumentOutOfRangeException(nameof(meta))
    };
}