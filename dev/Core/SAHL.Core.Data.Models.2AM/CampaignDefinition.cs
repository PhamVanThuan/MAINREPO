using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CampaignDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CampaignDefinitionDataModel(string campaignName, string campaignReference, DateTime? startdate, DateTime? endDate, int? marketingOptionKey, int organisationStructureKey, int generalStatusKey, int? campaignDefinitionParentKey, int? reportStatementKey, int aDUserKey, int dataProviderDataServiceKey, int marketingOptionRelevanceKey)
        {
            this.CampaignName = campaignName;
            this.CampaignReference = campaignReference;
            this.Startdate = startdate;
            this.EndDate = endDate;
            this.MarketingOptionKey = marketingOptionKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.GeneralStatusKey = generalStatusKey;
            this.CampaignDefinitionParentKey = campaignDefinitionParentKey;
            this.ReportStatementKey = reportStatementKey;
            this.ADUserKey = aDUserKey;
            this.DataProviderDataServiceKey = dataProviderDataServiceKey;
            this.MarketingOptionRelevanceKey = marketingOptionRelevanceKey;
		
        }
		[JsonConstructor]
        public CampaignDefinitionDataModel(int campaignDefinitionKey, string campaignName, string campaignReference, DateTime? startdate, DateTime? endDate, int? marketingOptionKey, int organisationStructureKey, int generalStatusKey, int? campaignDefinitionParentKey, int? reportStatementKey, int aDUserKey, int dataProviderDataServiceKey, int marketingOptionRelevanceKey)
        {
            this.CampaignDefinitionKey = campaignDefinitionKey;
            this.CampaignName = campaignName;
            this.CampaignReference = campaignReference;
            this.Startdate = startdate;
            this.EndDate = endDate;
            this.MarketingOptionKey = marketingOptionKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.GeneralStatusKey = generalStatusKey;
            this.CampaignDefinitionParentKey = campaignDefinitionParentKey;
            this.ReportStatementKey = reportStatementKey;
            this.ADUserKey = aDUserKey;
            this.DataProviderDataServiceKey = dataProviderDataServiceKey;
            this.MarketingOptionRelevanceKey = marketingOptionRelevanceKey;
		
        }		

        public int CampaignDefinitionKey { get; set; }

        public string CampaignName { get; set; }

        public string CampaignReference { get; set; }

        public DateTime? Startdate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MarketingOptionKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int? CampaignDefinitionParentKey { get; set; }

        public int? ReportStatementKey { get; set; }

        public int ADUserKey { get; set; }

        public int DataProviderDataServiceKey { get; set; }

        public int MarketingOptionRelevanceKey { get; set; }

        public void SetKey(int key)
        {
            this.CampaignDefinitionKey =  key;
        }
    }
}