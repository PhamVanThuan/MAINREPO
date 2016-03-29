using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO
    /// </summary>
    public partial interface ICampaignTargetContact : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.LegalEntityKey
        /// </summary>
        System.Int32 LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.AdUserKey
        /// </summary>
        System.Int32 AdUserKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.CampaignTarget
        /// </summary>
        ICampaignTarget CampaignTarget
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetContact_DAO.CampaignTargetResponse
        /// </summary>
        ICampaignTargetResponse CampaignTargetResponse
        {
            get;
            set;
        }
    }
}