using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("SPVFee", Schema = "spv", Lazy = true)]
    public partial class SPVFee_DAO : DB_2AM<SPVFee_DAO>
    {
        private int _key;
        private SPV_DAO _spv;
        private SPVFeeType_DAO _spvFeeType;
        private double _value;
        private double? _MaxFeeAmount;
        private double? _MinFeeAmount;
        private double? _RoundingYield;
        private double? _AdditionalYield;

        [PrimaryKey(PrimaryKeyType.Native, "SPVFeeKey", ColumnType = "Int32")]
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

        [BelongsTo("SPVKey", NotNull = true)]
        [ValidateNonEmpty("SPV is a mandatory field")]
        public virtual SPV_DAO SPV
        {
            get
            {
                return this._spv;
            }
            set
            {
                this._spv = value;
            }
        }

        [BelongsTo("SPVFeeTypeKey", NotNull = true)]
        [ValidateNonEmpty("SPVFeeType is a mandatory field")]
        public virtual SPVFeeType_DAO SPVFeeType
        {
            get
            {
                return this._spvFeeType;
            }
            set
            {
                this._spvFeeType = value;
            }
        }

        [Property("Value")]
        public virtual double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [Property("MaxFeeAmount")]
        public virtual double? MaxFeeAmount
        {
            get
            {
                return this._MaxFeeAmount;
            }
            set
            {
                this._MaxFeeAmount = value;
            }
        }

        [Property("MinFeeAmount")]
        public virtual double? MinFeeAmount
        {
            get
            {
                return this._MinFeeAmount;
            }
            set
            {
                this._MinFeeAmount = value;
            }
        }

        [Property("RoundingYield")]
        public virtual double? RoundingYield
        {
            get
            {
                return this._RoundingYield;
            }
            set
            {
                this._RoundingYield = value;
            }
        }

        [Property("AdditionalYield")]
        public virtual double? AdditionalYield
        {
            get
            {
                return this._AdditionalYield;
            }
            set
            {
                this._AdditionalYield = value;
            }
        }
    }
}