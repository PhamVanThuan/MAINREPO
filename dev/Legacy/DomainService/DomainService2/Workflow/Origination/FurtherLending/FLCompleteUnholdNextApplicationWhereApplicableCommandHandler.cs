using System.Collections.Generic;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class FLCompleteUnholdNextApplicationWhereApplicableCommandHandler : IHandlesDomainServiceCommand<FLCompleteUnholdNextApplicationWhereApplicableCommand>
    {
        private IApplicationRepository applicationRepository;
        private IX2Repository x2Repository;

        public FLCompleteUnholdNextApplicationWhereApplicableCommandHandler(IApplicationRepository applicationRepository, IX2Repository x2Repository)
        {
            this.applicationRepository = applicationRepository;
            this.x2Repository = x2Repository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, FLCompleteUnholdNextApplicationWhereApplicableCommand command)
        {
            IApplication app = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (app.Account != null && app.Account.Applications != null)
            {
                IEventList<IApplication> applications = app.Account.Applications;
                List<IApplication> applicationsInHold = new List<IApplication>();
                // build a list of further loan types that are open that we need to unhold
                // NB our current case is going to be in this list just be aware at this point
                foreach (IApplication currentApp in applications)
                {
                    if ((currentApp.ApplicationStatus.Key == (int)OfferStatuses.Open) && (currentApp.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                            currentApp.ApplicationType.Key == (int)OfferTypes.FurtherAdvance || currentApp.ApplicationType.Key == (int)OfferTypes.FurtherLoan ||
                            currentApp.ApplicationType.Key == (int)OfferTypes.Life))
                    {
                        applicationsInHold.Add(currentApp);
                    }
                }

                int appType = int.MaxValue;
                IApplication appToUnHold = null;
                IInstance iID = null;
                foreach (IApplication currentApp in applicationsInHold)
                {
                    // Make sure the case we are trying to raise a flag for is NOT the currently
                    // Executing case (this was happening in FL. As we finish the rapid the readvance doesnt
                    // unhold cause we were trying to unhold the rapid .. exclude current app key
                    if (currentApp.ApplicationType.Key < appType && currentApp.Key != command.ApplicationKey)
                    {
                        iID = x2Repository.GetInstanceForGenericKey(currentApp.Key, Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination);
                        if (iID != null)
                        {
                            appType = currentApp.ApplicationType.Key;
                            appToUnHold = currentApp;
                        }
                    }
                }
                if (appToUnHold != null)
                {
                    // if we have an app here we know it has an instance so unhold it
                    iID = x2Repository.GetInstanceForGenericKey(appToUnHold.Key, Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination);
                    if (iID != null)
                    {
                        IWorkFlow workflow = x2Repository.GetWorkFlowByName(Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination);
                        x2Repository.CreateAndSaveActiveExternalActivity(Constants.WorkFlowExternalActivity.ReturnApplicationFromHold, iID.ID, workflow.Name, Constants.WorkFlowProcessName.Origination, null);
                    }
                }
            }
        }
    }
}