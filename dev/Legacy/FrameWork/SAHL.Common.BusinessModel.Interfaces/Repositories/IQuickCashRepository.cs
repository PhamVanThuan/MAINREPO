using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IQuickCashRepository
    {
        IApplicationInformationQuickCashDetail CreateEmptyApplicationInformationQuickCashDetail();

        void SaveApplicationInformationQuickCash(IApplicationInformationQuickCash applicationInformationQuickCash);

        void SaveApplicationInformationQuickCashDetail(IApplicationInformationQuickCashDetail applicationInformationQuickCashDetail);

        IApplicationInformationQuickCashDetail GetApplicationInformationQuickCashDetailByKey(int quickCashDetailKey);

        void DeleteApplicationExpense(IApplicationExpense appExpense);

        void DeleteApplicationDebtSettlement(IApplicationDebtSettlement appDebtSettlement);

        List<IApplicationInformationQuickCashDetail> GetApplicationInformationQuickCashDetailFromDisbursementObj(IDisbursement disbursement);

        List<IApplicationInformationQuickCashDetail> GetApplicationInformationQuickCashDetailByAccountKey(int AccountKey);
    }
}