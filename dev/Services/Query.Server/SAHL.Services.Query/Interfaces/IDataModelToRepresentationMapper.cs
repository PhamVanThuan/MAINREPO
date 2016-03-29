using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Interfaces
{
    public interface IDataModelToRepresentationMapper<TDataModel, TRepresentation, TListRepresentation> 
        where TListRepresentation : IListRepresentation
        where TRepresentation : IRepresentation
        where TDataModel : IDataModel
    {
        TListRepresentation MapModelsToRepresentation(IEnumerable<TDataModel> dataItems);
        Representation MapModelToRepresentation(TDataModel dataModel);
    }
}