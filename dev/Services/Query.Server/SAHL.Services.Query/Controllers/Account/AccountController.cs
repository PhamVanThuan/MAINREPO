using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements.Account;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Account;
using SAHL.Services.Query.Resources.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SAHL.Services.Query.Resources.Process;
using StructureMap.Diagnostics;

namespace SAHL.Services.Query.Controllers.Account
{
    public class AccountController : QueryServiceBaseController
    {
        public AccountController(IQueryFactory queryFactory, IQueryCoordinator queryCoordinator, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<AccountDataModel, GetAccountStatement>(dbFactory, dataModelCoordinator))
        {
        }

        [RepresentationRoute("/api/accounts", typeof(AccountListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.Get();
        }

        [RepresentationRoute("/api/accounts/{id}", typeof(AccountRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }
    }
}
