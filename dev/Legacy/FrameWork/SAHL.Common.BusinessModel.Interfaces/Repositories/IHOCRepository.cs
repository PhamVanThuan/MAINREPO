using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IHOCRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="useAccount"></param>
        IAccountHOC RetrieveHOCByOfferKey(int applicationKey, ref bool useAccount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        IAccountHOC RetrieveHOCByOfferKey(int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="useAccount"></param>
        IAccountHOC RetrieveHOCByAccountKey(int accountKey, ref bool useAccount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        IAccountHOC RetrieveHOCByAccountKey(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="HOCInsurerKey"></param>
        /// <returns></returns>
        IHOCInsurer GetHOCInsurerByKey(int HOCInsurerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IHOC GetHOCByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IHOCHistoryDetail> GetHOCHistoryDetailByFinancialServiceKey(int FinancialServiceKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="originalInsurerKey"></param>
        /// <param name="hoc"></param>
        /// <param name="updateType"></param>
        void UpdateHOCWithHistory(IDomainMessageCollection messages, int originalInsurerKey, IHOC hoc, char updateType);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IHOC CreateEmptyHOC();

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <param name="UpdateType"></param>
        /// <returns></returns>
        IHOCHistoryDetail GetLastestHOCHistoryDetail(int FinancialServiceKey, char UpdateType);

        /// <summary>
        /// Saves a HOC record.
        /// </summary>
        /// <param name="hoc">The HOC entity.</param>
        void SaveHOC(IHOC hoc);

        /// <summary>
        /// Create empty HOCHistory Detail Record
        /// </summary>
        /// <returns></returns>
        IHOCHistoryDetail CreateEmptyHOCHistoryDetail();

        /// <summary>
        /// Create empty HOCHistory Detail Record and allows you to specify the update type
        /// </summary>
        /// <returns></returns>
        IHOCHistoryDetail CreateHOCHistoryDetailRecord(IHOC hoc, char updateType);

        /// <summary>
        /// Create empty HOCHistory Record
        /// </summary>
        /// <returns></returns>
        IHOCHistory CreateEmptyHOCHistory();

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IHOCHistory GetHOCHistoryByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hocHistory"></param>
        void SaveHOCHistory(IHOCHistory hocHistory);

        // void SaveHOCNewApplicationWithExcludedRules(IHOC hoc);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hocHistoryDetail"></param>
        void SaveHOCHistoryDetail(IHOCHistoryDetail hocHistoryDetail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        IHOCHistoryDetail GetLastestHOCHistoryDetail(int FinancialServiceKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        IHOC CreateHOC(int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hocKey"></param>
        void UpdateHOCPremium(int hocKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        /// <returns></returns>
        IHOCHistoryDetail CreateHOCHistoryDetailRecord(IHOC hoc);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        /// <returns></returns>
        IHOCHistory CreateHOCHistoryRecord(IHOC hoc);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        void CalculatePremium(IHOC hoc);

        void CalculatePremiumForUpdate(IHOC hoc);

		double GetMonthlyPremium(int accountKey);
    }
}