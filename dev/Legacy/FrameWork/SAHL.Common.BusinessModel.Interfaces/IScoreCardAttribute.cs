using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO
    /// </summary>
    public partial interface IScoreCardAttribute : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardKey
        /// </summary>
        IScoreCard ScoreCardKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.Code
        /// </summary>
        System.String Code
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
        /// </summary>
        IEventList<IScoreCardAttributeRange> ScoreCardAttributeRanges
        {
            get;
        }
    }
}