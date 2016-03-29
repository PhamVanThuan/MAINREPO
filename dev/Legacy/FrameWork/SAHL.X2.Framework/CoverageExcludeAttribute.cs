using System;

namespace SAHL.X2.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class CoverageExcludeAttribute : Attribute
    {
    }
}