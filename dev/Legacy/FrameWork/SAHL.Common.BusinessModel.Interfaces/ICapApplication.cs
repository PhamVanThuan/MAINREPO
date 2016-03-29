using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// CapOffer_DAO is instantiated in order to represent a CapOffer in the system, where a client will be offered the CAP 2 product.
    /// </summary>
    public partial interface ICapApplication : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The remaining instalments on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        System.Int32 RemainingInstallments
        {
            get;
            set;
        }

        /// <summary>
        /// The outstanding balance on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        System.Double CurrentBalance
        {
            get;
            set;
        }

        /// <summary>
        /// The current instalment due on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        System.Double CurrentInstallment
        {
            get;
            set;
        }

        /// <summary>
        /// The loan link rate at the time of the CapOffer being calculated.
        /// </summary>
        System.Double LinkRate
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CapOffer was calculated.
        /// </summary>
        System.DateTime ApplicationDate
        {
            get;
            set;
        }

        /// <summary>
        /// An indicator as to whether the CAP is forming part of a promotion given to the client. In order to defend cancellations
        /// clients are offered a free 3% CAP.
        /// </summary>
        Boolean? Promotion
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CapOffer was last changed.
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the person who last changed the CapOffer.
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
        /// CapOffer_DAO has a one-to-many relationship to the CapOfferDetail_DAO. Each CapOffer record has many CapOfferDetail
        /// records in the database, one for each of the 1%/2%/3% applications made available to the client.
        /// </summary>
        IEventList<ICapApplicationDetail> CapApplicationDetails
        {
            get;
        }

        /// <summary>
        /// The foreign key reference to the Account table. Each CapOffer can only belong to one Account.
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the Broker table. Each CapOffer can only belong to a single Broker at a time. This broker can change
        /// throughout the CapOffer process.
        /// </summary>
        IBroker Broker
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the CapStatus table. Each CapOffer can only belong to a single CapStatus, which changes
        /// throughout the life of the CapOffer.
        /// </summary>
        ICapStatus CapStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the CapTypeConfiguration table. Each CapOffer can only belong to a single CapTypeConfiguration
        /// where information regarding the sales configuration for the CAP product is maintained.
        /// </summary>
        ICapTypeConfiguration CapTypeConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapApplication_DAO.CAPPaymentOption
        /// </summary>
        ICapPaymentOption CAPPaymentOption
        {
            get;
            set;
        }
    }
}