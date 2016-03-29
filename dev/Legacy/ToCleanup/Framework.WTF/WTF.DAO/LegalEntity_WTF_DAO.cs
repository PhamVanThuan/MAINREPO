
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LegalEntity", Schema = "dbo")]
    public partial class LegalEntity_WTF_DAO : DB_Test_WTF<LegalEntity_WTF_DAO>
    {

        private int _legalEntityTypeKey;

        private int _maritalStatusKey;

        private int _genderKey;

        private int _populationGroupKey;

        private System.DateTime? _introductionDate;

        private int _salutationkey;

        private string _firstNames;

        private string _initials;

        private string _surname;

        private string _preferredName;

        private string _iDNumber;

        private string _passportNumber;

        private string _taxNumber;

        private string _registrationNumber;

        private string _registeredName;

        private string _tradingName;

        private System.DateTime? _dateOfBirth;

        private string _homePhoneCode;

        private string _homePhoneNumber;

        private string _workPhoneCode;

        private string _workPhoneNumber;

        private string _cellPhoneNumber;

        private string _emailAddress;

        private string _faxCode;

        private string _faxNumber;

        private string _password;

        private int _citizenTypeKey;

        private int _legalEntityStatusKey;

        private string _comments;

        private int _legalEntityExceptionStatusKey;

        private string _userID;

        private System.DateTime? _changeDate;

        private int _educationKey;

        private int _homeLanguageKey;

        private int _documentLanguageKey;

        private int _residenceStatusKey;

        private int _key;

        private IList<ADUser_WTF_DAO> _aDUsers;

        private IList<ApplicationRole_WTF_DAO> _applicationRoles;

        [Property("LegalEntityTypeKey", ColumnType = "Int32")]
        public virtual int LegalEntityTypeKey
        {
            get
            {
                return this._legalEntityTypeKey;
            }
            set
            {
                this._legalEntityTypeKey = value;
            }
        }

        [Property("MaritalStatusKey", ColumnType = "Int32")]
        public virtual int MaritalStatusKey
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

        [Property("GenderKey", ColumnType = "Int32")]
        public virtual int GenderKey
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

        [Property("PopulationGroupKey", ColumnType = "Int32")]
        public virtual int PopulationGroupKey
        {
            get
            {
                return this._populationGroupKey;
            }
            set
            {
                this._populationGroupKey = value;
            }
        }

        [Property("IntroductionDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime? IntroductionDate
        {
            get
            {
                return this._introductionDate;
            }
            set
            {
                this._introductionDate = value;
            }
        }

        [Property("Salutationkey", ColumnType = "Int32")]
        public virtual int Salutationkey
        {
            get
            {
                return this._salutationkey;
            }
            set
            {
                this._salutationkey = value;
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

        [Property("Initials", ColumnType = "String")]
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

        [Property("IDNumber", ColumnType = "String")]
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

        [Property("PassportNumber", ColumnType = "String")]
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

        [Property("TaxNumber", ColumnType = "String")]
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

        [Property("RegistrationNumber", ColumnType = "String")]
        public virtual string RegistrationNumber
        {
            get
            {
                return this._registrationNumber;
            }
            set
            {
                this._registrationNumber = value;
            }
        }

        [Property("RegisteredName", ColumnType = "String")]
        public virtual string RegisteredName
        {
            get
            {
                return this._registeredName;
            }
            set
            {
                this._registeredName = value;
            }
        }

        [Property("TradingName", ColumnType = "String")]
        public virtual string TradingName
        {
            get
            {
                return this._tradingName;
            }
            set
            {
                this._tradingName = value;
            }
        }

        [Property("DateOfBirth", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateOfBirth
        {
            get
            {
                return this._dateOfBirth;
            }
            set
            {
                this._dateOfBirth = value;
            }
        }

        [Property("HomePhoneCode", ColumnType = "String")]
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

        [Property("HomePhoneNumber", ColumnType = "String")]
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

        [Property("WorkPhoneCode", ColumnType = "String")]
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

        [Property("WorkPhoneNumber", ColumnType = "String")]
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

        [Property("CellPhoneNumber", ColumnType = "String")]
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

        [Property("FaxCode", ColumnType = "String")]
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

        [Property("FaxNumber", ColumnType = "String")]
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

        [Property("Password", ColumnType = "String")]
        public virtual string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        [Property("CitizenTypeKey", ColumnType = "Int32")]
        public virtual int CitizenTypeKey
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

        [Property("LegalEntityStatusKey", ColumnType = "Int32")]
        public virtual int LegalEntityStatusKey
        {
            get
            {
                return this._legalEntityStatusKey;
            }
            set
            {
                this._legalEntityStatusKey = value;
            }
        }

        [Property("Comments", ColumnType = "String")]
        public virtual string Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }

        [Property("LegalEntityExceptionStatusKey", ColumnType = "Int32")]
        public virtual int LegalEntityExceptionStatusKey
        {
            get
            {
                return this._legalEntityExceptionStatusKey;
            }
            set
            {
                this._legalEntityExceptionStatusKey = value;
            }
        }

        [Property("UserID", ColumnType = "String")]
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

        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ChangeDate
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

        [Property("EducationKey", ColumnType = "Int32")]
        public virtual int EducationKey
        {
            get
            {
                return this._educationKey;
            }
            set
            {
                this._educationKey = value;
            }
        }

        [Property("HomeLanguageKey", ColumnType = "Int32")]
        public virtual int HomeLanguageKey
        {
            get
            {
                return this._homeLanguageKey;
            }
            set
            {
                this._homeLanguageKey = value;
            }
        }

        [Property("DocumentLanguageKey", ColumnType = "Int32")]
        public virtual int DocumentLanguageKey
        {
            get
            {
                return this._documentLanguageKey;
            }
            set
            {
                this._documentLanguageKey = value;
            }
        }

        [Property("ResidenceStatusKey", ColumnType = "Int32")]
        public virtual int ResidenceStatusKey
        {
            get
            {
                return this._residenceStatusKey;
            }
            set
            {
                this._residenceStatusKey = value;
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

        [HasMany(typeof(ADUser_WTF_DAO), ColumnKey = "LegalEntityKey", Table = "ADUser")]
        public virtual IList<ADUser_WTF_DAO> ADUsers
        {
            get
            {
                return this._aDUsers;
            }
            set
            {
                this._aDUsers = value;
            }
        }

        [HasMany(typeof(ApplicationRole_WTF_DAO), ColumnKey = "LegalEntityKey", Table = "OfferRole")]
        public virtual IList<ApplicationRole_WTF_DAO> ApplicationRoles
        {
            get
            {
                return this._applicationRoles;
            }
            set
            {
                this._applicationRoles = value;
            }
        }
    }
}

