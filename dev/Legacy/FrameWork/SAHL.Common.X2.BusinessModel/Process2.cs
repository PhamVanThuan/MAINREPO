using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class Process : IEntityValidation, IProcess, IDAOObject
    {
        protected void OnProcesses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnProcesses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSecurityGroups_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSecurityGroups_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlows_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlows_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}