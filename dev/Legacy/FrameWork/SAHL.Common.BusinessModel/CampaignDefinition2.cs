using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO
    /// </summary>
    public partial class CampaignDefinition : BusinessModelBase<CampaignDefinition_DAO>, ICampaignDefinition
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargets_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargets_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargets_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.CampaignTargets
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnCampaignTargets_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildCampaignDefinitions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildCampaignDefinitions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildCampaignDefinitions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CampaignDefinition_DAO.ChildCampaignDefinitions
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildCampaignDefinitions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}