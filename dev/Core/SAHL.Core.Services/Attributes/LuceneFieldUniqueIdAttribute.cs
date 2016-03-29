using System;

namespace SAHL.Core.Services.Attributes
{
    /// <summary>
    /// This attribute is used by the Lucene index builder
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LuceneFieldAnalyseAttribute : Attribute
    {
    }
}