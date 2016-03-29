using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("StaticRateAdjustment", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class StaticRateAdjustment_DAO : DB_2AM<StaticRateAdjustment_DAO>
    {
        private double _rate;

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

        [Property("Rate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Adjustment is a mandatory field")]
        public virtual double Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
            }
        }
    }
}