using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("PaymentAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class PaymentAdjustment_DAO : DB_2AM<PaymentAdjustment_DAO>
    {
        private double _amount;

        private int _Key;

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialAdjustmentKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

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
    }
}