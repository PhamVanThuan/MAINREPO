using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class UpdateRegMailDetailToInstructionNotSentCommandHandler : IHandlesDomainServiceCommand<UpdateRegMailDetailToInstructionNotSentCommand>
    {
        IApplicationRepository applicationRepository;
        IRegistrationRepository registrationRepository;

        public UpdateRegMailDetailToInstructionNotSentCommandHandler(IApplicationRepository applicationRepository, IRegistrationRepository registrationRepository)
        {
            this.applicationRepository = applicationRepository;
            this.registrationRepository = registrationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, UpdateRegMailDetailToInstructionNotSentCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            IRegMail regmail = registrationRepository.GetRegmailByAccountKey(application.ReservedAccount.Key);
            if (regmail != null)
            {
                regmail.DetailTypeNumber = (int)DetailTypes.InstructionSent;
                regmail.RegMailDateTime = DateTime.Now;
                registrationRepository.SaveRegmail(regmail);
            }
        }
    }
}