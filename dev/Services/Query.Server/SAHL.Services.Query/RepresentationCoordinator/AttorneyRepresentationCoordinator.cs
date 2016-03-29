using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Attorney;
using WebApi.Hal;
using QueryModels = SAHL.Services.Query.Models;

namespace SAHL.Services.Query.RepresentationCoordinator
{

    public interface IAttorneyRepresentationCoordinator
    {
        AttorneyListRepresentation GetAttorneyList(IFindQuery findManyQuery);
        AttorneyRepresentation GetAttorneyById(Guid id, IFindQuery findManyQuery);
        AttorneyRepresentation GetOneAttorney(IFindQuery findManyQuery);
    }
    
    public class AttorneyRepresentationCoordinator : IAttorneyRepresentationCoordinator
    {
        private IQueryServiceDataManager DataManager { get; set; }
	    private IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> RepresentationMapper { get; set; }

        public AttorneyRepresentationCoordinator(IQueryServiceDataManager dataManager, 
            IDataModelToRepresentationMapper<IDataModel, IRepresentation, IListRepresentation> representationMapper)
	    {
		    DataManager = dataManager;
		    RepresentationMapper = representationMapper;
	    }

	    public AttorneyListRepresentation GetAttorneyList(IFindQuery findManyQuery)
	    {
		    IEnumerable<AttorneyDataModel> attorneyDataModels = (IEnumerable<AttorneyDataModel>)DataManager.GetList(findManyQuery);
            AttorneyListRepresentation listRepresentation = (AttorneyListRepresentation) RepresentationMapper.MapModelsToRepresentation(attorneyDataModels);
            return listRepresentation;
	    }

        private List<Link> GetLinks(List<Link> list)
        {
            return list;
        } 

		public AttorneyRepresentation GetAttorneyById(Guid id, IFindQuery findManyQuery)
	    {
		    var contact = DataManager.GetById(id.ToString(), findManyQuery);
		    if (contact != null)
		    {
			    return (AttorneyRepresentation) RepresentationMapper.MapModelToRepresentation(contact);    
		    }

		    return null;
	    }

	    public AttorneyRepresentation GetOneAttorney(IFindQuery findManyQuery)
	    {
		    return null;
	    }
        
    }

}