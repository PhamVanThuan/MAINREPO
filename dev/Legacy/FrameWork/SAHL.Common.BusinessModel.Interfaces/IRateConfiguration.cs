using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Rateconfiguration is used to expose available MarketRate / Margin combinations. Each mortgageLoan links to a RateConfiguration Entry,
    /// but a list of MortgageLoans is not exposed here as this is a lookup.
    /// </summary>
    public partial interface IRateConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// This is the primary key, used to identify an instance of RateConfiguration.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The margin.
        /// </summary>
        IMargin Margin
        {
            get;
            set;
        }

        /// <summary>
        /// The MarketRate.
        /// </summary>
        IMarketRate MarketRate
        {
            get;
            set;
        }
    }
}