using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Financial Adjustment
    /// </summary>
    public partial interface IFinancialAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        ///
        /// </summary>
        IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource { get; }

        /// <summary>
        ///
        /// </summary>
        int Term { get; }

        /// <summary>
        ///
        /// </summary>
        string Value { get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}