using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserProfileSetting", Schema = "dbo", Lazy = true)]
    public partial class UserProfileSetting_DAO : DB_2AM<UserProfileSetting_DAO>
    {
        private string _settingName;

        private string _settingValue;

        private string _settingType;

        private int _Key;

        private ADUser_DAO _aDUser;

        [Property("SettingName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Setting Name is a mandatory field")]
        public virtual string SettingName
        {
            get
            {
                return this._settingName;
            }
            set
            {
                this._settingName = value;
            }
        }

        [Property("SettingValue", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Setting Value is a mandatory field")]
        public virtual string SettingValue
        {
            get
            {
                return this._settingValue;
            }
            set
            {
                this._settingValue = value;
            }
        }

        [Property("SettingType", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Setting Type is a mandatory field")]
        public virtual string SettingType
        {
            get
            {
                return this._settingType;
            }
            set
            {
                this._settingType = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "UserProfileSettingKey", ColumnType = "Int32")]
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

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User is a mandatory field")]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
            }
        }
    }
}