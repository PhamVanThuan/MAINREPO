using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationExpense : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO>, IApplicationExpense
    {
        public void OnApplicationDebtSettlements_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnApplicationDebtSettlements_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnApplicationDebtSettlements_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnApplicationDebtSettlements_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}