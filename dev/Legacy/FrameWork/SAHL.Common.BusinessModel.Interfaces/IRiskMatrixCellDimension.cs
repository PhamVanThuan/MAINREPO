using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO
    /// </summary>
    public partial interface IRiskMatrixCellDimension : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.RiskMatrixCell
        /// </summary>
        IRiskMatrixCell RiskMatrixCell
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.RiskMatrixDimension
        /// </summary>
        IRiskMatrixDimension RiskMatrixDimension
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.RiskMatrixRange
        /// </summary>
        IRiskMatrixRange RiskMatrixRange
        {
            get;
            set;
        }
    }
}