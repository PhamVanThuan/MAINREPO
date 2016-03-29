using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class ApplicationHasSAHLLifeAppliedCommandHandler:IHandlesDomainServiceCommand<ApplicationHasSAHLLifeAppliedCommand>
    {
        IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;

        public ApplicationHasSAHLLifeAppliedCommandHandler(IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository)
        {
            this.applicationUnsecuredLendingRepository = applicationUnsecuredLendingRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ApplicationHasSAHLLifeAppliedCommand command)
        {
            command.Result = applicationUnsecuredLendingRepository.ApplicationHasSAHLLifeApplied(command.ApplicationKey);
        }
    }
}
