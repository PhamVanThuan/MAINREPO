using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformationVariFixLoan_DAO is instantiated in order to retrieve those details specific to a VariFix Loan
    /// Application.
    /// </summary>
    public partial interface IApplicationInformationVarifixLoan : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The Percentage of the Loan that the client wishes to fix.
        /// </summary>
        System.Double FixedPercent
        {
            get;
            set;
        }

        /// <summary>
        /// The Instalment due on the Fixed Portion of the Loan.
        /// </summary>
        System.Double FixedInstallment
        {
            get;
            set;
        }

        /// <summary>
        /// The date which the client elected to take the VariFix product.
        /// </summary>
        DateTime? ElectionDate
        {
            get;
            set;
        }

        /// <summary>
        /// The market rate key.
        /// </summary>
        IMarketRate MarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// The Conversion Status.
        /// </summary>
        System.Int32 ConversionStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationVarifixLoan_DAO.ApplicationInformation
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }
    }
}