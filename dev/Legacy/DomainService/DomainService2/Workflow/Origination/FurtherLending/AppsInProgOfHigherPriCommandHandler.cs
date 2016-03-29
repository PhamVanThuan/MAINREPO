using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class AppsInProgOfHigherPriCommandHandler : IHandlesDomainServiceCommand<AppsInProgOfHigherPriCommand>
    {
        private IApplicationRepository applicationRepository;
        private IX2Repository x2Repository;

        public AppsInProgOfHigherPriCommandHandler(IApplicationRepository applicationRepository, IX2Repository x2Repository)
        {
            this.applicationRepository = applicationRepository;
            this.x2Repository = x2Repository;
        }

        public void Handle(IDomainMessageCollection messages, AppsInProgOfHigherPriCommand command)
        {
            // Take app key
            // look at the account
            // look at other applications on the account.
            // if the app in prog is of higher pri return true
            command.Result = false;
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            int appType = application.ApplicationType.Key;
            IEventList<IApplication> applications = application.Account.Applications;
            foreach (IApplication currentapp in applications)
            {
                if (currentapp.ApplicationType.Key < appType && currentapp.ApplicationStatus.Key == (int)OfferStatuses.Open)
                {
                    // check that there is an instance for this. If no instance dont put it on hold
                    IInstance instance = x2Repository.GetInstanceForGenericKey(currentapp.Key, Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination);
                    if (instance != null && instance.State.Name != WorkflowState.AwaitingApplication)
                    {
                        command.Result = true;
                    }
                }
            }
        }
    }
}