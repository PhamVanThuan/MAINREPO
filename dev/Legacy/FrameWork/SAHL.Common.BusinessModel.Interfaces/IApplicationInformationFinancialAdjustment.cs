using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustment_DAO
    /// </summary>
    public partial interface IApplicationInformationFinancialAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Term applicable to the Rate Override e.g. A CAP Rate Override has a term of 24 months.
        /// </summary>
        Int32? Term
        {
            get;
            set;
        }

        /// <summary>
        /// No rate override currently uses this property.
        /// </summary>
        Double? FixedRate
        {
            get;
            set;
        }

        /// <summary>
        /// Certain Rate Overrides, such as Super Lo, require a discount to be given to the client. This is the value of the
        /// discount.
        /// </summary>
        Double? Discount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustment_DAO.FromDate
        /// </summary>
        DateTime? FromDate
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
        /// The ApplicationInformationFinancialAdjustment record belongs to an ApplicationInformation record.
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustment_DAO.FinancialAdjustmentTypeSource
        /// </summary>
        IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustment_DAO.ApplicationInformationAppliedRateAdjustments
        /// </summary>
        IEventList<IApplicationInformationAppliedRateAdjustment> ApplicationInformationAppliedRateAdjustments
        {
            get;
        }
    }
}