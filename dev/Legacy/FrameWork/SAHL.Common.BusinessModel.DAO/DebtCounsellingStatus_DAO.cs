using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DebtCounsellingStatus", Schema = "debtcounselling", Lazy = false)]
    public partial class DebtCounsellingStatus_DAO : DB_2AM<DebtCounsellingStatus_DAO>
    {
        private int _key;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Assigned, "DebtCounsellingStatusKey", ColumnType = "Int32")]
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