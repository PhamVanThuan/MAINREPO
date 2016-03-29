using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveEmptyThirdPartyInvoiceCommandHandler : IServiceCommandHandler<RemoveEmptyThirdPartyInvoiceCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveEmptyThirdPartyInvoiceCommandHandler(ITestDataManager testDataManager) 
	    {
            this.testDataManager = testDataManager;
	    }
        
        public ISystemMessageCollection HandleCommand(RemoveEmptyThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.RemoveEmptyThirdPartyInvoice(command.ThirdPartyInvoiceKey);
            return SystemMessageCollection.Empty();
        }
    }
}
