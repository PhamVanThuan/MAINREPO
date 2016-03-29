using System;
using System.Net.Http;
using System.Web.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.DataManagers.Statements.ThirdParty;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.ThirdParty;
using SAHL.Services.Query.Resources.Attorney;
using SAHL.Services.Query.Resources.ThirdParty;

namespace SAHL.Services.Query.Controllers.ThirdParty
{
    public class ThirdPartyAttorneyController : QueryServiceBaseController
    {

        public ThirdPartyAttorneyController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyAttorneyDataModel, GetThirdPartyAttorneyStatement>(dbFactory, dataModelCoordinator))
        {

        }

        [RepresentationRoute("/api/thirdparties/attorneys", typeof (ThirdPartyAttorneyListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.Get();
        }

        [RepresentationRoute("/api/thirdparties/attorneys/{id}", typeof(ThirdPartyAttorneyRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }        
    }

}