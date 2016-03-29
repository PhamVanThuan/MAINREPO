using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class State : IEntityValidation, IState, IDAOObject
    {
        protected void OnNextActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnNextActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstances_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstances_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLogs_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLogs_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStates_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStates_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlowHistories_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlowHistories_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSecurityGroups_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSecurityGroups_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnForms_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnForms_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is State)
                return _State.ID == ((State)obj).ID;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}