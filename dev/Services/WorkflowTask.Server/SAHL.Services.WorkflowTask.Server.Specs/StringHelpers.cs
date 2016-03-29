using System;
using System.Collections.Concurrent;

namespace SAHL.Services.WorkflowTask.Server.Specs
{
    internal static class StringHelpers
    {
        internal static Func<string, string> WorkflowItemNameTransformFunction;

        internal static string ToTransformedWorkFlowItemName(this string source)
        {
            return WorkflowItemNameTransformFunction(source);
        }

        internal static string ToFormatStringIfAny(this string columnHeader)
        {
            var indexOfPipe = columnHeader.IndexOf("|");
            if (indexOfPipe < 0)
            {
                return null;
            }
            if (indexOfPipe + 1 > columnHeader.Length)
            {
                return null;
            }
            var formatString = columnHeader.Substring(indexOfPipe + 1);
            return formatString.Length == 0 ? "{0}" : formatString;
        }

        internal static string ToColumnHeaderWithoutFormatString(this string columnHeader)
        {
            var indexOfPipe = columnHeader.IndexOf("|");
            if (indexOfPipe < 0)
            {
                return columnHeader;
            }
            return columnHeader.Substring(0, indexOfPipe);
        }
    }

    internal static class ExtentionMethods
    {
        internal static bool ContainsKeyValuePair(this ConcurrentDictionary<string, string> source, string key, string value)
        {
            string item;
            source.TryGetValue(key, out item);
            return item == value;
        }
    }
}