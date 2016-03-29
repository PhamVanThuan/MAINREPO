using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class WorkFlowIcon : IEntityValidation, IWorkFlowIcon, IDAOObject
    {
        protected void OnWorkFlows_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlows_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}