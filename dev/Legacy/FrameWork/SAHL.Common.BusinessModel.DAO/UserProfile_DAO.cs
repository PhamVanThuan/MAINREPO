using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserProfile", Schema = "dbo", Lazy = true)]
    public partial class UserProfile_DAO : DB_2AM<UserProfile_DAO>
    {
        private string _aDUserName;

        private string _value;

        private int _Key;

        private ProfileType_DAO _profileType;

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("AD User Name is a mandatory field")]
        public virtual string ADUserName
        {
            get
            {
                return this._aDUserName;
            }
            set
            {
                this._aDUserName = value;
            }
        }

        [Property("Value", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Value is a mandatory field")]
        public virtual string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "UserProfileKey", ColumnType = "Int32")]
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

        [BelongsTo("ProfileTypeKey", NotNull = true)]
        [ValidateNonEmpty("Profile Type is a mandatory field")]
        public virtual ProfileType_DAO ProfileType
        {
            get
            {
                return this._profileType;
            }
            set
            {
                this._profileType = value;
            }
        }
    }
}