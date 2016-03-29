using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ProfileType", Schema = "dbo")]
    public partial class ProfileType_DAO : DB_2AM<ProfileType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<UserProfile_DAO> _userProfiles;

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

        [PrimaryKey(PrimaryKeyType.Native, "ProfileTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(UserProfile_DAO), ColumnKey = "ProfileTypeKey", Table = "UserProfile")]
        //public virtual IList<UserProfile_DAO> UserProfiles
        //{
        //    get
        //    {
        //        return this._userProfiles;
        //    }
        //    set
        //    {
        //        this._userProfiles = value;
        //    }
        //}
    }
}