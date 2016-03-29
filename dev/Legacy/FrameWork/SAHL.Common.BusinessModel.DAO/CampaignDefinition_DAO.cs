using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTest]
    [ActiveRecord("CampaignDefinition", Schema = "dbo", Lazy = true)]
    public partial class CampaignDefinition_DAO : DB_2AM<CampaignDefinition_DAO>
    {
        private int _key;

        private string _campaignName;

        private string _campaignReference;

        private DateTime? _startdate;

        private DateTime? _endDate;

        private int? _marketingOptionKey;

        private int _organisationStructureKey;

        private int _generalStatusKey;

        private ReportStatement_DAO _reportStatement;

        private int _aDUserKey;

        private int _dataProviderDataServiceKey;

        private int _marketingOptionRelevanceKey;

        private CampaignDefinition_DAO _parentCampaignDefinition;

        private IList<CampaignDefinition_DAO> _childCampaignDefinitions;

        private IList<CampaignTarget_DAO> _campaignTargets;

        [PrimaryKey(PrimaryKeyType.Native, "CampaignDefinitionKey", ColumnType = "Int32")]
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

        [Property("CampaignName", ColumnType = "String", NotNull = true)]
        public virtual string CampaignName
        {
            get
            {
                return this._campaignName;
            }
            set
            {
                this._campaignName = value;
            }
        }

        [Property("CampaignReference", ColumnType = "String", NotNull = true)]
        public virtual string CampaignReference
        {
            get
            {
                return this._campaignReference;
            }
            set
            {
                this._campaignReference = value;
            }
        }

        [Property("Startdate", ColumnType = "Timestamp")]
        public virtual DateTime? Startdate
        {
            get
            {
                return this._startdate;
            }
            set
            {
                this._startdate = value;
            }
        }

        [Property("EndDate", ColumnType = "Timestamp")]
        public virtual DateTime? EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        [Property("MarketingOptionKey", ColumnType = "Int32")]
        public virtual int? MarketingOptionKey
        {
            get
            {
                return this._marketingOptionKey;
            }
            set
            {
                this._marketingOptionKey = value;
            }
        }

        [Property("OrganisationStructureKey", ColumnType = "Int32", NotNull = true)]
        public virtual int OrganisationStructureKey
        {
            get
            {
                return this._organisationStructureKey;
            }
            set
            {
                this._organisationStructureKey = value;
            }
        }

        [Property("GeneralStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GeneralStatusKey
        {
            get
            {
                return this._generalStatusKey;
            }
            set
            {
                this._generalStatusKey = value;
            }
        }

        [BelongsTo("ReportStatementKey")]
        public virtual ReportStatement_DAO ReportStatement
        {
            get
            {
                return this._reportStatement;
            }
            set
            {
                this._reportStatement = value;
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

        [Property("DataProviderDataServiceKey", ColumnType = "Int32", NotNull = true)]
        public virtual int DataProviderDataServiceKey
        {
            get
            {
                return this._dataProviderDataServiceKey;
            }
            set
            {
                this._dataProviderDataServiceKey = value;
            }
        }

        [Property("MarketingOptionRelevanceKey", ColumnType = "Int32", NotNull = true)]
        public virtual int MarketingOptionRelevanceKey
        {
            get
            {
                return this._marketingOptionRelevanceKey;
            }
            set
            {
                this._marketingOptionRelevanceKey = value;
            }
        }

        [HasMany(typeof(CampaignTarget_DAO), ColumnKey = "CampaignDefinitionKey", Table = "CampaignTarget", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
        public virtual IList<CampaignTarget_DAO> CampaignTargets
        {
            get
            {
                return this._campaignTargets;
            }
            set
            {
                this._campaignTargets = value;
            }
        }

        [HasMany(typeof(CampaignDefinition_DAO), ColumnKey = "CampaignDefinitionParentKey", Table = "CampaignDefinition", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
        public virtual IList<CampaignDefinition_DAO> ChildCampaignDefinitions
        {
            get
            {
                return this._childCampaignDefinitions;
            }
            set
            {
                this._childCampaignDefinitions = value;
            }
        }

        [BelongsTo("CampaignDefinitionParentKey")]
        public virtual CampaignDefinition_DAO ParentCampaignDefinition
        {
            get
            {
                return this._parentCampaignDefinition;
            }
            set
            {
                this._parentCampaignDefinition = value;
            }
        }
    }
}