using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// MortgageLoan Repository
    /// </summary>
    public interface IMortgageLoanRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        double GetReadvanceComparisonAmount(int AccountKey);

        //bool IsSPVTermWithinMax(int term, int spv);

        /// <summary>
        /// Perform Installment Change given AccountKey with a reference
        /// </summary>
        /// <param name="accountkey">AccountKey</param>
        /// <param name="userid">User Id</param>
        /// <param name="reference">Reference</param>
        void InstallmentChange(int accountkey, string userid, string reference);

        /// <summary>
        /// Perform Installment Change given AccountKey without a reference
        /// </summary>
        /// <param name="accountkey">AccountKey</param>
        /// <param name="userid">User ID</param>
        void InstallmentChange(int accountkey, string userid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="term"></param>
        /// <param name="userid"></param>
        void TermChange(int accountkey, int term, string userid);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="newSPV"></param>
        /// <param name="userid"></param>
        void TransferSPV(int accountkey, int newSPV, string userid);

        void LookUpPendingTermChangeDetailFromX2(out int NewTerm, long instanceID);

        int LookUpPendingTermChangeFromX2(long instanceId);

        bool IsThereSPVMovementInProgress(int Offerkey);

        bool LookUpPendingTermChangeByAccount(int AccountKey);

        //int GetNewSPVKeyTermChange(int spvCurrent);

        //string GetNewSPVTermChange(int spvCurrent);

        string GetNewSPVDescription(int spv);

        /// <summary>
        /// Save Mortgage Loan Record
        /// </summary>
        /// <param name="MortgageLoan"></param>
        void SaveMortgageLoan(IMortgageLoan MortgageLoan);

        //TODO: Amend or rip this out when the generic get by key is finalised
        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IMortgageLoanPurpose GetMortgageLoanPurposeByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IMortgageLoan GetMortgageLoanByKey(int Key);

        /// <summary>
        /// Return a Mortgageloan by account key
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        IMortgageLoan GetMortgageloanByAccountKey(int accountkey);

        /// <summary>
        /// Returns a collection of mortgageloans for an accountkey
        /// </summary>
        /// <param name="accountkey">accountkey</param>
        /// <returns>A collection of IMortgageLoan.</returns>
        IReadOnlyEventList<IMortgageLoan> GetMortgageLoansByAccountKey(int accountkey);

        /// <summary>
        /// Returns a new Empty MortgageLoan
        /// </summary>
        /// <returns></returns>
        IMortgageLoan CreateEmptyMortgageLoan();

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IEventList<ICAP> GetCapProducts(int AccountKey);
    }
}