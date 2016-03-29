using System.Net.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Resources.Attorney;

namespace SAHL.Services.Query.Controllers.Attorney
{
    public class AttorneyContactInformationController : QueryServiceBaseController
    {

        private IQueryFactory queryFactory;

        public AttorneyContactInformationController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory,
            IDataModelCoordinator dataModelCoordinator, IRouteMetadataCollection routeMetadataCollection)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<AttorneyContactInformationDataModel, GetAttorneyContactInformationStatement>
                    (dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/attorneys/contactinformation", typeof (AttorneyContactInformationListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.CreateNoContentMessage();
        }

        [RepresentationRoute("/api/attorneys/{id}/contactinformation", typeof(AttorneyContactInformationRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(id, "AttorneyId", "AttorneyId"));
            }
            return ExecuteOne(findQuery);
        }
    }
}
