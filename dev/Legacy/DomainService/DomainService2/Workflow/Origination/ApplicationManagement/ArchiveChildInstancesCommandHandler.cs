using System;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ArchiveChildInstancesCommandHandler : IHandlesDomainServiceCommand<ArchiveChildInstancesCommand>
    {
        IX2Repository x2Repository;

        public ArchiveChildInstancesCommandHandler(IX2Repository x2Repository)
        {
            this.x2Repository = x2Repository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ArchiveChildInstancesCommand command)
        {
            IEventList<IInstance> instances = x2Repository.GetChildInstances(command.InstanceID);
            string activityToFire = String.Empty;
            foreach (var instance in instances)
            {
                switch (instance.State.Name)
                {
                    case SAHL.Common.WorkflowState.ValuationHold:
                        activityToFire = SAHL.Common.Constants.WorkFlowExternalActivity.ApplicationManagementArchiveValuation;

                        // cleanup the valuations child instances where this instance is the source instanceid
                        var sourceInstancesAll = x2Repository.GetInstanceForSourceInstanceID(instance.ID);
                        if (sourceInstancesAll != null && sourceInstancesAll.Count > 0)
                        {
                            IEnumerable<IInstance> sourceInstances = sourceInstancesAll.Where(x => x.WorkFlow.Name == SAHL.Common.Constants.WorkFlowName.Valuations);
                            foreach (IInstance sourceInstance in sourceInstances)
                            {
                                x2Repository.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.EXTCleanupArchive, sourceInstance.ID, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination, null);
                            }
                        }
                        break;
                    case SAHL.Common.WorkflowState.QuickCashHold:
                        activityToFire = SAHL.Common.Constants.WorkFlowExternalActivity.ApplicationManagementArchiveQuickCash;
                        break;
                    default:
                        activityToFire = SAHL.Common.Constants.WorkFlowExternalActivity.ApplicationManagementArchiveMainCase;
                        break;
                }
                x2Repository.CreateAndSaveActiveExternalActivity(activityToFire, instance.ID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, SAHL.Common.Constants.WorkFlowProcessName.Origination, null);
            }
            command.Result = true;
        }
    }
}