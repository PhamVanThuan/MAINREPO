using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FinancialAdjustmentType", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FinancialAdjustmentType_DAO : DB_2AM<FinancialAdjustmentType_DAO>
    {
        private int _key;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialAdjustmentTypeKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }
    }
}