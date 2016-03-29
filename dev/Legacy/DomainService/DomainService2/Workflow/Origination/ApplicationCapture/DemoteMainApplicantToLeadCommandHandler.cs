using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class DemoteMainApplicantToLeadCommandHandler : IHandlesDomainServiceCommand<DemoteMainApplicantToLeadCommand>
    {
        private IApplicationRepository applicationRepository;

        public DemoteMainApplicantToLeadCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, DemoteMainApplicantToLeadCommand command)
        {
            var application = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationMortgageLoan;
            applicationRepository.DemoteMainToLead(application);
            //if we got to this point, we can be assured that the method was succesfull
            command.Result = true;
        }
    }
}