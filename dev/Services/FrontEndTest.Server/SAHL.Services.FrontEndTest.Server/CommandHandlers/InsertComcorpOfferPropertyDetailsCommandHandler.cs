using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertComcorpOfferPropertyDetailsCommandHandler : IServiceCommandHandler<InsertComcorpOfferPropertyDetailsCommand>
    {
        private ITestDataManager testDataManager;

        public InsertComcorpOfferPropertyDetailsCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(InsertComcorpOfferPropertyDetailsCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.InsertComcorpOfferPropertyDetails(command.model);
            return systemMessages;
        }
    }
}