using DomainService2.SharedServices;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class ArchiveFLRelatedCasesCommandHandler : IHandlesDomainServiceCommand<ArchiveFLRelatedCasesCommand>
    {
        private IX2Repository x2Repository;
        private IX2WorkflowService workflowService;

        public ArchiveFLRelatedCasesCommandHandler(IX2Repository x2Repository, IX2WorkflowService workflowService)
        {
            this.x2Repository = x2Repository;
            this.workflowService = workflowService;
        }

        public void Handle(IDomainMessageCollection messages, ArchiveFLRelatedCasesCommand command)
        {
            IInstance Instance = this.x2Repository.GetInstanceByKey(command.InstanceID);

            IEventList<IInstance> iids = this.x2Repository.GetChildInstances(Instance.ID);
            foreach (IInstance iid in iids)
            {
                // These Cases will be in App Management
                if (iid.State.Name.ToUpper() == "VALUATION HOLD")
                {
                    // Handle the Valuation Hold Lot
                    this.workflowService.ArchiveValuationsFromSourceInstanceID(iid.ID, command.ADUser, command.ApplicationKey);

                    this.x2Repository.CreateAndSaveActiveExternalActivity("EXTArchiveValuation", iid.ID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, SAHL.Common.Constants.WorkFlowProcessName.Origination, null);
                }
                else if (iid.State.Name.ToUpper() == "QUICKCASH HOLD")
                {
                    // Handle the QC Cases
                    this.workflowService.ArchiveQuickCashFromSourceInstanceID(iid.ID, command.ADUser, command.ApplicationKey);

                    this.x2Repository.CreateAndSaveActiveExternalActivity("EXTArchiveQC", iid.ID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, SAHL.Common.Constants.WorkFlowProcessName.Origination, null);
                }
                else
                {
                    // now archive the case
                    this.x2Repository.CreateAndSaveActiveExternalActivity("EXTArchiveMainCase", iid.ID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, SAHL.Common.Constants.WorkFlowProcessName.Origination, null);
                }
            }
        }
    }
}