using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using SAHL.Services.Query;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.Metadata;

namespace SAHL.Config.Services.Query.Server
{
    public class RepresentationRouteAttributeScanner
    {
        private readonly string assemblyToScan;

        public RepresentationRouteAttributeScanner(string assemblyToScan)
        {
            this.assemblyToScan = assemblyToScan;
        }

        public IEnumerable<LinkMetadata> GetLinks()
        {
            var result = new List<LinkMetadata>();

            var methodsToProcess = GetMethods();

            foreach (var item in methodsToProcess)
            {
                var attribute = (RepresentationRouteAttribute)item.GetCustomAttributes(typeof(RepresentationRouteAttribute), false).Single();

                foreach (var type in attribute.RepresentationTypesToInclude)
                {
                    result.Add(new LinkMetadata(type, attribute.UrlTemplate, GetDeclaringTypeName(item), attribute.Name, attribute.Action));
                }
            }

            return result;
        }

        private static string GetDeclaringTypeName(MethodInfo item)
        {
            return item == null || item.DeclaringType == null ? null : item.DeclaringType.Name;
        }

        private IEnumerable<MethodInfo> GetMethods()
        {
            var apiControllerType = typeof (ApiController);
            var representationRouteAttributeType = typeof (RepresentationRouteAttribute);
            return Assembly
                .Load(this.assemblyToScan)
                .GetTypes()
                .Where(apiControllerType.IsAssignableFrom)
                .SelectMany(a => a.GetMethods())
                .Where(a => a.GetCustomAttributes(representationRouteAttributeType, false).Any());
        }
    }
}