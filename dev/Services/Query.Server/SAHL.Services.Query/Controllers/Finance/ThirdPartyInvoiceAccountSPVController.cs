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

namespace SAHL.Services.Query.Controllers.Finance
{
    public class ThirdPartyInvoiceAccountSPVController : QueryServiceBaseController
    {

        private IQueryFactory queryFactory;
        private IDataModelCoordinator dataModelCoordinator;
        private IDbFactory dbFactory;

        public ThirdPartyInvoiceAccountSPVController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyInvoiceAccountDataModel, GetThirdPartyInvoiceAccountStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
            this.dbFactory = dbFactory;
            this.dataModelCoordinator = dataModelCoordinator;
        }

        public HttpResponseMessage Get(string thirdPartyInvoiceKey, string id)
        {
            var findQuery = this.queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());
            if (!findQuery.Where.Any())
            {
                findQuery.Where.Add(CreateWherePart(thirdPartyInvoiceKey, "invoiceId", "invoiceId"));
            }
            return Execute(findQuery, id);
        }
    }
}