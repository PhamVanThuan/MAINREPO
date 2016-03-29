using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustmentTypeSource_DAO
    /// </summary>
    public partial interface IApplicationInformationFinancialAdjustmentTypeSource : IEntityValidation, IBusinessModelObject
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
        /// The rate at which the client has decided to CAP their Mortgage Loan. The CAP is invoked when the client's rate exceeds
        /// this rate.
        /// </summary>
        Double? CapRate
        {
            get;
            set;
        }

        /// <summary>
        /// The Outstanding Loan Balance when the client accepted the CAP Application. The CAP Rate applies to this balance.
        /// </summary>
        Double? CapBalance
        {
            get;
            set;
        }

        /// <summary>
        /// No rate override currently uses this property.
        /// </summary>
        Double? FloorRate
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustmentTypeSource_DAO.FromDate
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
        /// The ApplicationInformationRateOverride record belongs to an ApplicationInformation record.
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustmentTypeSource_DAO.FinancialAdjustmentTypeSource
        /// </summary>
        IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationFinancialAdjustmentTypeSource_DAO.ApplicationInformationAppliedRateAdjustments
        /// </summary>
        IEventList<IApplicationInformationAppliedRateAdjustment> ApplicationInformationAppliedRateAdjustments
        {
            get;
        }
    }
}