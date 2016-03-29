using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class LTVForFLCaseCommandHandler : IHandlesDomainServiceCommand<LTVForFLCaseCommand>
    {
        private IApplicationRepository applicationRepository;

        public LTVForFLCaseCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, LTVForFLCaseCommand command)
        {
            command.LTV = -1;
            IApplication app = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            IApplicationFurtherLending appFurtherLending = app as IApplicationFurtherLending;
            if (appFurtherLending != null)
            {
                command.LTV = appFurtherLending.EstimatedDisbursedLTV;
            }
            else
            {
                messages.Add(new Error("Application is not a Further Lending Application.", ""));
            }
        }
    }
}