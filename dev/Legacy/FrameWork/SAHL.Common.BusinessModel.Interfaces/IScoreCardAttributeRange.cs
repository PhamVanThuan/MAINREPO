using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO
    /// </summary>
    public partial interface IScoreCardAttributeRange : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Min
        /// </summary>
        Double? Min
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Max
        /// </summary>
        Double? Max
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Points
        /// </summary>
        System.Double Points
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.ScoreCardAttribute
        /// </summary>
        IScoreCardAttribute ScoreCardAttribute
        {
            get;
            set;
        }
    }
}