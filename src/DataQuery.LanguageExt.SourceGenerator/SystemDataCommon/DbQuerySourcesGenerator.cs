namespace DataQuery.LanguageExt.SourceGenerator.SystemDataCommon;

using System;

public static class DbQuerySourcesGenerator
{
    public static string GenerateDelegates(DataQueryMetadata meta) => meta switch
    {
        { ReturnIsTask: true } => DbQuerySourcesGeneratorTask.GenerateAff(
            new DbQuerySourcesGeneratorTask.DataQueryTask
            {
                FuncName = meta.FuncName,
                Parameters = meta.Parameters,
                NamespaceName = meta.NamespaceName,
                ParentClassName = meta.ParentClassName,
                ParentClassIsStatic = meta.ParentClassIsStatic,
                ReturnSubTypeName = meta.ReturnSubTypeName
            }
        ),
        _ => throw new ArgumentOutOfRangeException(nameof(meta))
    };
}