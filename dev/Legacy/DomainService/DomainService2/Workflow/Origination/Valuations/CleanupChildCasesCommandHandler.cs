using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using DomainService2.SharedServices;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class CleanupChildCasesCommandHandler : IHandlesDomainServiceCommand<CleanupChildCasesCommand>
    {
        private IX2Repository x2Repository;
        
        public CleanupChildCasesCommandHandler(IX2Repository x2Repository)
        {
            this.x2Repository = x2Repository;
        }

        public void Handle(IDomainMessageCollection messages, CleanupChildCasesCommand command)
        {
            IEventList<IInstance> childInstances = x2Repository.GetChildInstances(command.ParentInstanceID);
            foreach (var childInstance in childInstances)
            {
                x2Repository.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.CloneCleanupArchive, childInstance.ID, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination, null);
            }
        }
    }
}
