
using System.Net.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Treasury;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Treasury;
using SAHL.Services.Query.Resources.Treasury;

namespace SAHL.Services.Query.Controllers.Treasury
{
    public class SPVController : QueryServiceBaseController
    {
        public SPVController(IQueryCoordinator queryCoordinator
            , IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator, IQueryFactory queryFactory, IRouteMetadataCollection routeMetadataCollection)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<SPVDataModel, GetSPVStatement>(dbFactory, dataModelCoordinator))
        {
        }

        [RepresentationRoute("/api/treasury/spvs", typeof(SPVListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.Get();
        }

        [RepresentationRoute("/api/treasury/spvs/{id}", typeof(SPVRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }
    }
}

