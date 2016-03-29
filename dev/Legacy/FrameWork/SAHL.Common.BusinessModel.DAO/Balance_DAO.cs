using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("Balance", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class Balance_DAO : DB_2AM<Balance_DAO>
    {
        private int _key;

        private double _amount;

        private BalanceType_DAO _balanceType;

        private LoanBalance_DAO _loanBalance;

        [Property("Amount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Amount is a mandatory field")]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [BelongsTo("BalanceTypeKey", NotNull = true)]
        [ValidateNonEmpty("Balance Type is a mandatory field")]
        public virtual BalanceType_DAO BalanceType
        {
            get
            {
                return this._balanceType;
            }
            set
            {
                this._balanceType = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialServiceKey", ColumnType = "Int32")]
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

        [OneToOne]
        public virtual LoanBalance_DAO LoanBalance
        {
            get
            {
                return this._loanBalance;
            }
            set
            {
                this._loanBalance = value;
            }
        }
    }
}