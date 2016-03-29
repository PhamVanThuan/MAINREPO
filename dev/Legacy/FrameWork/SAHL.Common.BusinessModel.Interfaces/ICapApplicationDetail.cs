using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// CapOfferDetail_DAO is instantiated in order to represent the detailed information regarding the 3 different CAPOffers
    /// which the client is offered.
    /// </summary>
    public partial interface ICapApplicationDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The CAP effective rate. This is the rate which the client is being offered to CAP their loan at. This rate will either
        /// be 1%, 2% or 3% above their current rate.
        /// </summary>
        System.Double EffectiveRate
        {
            get;
            set;
        }

        /// <summary>
        /// This is the loan instalment which will be due by the client after the CAP 2 Readvance has been performed.
        /// </summary>
        System.Double Payment
        {
            get;
            set;
        }

        /// <summary>
        /// This the amount that the client is paying for the CAP 2 product.
        /// </summary>
        System.Double Fee
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CapOffer was accepted.
        /// </summary>
        DateTime? AcceptanceDate
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the client decided to not take up the CapOffer.
        /// </summary>
        DateTime? CapNTUReasonDate
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CapOfferDetail record was last updated.
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the person who last updated the CapOfferDetail record.
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the Reason table. Each CapOfferDetail record that is NTU'd by the client requires a Reason
        /// for the NTU decision. The CapOfferDetail can only belong to a single Reason.
        /// </summary>
        ICapNTUReason CapNTUReason
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the CapOffer table. Each CapOfferDetail record belongs to a single CapOffer record.
        /// </summary>
        ICapApplication CapApplication
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the CapStatus table. Each CapOfferDetail record can have only one status which changes
        /// throughout the life of the loan.
        /// </summary>
        ICapStatus CapStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the CapTypeConfigurationDetail table. Each CapOfferDetail record belongs to a
        /// CapTypeConfigurationDetail record. This is dependent on the type of the CapOffer. i.e. The 1%
        /// </summary>
        ICapTypeConfigurationDetail CapTypeConfigurationDetail
        {
            get;
            set;
        }
    }
}