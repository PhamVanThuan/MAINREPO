using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformationQuickCash_DAO is instantiated in order to retrieve information regarding the Quick Cash Application
    /// associated to the Mortgage Loan Application.
    /// </summary>
    public partial interface IApplicationInformationQuickCash : IEntityValidation, IBusinessModelObject
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
        /// The total amount of Quick Cash which Credit has approved the client for.
        /// </summary>
        System.Double CreditApprovedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// The Term of the Quick Cash loan.
        /// </summary>
        System.Int32 Term
        {
            get;
            set;
        }

        /// <summary>
        /// The amount that Credit has approved as the maximum allowed for the Up Front payment to the Client.
        /// </summary>
        System.Double CreditUpfrontApprovedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// A OfferInformationQuickCash record has many records associated to it in the OfferInformationQuickCashDetails table.
        /// </summary>
        IEventList<IApplicationInformationQuickCashDetail> ApplicationInformationQuickCashDetails
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCash_DAO.ApplicationInformation
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }
    }
}