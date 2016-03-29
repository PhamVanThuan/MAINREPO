using SAHL.Services.Capitec.Models.Shared;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.CapitecApplication;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Capitec.Managers.RequestPublisher;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class CreateSAHomeLoansNewPurchaseApplicationCommandHandler : IServiceCommandHandler<CreateSAHomeLoansNewPurchaseApplicationCommand>
    {
        private IRequestPublisherManager capitecRequestPublisher;
        private ICapitecApplicationManager capitecApplicationRepository;

        public CreateSAHomeLoansNewPurchaseApplicationCommandHandler(IRequestPublisherManager capitecRequestPublisher, ICapitecApplicationManager capitecApplicationRepository)
        {
            this.capitecRequestPublisher = capitecRequestPublisher;
            this.capitecApplicationRepository = capitecApplicationRepository;
        }

        public ISystemMessageCollection HandleCommand(CreateSAHomeLoansNewPurchaseApplicationCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            var capitecApplication = this.capitecApplicationRepository.CreateCapitecApplicationFromNewPurchaseApplication(command.ApplicationNumber,command.ApplicationType
                , command.NewPurchaseApplication, command.SystemMessageCollection, command.ApplicationId);
            this.capitecRequestPublisher.PublishWithRetry(capitecApplication);
            return messages;
        }
    }
}