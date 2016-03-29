using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Account;
using SAHL.Services.Query.DataManagers.Statements.ThirdParty;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Account;
using SAHL.Services.Query.Models.ThirdParty;
using SAHL.Services.Query.Resources.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Query.Controllers.ThirdParty
{
   public class ThirdPartyPaymentBankAccountController: QueryServiceBaseController
    {
        private IQueryFactory queryFactory;

        public ThirdPartyPaymentBankAccountController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyPaymentBankAccountDataModel, GetThirdPartyPaymentBankAccountStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/thirdparty/paymentbankaccount", typeof(ThirdPartyPaymentBankAccountListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.CreateNoContentMessage();
        }

        [RepresentationRoute("/api/thirdparty/{id}/paymentbankaccount", typeof(ThirdPartyPaymentBankAccountRepresentation))]
        public override HttpResponseMessage Get(string id)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(id, "ThirdPartyId", "ThirdPartyId"));
            }
            return ExecuteOne(findQuery);

        }

    }

}

