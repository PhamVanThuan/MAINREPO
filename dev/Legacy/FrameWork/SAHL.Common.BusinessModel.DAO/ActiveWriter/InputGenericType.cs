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
    
    
    [ActiveRecord("InputGenericType", Schema="dbo")]
    public partial class InputGenericType : ActiveRecordBase<InputGenericType> {
        
        private int _genericTypeKey;
        
        private int _coreBusinessObjectKey;
        
        private int _inputGenericTypeKey;
        
        [Property("GenericTypeKey", ColumnType="Int32", NotNull=true)]
        public virtual int GenericTypeKey {
            get {
                return this._genericTypeKey;
            }
            set {
                this._genericTypeKey = value;
            }
        }
        
        [Property("CoreBusinessObjectKey", ColumnType="Int32", NotNull=true)]
        public virtual int CoreBusinessObjectKey {
            get {
                return this._coreBusinessObjectKey;
            }
            set {
                this._coreBusinessObjectKey = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "InputGenericTypeKey", ColumnType="Int32")]
        public virtual int InputGenericTypeKey {
            get {
                return this._inputGenericTypeKey;
            }
            set {
                this._inputGenericTypeKey = value;
            }
        }
    }
}
