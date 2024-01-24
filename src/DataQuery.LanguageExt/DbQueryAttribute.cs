using System;

namespace DataQuery.LanguageExt.NormNet
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbQueryAttribute : Attribute { }
}

namespace DataQuery.LanguageExt.SystemData
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbQueryAttribute : Attribute { }
}
