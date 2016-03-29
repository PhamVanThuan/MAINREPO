using System;

namespace SAHL.Core.Services.Attributes
{
    /// <summary>
    /// This attribute is used by the CSJsonifier tool to ignore a command when creating the Java Script implementation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CSJsonifierIgnore : Attribute
    {
    }
}