using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class PromoteLeadToMainApplicantCommandHandler : IHandlesDomainServiceCommand<PromoteLeadToMainApplicantCommand>
    {
        private IApplicationRepository applicationRepository;

        public PromoteLeadToMainApplicantCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, PromoteLeadToMainApplicantCommand command)
        {
            var application = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationMortgageLoan;
            applicationRepository.PromoteLeadToMain(application);
            //if we got to this point, we can be assured that the method was succesfull
            command.Result = true;
        }
    }
}