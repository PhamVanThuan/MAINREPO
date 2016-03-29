using System.Linq;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    internal static class ConventionHelper
    {
        internal static string ParseType(string typeString)
        {
            if (typeString.EndsWith(">"))
            {
                int i = 1;
                string[] genericStrings = typeString.Split('<');
                i += genericStrings[1].Count(c => c == ',');
                return string.Format("{0}`{1}", genericStrings[0], i);
            }
            return typeString;
        }
    }
}