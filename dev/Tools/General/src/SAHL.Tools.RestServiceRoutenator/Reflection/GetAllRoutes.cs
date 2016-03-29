using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator.Reflection
{
    public static class GetAllRoutes
    {
        public static IEnumerable<string> GetAllRoutesForGivenTypes(this IEnumerable<TypeDefinition> typesFound)
        {
            return typesFound.SelectMany(theType => theType.Methods.Where(method => method.RouteableMethod()))
                .Select(
                    method =>
                        method.CustomAttributes.First(
                            attribute => attribute.AttributeType.Name == "RepresentationRouteAttribute")
                            .ConstructorArguments.First()
                            .Value.ToString());
        }
    }

}