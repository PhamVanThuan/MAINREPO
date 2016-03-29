using System.Net;
using System.Net.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Finance;
using SAHL.Services.Query.DataManagers.Statements.Lookup;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Resources.Finance;
using SAHL.Services.Query.Resources.Lookup;

namespace SAHL.Services.Query.Controllers.Lookup
{
    public class InvoiceLineItemDescriptionController : QueryServiceBaseController
    {

        private IQueryFactory queryFactory;

        public InvoiceLineItemDescriptionController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<InvoiceLineItemDescriptionDataModel, GetInvoiceLineItemDescriptionStatement>(dbFactory, dataModelCoordinator))
        {
            this.queryFactory = queryFactory;
        }

        [RepresentationRoute("/api/lookup/InvoiceLineItemCategory/{parentId}/InvoiceLineItemDescription", typeof(InvoiceLineItemDescriptionListRepresentation))]
        public override HttpResponseMessage Get(string parentId)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(parentId, "InvoiceLineItemCategoryKey", "InvoiceLineItemCategoryKey"));
            }

            return base.Execute(findQuery);

        }

        [RepresentationRoute("/api/lookup/InvoiceLineItemCategory/{parentId}/InvoiceLineItemDescription/{id}", typeof(InvoiceLineItemDescriptionRepresentation))]
        public HttpResponseMessage Get(string parentId, string id)
        {

            IFindQuery findQuery = queryFactory.CreateFindManyQuery(Request.RequestUri.ParseQueryString());

            if (findQuery.Where.Count == 0)
            {
                findQuery.Where.Add(CreateWherePart(parentId, "InvoiceId", "InvoiceId"));
            }

            return base.Execute(findQuery, id);

        }


    }

}