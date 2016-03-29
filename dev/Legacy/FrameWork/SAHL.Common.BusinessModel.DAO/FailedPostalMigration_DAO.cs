using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FailedPostalMigration", Schema = "mig")]
    public partial class FailedPostalMigration_DAO : DB_2AM<FailedPostalMigration_DAO>
    {
        private string _recordType;

        private decimal _clientNumber;

        private string _clientBoxNumber;

        private string _clientBoxNumber2;

        private string _newAdd3;

        private string _clientPostOffice;

        private string _clientPostalCode;

        private string _newCity;

        private string _newProvince;

        private string _newCountry;

        private string _faults;

        private string _resultSet;

        private int _key;

        private IList<FailedLegalEntityAddress_DAO> _failedLegalEntityAddresses;

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

        [Property("ClientBoxNumber", ColumnType = "String")]
        public virtual string ClientBoxNumber
        {
            get
            {
                return this._clientBoxNumber;
            }
            set
            {
                this._clientBoxNumber = value;
            }
        }

        [Property("ClientBoxNumber2", ColumnType = "String")]
        public virtual string ClientBoxNumber2
        {
            get
            {
                return this._clientBoxNumber2;
            }
            set
            {
                this._clientBoxNumber2 = value;
            }
        }

        [Property("NewAdd3", ColumnType = "String")]
        public virtual string NewAdd3
        {
            get
            {
                return this._newAdd3;
            }
            set
            {
                this._newAdd3 = value;
            }
        }

        [Property("ClientPostOffice", ColumnType = "String")]
        public virtual string ClientPostOffice
        {
            get
            {
                return this._clientPostOffice;
            }
            set
            {
                this._clientPostOffice = value;
            }
        }

        [Property("ClientPostalCode", ColumnType = "String")]
        public virtual string ClientPostalCode
        {
            get
            {
                return this._clientPostalCode;
            }
            set
            {
                this._clientPostalCode = value;
            }
        }

        [Property("NewCity", ColumnType = "String")]
        public virtual string NewCity
        {
            get
            {
                return this._newCity;
            }
            set
            {
                this._newCity = value;
            }
        }

        [Property("NewProvince", ColumnType = "String")]
        public virtual string NewProvince
        {
            get
            {
                return this._newProvince;
            }
            set
            {
                this._newProvince = value;
            }
        }

        [Property("NewCountry", ColumnType = "String")]
        public virtual string NewCountry
        {
            get
            {
                return this._newCountry;
            }
            set
            {
                this._newCountry = value;
            }
        }

        [Property("Faults", ColumnType = "String")]
        public virtual string Faults
        {
            get
            {
                return this._faults;
            }
            set
            {
                this._faults = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "FailedPostalMigrationKey", ColumnType = "Int32")]
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

        [HasMany(typeof(FailedLegalEntityAddress_DAO), ColumnKey = "FailedPostalMigrationKey", Table = "FailedLegalEntityAddress")]
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
    }
}