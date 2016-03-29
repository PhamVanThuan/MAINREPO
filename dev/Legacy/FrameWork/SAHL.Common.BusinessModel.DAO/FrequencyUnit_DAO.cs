using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FrequencyUnit", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FrequencyUnit_DAO : DB_2AM<FrequencyUnit_DAO>
    {
        private int _key;

        private string _unit;

        [PrimaryKey(PrimaryKeyType.Assigned, "FrequencyUnitKey", ColumnType = "Int32")]
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

        [Property("Unit", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Unit is a mandatory field")]
        public virtual string Unit
        {
            get
            {
                return this._unit;
            }
            set
            {
                this._unit = value;
            }
        }
    }
}