using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    //[DoNotTestWithGenericTest]
    [GenericTest(TestType.Find)]
    [ActiveRecord("MarketingOptionRelevance", Schema = "dbo", Lazy = true)]
    public partial class MarketingOptionRelevance_DAO : DB_2AM<MarketingOptionRelevance_DAO>
    {
        private int _key;

        private string _description;

        private IList<CampaignDefinition_DAO> _campaignDefinitions;

        [PrimaryKey(PrimaryKeyType.Assigned, "MarketingOptionRelevanceKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [Property("Description", ColumnType = "String", NotNull = true)]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [HasMany(typeof(CampaignDefinition_DAO), ColumnKey = "MarketingOptionRelevanceKey", Table = "CampaignDefinition")]
        public virtual IList<CampaignDefinition_DAO> CampaignDefinitions
        {
            get
            {
                return this._campaignDefinitions;
            }
            set
            {
                this._campaignDefinitions = value;
            }
        }
    }
}