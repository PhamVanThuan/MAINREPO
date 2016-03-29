using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Finance;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Resources.Finance;
using System.Net.Http;

namespace SAHL.Services.Query.Controllers.Finance
{
    public class ThirdPartyInvoiceLineItemsController : QueryServiceBaseController
    {

        private IQueryFactory queryFactory;

        public ThirdPartyInvoiceLineItemsController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyInvoiceLineItemsDataModel, GetThirdPartyInvoiceLineItemsStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/{invoiceId}/lineItems", typeof(ThirdPartyInvoiceLineItemsListRepresentation))]
        public override HttpResponseMessage Get(string invoiceId)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(invoiceId, "InvoiceId", "InvoiceId"));
            }

            return base.Execute(findQuery);
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/{invoiceId}/lineItems/{itemId}", typeof(ThirdPartyInvoiceLineItemsRepresentation))]
        public HttpResponseMessage Get(string invoiceId, string itemId)
        {
            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(invoiceId, "InvoiceId", "InvoiceId"));
            }

            return base.Execute(findQuery, itemId);
        }
    }
}
