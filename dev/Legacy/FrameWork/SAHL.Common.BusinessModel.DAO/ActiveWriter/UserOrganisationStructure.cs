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
    
    
    [ActiveRecord("UserOrganisationStructure", Schema="dbo")]
    public partial class UserOrganisationStructure : ActiveRecordBase<UserOrganisationStructure> {
        
        private int _aDUserKey;
        
        private int _organisationStructureKey;
        
        private int _userOrganisationStructureKey;
        
        [Property("ADUserKey", ColumnType="Int32", NotNull=true)]
        public virtual int ADUserKey {
            get {
                return this._aDUserKey;
            }
            set {
                this._aDUserKey = value;
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
        
        [PrimaryKey(PrimaryKeyType.Native, "UserOrganisationStructureKey", ColumnType="Int32")]
        public virtual int UserOrganisationStructureKey {
            get {
                return this._userOrganisationStructureKey;
            }
            set {
                this._userOrganisationStructureKey = value;
            }
        }
    }
}
