using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO
    /// </summary>
    public partial interface ICampaignTarget : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.ADUserKey
        /// </summary>
        System.Int32 ADUserKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.GenericKeyTypeKey
        /// </summary>
        System.Int32 GenericKeyTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.CampaignTargetContacts
        /// </summary>
        IEventList<ICampaignTargetContact> CampaignTargetContacts
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTarget_DAO.CampaignDefinition
        /// </summary>
        ICampaignDefinition CampaignDefinition
        {
            get;
            set;
        }
    }
}