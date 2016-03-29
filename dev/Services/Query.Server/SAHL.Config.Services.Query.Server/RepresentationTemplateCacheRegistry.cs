using SAHL.Services.Query;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StructureMap.Configuration.DSL;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class RepresentationTemplateCacheRegistry : Registry
    {
        public RepresentationTemplateCacheRegistry()
        {
            var dataModelTypeRepresentationTemplateMap = new Dictionary<Type, Representation>();

            var scanner = new RepresentationDataModelScanner("SAHL.Services.Query");
            var typeMaps = scanner
                .GetMappings()
                .GroupBy(a => a.Value); //group by representationType

            var container = new Container(a =>
            {
                a.AddRegistry<LinkRegistry>();
                a.AddRegistry<IocRegistry>();
            });

            foreach (var grouping in typeMaps)
            {
                var representationType = grouping.Key;

                if (typeof(IEnumerable).IsAssignableFrom(representationType))
                {
                    var genericArgument = grouping.Key.GetGenericArguments()[0];

                    var listRepresentationTypeString = genericArgument.AssemblyQualifiedName
                        .Replace("DataModel, ", "ListRepresentation, ");

                    var type = Type.GetType(listRepresentationTypeString);
                    if (type == null)
                    {
                        continue;
                    }

                    dataModelTypeRepresentationTemplateMap.Add(representationType, GetRepresentationInstance(container, grouping.Single().Value));
                }

                foreach (var type in grouping)
                {
                    dataModelTypeRepresentationTemplateMap.Add(type.Key, GetRepresentationInstance(container, representationType));
                }
            }

            this.For<IRepresentationTemplateCache>()
                .Singleton()
                .Use<RepresentationTemplateCache>()
                .Ctor<IDictionary<Type, Representation>>()
                .Is(dataModelTypeRepresentationTemplateMap);
        }

        private static Representation GetRepresentationInstance(Container container, Type representationType)
        {
            return (Representation)container.GetInstance(representationType);
        }
    }
}