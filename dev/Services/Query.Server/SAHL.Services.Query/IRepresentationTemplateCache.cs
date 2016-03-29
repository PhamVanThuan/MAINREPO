using System;
using WebApi.Hal;

namespace SAHL.Services.Query
{
    public interface IRepresentationTemplateCache
    {
        Representation Get(Type dataModelType);
    }
}