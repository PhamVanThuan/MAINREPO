using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IRateOverrideRepository
    {
        //IList<IRateOverride> GetRateOverridesByAccount(int AccountKey);

        DataTable GetRateOverridesByAccount(int AccountKey, int StageDefinitionStageDefinitionGroupKey);

        IRateOverride GetRateOverrideByKey(int RateOverrideKey);

        IRateOverride GetEmptyRateOverride();

        void SaveRateOverride(IRateOverride RateOverride);

        void RecalculateRate(int financialServiceKey, int rateConfigurationKey, string userID, string loanTransactionReference, bool baseRateReset);

        int ConvertDefendingDiscount(int LoanNumber, ref string ErrorDescription);

        void UpdateInstalment(string userID, int AccountKey);
        
    }
}
