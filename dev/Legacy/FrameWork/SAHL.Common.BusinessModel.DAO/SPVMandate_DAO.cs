using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("SPVMandate", Schema = "spv", Lazy = true)]
    public partial class SPVMandate_DAO : DB_2AM<SPVMandate_DAO>
    {
        private double? _maxLTV;

        private double? _maxPTI;

        private double? _maxLoanAmount;

        private double? _exceedBondPercent;

        private double? _exceedLoanAgreementPercent;

        private double? _exceedBondAmount;

        private int? _sPVMaxTerm;

        private int _key;

        private SPV_DAO _sPV;

        [Property("MaxLTV", ColumnType = "Double")]
        public virtual double? MaxLTV
        {
            get
            {
                return this._maxLTV;
            }
            set
            {
                this._maxLTV = value;
            }
        }

        [Property("MaxPTI", ColumnType = "Double")]
        public virtual double? MaxPTI
        {
            get
            {
                return this._maxPTI;
            }
            set
            {
                this._maxPTI = value;
            }
        }

        [Property("MaxLoanAmount", ColumnType = "Double")]
        public virtual double? MaxLoanAmount
        {
            get
            {
                return this._maxLoanAmount;
            }
            set
            {
                this._maxLoanAmount = value;
            }
        }

        [Property("ExceedBondPercent", ColumnType = "Double")]
        public virtual double? ExceedBondPercent
        {
            get
            {
                return this._exceedBondPercent;
            }
            set
            {
                this._exceedBondPercent = value;
            }
        }

        [Property("ExceedLoanAgreementPercent", ColumnType = "Double")]
        public virtual double? ExceedLoanAgreementPercent
        {
            get
            {
                return this._exceedLoanAgreementPercent;
            }
            set
            {
                this._exceedLoanAgreementPercent = value;
            }
        }

        [Property("ExceedBondAmount", ColumnType = "Double")]
        public virtual double? ExceedBondAmount
        {
            get
            {
                return this._exceedBondAmount;
            }
            set
            {
                this._exceedBondAmount = value;
            }
        }

        [Property("SPVMaxTerm", ColumnType = "Int32")]
        public virtual int? SPVMaxTerm
        {
            get
            {
                return this._sPVMaxTerm;
            }
            set
            {
                this._sPVMaxTerm = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "SPVMandateKey", ColumnType = "Int32")]
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
                return this._sPV;
            }
            set
            {
                this._sPV = value;
            }
        }
    }
}