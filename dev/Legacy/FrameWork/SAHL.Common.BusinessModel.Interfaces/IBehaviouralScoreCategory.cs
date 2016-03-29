using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO
    /// </summary>
    public partial interface IBehaviouralScoreCategory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.BehaviouralScore
        /// </summary>
        System.Double BehaviouralScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BehaviouralScoreCategory_DAO.ThresholdColour
        /// </summary>
        System.String ThresholdColour
        {
            get;
            set;
        }
    }
}