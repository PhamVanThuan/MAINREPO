using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentTypeSource_DAO
    /// </summary>
    public partial interface IOSPFinancialAdjustmentTypeSource : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentTypeSource_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentTypeSource_DAO.FinancialAdjustmentTypeSource
        /// </summary>
        IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentTypeSource_DAO.OriginationSourceProduct
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }
    }
}