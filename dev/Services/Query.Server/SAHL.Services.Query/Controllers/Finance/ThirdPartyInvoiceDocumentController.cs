using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Finance;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Resources.Finance;
using System.Net.Http;

namespace SAHL.Services.Query.Controllers.Finance
{
    public class ThirdPartyInvoiceDocumentController : QueryServiceBaseController
    {

        private IQueryFactory queryFactory;

        public ThirdPartyInvoiceDocumentController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyInvoiceDocumentDataModel, GetThirdPartyInvoiceDocumentStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/{invoiceId}/documents", typeof(ThirdPartyInvoiceDocumentListRepresentation))]
        public override HttpResponseMessage Get(string invoiceId)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(invoiceId, "InvoiceId", "InvoiceId"));
            }

            return base.Execute(findQuery);

        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/{invoiceId}/documents/{documentId}", typeof(ThirdPartyInvoiceDocumentRepresentation))]
        public HttpResponseMessage Get(string invoiceId, string documentId)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(invoiceId, "InvoiceId", "InvoiceId"));
            }

            return base.Execute(findQuery, documentId);

        }


    }
}