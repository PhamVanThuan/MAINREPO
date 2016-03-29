using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryHandlers
{
    public class GetFirstThirdPartyInvoiceWhereFileExistsQueryHandler : IServiceQueryHandler<GetFirstThirdPartyInvoiceWhereFileExistsQuery>
    {
        private ITestDataManager testDataManager;
        public GetFirstThirdPartyInvoiceWhereFileExistsQueryHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public ISystemMessageCollection HandleQuery(GetFirstThirdPartyInvoiceWhereFileExistsQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            string guid = testDataManager.GetGuidForFirstThirdPartyInvoice();
            var results = new List<string>();
            results.Add(guid);
            query.Result = new ServiceQueryResult<string>(results);
            return messages;
        }
    }
}
