using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertInvoiceLineItemsCommandHandler : IServiceCommandHandler<InsertInvoiceLineItemsCommand>
    {
        private ITestDataManager testDataManager;

        public InsertInvoiceLineItemsCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(InsertInvoiceLineItemsCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = new SystemMessageCollection();
            testDataManager.InsertInvoiceLineItems(command.InvoiceLineItems);
            return systemMessages;
        }
    }
}
