﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Common.BusinessModel.ActiveWriter {
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Castle.ActiveRecord;
    
    
    [ActiveRecord("ContextMenu", Schema="dbo")]
    public partial class ContextMenu : ActiveRecordBase<ContextMenu> {
        
        private int _coreBusinessObjectKey;
        
        private string _description;
        
        private string _uRL;
        
        private int _featureKey;
        
        private int _sequence;
        
        private int _contextKey;
        
        private IList<ContextMenu> _contextMenus;
        
        private ContextMenu _contextMenu;
        
        [Property("CoreBusinessObjectKey", ColumnType="Int32")]
        public virtual int CoreBusinessObjectKey {
            get {
                return this._coreBusinessObjectKey;
            }
            set {
                this._coreBusinessObjectKey = value;
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
        
        [Property("URL", ColumnType="String")]
        public virtual string URL {
            get {
                return this._uRL;
            }
            set {
                this._uRL = value;
            }
        }
        
        [Property("FeatureKey", ColumnType="Int32", NotNull=true)]
        public virtual int FeatureKey {
            get {
                return this._featureKey;
            }
            set {
                this._featureKey = value;
            }
        }
        
        [Property("Sequence", ColumnType="Int32", NotNull=true)]
        public virtual int Sequence {
            get {
                return this._sequence;
            }
            set {
                this._sequence = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "ContextKey", ColumnType="Int32")]
        public virtual int ContextKey {
            get {
                return this._contextKey;
            }
            set {
                this._contextKey = value;
            }
        }
        
        [HasMany(typeof(ContextMenu), ColumnKey="ParentKey", Table="ContextMenu")]
        public virtual IList<ContextMenu> ContextMenus {
            get {
                return this._contextMenus;
            }
            set {
                this._contextMenus = value;
            }
        }
        
        [BelongsTo("ParentKey", NotNull=false)]
        public virtual ContextMenu ContextMenu {
            get {
                return this._contextMenu;
            }
            set {
                this._contextMenu = value;
            }
        }
    }
}