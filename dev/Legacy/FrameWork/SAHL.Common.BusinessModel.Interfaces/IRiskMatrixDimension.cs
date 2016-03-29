using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO
    /// </summary>
    public partial interface IRiskMatrixDimension : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.ScoreCards
        /// </summary>
        IEventList<IScoreCard> ScoreCards
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.RiskMatrixRanges
        /// </summary>
        IEventList<IRiskMatrixRange> RiskMatrixRanges
        {
            get;
        }
    }
}