﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Common.BusinessModel {
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Castle.ActiveRecord;
    
    
    [ActiveRecord("MarketingOptionRelevance", Schema="dbo")]
    public partial class MarketingOptionRelevance : ActiveRecordBase<MarketingOptionRelevance> {
        
        private int _marketingOptionRelevanceKey;
        
        private string _description;
        
        private IList<CampaignDefinition> _campaignDefinitions;
        
        [PrimaryKey(PrimaryKeyType.Native, "MarketingOptionRelevanceKey", ColumnType="Int32")]
        public virtual int MarketingOptionRelevanceKey {
            get {
                return this._marketingOptionRelevanceKey;
            }
            set {
                this._marketingOptionRelevanceKey = value;
            }
        }
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [HasMany(typeof(CampaignDefinition), ColumnKey="MarketingOptionRelevanceKey", Table="CampaignDefinition")]
        public virtual IList<CampaignDefinition> CampaignDefinitions {
            get {
                return this._campaignDefinitions;
            }
            set {
                this._campaignDefinitions = value;
            }
        }
    }
    
    [ActiveRecord("CampaignDefinition", Schema="dbo")]
    public partial class CampaignDefinition : ActiveRecordBase<CampaignDefinition> {
        
        private int _campaignDefinitionKey;
        
        private string _campaignName;
        
        private string _campaignReference;
        
        private System.DateTime _startdate;
        
        private System.DateTime _endDate;
        
        private int _marketingOptionKey;
        
        private int _organisationStructureKey;
        
        private int _generalStatusKey;
        
        private int _reportStatementKey;
        
        private int _aDUserKey;
        
        private int _dataProviderDataServiceKey;
        
        private IList<CampaignDefinition> _campaignDefinitions;
        
        private IList<CampaignTarget> _campaignTargets;
        
        private MarketingOptionRelevance _marketingOptionRelevance;
        
        private CampaignDefinition _campaignDefinition;
        
        [PrimaryKey(PrimaryKeyType.Native, "CampaignDefinitionKey", ColumnType="Int32")]
        public virtual int CampaignDefinitionKey {
            get {
                return this._campaignDefinitionKey;
            }
            set {
                this._campaignDefinitionKey = value;
            }
        }
        
        [Property("CampaignName", ColumnType="String", NotNull=true)]
        public virtual string CampaignName {
            get {
                return this._campaignName;
            }
            set {
                this._campaignName = value;
            }
        }
        
        [Property("CampaignReference", ColumnType="String", NotNull=true)]
        public virtual string CampaignReference {
            get {
                return this._campaignReference;
            }
            set {
                this._campaignReference = value;
            }
        }
        
        [Property("Startdate", ColumnType="Timestamp")]
        public virtual System.DateTime Startdate {
            get {
                return this._startdate;
            }
            set {
                this._startdate = value;
            }
        }
        
        [Property("EndDate", ColumnType="Timestamp")]
        public virtual System.DateTime EndDate {
            get {
                return this._endDate;
            }
            set {
                this._endDate = value;
            }
        }
        
        [Property("MarketingOptionKey", ColumnType="Int32")]
        public virtual int MarketingOptionKey {
            get {
                return this._marketingOptionKey;
            }
            set {
                this._marketingOptionKey = value;
            }
        }
        
        [Property("OrganisationStructureKey", ColumnType="Int32", NotNull=true)]
        public virtual int OrganisationStructureKey {
            get {
                return this._organisationStructureKey;
            }
            set {
                this._organisationStructureKey = value;
            }
        }
        
        [Property("GeneralStatusKey", ColumnType="Int32", NotNull=true)]
        public virtual int GeneralStatusKey {
            get {
                return this._generalStatusKey;
            }
            set {
                this._generalStatusKey = value;
            }
        }
        
        [Property("ReportStatementKey", ColumnType="Int32")]
        public virtual int ReportStatementKey {
            get {
                return this._reportStatementKey;
            }
            set {
                this._reportStatementKey = value;
            }
        }
        
        [Property("ADUserKey", ColumnType="Int32", NotNull=true)]
        public virtual int ADUserKey {
            get {
                return this._aDUserKey;
            }
            set {
                this._aDUserKey = value;
            }
        }
        
        [Property("DataProviderDataServiceKey", ColumnType="Int32", NotNull=true)]
        public virtual int DataProviderDataServiceKey {
            get {
                return this._dataProviderDataServiceKey;
            }
            set {
                this._dataProviderDataServiceKey = value;
            }
        }
        
        [HasMany(typeof(CampaignDefinition), ColumnKey="CampaignDefinitionParentKey", Table="CampaignDefinition")]
        public virtual IList<CampaignDefinition> CampaignDefinitions {
            get {
                return this._campaignDefinitions;
            }
            set {
                this._campaignDefinitions = value;
            }
        }
        
        [HasMany(typeof(CampaignTarget), ColumnKey="CampaignDefinitionKey", Table="CampaignTarget")]
        public virtual IList<CampaignTarget> CampaignTargets {
            get {
                return this._campaignTargets;
            }
            set {
                this._campaignTargets = value;
            }
        }
        
        [BelongsTo("MarketingOptionRelevanceKey")]
        public virtual MarketingOptionRelevance MarketingOptionRelevance {
            get {
                return this._marketingOptionRelevance;
            }
            set {
                this._marketingOptionRelevance = value;
            }
        }
        
        [BelongsTo("CampaignDefinitionParentKey")]
        public virtual CampaignDefinition CampaignDefinition {
            get {
                return this._campaignDefinition;
            }
            set {
                this._campaignDefinition = value;
            }
        }
    }
    
    [ActiveRecord("CampaignTarget", Schema="dbo")]
    public partial class CampaignTarget : ActiveRecordBase<CampaignTarget> {
        
        private int _campaignTargetKey;
        
        private int _genericKey;
        
        private int _aDUserKey;
        
        private int _genericKeyTypeKey;
        
        private IList<CampaignTargetContact> _campaignTargetContacts;
        
        private CampaignDefinition _campaignDefinition;
        
        [PrimaryKey(PrimaryKeyType.Native, "CampaignTargetKey", ColumnType="Int32")]
        public virtual int CampaignTargetKey {
            get {
                return this._campaignTargetKey;
            }
            set {
                this._campaignTargetKey = value;
            }
        }
        
        [Property("GenericKey", ColumnType="Int32", NotNull=true)]
        public virtual int GenericKey {
            get {
                return this._genericKey;
            }
            set {
                this._genericKey = value;
            }
        }
        
        [Property("ADUserKey", ColumnType="Int32", NotNull=true)]
        public virtual int ADUserKey {
            get {
                return this._aDUserKey;
            }
            set {
                this._aDUserKey = value;
            }
        }
        
        [Property("GenericKeyTypeKey", ColumnType="Int32", NotNull=true)]
        public virtual int GenericKeyTypeKey {
            get {
                return this._genericKeyTypeKey;
            }
            set {
                this._genericKeyTypeKey = value;
            }
        }
        
        [HasMany(typeof(CampaignTargetContact), ColumnKey="CampaignTargetKey", Table="CampaignTargetContact")]
        public virtual IList<CampaignTargetContact> CampaignTargetContacts {
            get {
                return this._campaignTargetContacts;
            }
            set {
                this._campaignTargetContacts = value;
            }
        }
        
        [BelongsTo("CampaignDefinitionKey")]
        public virtual CampaignDefinition CampaignDefinition {
            get {
                return this._campaignDefinition;
            }
            set {
                this._campaignDefinition = value;
            }
        }
    }
    
    [ActiveRecord("CampaignTargetContact", Schema="dbo")]
    public partial class CampaignTargetContact : ActiveRecordBase<CampaignTargetContact> {
        
        private int _campaignTargetContactKey;
        
        private int _legalEntityKey;
        
        private System.DateTime _changeDate;
        
        private int _adUserKey;
        
        private CampaignTarget _campaignTarget;
        
        private CampaignTargetResponse _campaignTargetResponse;
        
        [PrimaryKey(PrimaryKeyType.Native, "CampaignTargetContactKey", ColumnType="Int32")]
        public virtual int CampaignTargetContactKey {
            get {
                return this._campaignTargetContactKey;
            }
            set {
                this._campaignTargetContactKey = value;
            }
        }
        
        [Property("LegalEntityKey", ColumnType="Int32", NotNull=true)]
        public virtual int LegalEntityKey {
            get {
                return this._legalEntityKey;
            }
            set {
                this._legalEntityKey = value;
            }
        }
        
        [Property("ChangeDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime ChangeDate {
            get {
                return this._changeDate;
            }
            set {
                this._changeDate = value;
            }
        }
        
        [Property("AdUserKey", ColumnType="Int32", NotNull=true)]
        public virtual int AdUserKey {
            get {
                return this._adUserKey;
            }
            set {
                this._adUserKey = value;
            }
        }
        
        [BelongsTo("CampaignTargetKey")]
        public virtual CampaignTarget CampaignTarget {
            get {
                return this._campaignTarget;
            }
            set {
                this._campaignTarget = value;
            }
        }
        
        [BelongsTo("CampaignTargetResponseKey")]
        public virtual CampaignTargetResponse CampaignTargetResponse {
            get {
                return this._campaignTargetResponse;
            }
            set {
                this._campaignTargetResponse = value;
            }
        }
    }
    
    [ActiveRecord("CampaignTargetResponse", Schema="dbo")]
    public partial class CampaignTargetResponse : ActiveRecordBase<CampaignTargetResponse> {
        
        private int _campaignTargetResponseKey;
        
        private string _description;
        
        private IList<CampaignTargetContact> _campaignTargetContacts;
        
        [PrimaryKey(PrimaryKeyType.Native, "CampaignTargetResponseKey", ColumnType="Int32")]
        public virtual int CampaignTargetResponseKey {
            get {
                return this._campaignTargetResponseKey;
            }
            set {
                this._campaignTargetResponseKey = value;
            }
        }
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [HasMany(typeof(CampaignTargetContact), ColumnKey="CampaignTargetResponseKey", Table="CampaignTargetContact")]
        public virtual IList<CampaignTargetContact> CampaignTargetContacts {
            get {
                return this._campaignTargetContacts;
            }
            set {
                this._campaignTargetContacts = value;
            }
        }
    }
    
    public class CampaignDefinitionHelper {
        
        public static Type[] GetTypes() {
            return new Type[] {
                    typeof(MarketingOptionRelevance),
                    typeof(CampaignDefinition),
                    typeof(CampaignTarget),
                    typeof(CampaignTargetContact),
                    typeof(CampaignTargetResponse)};
        }
    }
}
