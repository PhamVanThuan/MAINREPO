using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("InterestRateAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class InterestRateAdjustment_DAO : DB_2AM<InterestRateAdjustment_DAO>
    {
        private double _adjustment;

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

        [Property("Adjustment", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Adjustment is a mandatory field")]
        public virtual double Adjustment
        {
            get
            {
                return this._adjustment;
            }
            set
            {
                this._adjustment = value;
            }
        }
    }
}