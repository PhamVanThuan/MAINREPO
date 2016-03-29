using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Broker", Schema = "dbo")]
    public partial class Broker_DAO : DB_2AM<Broker_DAO>
    {
        private string _aDUserName;

        private string _fullName;

        private string _initials;

        private string _telephoneNumber;

        private string _faxNumber;

        private string _emailAddress;

        private string _password;

        private string _passwordQuestion;

        private string _passwordAnswer;

        private ADUser_DAO _adUser;

        private int _key;

        private int? _brokerStatusNumber;

        private int? _brokerTypeNumber;

        private int? _brokerCommissionTrigger;

        private double? _brokerMinimumSAHL;

        private double? _brokerMinimumSCMB;

        private float? _brokerPercentageSAHL;

        private float? _brokerPercentageSCMB;

        private double? _brokerTarget;

        // private IList<CapApplication_DAO> _capApplications;

        private GeneralStatus_DAO _generalStatus;

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("AD User Name is a mandatory field")]
        public virtual string ADUserName
        {
            get
            {
                return this._aDUserName;
            }
            set
            {
                this._aDUserName = value;
            }
        }

        [Property("FullName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Full Name is a mandatory field")]
        public virtual string FullName
        {
            get
            {
                return this._fullName;
            }
            set
            {
                this._fullName = value;
            }
        }

        [Property("Initials", ColumnType = "String", NotNull = true, Length = 5)]
        [ValidateNonEmpty("Initials is a mandatory field")]
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

        [Property("TelephoneNumber", ColumnType = "String", Length = 25)]
        public virtual string TelephoneNumber
        {
            get
            {
                return this._telephoneNumber;
            }
            set
            {
                this._telephoneNumber = value;
            }
        }

        [Property("FaxNumber", ColumnType = "String", Length = 25)]
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

        [Property("PasswordQuestion", ColumnType = "String")]
        public virtual string PasswordQuestion
        {
            get
            {
                return this._passwordQuestion;
            }
            set
            {
                this._passwordQuestion = value;
            }
        }

        [Property("PasswordAnswer", ColumnType = "String")]
        public virtual string PasswordAnswer
        {
            get
            {
                return this._passwordAnswer;
            }
            set
            {
                this._passwordAnswer = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "BrokerKey", ColumnType = "Int32")]
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

        // commented, this will be a performance nightmare.
        //[HasMany(typeof(CapApplication_DAO), ColumnKey = "BrokerKey", Table = "CapOffer", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        //public virtual IList<CapApplication_DAO> CapApplications
        //{
        //    get
        //    {
        //        return this._capApplications;
        //    }
        //    set
        //    {
        //        this._capApplications = value;
        //    }
        //}

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User is a mandatory field")]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._adUser;
            }
            set
            {
                this._adUser = value;
            }
        }

        [Property("BrokerStatusNumber", ColumnType = "Int32")]
        public virtual int? BrokerStatusNumber
        {
            get
            {
                return this._brokerStatusNumber;
            }
            set
            {
                this._brokerStatusNumber = value;
            }
        }

        [Property("BrokerTypeNumber", ColumnType = "Int32")]
        public virtual int? BrokerTypeNumber
        {
            get
            {
                return this._brokerTypeNumber;
            }
            set
            {
                this._brokerTypeNumber = value;
            }
        }

        [Property("BrokerCommissionTrigger", ColumnType = "Int32")]
        public virtual int? BrokerCommissionTrigger
        {
            get
            {
                return this._brokerCommissionTrigger;
            }
            set
            {
                this._brokerCommissionTrigger = value;
            }
        }

        [Property("BrokerMinimumSAHL", ColumnType = "Double")]
        public virtual double? BrokerMinimumSAHL
        {
            get
            {
                return this._brokerMinimumSAHL;
            }
            set
            {
                this._brokerMinimumSAHL = value;
            }
        }

        [Property("BrokerMinimumSCMB", ColumnType = "Double")]
        public virtual double? BrokerMinimumSCMB
        {
            get
            {
                return this._brokerMinimumSCMB;
            }
            set
            {
                this._brokerMinimumSCMB = value;
            }
        }

        [Property("BrokerPercentageSAHL", ColumnType = "Single")]
        public virtual float? BrokerPercentageSAHL
        {
            get
            {
                return this._brokerPercentageSAHL;
            }
            set
            {
                this._brokerPercentageSAHL = value;
            }
        }

        [Property("BrokerPercentageSCMB", ColumnType = "Single")]
        public virtual float? BrokerPercentageSCMB
        {
            get
            {
                return this._brokerPercentageSCMB;
            }
            set
            {
                this._brokerPercentageSCMB = value;
            }
        }

        [Property("BrokerTarget", ColumnType = "Double")]
        public virtual double? BrokerTarget
        {
            get
            {
                return this._brokerTarget;
            }
            set
            {
                this._brokerTarget = value;
            }
        }
    }
}