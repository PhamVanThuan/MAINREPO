using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO
    /// </summary>
    public partial interface IRiskMatrixDimensionScoreCard : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO.RiskMatrixDimension
        /// </summary>
        IRiskMatrixDimension RiskMatrixDimension
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO.ScoreCard
        /// </summary>
        IScoreCard ScoreCard
        {
            get;
            set;
        }
    }
}