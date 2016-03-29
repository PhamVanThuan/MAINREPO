using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTest]
    [ActiveRecord("CampaignTarget", Schema = "dbo", Lazy = true)]
    public partial class CampaignTarget_DAO : DB_2AM<CampaignTarget_DAO>
    {
        private int _key;

        private int _genericKey;

        private int _aDUserKey;

        private int _genericKeyTypeKey;

        private IList<CampaignTargetContact_DAO> _campaignTargetContacts;

        private CampaignDefinition_DAO _campaignDefinition;

        [PrimaryKey(PrimaryKeyType.Native, "CampaignTargetKey", ColumnType = "Int32")]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [Property("ADUserKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ADUserKey
        {
            get
            {
                return this._aDUserKey;
            }
            set
            {
                this._aDUserKey = value;
            }
        }

        [Property("GenericKeyTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKeyTypeKey
        {
            get
            {
                return this._genericKeyTypeKey;
            }
            set
            {
                this._genericKeyTypeKey = value;
            }
        }

        [HasMany(typeof(CampaignTargetContact_DAO), ColumnKey = "CampaignTargetKey", Table = "CampaignTargetContact", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CampaignTargetContact_DAO> CampaignTargetContacts
        {
            get
            {
                return this._campaignTargetContacts;
            }
            set
            {
                this._campaignTargetContacts = value;
            }
        }

        [BelongsTo("CampaignDefinitionKey", NotNull = true)]
        public virtual CampaignDefinition_DAO CampaignDefinition
        {
            get
            {
                return this._campaignDefinition;
            }
            set
            {
                this._campaignDefinition = value;
            }
        }
    }
}