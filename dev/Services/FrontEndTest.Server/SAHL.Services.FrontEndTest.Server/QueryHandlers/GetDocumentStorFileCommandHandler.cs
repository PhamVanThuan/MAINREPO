using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.IO;

namespace SAHL.Services.FrontEndTest.QueryHandlers
{
    public class GetLossControlAttorneyInvoiceDocumentQueryHandler : IServiceQueryHandler<GetLossControlAttorneyInvoiceDocumentQuery>
    {
        private ITestDataManager testDataManager;
        public GetLossControlAttorneyInvoiceDocumentQueryHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public Core.SystemMessages.ISystemMessageCollection HandleQuery(GetLossControlAttorneyInvoiceDocumentQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            var lossControlAttroneyInvoiceStorData = testDataManager.GetLossControlAttorneyInvoiceStorData(query.ThirdPartyInvoiceKey);
            var results = new List<string>();
            results.Add(Convert.ToBase64String(File.ReadAllBytes(lossControlAttroneyInvoiceStorData.FilePath)));
            query.Result = new ServiceQueryResult<string>(results);
            return messages;
        }
    }
}