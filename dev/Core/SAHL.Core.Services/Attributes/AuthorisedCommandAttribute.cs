using System;
using System.Linq;

namespace SAHL.Core.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorisedCommandAttribute : Attribute
    {
        private string roles;

        public string[] SplitRoles { get; private set; }

        public string Roles
        {
            get
            {
                return roles;
            }
            set
            {
                roles = value ?? string.Empty;
                SplitRoles = SplitString(roles);
            }
        }

        private static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            var stringArray = original.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            return stringArray.Select(x => x.Trim()).ToArray();
        }
    }
}