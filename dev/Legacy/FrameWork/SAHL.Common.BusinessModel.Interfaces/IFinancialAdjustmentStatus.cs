using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentStatus_DAO
    /// </summary>
    public partial interface IFinancialAdjustmentStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentStatus_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentStatus_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
        }
    }
}