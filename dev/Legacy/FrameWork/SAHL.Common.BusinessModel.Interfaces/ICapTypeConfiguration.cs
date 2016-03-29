using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// CapTypeConfiguration_DAO is used when creating a new CAP Sales Configuration for a CAP Selling Period.
    /// </summary>
    public partial interface ICapTypeConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The date on which the CAP Selling Period begins.
        /// </summary>
        System.DateTime ApplicationStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CAP Selling Period ends.
        /// </summary>
        System.DateTime ApplicationEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// The status of the Sales Configuration
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CAP sold will become effective. This is on the next reset date the client reaches after
        /// accepting the CAP Offer.
        /// </summary>
        System.DateTime CapEffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// The Date on which the CAP ends. This is currently set at 24 months after the CapEffectiveDate.
        /// </summary>
        System.DateTime CapClosureDate
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the ResetConfiguration table, where the details regarding the next reset dates are stored.
        /// Each CapTypeConfiguration belongs to a single ResetConfiguration that determines whether the CAP is sold to 21st or 18th
        /// reset clients.
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// The ResetDate which is applicable for the Cap Sales Configuration.
        /// </summary>
        System.DateTime ResetDate
        {
            get;
            set;
        }

        /// <summary>
        /// The term of the CAP. Currently this is 24 months.
        /// </summary>
        System.Int32 Term
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the CAP Configuration records were last changed.
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the person who last updated the CAP Configuration records.
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapTypeConfiguration_DAO.NACQDiscount
        /// </summary>
        Double? NACQDiscount
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
        /// Each CapTypeConfiguration has many detail records in the CapTypeConfigurationDetail, where the individual admin fees/premiums
        /// per CapType for the sales configuration is stored.
        /// </summary>
        IEventList<ICapTypeConfigurationDetail> CapTypeConfigurationDetails
        {
            get;
        }
    }
}