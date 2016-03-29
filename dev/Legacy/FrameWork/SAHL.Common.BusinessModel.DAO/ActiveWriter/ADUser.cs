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
    
    
    [ActiveRecord("ADUser", Schema="dbo")]
    public partial class ADUser : ActiveRecordBase<ADUser> {
        
        private string _aDUserName;
        
        private int _generalStatusKey;
        
        private string _fullName;
        
        private string _initials;
        
        private string _telephoneNumber;
        
        private string _faxNumber;
        
        private string _emailAddress;
        
        private string _password;
        
        private string _passwordQuestion;
        
        private string _passwordAnswer;
        
        private int _aDUserKey;
        
        [Property("ADUserName", ColumnType="String", NotNull=true)]
        public virtual string ADUserName {
            get {
                return this._aDUserName;
            }
            set {
                this._aDUserName = value;
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
        
        [Property("FullName", ColumnType="String")]
        public virtual string FullName {
            get {
                return this._fullName;
            }
            set {
                this._fullName = value;
            }
        }
        
        [Property("Initials", ColumnType="String")]
        public virtual string Initials {
            get {
                return this._initials;
            }
            set {
                this._initials = value;
            }
        }
        
        [Property("TelephoneNumber", ColumnType="String")]
        public virtual string TelephoneNumber {
            get {
                return this._telephoneNumber;
            }
            set {
                this._telephoneNumber = value;
            }
        }
        
        [Property("FaxNumber", ColumnType="String")]
        public virtual string FaxNumber {
            get {
                return this._faxNumber;
            }
            set {
                this._faxNumber = value;
            }
        }
        
        [Property("EmailAddress", ColumnType="String")]
        public virtual string EmailAddress {
            get {
                return this._emailAddress;
            }
            set {
                this._emailAddress = value;
            }
        }
        
        [Property("Password", ColumnType="String")]
        public virtual string Password {
            get {
                return this._password;
            }
            set {
                this._password = value;
            }
        }
        
        [Property("PasswordQuestion", ColumnType="String")]
        public virtual string PasswordQuestion {
            get {
                return this._passwordQuestion;
            }
            set {
                this._passwordQuestion = value;
            }
        }
        
        [Property("PasswordAnswer", ColumnType="String")]
        public virtual string PasswordAnswer {
            get {
                return this._passwordAnswer;
            }
            set {
                this._passwordAnswer = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "ADUserKey", ColumnType="Int32")]
        public virtual int ADUserKey {
            get {
                return this._aDUserKey;
            }
            set {
                this._aDUserKey = value;
            }
        }
    }
}
