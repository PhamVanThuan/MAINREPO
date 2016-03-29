using System;
using System.Net.Http;
using System.Web.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Controllers.Attorney;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.DataManagers.Statements.Finance;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Models.Finance;
using SAHL.Services.Query.Resources.Attorney;
using SAHL.Services.Query.Resources.Finance;
using SAHL.Services.Query.Resources.ThirdParty;

namespace SAHL.Services.Query.Controllers.Finance
{
    public class ThirdPartyInvoiceController : QueryServiceBaseController
    {
        public ThirdPartyInvoiceController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<ThirdPartyInvoiceDataModel, GetThirdPartyInvoiceStatement>(dbFactory, dataModelCoordinator))
        {
            
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices", typeof(ThirdPartyInvoiceListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.Get();
        }

        [RepresentationRoute("/api/finance/thirdpartyinvoices/{id}", typeof(ThirdPartyInvoiceRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }

    }

}