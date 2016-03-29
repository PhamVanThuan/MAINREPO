using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.Capitec.CSJsonifier.Reflection
{
    public class TypeScanConventionBase
    {
        internal IList<Mono.Cecil.TypeDefinition> commandTypes = new List<Mono.Cecil.TypeDefinition>();
        internal IList<Mono.Cecil.TypeDefinition> commandResultTypes = new List<Mono.Cecil.TypeDefinition>();

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