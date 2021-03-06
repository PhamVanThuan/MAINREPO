﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
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
    
    
    [ActiveRecord("RuleItem", Lazy=true, Schema="dbo")]
    public partial class RuleItem : ActiveRecordBase<RuleItem> {
        
        private string _name;
        
        private string _description;
        
        private int _ruleItemKey;
        
        private IList<RuleSetRule> _ruleSetRules;
        
        private IList<RuleParameter> _ruleParameters;
        
        [Property("Name", ColumnType="String", NotNull=true)]
        public virtual string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
        
        [Property("Description", ColumnType="String")]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "RuleItemKey", ColumnType="Int32")]
        public virtual int RuleItemKey {
            get {
                return this._ruleItemKey;
            }
            set {
                this._ruleItemKey = value;
            }
        }
        
        [HasMany(typeof(RuleSetRule), ColumnKey="RuleItemKey", Table="RuleSetRule")]
        public virtual IList<RuleSetRule> RuleSetRules {
            get {
                return this._ruleSetRules;
            }
            set {
                this._ruleSetRules = value;
            }
        }
        
        [HasMany(typeof(RuleParameter), ColumnKey="RuleItemKey", Table="RuleParameter")]
        public virtual IList<RuleParameter> RuleParameters {
            get {
                return this._ruleParameters;
            }
            set {
                this._ruleParameters = value;
            }
        }
    }
    
    [ActiveRecord("RuleSetRule", Lazy=true, Schema="dbo")]
    public partial class RuleSetRule : ActiveRecordBase<RuleSetRule> {
        
        private bool _enForceRule;
        
        private int _ruleSetRuleKey;
        
        private RuleItem _ruleItem;
        
        private RuleSet _ruleSet;
        
        [Property("EnForceRule", ColumnType="Boolean", NotNull=true)]
        public virtual bool EnForceRule {
            get {
                return this._enForceRule;
            }
            set {
                this._enForceRule = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "RuleSetRuleKey", ColumnType="Int32")]
        public virtual int RuleSetRuleKey {
            get {
                return this._ruleSetRuleKey;
            }
            set {
                this._ruleSetRuleKey = value;
            }
        }
        
        [BelongsTo("RuleItemKey", NotNull=false)]
        public virtual RuleItem RuleItem {
            get {
                return this._ruleItem;
            }
            set {
                this._ruleItem = value;
            }
        }
        
        [BelongsTo("RuleSetKey", NotNull=false)]
        public virtual RuleSet RuleSet {
            get {
                return this._ruleSet;
            }
            set {
                this._ruleSet = value;
            }
        }
    }
    
    [ActiveRecord("RuleSet", Lazy=true, Schema="dbo")]
    public partial class RuleSet : ActiveRecordBase<RuleSet> {
        
        private string _name;
        
        private string _description;
        
        private int _ruleSetKey;
        
        private IList<RuleSetRule> _ruleSetRules;
        
        [Property("Name", ColumnType="String", NotNull=true)]
        public virtual string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
        
        [Property("Description", ColumnType="String")]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "RuleSetKey", ColumnType="Int32")]
        public virtual int RuleSetKey {
            get {
                return this._ruleSetKey;
            }
            set {
                this._ruleSetKey = value;
            }
        }
        
        [HasMany(typeof(RuleSetRule), ColumnKey="RuleSetKey", Table="RuleSetRule")]
        public virtual IList<RuleSetRule> RuleSetRules {
            get {
                return this._ruleSetRules;
            }
            set {
                this._ruleSetRules = value;
            }
        }
    }
    
    [ActiveRecord("RuleParameter", Lazy=true, Schema="dbo")]
    public partial class RuleParameter : ActiveRecordBase<RuleParameter> {
        
        private string _name;
        
        private string _value;
        
        private int _ruleParameterKey;
        
        private RuleItem _ruleItem;
        
        private RuleParameterType _ruleParameterType;
        
        [Property("Name", ColumnType="String", NotNull=true)]
        public virtual string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
        
        [Property("Value", ColumnType="String")]
        public virtual string Value {
            get {
                return this._value;
            }
            set {
                this._value = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "RuleParameterKey", ColumnType="Int32")]
        public virtual int RuleParameterKey {
            get {
                return this._ruleParameterKey;
            }
            set {
                this._ruleParameterKey = value;
            }
        }
        
        [BelongsTo("RuleItemKey", NotNull=false)]
        public virtual RuleItem RuleItem {
            get {
                return this._ruleItem;
            }
            set {
                this._ruleItem = value;
            }
        }
        
        [BelongsTo("RuleParameterTypeKey", NotNull=false)]
        public virtual RuleParameterType RuleParameterType {
            get {
                return this._ruleParameterType;
            }
            set {
                this._ruleParameterType = value;
            }
        }
    }
    
    [ActiveRecord("RuleParameterType", Lazy=true, Schema="dbo")]
    public partial class RuleParameterType : ActiveRecordBase<RuleParameterType> {
        
        private string _description;
        
        private int _ruleParameterTypeKey;
        
        private IList<RuleParameter> _ruleParameters;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "RuleParameterTypeKey", ColumnType="Int32")]
        public virtual int RuleParameterTypeKey {
            get {
                return this._ruleParameterTypeKey;
            }
            set {
                this._ruleParameterTypeKey = value;
            }
        }
        
        [HasMany(typeof(RuleParameter), ColumnKey="RuleParameterTypeKey", Table="RuleParameter")]
        public virtual IList<RuleParameter> RuleParameters {
            get {
                return this._ruleParameters;
            }
            set {
                this._ruleParameters = value;
            }
        }
    }
}
