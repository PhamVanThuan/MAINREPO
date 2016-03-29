using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO
    /// </summary>
    public partial class CampaignTargetResponse : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO>, ICampaignTargetResponse
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.CampaignTargetContacts
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargetContacts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.CampaignTargetContacts
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargetContacts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.CampaignTargetContacts
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargetContacts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignTargetResponse_DAO.CampaignTargetContacts
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargetContacts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}