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
    
    
    [ActiveRecord("State", Schema="X2")]
    public partial class X2State : ActiveRecordBase<X2State> {
        
        private int _workFlowID;
        
        private string _name;
        
        private bool _forwardState;
        
        private IList<X2State> _states;
        
        private X2State _state;
        
        private StateType _stateType;
        
        [Property("WorkFlowID", ColumnType="Int32", NotNull=true)]
        public virtual int WorkFlowID {
            get {
                return this._workFlowID;
            }
            set {
                this._workFlowID = value;
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
        
        [Property("ForwardState", ColumnType="Boolean", NotNull=true)]
        public virtual bool ForwardState {
            get {
                return this._forwardState;
            }
            set {
                this._forwardState = value;
            }
        }
        
        [HasMany(typeof(X2State), ColumnKey="ID", Table="State")]
        public virtual IList<X2State> States {
            get {
                return this._states;
            }
            set {
                this._states = value;
            }
        }
        
        [BelongsTo("ID", NotNull=false)]
        public virtual X2State State {
            get {
                return this._state;
            }
            set {
                this._state = value;
            }
        }
        
        [BelongsTo("Type", NotNull=false)]
        public virtual StateType StateType {
            get {
                return this._stateType;
            }
            set {
                this._stateType = value;
            }
        }
    }
    
    [ActiveRecord("StateType", Schema="X2")]
    public partial class StateType : ActiveRecordBase<StateType> {
        
        private string _name;
        
        private int _iD;
        
        private IList<X2State> _states;
        
        [Property("Name", ColumnType="String", NotNull=true)]
        public virtual string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
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
        
        [HasMany(typeof(X2State), ColumnKey="Type", Table="State")]
        public virtual IList<X2State> States {
            get {
                return this._states;
            }
            set {
                this._states = value;
            }
        }
    }
}
