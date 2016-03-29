using System.Linq;
using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator
{
    public static class DesiredTypeHelper
    {
        public static bool RouteableMethod(this MethodDefinition checkingDefinition)
        {
            return checkingDefinition.CustomAttributes.Any(x => x.AttributeType.Name == "RepresentationRouteAttribute");
        }
    }
}