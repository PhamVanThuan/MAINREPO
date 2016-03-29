using System.Collections;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// Interface for FutureDatedChange Repository
    /// </summary>
    public interface IFutureDatedChangeRepository
    {
        /// <summary>
        /// Returns a list of FutureDatedChange Records
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="futureDatedChangeTypeKey"></param>
        /// <returns></returns>
        IList<IFutureDatedChange> GetFutureDatedChangesByGenericKey(int accountKey, int futureDatedChangeTypeKey);

        /// <summary>
        /// Returns a FutureDatedChange object given it's key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IFutureDatedChange GetFutureDatedChangeByKey(int Key);

        /// <summary>
        /// Returns a FutureDatedChangeDetail record given it's key
        /// </summary>
        /// <param name="FutureDatedChangeDetailKey"></param>
        /// <returns></returns>
        IFutureDatedChangeDetail GetFutureDatedChangeDetailByKey(int FutureDatedChangeDetailKey);

        /// <summary>
        /// Method to save updated FutureDated Change Record
        /// </summary>
        /// <param name="fdc"></param>
        void SaveFutureDatedChange(IFutureDatedChange fdc);

        /// <summary>
        /// Method to save updated FutureDated Change Detail Record
        /// </summary>
        /// <param name="fdcd"></param>
        void SaveFutureDatedChangeDetail(IFutureDatedChangeDetail fdcd);

        /// <summary>
        /// Delete FutureDateChange By Key
        /// </summary>
        /// <param name="futureDatedChangeKey"></param>
        /// <param name="includeSameDayTransactions"></param>
        void DeleteFutureDateChangeByKey(int futureDatedChangeKey, bool includeSameDayTransactions);

        /// <summary>
        /// Creates and Future Dated Change Object
        /// </summary>
        /// <returns>IFutureDatedChange</returns>
        IFutureDatedChange CreateEmptyFutureDatedChange();

        /// <summary>
        /// Creates and Future Dated Change Detail Object
        /// </summary>
        /// <returns>IFutureDatedChange</returns>
        IFutureDatedChangeDetail CreateEmptyFutureDatedChangeDetail();

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        Hashtable FutureDatedChangeMap(int FinancialServiceKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        Hashtable FutureDatedChangeDetailMap(int FinancialServiceKey);
    }
}