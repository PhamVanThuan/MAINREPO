using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Interfaces.Query.Coordinators
{
    public interface IDataModelCoordinator
    {
        IQueryDataModel ResolveDataModelRelationships(IQueryDataModel datamodel);
        IEnumerable<IQueryDataModel> ResolveDataModelRelationships(IEnumerable<IQueryDataModel> datamodels);
    }
}