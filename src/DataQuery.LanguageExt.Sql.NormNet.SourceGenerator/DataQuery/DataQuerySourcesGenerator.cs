namespace DataQuery.LanguageExt.Sql.NormNet.SourceGenerator.DataQuery;

using System;

public static class DataQuerySourcesGenerator
{
    public static string GenerateDelegates(DataQueryMetadata meta) => meta switch
    {
        { ReturnIsTask: true } => DataQuerySourcesGeneratorTask.GenerateAff(
            new DataQuerySourcesGeneratorTask.DataQueryTask
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