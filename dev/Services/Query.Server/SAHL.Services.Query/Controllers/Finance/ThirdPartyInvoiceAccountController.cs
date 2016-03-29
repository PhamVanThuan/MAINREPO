using System.Linq;
using System.Net.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Models.Account;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Resources.Finance;
using SAHL.Services.Query.DataManagers.Statements.Finance;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Serialiser;

namespace SAHL.Services.Query.Controllers.Finance
{
    public class ThirdPartyInvoiceAccountController : QueryServiceBaseController
    {

        private IQueryFactory queryFactory;
        private IDataModelCoordinator dataModelCoordinator;
        private IDbFactory dbFactory;

        public ThirdPartyInvoiceAccountController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyInvoiceAccountDataModel, GetThirdPartyInvoiceAccountStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
            this.dbFactory = dbFactory;
            this.dataModelCoordinator = dataModelCoordinator;
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/{id}/account", typeof(ThirdPartyInvoiceAccountRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/accounts", typeof(ThirdPartyInvoiceAccountListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return CreateNoContentMessage();
        }

    }
}