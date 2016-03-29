using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentType_DAO
    /// </summary>
    public partial interface IOSPFinancialAdjustmentType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentType_DAO.OriginationSourceProduct
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OSPFinancialAdjustmentType_DAO.FinancialAdjustmentTypeSource
        /// </summary>
        IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource
        {
            get;
            set;
        }
    }
}