using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.RestServiceRoutenator.Reflection
{
    public class TypeScanConventionBase
    {
        internal IList<Mono.Cecil.TypeDefinition> FoundTypes = new List<Mono.Cecil.TypeDefinition>();
        internal IList<Mono.Cecil.TypeDefinition> FoundReturnTypes = new List<Mono.Cecil.TypeDefinition>();
        public string[] AdditionalData { get; set; }

        internal string ParseType(string typeString)
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

        internal bool HasInterface(Mono.Cecil.TypeDefinition type, string interfaceFullName)
        {
            bool result = type.Interfaces.Any(x => x.Name == interfaceFullName || HasInterface(x.Resolve(), interfaceFullName)) || (type.BaseType != null && HasInterface(type.BaseType.Resolve(), interfaceFullName));
            return result;
        }
    }
}