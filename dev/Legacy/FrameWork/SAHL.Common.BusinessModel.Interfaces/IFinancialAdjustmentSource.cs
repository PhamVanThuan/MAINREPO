using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentSource_DAO
    /// </summary>
    public partial interface IFinancialAdjustmentSource : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentSource_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentSource_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
        }
    }
}