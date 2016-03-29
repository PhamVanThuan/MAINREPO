using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Attorney;
using QueryModels = SAHL.Services.Query.Models;

namespace SAHL.Services.Query.RepresentationCoordinator
{

    public interface IAttorneyContactsRepresentationCoordinator
    {
        AttorneyContactListRepresentation GetAttorneyContactsList(IFindQuery findManyQuery);
        AttorneyContactRepresentation GetAttorneyContactById(int id, IFindQuery findManyQuery);
        AttorneyContactRepresentation GetOneAttorneyContact(IFindQuery findManyQuery);
    }

    public class AttorneyContactsRepresentationCoordinator : IAttorneyContactsRepresentationCoordinator
    {
        private IQueryServiceDataManager DataManager { get; set; }
        private IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> RepresentationMapper { get; set; }

        public AttorneyContactsRepresentationCoordinator(IQueryServiceDataManager dataManager, IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> representationMapper)
        {
            DataManager = dataManager;
            RepresentationMapper = representationMapper;
        }

        public AttorneyContactListRepresentation GetAttorneyContactsList(IFindQuery findManyQuery)
        {
            IEnumerable<AttorneyContactDataModel> attorneyContactDataModels = (IEnumerable<AttorneyContactDataModel>)DataManager.GetList(findManyQuery);
            return (AttorneyContactListRepresentation) RepresentationMapper.MapModelsToRepresentation(attorneyContactDataModels);
        }

        public AttorneyContactRepresentation GetAttorneyContactById(int id, IFindQuery findManyQuery)
        {
            var contact = DataManager.GetById(id.ToString(), findManyQuery);
            if (contact != null)
            {
                return (AttorneyContactRepresentation) RepresentationMapper.MapModelToRepresentation(contact);    
            }

            return null;
        }

        public AttorneyContactRepresentation GetOneAttorneyContact(IFindQuery findManyQuery)
        {
            return null;
        }


    }

}