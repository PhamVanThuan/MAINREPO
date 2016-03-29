using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Validator", Schema = "dbo")]
    public partial class Validator_DAO : DB_2AM<Validator_DAO>
    {
        private string _initialValue;

        private string _regularExpression;

        private double? _minimumValue;

        private double? _maximumValue;

        private int _Key;

        private DomainField_DAO _domainField;

        private ErrorRepository_DAO _errorRepository;

        private ValidatorType_DAO _validatorType;

        [Property("InitialValue", ColumnType = "String")]
        public virtual string InitialValue
        {
            get
            {
                return this._initialValue;
            }
            set
            {
                this._initialValue = value;
            }
        }

        [Property("RegularExpression", ColumnType = "String")]
        public virtual string RegularExpression
        {
            get
            {
                return this._regularExpression;
            }
            set
            {
                this._regularExpression = value;
            }
        }

        [Property("MinimumValue", ColumnType = "Double")]
        public virtual double? MinimumValue
        {
            get
            {
                return this._minimumValue;
            }
            set
            {
                this._minimumValue = value;
            }
        }

        [Property("MaximumValue", ColumnType = "Double")]
        public virtual double? MaximumValue
        {
            get
            {
                return this._maximumValue;
            }
            set
            {
                this._maximumValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ValidatorKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [BelongsTo("DomainFieldKey", NotNull = true)]
        [ValidateNonEmpty("Domain Field is a mandatory field")]
        public virtual DomainField_DAO DomainField
        {
            get
            {
                return this._domainField;
            }
            set
            {
                this._domainField = value;
            }
        }

        [BelongsTo("ErrorRepositoryKey", NotNull = true)]
        [ValidateNonEmpty("Error Repository is a mandatory field")]
        public virtual ErrorRepository_DAO ErrorRepository
        {
            get
            {
                return this._errorRepository;
            }
            set
            {
                this._errorRepository = value;
            }
        }

        [BelongsTo("ValidatorTypeKey", NotNull = true)]
        [ValidateNonEmpty("Validator Type is a mandatory field")]
        public virtual ValidatorType_DAO ValidatorType
        {
            get
            {
                return this._validatorType;
            }
            set
            {
                this._validatorType = value;
            }
        }
    }
}