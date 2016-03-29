using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformationQuickCashDetail_DAO is instantiated in order to retrieve the details of the Quick Cash Payments associated
    /// to the Quick Cash Application.
    /// </summary>
    public partial interface IApplicationInformationQuickCashDetail : IEntityValidation, IBusinessModelObject
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
        /// The Interest Rate applicable to the Quick Cash Payment
        /// </summary>
        Double? InterestRate
        {
            get;
            set;
        }

        /// <summary>
        /// The Requested Amount for the particular Quick Cash Payment.
        /// </summary>
        Double? RequestedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// The Rate Configuration which applies to the Quick Cash Payment. This allows you to determine the margin and market
        /// rate for the Quick Cash Payment.
        /// </summary>
        IRateConfiguration RateConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// An indicator as to whether the Quick Cash payment has been disbursed.
        /// </summary>
        Boolean? Disbursed
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO.QuickCashPaymentType
        /// </summary>
        IQuickCashPaymentType QuickCashPaymentType
        {
            get;
            set;
        }

        /// <summary>
        /// Each of the OfferInformationQuickCashDetail records belong to a single OfferInformationQuickCash key.
        /// </summary>
        IApplicationInformationQuickCash OfferInformationQuickCash
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO.ApplicationExpenses
        /// </summary>
        IEventList<IApplicationExpense> ApplicationExpenses
        {
            get;
        }
    }
}