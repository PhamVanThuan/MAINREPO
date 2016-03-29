using System;
using System.Collections.Generic;

namespace SAHL.Services.Query.Metadata
{
    public class RepresentationDataModelMapCollection : IRepresentationDataModelMapCollection
    {
        private readonly IDictionary<Type, Type> typeMaps;

        public RepresentationDataModelMapCollection(IDictionary<Type, Type> typeMaps)
        {
            this.typeMaps = typeMaps;
        }

        public Type Get(Type type)
        {
            Type result;
            this.typeMaps.TryGetValue(type, out result);
            return result;
        }
    }
}