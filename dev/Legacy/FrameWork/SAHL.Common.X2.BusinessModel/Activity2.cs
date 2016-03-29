using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class Activity : IEntityValidation, IActivity, IDAOObject
    {
        protected void OnInstanceActivitySecurities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstanceActivitySecurities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLogs_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnLogs_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStageActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStageActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlowActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlowActivities_BeforeRemove(ICancelDomainArgs args, object Item)
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
    }
}