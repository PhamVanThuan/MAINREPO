using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO
    /// </summary>
    public partial class BehaviouralScoreCategory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO>, IBehaviouralScoreCategory
    {
        public BehaviouralScoreCategory(SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO BehaviouralScoreCategory)
            : base(BehaviouralScoreCategory)
        {
            this._DAO = BehaviouralScoreCategory;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.BehaviouralScore
        /// </summary>
        public Double BehaviouralScore
        {
            get { return _DAO.BehaviouralScore; }
            set { _DAO.BehaviouralScore = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.ThresholdColour
        /// </summary>
        public String ThresholdColour
        {
            get { return _DAO.ThresholdColour; }
            set { _DAO.ThresholdColour = value; }
        }
    }
}