using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTest]
    [ActiveRecord("RegentStatus", Schema = "dbo")]
    public partial class RegentStatus_DAO : DB_2AM<RegentStatus_DAO>
    {
        private string _regentStatusDescription;

        private int _key;

        [Property("RegentStatusDescription", ColumnType = "String")]
        public virtual string RegentStatusDescription
        {
            get
            {
                return this._regentStatusDescription;
            }
            set
            {
                this._regentStatusDescription = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "RegentStatusID", ColumnType = "Int32")]
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
    }
}