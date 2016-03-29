using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FailedStreetMigration", Schema = "mig")]
    public partial class FailedStreetMigration_DAO : DB_2AM<FailedStreetMigration_DAO>
    {
        private string _recordType;

        private decimal _clientNumber;

        private string _add1;

        private string _add2;

        private string _add3;

        private string _add4;

        private string _add5;

        private string _pCode;

        private string _city;

        private string _province;

        private string _country;

        private string _errCode;

        private string _resultSet;

        private int _key;

        private IList<FailedLegalEntityAddress_DAO> _failedLegalEntityAddresses;

        private IList<FailedPropertyAddress_DAO> _failedPropertyAddresses;

        [Property("RecordType", ColumnType = "String")]
        public virtual string RecordType
        {
            get
            {
                return this._recordType;
            }
            set
            {
                this._recordType = value;
            }
        }

        [Property("ClientNumber", ColumnType = "Decimal")]
        public virtual decimal ClientNumber
        {
            get
            {
                return this._clientNumber;
            }
            set
            {
                this._clientNumber = value;
            }
        }

        [Property("Add1", ColumnType = "String")]
        public virtual string Add1
        {
            get
            {
                return this._add1;
            }
            set
            {
                this._add1 = value;
            }
        }

        [Property("Add2", ColumnType = "String")]
        public virtual string Add2
        {
            get
            {
                return this._add2;
            }
            set
            {
                this._add2 = value;
            }
        }

        [Property("Add3", ColumnType = "String")]
        public virtual string Add3
        {
            get
            {
                return this._add3;
            }
            set
            {
                this._add3 = value;
            }
        }

        [Property("Add4", ColumnType = "String")]
        public virtual string Add4
        {
            get
            {
                return this._add4;
            }
            set
            {
                this._add4 = value;
            }
        }

        [Property("Add5", ColumnType = "String")]
        public virtual string Add5
        {
            get
            {
                return this._add5;
            }
            set
            {
                this._add5 = value;
            }
        }

        [Property("PCode", ColumnType = "String")]
        public virtual string PCode
        {
            get
            {
                return this._pCode;
            }
            set
            {
                this._pCode = value;
            }
        }

        [Property("City", ColumnType = "String")]
        public virtual string City
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
            }
        }

        [Property("Province", ColumnType = "String")]
        public virtual string Province
        {
            get
            {
                return this._province;
            }
            set
            {
                this._province = value;
            }
        }

        [Property("Country", ColumnType = "String")]
        public virtual string Country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }

        [Property("ErrCode", ColumnType = "String")]
        public virtual string ErrCode
        {
            get
            {
                return this._errCode;
            }
            set
            {
                this._errCode = value;
            }
        }

        [Property("ResultSet", ColumnType = "String")]
        public virtual string ResultSet
        {
            get
            {
                return this._resultSet;
            }
            set
            {
                this._resultSet = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FailedStreetMigrationKey", ColumnType = "Int32")]
        public virtual int Key
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

        [HasMany(typeof(FailedLegalEntityAddress_DAO), ColumnKey = "FailedStreetMigrationKey", Table = "FailedLegalEntityAddress")]
        public virtual IList<FailedLegalEntityAddress_DAO> FailedLegalEntityAddresses
        {
            get
            {
                return this._failedLegalEntityAddresses;
            }
            set
            {
                this._failedLegalEntityAddresses = value;
            }
        }

        [HasMany(typeof(FailedPropertyAddress_DAO), ColumnKey = "FailedStreetMigrationKey", Table = "FailedPropertyAddress")]
        public virtual IList<FailedPropertyAddress_DAO> FailedPropertyAddresses
        {
            get
            {
                return this._failedPropertyAddresses;
            }
            set
            {
                this._failedPropertyAddresses = value;
            }
        }
    }
}