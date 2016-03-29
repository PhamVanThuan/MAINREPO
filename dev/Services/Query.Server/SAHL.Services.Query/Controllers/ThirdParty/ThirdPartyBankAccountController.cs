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
using System;
using System.Net.Http;
using System.Web.Http;

namespace SAHL.Services.Query.Controllers.ThirdParty
{
    public class ThirdPartyBankAccountController : QueryServiceBaseController
    {
        private readonly IQueryFactory queryFactory;

        public ThirdPartyBankAccountController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyBankAccountDataModel, GetThirdPartyBankAccountStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }
        
        [RepresentationRoute("/api/thirdparties/{id}/bankaccounts", typeof(ThirdPartyBankAccountListRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(id, "ThirdPartyId", "ThirdPartyId"));
            }

            return base.Execute(findQuery);
        }

        [RepresentationRoute("/api/thirdparties/{thirdPartyId}/bankaccounts/{bankAccountId}", typeof(ThirdPartyBankAccountRepresentation))]
        public HttpResponseMessage Get(string thirdPartyId, string bankAccountId)
        {
            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(thirdPartyId, "ThirdPartyId", "ThirdPartyId"));
                findQuery.Where.Add(CreateWherePart(bankAccountId, "Id", "Id"));
            }
            
            return base.Execute(findQuery);
        }
    }
}