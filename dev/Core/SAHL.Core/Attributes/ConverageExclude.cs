using System;

namespace SAHL.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class CoverageExcludeAttribute : Attribute
    {
    }
}