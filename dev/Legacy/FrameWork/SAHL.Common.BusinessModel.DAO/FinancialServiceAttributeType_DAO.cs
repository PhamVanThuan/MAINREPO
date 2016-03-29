using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FinancialServiceAttributeType", Schema = "product")]
    public partial class FinancialServiceAttributeType_DAO : DB_2AM<FinancialServiceAttributeType_DAO>
    {
        private int _key;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialServiceAttributeTypeKey", ColumnType = "Int32")]
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