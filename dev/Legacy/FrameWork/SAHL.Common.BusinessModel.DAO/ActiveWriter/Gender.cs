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
    
    
    [ActiveRecord("Gender", Schema="dbo")]
    public partial class Gender : ActiveRecordBase<Gender> {
        
        private string _description;
        
        private int _genderKey;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "GenderKey", ColumnType="Int32")]
        public virtual int GenderKey {
            get {
                return this._genderKey;
            }
            set {
                this._genderKey = value;
            }
        }
    }
}
