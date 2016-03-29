using System;
using SAHL.Services.Interfaces.Query.DataManagers;

namespace SAHL.Services.Query.Metadata
{
    public interface IDataManagerCollection
    {
        IQueryServiceDataManager Get(Type dataModelType);
    }
}