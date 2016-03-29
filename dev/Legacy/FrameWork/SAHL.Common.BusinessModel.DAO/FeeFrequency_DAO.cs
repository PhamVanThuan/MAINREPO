using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FeeFrequency", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FeeFrequency_DAO : DB_2AM<FeeFrequency_DAO>
    {
        private int _key;

        private int _interval;

        private FrequencyUnit_DAO _frequencyUnit;

        [PrimaryKey(PrimaryKeyType.Assigned, "FeeFrequencyKey", ColumnType = "Int32")]
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

        [Property("Interval", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Interval is a mandatory field")]
        public virtual int Interval
        {
            get
            {
                return this._interval;
            }
            set
            {
                this._interval = value;
            }
        }

        [BelongsTo("FrequencyUnitKey", NotNull = true)]
        [ValidateNonEmpty("Frequency Unit is a mandatory field")]
        public virtual FrequencyUnit_DAO FrequencyUnit
        {
            get
            {
                return this._frequencyUnit;
            }
            set
            {
                this._frequencyUnit = value;
            }
        }
    }
}