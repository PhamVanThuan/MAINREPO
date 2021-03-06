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
    
    
    [ActiveRecord("SecurityGroup", Schema="X2")]
    public partial class SecurityGroup : ActiveRecordBase<SecurityGroup> {
        
        private bool _isDynamic;
        
        private string _name;
        
        private string _description;
        
        private int _processID;
        
        private int _workFlowID;
        
        private int _iD;
        
        [Property("IsDynamic", ColumnType="Boolean", NotNull=true)]
        public virtual bool IsDynamic {
            get {
                return this._isDynamic;
            }
            set {
                this._isDynamic = value;
            }
        }
        
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
        
        [Property("ProcessID", ColumnType="Int32")]
        public virtual int ProcessID {
            get {
                return this._processID;
            }
            set {
                this._processID = value;
            }
        }
        
        [Property("WorkFlowID", ColumnType="Int32")]
        public virtual int WorkFlowID {
            get {
                return this._workFlowID;
            }
            set {
                this._workFlowID = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType="Int32")]
        public virtual int ID {
            get {
                return this._iD;
            }
            set {
                this._iD = value;
            }
        }
    }
}
