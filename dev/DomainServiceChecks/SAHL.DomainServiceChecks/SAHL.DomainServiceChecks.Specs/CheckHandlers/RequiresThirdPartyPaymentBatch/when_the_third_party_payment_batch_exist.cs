using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.CatsDataManager;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresThirdPartyPaymentBatch
{
    public class when_the_third_party_payment_batch_exist : WithFakes
    {
        private static ICatsDataManager dataManager;
        private static RequiresProcessedCATSPaymentBatchHandler handler;
        private static IRequiresCATSPaymentBatch commandCheck;
        private static ISystemMessageCollection systemMessages;
        private static string expectedError;

        private Establish context = () =>
        {
            var validBatchNumber = 1;
            systemMessages = SystemMessageCollection.Empty();
            dataManager = An<ICatsDataManager>();
            commandCheck = An<IRequiresCATSPaymentBatch>();
            handler = new RequiresProcessedCATSPaymentBatchHandler(dataManager);
            commandCheck.WhenToldTo(x => x.CATSPaymentBatchKey).Return(validBatchNumber);
            dataManager.WhenToldTo(x => x.DoesCATSPaymentBatchExist(validBatchNumber)).Return(true);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(commandCheck);
        };

        private It should_return_an_error_message = () =>
        {
            systemMessages.HasErrors.ShouldBeFalse();
        };
    }
}