using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IFinancialAdjustmentRepository
    {
        IFinancialAdjustmentTypeSource GetFinancialAdjustmentTypeSourceByKey(int Key);

        IEventList<IFinancialAdjustment> GetFinancialAdjustmentsByAccount(int AccountKey, int StageDefinitionStageDefinitionGroupKey);

        IFinancialAdjustment GetFinancialAdjustmentByKey(int financialAdjustmentKey);

        IFinancialAdjustment GetEmptyFinancialAdjustment();

        void SaveFinancialAdjustment(IFinancialAdjustment FinancialAdjustment);

        //void RecalculateRate(int financialServiceKey, int rateConfigurationKey, string userID, string loanTransactionReference, bool baseRateReset);

        //void UpdateInstalment(string userID, int financialServiceKey, string reference);

        IFinancialAdjustmentTypeSource GetFinancialAdjustmentTypeSource(int financialAdjustmentTypeSourceKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        void DefendingDiscountOptOut(int accountKey, string userID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialAdjustment"></param>
        /// <param name="userID"></param>
        void DiscountedLinkRateOptIn(IFinancialAdjustment financialAdjustment, string userID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialAdjustment"></param>
        /// <param name="userID"></param>
        void DiscountedLinkRateOptOut(IFinancialAdjustment financialAdjustment, string userID);
    }
}