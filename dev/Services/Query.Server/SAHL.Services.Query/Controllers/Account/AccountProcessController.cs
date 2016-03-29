using System.Net.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Process;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Process;
using SAHL.Services.Query.Resources.Process;

namespace SAHL.Services.Query.Controllers.Account
{
    public class AccountProcessController : QueryServiceBaseController
    {
        private IQueryFactory queryFactory;

        public AccountProcessController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ProcessDataModel, GetProcessStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/accounts/{id}/processes/{process}/stage", typeof(ProcessRepresentation))]
        public HttpResponseMessage Get(string id, string process)
        {
            if (process.ToLower() == "losscontrol")
            {
                IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
                if (findQuery.Where.Count == 0)
                {
                    findQuery.Where.Add(CreateWherePart(id, "AccountKey", "AccountKey"));
                }
                return ExecuteOne(findQuery);
            }
            return CreateNoContentMessage();
        }

        [RepresentationRoute("/api/accounts/{id}/processes/{process}/stage/{value}", typeof(ProcessListRepresentation))]
        public HttpResponseMessage Get(string id, string process, string value)
        {
            return base.CreateNoContentMessage();
        }
    }
}
