using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LifeCommissionSecurity", Schema = "dbo")]
    public partial class LifeCommissionSecurity_DAO : DB_2AM<LifeCommissionSecurity_DAO>
    {
        private string _userID;

        private bool _administrator;

        private int _Key;

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("Administrator", ColumnType = "Boolean")]
        public virtual bool Administrator
        {
            get
            {
                return this._administrator;
            }
            set
            {
                this._administrator = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "SecurityKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }
    }
}