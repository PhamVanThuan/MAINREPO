using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MarketRate_DAO
    /// </summary>
    public partial interface IMarketRate : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The current value of this marketrate.  This is always the current marketrate, irrespective of resets.
        /// </summary>
        System.Double Value
        {
            get;
            set;
        }

        /// <summary>
        /// The description of this marketrate.
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// This is the primary key, used to identify an instance of Marketrate.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// A list that contains the history of this marketrate's changes through time.
        /// </summary>
        IEventList<IMarketRateHistory> MarketRateHistories
        {
            get;
        }

        /// <summary>
        /// A collection of all the OriginationSourceProductConfigurtation entries that use this Marketrate.
        /// </summary>
        IEventList<IRateConfiguration> RateConfigurations
        {
            get;
        }
    }
}