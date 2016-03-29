using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.ThirdParty;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.ThirdParty;
using SAHL.Services.Query.Resources.ThirdParty;

namespace SAHL.Services.Query.Controllers.ThirdParty
{
    public class ThirdPartyContactInformationController: QueryServiceBaseController
    {

        public ThirdPartyContactInformationController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyContactInformationDataModel, GetThirdPartyContactInformationStatement>(dbFactory, dataModelCoordinator))
        {

        }
        
        [RepresentationRoute("/api/thirdparties/contactinformation", typeof(ThirdPartyContactInformationListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.Get();
        }

        [RepresentationRoute("/api/thirdparties/{id}/contactinformation", typeof(ThirdPartyContactInformationRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }

    }

}