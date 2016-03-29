using SAHL.Services.Capitec.Models.Shared;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.CapitecApplication;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.Capitec.Managers.RequestPublisher;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class CreateSAHomeLoansSwitchApplicationCommandHandler: IServiceCommandHandler<CreateSAHomeLoansSwitchApplicationCommand>
    {
        private IRequestPublisherManager capitecRequestPublisher;
        private ICapitecApplicationManager capitecApplicationRepository;

        public CreateSAHomeLoansSwitchApplicationCommandHandler(IRequestPublisherManager capitecRequestPublisher, ICapitecApplicationManager capitecApplicationRepository)
        {
            this.capitecRequestPublisher = capitecRequestPublisher;
            this.capitecApplicationRepository = capitecApplicationRepository;
        }

        public ISystemMessageCollection HandleCommand(CreateSAHomeLoansSwitchApplicationCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            var capitecApplication = this.capitecApplicationRepository.CreateCapitecApplicationFromSwitchLoanApplication(command.ApplicationNumber,command.ApplicationType, command.SwitchLoanApplication, command.SystemMessageCollection, command.ApplicationId);
            this.capitecRequestPublisher.PublishWithRetry(capitecApplication);
            return messages;
        }
    }
}