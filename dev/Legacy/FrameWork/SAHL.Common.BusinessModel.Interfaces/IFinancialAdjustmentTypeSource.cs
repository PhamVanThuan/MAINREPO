using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO
    /// </summary>
    public partial interface IFinancialAdjustmentTypeSource : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO.FinancialAdjustmentSource
        /// </summary>
        IFinancialAdjustmentSource FinancialAdjustmentSource
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO.FinancialAdjustmentType
        /// </summary>
        IFinancialAdjustmentType FinancialAdjustmentType
        {
            get;
        }
    }
}