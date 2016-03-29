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
    public class AttorneyContactsController : QueryServiceBaseController
    {
        private IQueryFactory queryFactory;

        public AttorneyContactsController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<AttorneyContactDataModel, GetAttorneyContactStatement>
                (dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/attorneys/{attorneyId}/contacts/{contactId}", typeof(AttorneyContactRepresentation))]
        public HttpResponseMessage Get(string attorneyId, string contactId)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(attorneyId, "AttorneyId", "AttorneyId"));
            }

            return base.Execute(findQuery, contactId);

        }

        [RepresentationRoute("/api/attorneys/{attorneyId}/contacts", typeof(AttorneyContactListRepresentation))]
        public override HttpResponseMessage Get(string attorneyId)
        {
            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(attorneyId, "AttorneyId", "AttorneyId"));
            }

            return base.Execute(findQuery);

        }
        
    }
}
