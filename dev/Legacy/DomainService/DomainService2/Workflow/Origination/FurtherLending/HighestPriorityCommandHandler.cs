using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class HighestPriorityCommandHandler : IHandlesDomainServiceCommand<HighestPriorityCommand>
    {
        private IApplicationRepository applicationRepository;
        private IX2Repository x2Repository;

        public HighestPriorityCommandHandler(IApplicationRepository applicationRepository, IX2Repository x2Repository)
        {
            this.applicationRepository = applicationRepository;
            this.x2Repository = x2Repository;
        }

        public void Handle(IDomainMessageCollection messages, HighestPriorityCommand command)
        {
            command.Result = false;
            // look at any other apps in progress.
            // If they are of a lower pri type, go find their instance and stick em in hold
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            IEventList<IApplication> applications = application.Account.Applications;
            foreach (IApplication currentApp in applications)
            {
                // makes sure we only look at open applications
                if (currentApp.ApplicationStatus.Key == (int)OfferStatuses.Open)
                {
                    // check if its a further loan type
                    if (currentApp.ApplicationType.Key == (int)OfferTypes.ReAdvance || currentApp.ApplicationType.Key == (int)OfferTypes.FurtherAdvance || currentApp.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                    {
                        // move instance to the hold state ONLY IF ITS OF A LOWER PRI
                        if (currentApp.ApplicationType.Key > application.ApplicationType.Key)
                        {
                            int AppKey = currentApp.Key;
                            IInstance instance = this.x2Repository.GetInstanceForGenericKey(currentApp.Key, Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination);
                            if (instance != null && instance.State.Name != WorkflowState.AwaitingApplication)
                            {
                                IWorkFlow worklflow = x2Repository.GetWorkFlowByName(Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination);
                                x2Repository.CreateAndSaveActiveExternalActivity(Constants.WorkFlowExternalActivity.MoveApplicationToHold, instance.ID, worklflow.Name, Constants.WorkFlowProcessName.Origination, null);
                            }
                        }
                    }
                }
            }
            command.Result = true;
        }
    }
}