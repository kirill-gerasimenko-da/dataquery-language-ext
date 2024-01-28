using System;

namespace DataQuery.LanguageExt.NormNet
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseQueryAttribute : Attribute { }
}

namespace DataQuery.LanguageExt.SystemData
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseQueryAttribute : Attribute { }
}
