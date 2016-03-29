using System;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICampaignRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICampaignDefinition CreateEmptyCampaignDefinition();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICampaignTarget CreateEmptyCampaignTarget();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICampaignTargetContact CreateEmptyCampaignTargetContact();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICampaignTargetResponse CreateEmptyCampaignTargetResponse();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICampaignDefinition GetCampaignDefinitionByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CampaignName"></param>
        /// <returns></returns>
        IList<ICampaignDefinition> GetCampaignDefinitionByName(string CampaignName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CampaignName"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="CampaignReference"></param>
        /// <returns></returns>
        IList<ICampaignDefinition> GetCampaignDefinitionByNameAndConfiguration(string CampaignName, DateTime StartDate,
                                                        DateTime EndDate, string CampaignReference);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICampaignTargetResponse GetCampaignTargetResponseByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="campaignDefinition"></param>
        void SaveCampaignDefinition(ICampaignDefinition campaignDefinition);
    }
}