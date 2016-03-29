using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditAddress",  Schema = "dbo")]
    public partial class AuditAddress_DAO : DB_2AM<AuditAddress_DAO>
    {

        private string _auditLogin;

        private string _auditHostName;

        private string _auditProgramName;

        private System.DateTime _auditDate;

        private char _auditAddUpdateDelete;

        private int _addressKey;

        private int? _addressFormatKey;

        private string _boxNumber;

        private string _unitNumber;

        private string _buildingNumber;

        private string _buildingName;

        private string _streetNumber;

        private string _streetName;

        private int? _suburbKey;

        private int? _postOfficeKey;

        private string _rRR_CountryDescription;

        private string _rRR_ProvinceDescription;

        private string _rRR_CityDescription;

        private string _rRR_SuburbDescription;

        private string _rRR_PostalCode;

        private string _userID;

        private System.DateTime _changeDate;

        private string _suiteNumber;

        private string _freeText1;

        private string _freeText2;

        private string _freeText3;

        private string _freeText4;

        private string _freeText5;

        private decimal _key;

        [Property("AuditLogin", ColumnType = "String", NotNull = true)]
        public virtual string AuditLogin
        {
            get
            {
                return this._auditLogin;
            }
            set
            {
                this._auditLogin = value;
            }
        }

        [Property("AuditHostName", ColumnType = "String", NotNull = true)]
        public virtual string AuditHostName
        {
            get
            {
                return this._auditHostName;
            }
            set
            {
                this._auditHostName = value;
            }
        }

        [Property("AuditProgramName", ColumnType = "String", NotNull = true)]
        public virtual string AuditProgramName
        {
            get
            {
                return this._auditProgramName;
            }
            set
            {
                this._auditProgramName = value;
            }
        }

        [Property("AuditDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime AuditDate
        {
            get
            {
                return this._auditDate;
            }
            set
            {
                this._auditDate = value;
            }
        }

        [Property("AuditAddUpdateDelete", ColumnType = "AnsiChar", NotNull = true)]
        public virtual char AuditAddUpdateDelete
        {
            get
            {
                return this._auditAddUpdateDelete;
            }
            set
            {
                this._auditAddUpdateDelete = value;
            }
        }

        [Property("AddressKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AddressKey
        {
            get
            {
                return this._addressKey;
            }
            set
            {
                this._addressKey = value;
            }
        }

        [Property("AddressFormatKey", ColumnType = "Int32")]
        public virtual int? AddressFormatKey
        {
            get
            {
                return this._addressFormatKey;
            }
            set
            {
                this._addressFormatKey = value;
            }
        }

        [Property("BoxNumber", ColumnType = "String")]
        public virtual string BoxNumber
        {
            get
            {
                return this._boxNumber;
            }
            set
            {
                this._boxNumber = value;
            }
        }

        [Property("UnitNumber", ColumnType = "String")]
        public virtual string UnitNumber
        {
            get
            {
                return this._unitNumber;
            }
            set
            {
                this._unitNumber = value;
            }
        }

        [Property("BuildingNumber", ColumnType = "String")]
        public virtual string BuildingNumber
        {
            get
            {
                return this._buildingNumber;
            }
            set
            {
                this._buildingNumber = value;
            }
        }

        [Property("BuildingName", ColumnType = "String")]
        public virtual string BuildingName
        {
            get
            {
                return this._buildingName;
            }
            set
            {
                this._buildingName = value;
            }
        }

        [Property("StreetNumber", ColumnType = "String")]
        public virtual string StreetNumber
        {
            get
            {
                return this._streetNumber;
            }
            set
            {
                this._streetNumber = value;
            }
        }

        [Property("StreetName", ColumnType = "String")]
        public virtual string StreetName
        {
            get
            {
                return this._streetName;
            }
            set
            {
                this._streetName = value;
            }
        }

        [Property("SuburbKey", ColumnType = "Int32")]
        public virtual int? SuburbKey
        {
            get
            {
                return this._suburbKey;
            }
            set
            {
                this._suburbKey = value;
            }
        }

        [Property("PostOfficeKey", ColumnType = "Int32")]
        public virtual int? PostOfficeKey
        {
            get
            {
                return this._postOfficeKey;
            }
            set
            {
                this._postOfficeKey = value;
            }
        }

        [Property("RRR_CountryDescription", ColumnType = "String")]
        public virtual string RRR_CountryDescription
        {
            get
            {
                return this._rRR_CountryDescription;
            }
            set
            {
                this._rRR_CountryDescription = value;
            }
        }

        [Property("RRR_ProvinceDescription", ColumnType = "String")]
        public virtual string RRR_ProvinceDescription
        {
            get
            {
                return this._rRR_ProvinceDescription;
            }
            set
            {
                this._rRR_ProvinceDescription = value;
            }
        }

        [Property("RRR_CityDescription", ColumnType = "String")]
        public virtual string RRR_CityDescription
        {
            get
            {
                return this._rRR_CityDescription;
            }
            set
            {
                this._rRR_CityDescription = value;
            }
        }

        [Property("RRR_SuburbDescription", ColumnType = "String")]
        public virtual string RRR_SuburbDescription
        {
            get
            {
                return this._rRR_SuburbDescription;
            }
            set
            {
                this._rRR_SuburbDescription = value;
            }
        }

        [Property("RRR_PostalCode", ColumnType = "String")]
        public virtual string RRR_PostalCode
        {
            get
            {
                return this._rRR_PostalCode;
            }
            set
            {
                this._rRR_PostalCode = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [Property("SuiteNumber", ColumnType = "String")]
        public virtual string SuiteNumber
        {
            get
            {
                return this._suiteNumber;
            }
            set
            {
                this._suiteNumber = value;
            }
        }

        [Property("FreeText1", ColumnType = "String")]
        public virtual string FreeText1
        {
            get
            {
                return this._freeText1;
            }
            set
            {
                this._freeText1 = value;
            }
        }

        [Property("FreeText2", ColumnType = "String")]
        public virtual string FreeText2
        {
            get
            {
                return this._freeText2;
            }
            set
            {
                this._freeText2 = value;
            }
        }

        [Property("FreeText3", ColumnType = "String")]
        public virtual string FreeText3
        {
            get
            {
                return this._freeText3;
            }
            set
            {
                this._freeText3 = value;
            }
        }

        [Property("FreeText4", ColumnType = "String")]
        public virtual string FreeText4
        {
            get
            {
                return this._freeText4;
            }
            set
            {
                this._freeText4 = value;
            }
        }

        [Property("FreeText5", ColumnType = "String")]
        public virtual string FreeText5
        {
            get
            {
                return this._freeText5;
            }
            set
            {
                this._freeText5 = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AuditNumber", ColumnType = "Decimal")]
        public virtual decimal Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }
    }
}
