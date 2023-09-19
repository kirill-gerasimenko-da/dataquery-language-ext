namespace DataQuery.LanguageExt;

using System;

public static class SystemDataCommon
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbQueryAttribute : Attribute
    { }
}

public static class NormNet
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbQueryAttribute : Attribute
    { }
}