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
    
    
    [ActiveRecord("OriginationSource", Schema="dbo")]
    public partial class OriginationSource : ActiveRecordBase<OriginationSource> {
        
        private string _description;
        
        private int _originationSourceKey;
        
        private IList<OriginationSourceIcon> _originationSourceIcons;
        
        [Property("Description", ColumnType="String")]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "OriginationSourceKey", ColumnType="Int32")]
        public virtual int OriginationSourceKey {
            get {
                return this._originationSourceKey;
            }
            set {
                this._originationSourceKey = value;
            }
        }
        
        [HasMany(typeof(OriginationSourceIcon))]
        public virtual IList<OriginationSourceIcon> OriginationSourceIcons {
            get {
                return this._originationSourceIcons;
            }
            set {
                this._originationSourceIcons = value;
            }
        }
    }
    
    [ActiveRecord("OriginationSourceIcon", Schema="dbo")]
    public partial class OriginationSourceIcon : ActiveRecordBase<OriginationSourceIcon> {
        
        private int _originationSourceKey;
        
        private string _icon;
        
        private int _originationSourceIconKey;
        
        private OriginationSource _originationSource;
        
        [Property("OriginationSourceKey", ColumnType="Int32", NotNull=true)]
        public virtual int OriginationSourceKey {
            get {
                return this._originationSourceKey;
            }
            set {
                this._originationSourceKey = value;
            }
        }
        
        [Property("Icon", ColumnType="String", NotNull=true)]
        public virtual string Icon {
            get {
                return this._icon;
            }
            set {
                this._icon = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "OriginationSourceIconKey", ColumnType="Int32")]
        public virtual int OriginationSourceIconKey {
            get {
                return this._originationSourceIconKey;
            }
            set {
                this._originationSourceIconKey = value;
            }
        }
        
        [BelongsTo(NotNull=false)]
        public virtual OriginationSource OriginationSource {
            get {
                return this._originationSource;
            }
            set {
                this._originationSource = value;
            }
        }
    }
}
