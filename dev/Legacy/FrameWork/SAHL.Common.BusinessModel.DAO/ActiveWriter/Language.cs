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
    
    
    [ActiveRecord("Language", Schema="dbo")]
    public partial class Language : ActiveRecordBase<Language> {
        
        private string _description;
        
        private bool _translatable;
        
        private int _languageKey;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [Property("Translatable", ColumnType="Boolean", NotNull=true)]
        public virtual bool Translatable {
            get {
                return this._translatable;
            }
            set {
                this._translatable = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "LanguageKey", ColumnType="Int32")]
        public virtual int LanguageKey {
            get {
                return this._languageKey;
            }
            set {
                this._languageKey = value;
            }
        }
    }
}
