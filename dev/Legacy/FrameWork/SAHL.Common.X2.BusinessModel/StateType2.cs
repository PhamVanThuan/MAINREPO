using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class StateType : IEntityValidation, IStateType, IDAOObject
    {
        protected void OnStates_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStates_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}