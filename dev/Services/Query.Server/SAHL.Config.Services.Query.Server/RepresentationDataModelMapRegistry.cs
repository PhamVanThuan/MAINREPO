using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Query.Metadata;
using StructureMap.Configuration.DSL;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class RepresentationDataModelMapRegistry : Registry
    {
        public RepresentationDataModelMapRegistry()
        {
            var scanner = new RepresentationDataModelScanner("SAHL.Services.Query");

            //Key is Representation/DataModel
            //Value is Representation only
            var mappings = scanner.GetMappings()
                .OrderBy(a => a.Key.Name);

            var typeMappings = new Dictionary<Type, Type>();
            foreach (var item in mappings)
            {
                if (item.Key == item.Value)
                {
                    //skip, don't register links where representations point to themselves
                    continue;
                }
                typeMappings.Add(item.Key, item.Value);
                typeMappings.Add(item.Value, item.Key);
            }

            this.For<IRepresentationDataModelMapCollection>()
                .Singleton()
                .Use<RepresentationDataModelMapCollection>()
                .Ctor<IDictionary<Type, Type>>()
                .Is(typeMappings);
        }
    }
}