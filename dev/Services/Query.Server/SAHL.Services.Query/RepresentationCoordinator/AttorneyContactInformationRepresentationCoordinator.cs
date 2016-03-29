using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Resources.Attorney;
using QueryModels = SAHL.Services.Query.Models;

namespace SAHL.Services.Query.RepresentationCoordinator
{
    public interface IAttorneyContactInformationRepresentationCoordinator
    {
        AttorneyContactInformationListRepresentation GetList(IFindQuery findManyQuery);
        AttorneyContactInformationRepresentation GetById(int id, IFindQuery findManyQuery);
        AttorneyContactInformationRepresentation GetOne(IFindQuery findManyQuery);
    }

    public class AttorneyContactInformationRepresentationCoordinator : IAttorneyContactInformationRepresentationCoordinator
    {
        public AttorneyContactInformationRepresentationCoordinator(IQueryServiceDataManager dataManager,
            IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> representationMapper)
        {
            DataManager = dataManager;
            RepresentationMapper = representationMapper;
        }

        private IQueryServiceDataManager DataManager { get; set; }
        private IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> RepresentationMapper { get; set; }

        public AttorneyContactInformationListRepresentation GetList(IFindQuery findManyQuery)
        {
            IEnumerable<AttorneyContactInformationDataModel> attorneyContactDataModels =
                (IEnumerable<AttorneyContactInformationDataModel>) DataManager.GetList(findManyQuery);
            return (AttorneyContactInformationListRepresentation) RepresentationMapper.MapModelsToRepresentation(attorneyContactDataModels);
        }

        public AttorneyContactInformationRepresentation GetById(int id, IFindQuery findManyQuery)
        {
            var contact = DataManager.GetById(id.ToString(), findManyQuery);
            if (contact != null)
            {
                return (AttorneyContactInformationRepresentation) RepresentationMapper.MapModelToRepresentation(contact);
            }

            return null;
        }

        public AttorneyContactInformationRepresentation GetOne(IFindQuery findManyQuery)
        {
            return null;
        }
    }
}
