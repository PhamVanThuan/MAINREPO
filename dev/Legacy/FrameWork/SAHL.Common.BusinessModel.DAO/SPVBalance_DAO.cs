using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("SPVBalance", Schema = "spv", Lazy = true)]
    public partial class SPVBalance_DAO : DB_2AM<SPVBalance_DAO>
    {
        private int _key;

        private double _amount;

        private SPV_DAO _sPV;

        private BalanceType_DAO _balanceType;

        [PrimaryKey(PrimaryKeyType.Native, "SPVBalanceKey", ColumnType = "Int32")]
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

        [Property("Amount", ColumnType = "Double")]
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

        [BelongsTo("SPVKey")]
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

        [BelongsTo("BalanceTypeKey")]
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
    }
}