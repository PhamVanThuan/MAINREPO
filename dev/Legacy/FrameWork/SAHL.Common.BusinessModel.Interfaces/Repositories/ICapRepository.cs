using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICapRepository
    {
        #region Create Methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICapApplication CreateCapApplication();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICapApplicationDetail CreateCapApplicationDetail();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICapTypeConfiguration CreateCapTypeConfiguration();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICapTypeConfigurationDetail CreateCapTypeConfigurationDetail();

        #endregion Create Methods

        #region Save Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="capTypeConfiguration"></param>
        void SaveCapTypeConfiguration(ICapTypeConfiguration capTypeConfiguration);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capTypeConfigurationDetail"></param>
        void SaveCapTypeConfigurationDetail(ICapTypeConfigurationDetail capTypeConfigurationDetail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        void SaveCapApplication(ICapApplication capApplication);

        #endregion Save Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        List<IAccount> CapAccountSearch(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        ICapApplication GetCapOfferByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IList<ICapApplication> GetCapOfferByAccountKey(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        ICapApplication GetCapApplicationFromInstance(IInstance instance);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="statusKey"></param>
        /// <returns></returns>
        IList<ICapApplication> GetCapOfferByAccountKeyAndStatus(int accountKey, int statusKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<ICapNTUReason> GetCapNTUReasons();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<ICapNTUReason> GetCapDeclineReasons();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<ICancellationReason> GetCapCancellationReasons();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetCurrentCAPResetConfigDates();

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetDate"></param>
        /// <returns></returns>
        IList<ICapTypeConfiguration> GetCapTypeConfigByResetDate(DateTime resetDate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capConfigKey"></param>
        /// <returns></returns>
        ICapTypeConfiguration GetCapTypeConfigByKey(int capConfigKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<ICapType> GetCapTypes();

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetDate"></param>
        /// <param name="resetConfigKey"></param>
        /// <returns></returns>
        IReset GetPreviousResetByResetDateAndRCKey(DateTime resetDate, int resetConfigKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<IBroker> GetCapBrokers();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<IBroker> GetCapCreditBrokers();

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerKey"></param>
        /// <returns></returns>
        DataTable GetCapOffersByBrokerKey(int brokerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerKey"></param>
        /// <returns></returns>
        DataTable GetCapCreditOffersByBrokerKey(int brokerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerKey"></param>
        /// <returns></returns>
        IBroker GetBrokerByBrokerKey(int brokerKey);

        /// <summary>
        /// Gets a broker by the full name.  This is used for the Cap Import report, and unfortunately doesn't take
        /// duplicate names into account - it just returns the first broker found matching the name.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        /// <remarks>This is not case-sensitive.</remarks>
        IBroker GetBrokerByFullName(string fullName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserKey"></param>
        /// <returns></returns>
        IBroker GetBrokerByADUserKey(int adUserKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <param name="brokerKey"></param>
        void UpdateX2CapCaseData(int capOfferKey, int brokerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <param name="brokerKey"></param>
        void UpdateX2CapCreditCaseData(int capOfferKey, int brokerKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IList<ICapApplication> GetAcceptedHistoryForCancel(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigKey"></param>
        /// <returns></returns>
        ICapTypeConfiguration GetCurrentCapTypeConfigByResetConfigKey(int resetConfigKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetNextCapCreditBroker();

        /// <summary>
        ///
        /// </summary>
        /// <param name="productKey"></param>
        /// <returns></returns>
        bool ProductQualifyForCap(int productKey);

        /// <summary>
        /// Returns all Cap Payment Options
        /// </summary>
        /// <returns></returns>
        IList<ICapPaymentOption> GetCapPaymentOptions();

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        /// <returns></returns>
        bool IsReAdvance(ICapApplication capApplication);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        /// <returns></returns>
        bool IsReAdvanceLAA(ICapApplication capApplication);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        /// <returns></returns>
        bool CheckLTVThreshold(ICapApplication capApplication);

        #region Trade Screen Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        DataTable GetResetDatesByTradeType(string tradeType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        DataTable GetResetDatesForAddingByTradeType(string tradeType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        DataTable GetTradeGroupingsByResetConfigurationKey(int resetConfigurationKey, string tradeType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        DataTable GetTradeGroupsByResetConfigKeyForDelete(int resetConfigurationKey, string tradeType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        DataTable GetActiveTradeGroupingsByResetConfigurationKey(int resetConfigurationKey, string tradeType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="capTypeKey"></param>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="closureDate"></param>
        /// <returns></returns>
        IList<ITrade> GetTradeByGrouping(int capTypeKey, int resetConfigurationKey, DateTime effectiveDate, DateTime closureDate);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<IResetConfiguration> GetResetConfigurations();

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigKey"></param>
        /// <returns></returns>
        IResetConfiguration GetResetConfigurationByKey(int resetConfigKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ITrade CreateTrade();

        /// <summary>
        ///
        /// </summary>
        /// <param name="trade"></param>
        void SaveTrade(ITrade trade);

        /// <summary>
        ///
        /// </summary>
        /// <param name="trade"></param>
        void RemoveTrade(ITrade trade);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IList<ICapType> GetCapTypesForTrade();

        #endregion Trade Screen Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="cancellationReasonKey"></param>
        /// <param name="userid"></param>
        void OptOutCAP(int accountkey, int cancellationReasonKey, string userid);

        /// <summary>
        /// Returns the CLV and Application Type for each CAP Type
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="capTypeConfigurationKey"></param>
        /// <param name="capOfferKey"></param>
        /// <param name="committedLoanValue"></param>
        /// <param name="appType1"></param>
        /// <param name="appType2"></param>
        /// <param name="appType3"></param>
        void CapTypeDetermineAppType(int accountKey, int capTypeConfigurationKey, int capOfferKey, out double committedLoanValue, out string appType1, out string appType2, out string appType3);
    }
}