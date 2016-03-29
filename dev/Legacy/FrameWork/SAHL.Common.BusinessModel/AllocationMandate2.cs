using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AllocationMandate : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO>, IAllocationMandate
    {
        protected void OnUserOrganisationStructures_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnUserOrganisationStructures_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnUserOrganisationStructures_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnUserOrganisationStructures_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}