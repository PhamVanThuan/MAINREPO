using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportLegalEntity", Schema = "dbo")]
    public partial class ImportLegalEntity_DAO : DB_2AM<ImportLegalEntity_DAO>
    {
        private string _maritalStatusKey;

        private string _genderKey;

        private string _citizenTypeKey;

        private string _salutationKey;

        private string _firstNames;

        private string _initials;

        private string _surname;

        private string _preferredName;

        private string _iDNumber;

        private string _passportNumber;

        private string _taxNumber;

        private string _homePhoneCode;

        private string _homePhoneNumber;

        private string _workPhoneCode;

        private string _workPhoneNumber;

        private string _cellPhoneNumber;

        private string _emailAddress;

        private string _faxCode;

        private string _faxNumber;

        private int _importID;

        private int _key;

        private ImportApplication_DAO _importApplication;

        [Property("MaritalStatusKey", ColumnType = "String")]
        public virtual string MaritalStatusKey
        {
            get
            {
                return this._maritalStatusKey;
            }
            set
            {
                this._maritalStatusKey = value;
            }
        }

        [Property("GenderKey", ColumnType = "String")]
        public virtual string GenderKey
        {
            get
            {
                return this._genderKey;
            }
            set
            {
                this._genderKey = value;
            }
        }

        [Property("CitizenTypeKey", ColumnType = "String")]
        public virtual string CitizenTypeKey
        {
            get
            {
                return this._citizenTypeKey;
            }
            set
            {
                this._citizenTypeKey = value;
            }
        }

        [Property("SalutationKey", ColumnType = "String")]
        public virtual string SalutationKey
        {
            get
            {
                return this._salutationKey;
            }
            set
            {
                this._salutationKey = value;
            }
        }

        [Property("FirstNames", ColumnType = "String")]
        public virtual string FirstNames
        {
            get
            {
                return this._firstNames;
            }
            set
            {
                this._firstNames = value;
            }
        }

        [Property("Initials", ColumnType = "String", Length = 5)]
        public virtual string Initials
        {
            get
            {
                return this._initials;
            }
            set
            {
                this._initials = value;
            }
        }

        [Property("Surname", ColumnType = "String")]
        public virtual string Surname
        {
            get
            {
                return this._surname;
            }
            set
            {
                this._surname = value;
            }
        }

        [Property("PreferredName", ColumnType = "String")]
        public virtual string PreferredName
        {
            get
            {
                return this._preferredName;
            }
            set
            {
                this._preferredName = value;
            }
        }

        [Property("IDNumber", ColumnType = "String", Length = 20)]
        public virtual string IDNumber
        {
            get
            {
                return this._iDNumber;
            }
            set
            {
                this._iDNumber = value;
            }
        }

        [Property("PassportNumber", ColumnType = "String", Length = 20)]
        public virtual string PassportNumber
        {
            get
            {
                return this._passportNumber;
            }
            set
            {
                this._passportNumber = value;
            }
        }

        [Property("TaxNumber", ColumnType = "String", Length = 20)]
        public virtual string TaxNumber
        {
            get
            {
                return this._taxNumber;
            }
            set
            {
                this._taxNumber = value;
            }
        }

        [Property("HomePhoneCode", ColumnType = "String", Length = 10)]
        public virtual string HomePhoneCode
        {
            get
            {
                return this._homePhoneCode;
            }
            set
            {
                this._homePhoneCode = value;
            }
        }

        [Property("HomePhoneNumber", ColumnType = "String", Length = 15)]
        public virtual string HomePhoneNumber
        {
            get
            {
                return this._homePhoneNumber;
            }
            set
            {
                this._homePhoneNumber = value;
            }
        }

        [Property("WorkPhoneCode", ColumnType = "String", Length = 10)]
        public virtual string WorkPhoneCode
        {
            get
            {
                return this._workPhoneCode;
            }
            set
            {
                this._workPhoneCode = value;
            }
        }

        [Property("WorkPhoneNumber", ColumnType = "String", Length = 15)]
        public virtual string WorkPhoneNumber
        {
            get
            {
                return this._workPhoneNumber;
            }
            set
            {
                this._workPhoneNumber = value;
            }
        }

        [Property("CellPhoneNumber", ColumnType = "String", Length = 15)]
        public virtual string CellPhoneNumber
        {
            get
            {
                return this._cellPhoneNumber;
            }
            set
            {
                this._cellPhoneNumber = value;
            }
        }

        [Property("EmailAddress", ColumnType = "String")]
        public virtual string EmailAddress
        {
            get
            {
                return this._emailAddress;
            }
            set
            {
                this._emailAddress = value;
            }
        }

        [Property("FaxCode", ColumnType = "String", Length = 10)]
        public virtual string FaxCode
        {
            get
            {
                return this._faxCode;
            }
            set
            {
                this._faxCode = value;
            }
        }

        [Property("FaxNumber", ColumnType = "String", Length = 15)]
        public virtual string FaxNumber
        {
            get
            {
                return this._faxNumber;
            }
            set
            {
                this._faxNumber = value;
            }
        }

        [Property("ImportID", ColumnType = "Int32")]
        public virtual int ImportID
        {
            get
            {
                return this._importID;
            }
            set
            {
                this._importID = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Import Application is a mandatory field")]
        public virtual ImportApplication_DAO ImportApplication
        {
            get
            {
                return this._importApplication;
            }
            set
            {
                this._importApplication = value;
            }
        }
    }
}