using System;

namespace SAHL.Services.Query.Metadata
{
    public interface IRepresentationDataModelMapCollection
    {
        Type Get(Type type);
    }
}