using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Account;
using SAHL.Services.Query.DataManagers.Statements.Finance;
using SAHL.Services.Query.DataManagers.Statements.Treasury;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Account;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Models.Treasury;
using SAHL.Services.Query.Resources.Account;
using SAHL.Services.Query.Resources.Finance;
using SAHL.Services.Query.Resources.Treasury;

namespace SAHL.Services.Query.Controllers.Account
{
    public class AccountSpvController: QueryServiceBaseController
    {

        private IQueryFactory queryFactory;
        private IDbFactory dbFactory;
        private IDataModelCoordinator dataModelCoordinator;

        public AccountSpvController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<SPVDataModel, GetSPVStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
            this.dataModelCoordinator = dataModelCoordinator;
            this.dbFactory = dbFactory;
        }

        [RepresentationRoute("/api/accounts/{id}/spv", typeof(AccountSPVRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
            var dataModel = GetRootDataModel(id, "account");
            return base.ExecuteUrl(dataModel.Relationships, "spv", findQuery);
        }

        [RepresentationRoute("/api/accounts/{id}/spvs/{spvId}", typeof(AccountSpvListRepresentation))]
        public HttpResponseMessage Get(string id, string spvId)
        {
            return CreateNoContentMessage();
        }

        private IQueryDataModel GetRootDataModel(string id, string rootElement)
        {

            var queryServiceDataManager = FindRootDataManager(rootElement);

            AccountDataModel accountDataModel =
                (AccountDataModel)queryServiceDataManager.GetById(id, queryFactory.CreateEmptyFindQuery());

            return accountDataModel;
        }

        private IQueryServiceDataManager FindRootDataManager(string rootElement)
        {
            IQueryServiceDataManager queryServiceDataManager = new QueryServiceDataManager<AccountDataModel, GetAccountStatement>(dbFactory, dataModelCoordinator);
            return queryServiceDataManager;
        }
    }

}