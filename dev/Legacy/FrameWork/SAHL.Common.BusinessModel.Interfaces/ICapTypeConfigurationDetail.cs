using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// CapTypeConfigurationDetail_DAO is created in order to store the detailed sales configurations for each of the
    /// CapTypes.
    /// </summary>
    public partial interface ICapTypeConfigurationDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The CAP Base Rate for the CapOffer. This is calculated as JIBAR as of the last rate reset + the margin for the CAP Type.
        /// e.g. For the CAP sales period commencing 18/01/08 and ending 18/04/08, a 2% CAP would have a CAP Base Rate of 13.30%,
        /// which is JIBAR as of the 18/01/08 reset (11.30%) + the 2% margin.
        /// </summary>
        System.Double Rate
        {
            get;
            set;
        }

        /// <summary>
        /// The status of the CapTypeConfigurationDetail
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The total premium (FeePremium + AdminPremium)  payable for the CAP product. This is expressed per rand that the
        /// client wishes to CAP
        /// </summary>
        System.Double Premium
        {
            get;
            set;
        }

        /// <summary>
        /// The Premium Fee payable for the CAP product. This is expressed per rand that the client wishes to CAP.
        /// </summary>
        System.Double FeePremium
        {
            get;
            set;
        }

        /// <summary>
        /// The Administration Fee payable for the CAP product. This is expressed per rand that the client wishes to CAP.
        /// </summary>
        System.Double FeeAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// This the strike rate for the CAP, which is related to the trade bought for the CAP.
        /// </summary>
        System.Double RateFinance
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CapTypeConfigurationDetail records were last changed.
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the user who last changed the CapTypeConfigurationDetail records.
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
        /// A foreign key reference to the CapTypeConfigurationDetailKey is stored against the CapOfferDetail record. Each of the
        /// CapOffers will have a 1%,2% and 3% CapOfferDetail record which is then linked back to the Sales Configuration via this
        /// one-to-many relationship.
        /// </summary>
        IEventList<ICapApplicationDetail> CapApplicationDetails
        {
            get;
        }

        /// <summary>
        /// Each CapTypeConfigurationDetail record belongs to a CapType.
        /// </summary>
        ICapType CapType
        {
            get;
            set;
        }

        /// <summary>
        /// Each CapTypeConfigurationDetail belongs to a CapTypeConfiguration.
        /// </summary>
        ICapTypeConfiguration CapTypeConfiguration
        {
            get;
            set;
        }
    }
}