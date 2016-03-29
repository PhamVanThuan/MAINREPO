using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateThirdPartyInvoiceCommandHandler : IServiceCommandHandler<UpdateThirdPartyInvoiceCommand>
    {
        private ITestDataManager testDataManager;

        public UpdateThirdPartyInvoiceCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.UpdateThirdPartyInvoice(command.ThirdPartyInvoice);
            return systemMessages;
        }
    }
}