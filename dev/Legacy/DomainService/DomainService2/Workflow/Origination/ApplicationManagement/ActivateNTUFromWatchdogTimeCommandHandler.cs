using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ActivateNTUFromWatchdogTimeCommandHandler : IHandlesDomainServiceCommand<ActivateNTUFromWatchdogTimeCommand>
    {
        IX2Repository x2Repository;

        public ActivateNTUFromWatchdogTimeCommandHandler(IX2Repository x2Repository)
        {
            this.x2Repository = x2Repository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ActivateNTUFromWatchdogTimeCommand command)
        {
            x2Repository.CreateAndSaveActiveExternalActivity(Constants.WorkFlowExternalActivity.EXTWatchdog, command.InstanceID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, Constants.WorkFlowProcessName.Origination, null);
            command.Result = true;
        }
    }
}