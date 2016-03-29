using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountSequence", Schema = "dbo", Lazy = true)]
    public class AccountSequence_DAO : DB_2AM<AccountSequence_DAO>
    {
        private int _Key;
        private bool _IsUsed;

        [Property("IsUsed", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Used is a mandatory field")]
        public virtual bool IsUsed
        {
            get
            {
                return this._IsUsed;
            }
            set
            {
                this._IsUsed = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AccountKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _Key;
            }
            set { _Key = value; }
        }
    }
}