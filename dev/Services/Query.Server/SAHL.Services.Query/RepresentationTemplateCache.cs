using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using WebApi.Hal;

namespace SAHL.Services.Query
{
    public class RepresentationTemplateCache : IRepresentationTemplateCache
    {
        private IDictionary<Type, Representation> TypeMap { get; set; }

        public RepresentationTemplateCache(IDictionary<Type, Representation> typeMap)
        {
            this.TypeMap = typeMap;
        }

        public Representation Get(Type dataModelType)
        {
            var result = GetRepresentation(dataModelType);
            return result == null ? null : result.AsNew();
        }

        public Type GetType(Type dataModelType)
        {
            var result = GetRepresentation(dataModelType);
            return result == null ? null : result.GetType();
        }

        private Representation GetRepresentation(Type dataModelType)
        {
            Representation result;
            this.TypeMap.TryGetValue(dataModelType, out result);
            return result;
        }
    }
}
