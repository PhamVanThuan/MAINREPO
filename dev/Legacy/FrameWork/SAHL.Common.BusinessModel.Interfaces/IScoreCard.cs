using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO
    /// </summary>
    public partial interface IScoreCard : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.BasePoints
        /// </summary>
        System.Double BasePoints
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.RevisionDate
        /// </summary>
        System.DateTime RevisionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.ScoreCardAttributes
        /// </summary>
        IEventList<IScoreCardAttribute> ScoreCardAttributes
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.RiskMatrixDimensions
        /// </summary>
        IEventList<IRiskMatrixDimension> RiskMatrixDimensions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }
    }
}