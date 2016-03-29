using System;

namespace SAHL.Core.Services.Extensions
{
    public static class CommandExtensions
    {
        public static bool IsReadOnlyCommand(this Type type)
        {
            return type.Name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}