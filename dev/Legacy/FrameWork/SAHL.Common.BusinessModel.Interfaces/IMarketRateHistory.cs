using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO
    /// </summary>
    public partial interface IMarketRateHistory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.RateBefore
        /// </summary>
        System.Double RateBefore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.RateAfter
        /// </summary>
        System.Double RateAfter
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangedBy
        /// </summary>
        System.String ChangedBy
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangedByHost
        /// </summary>
        System.String ChangedByHost
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangedByApp
        /// </summary>
        System.String ChangedByApp
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.MarketRate
        /// </summary>
        IMarketRate MarketRate
        {
            get;
            set;
        }
    }
}