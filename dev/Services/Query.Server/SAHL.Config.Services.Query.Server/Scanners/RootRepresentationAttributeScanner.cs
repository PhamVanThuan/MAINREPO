using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using SAHL.Services.Query;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class QueryServiceApiUrl
    {
        public const string Prefix = "api/";
    }

    public class RootRepresentationAttributeScanner
    {
        private readonly string assemblyToScan;

        public RootRepresentationAttributeScanner(string assemblyToScan)
        {
            this.assemblyToScan = assemblyToScan;
        }

        public IEnumerable<LinkMetadata> GetLinks()
        {
            var result = new List<LinkMetadata>();

            var rootRepresentations = GetRootRepresentations();

            foreach (var item in rootRepresentations)
            {
                var entityName = item.Name
                    .Replace("ListRepresentation", string.Empty)
                    .Replace("Representation", string.Empty);

                var pluraliser = PluralizationService.CreateService(CultureInfo.CreateSpecificCulture("en-ZA"));
                var pluralisedEntityName = pluraliser.Pluralize(entityName);

                var urlTemplate = QueryServiceApiUrl.Prefix + pluralisedEntityName;
                result.Add(new LinkMetadata(item, urlTemplate, "QueryServiceBase", item.Name.Replace("Representation", string.Empty)));
            }

            return result;
        }

        private IEnumerable<Type> GetRootRepresentations()
        {
            var serviceGenerationExcludeAttributeType = typeof (ServiceGenerationToolExcludeAttribute);
            var representationRootAttributeType = typeof (RootRepresentationAttribute);
            return Assembly
                .Load(this.assemblyToScan)
                .GetTypes()
                .Where(a => a.GetCustomAttributes(representationRootAttributeType, false).Any()
                    && !a.GetCustomAttributes(serviceGenerationExcludeAttributeType, false).Any()
                );
        }
    }
}