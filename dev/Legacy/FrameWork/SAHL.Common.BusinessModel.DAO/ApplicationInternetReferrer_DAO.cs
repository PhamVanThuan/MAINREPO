using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferInternetReferrer", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInternetReferrer_DAO : DB_2AM<ApplicationInternetReferrer_DAO>
    {
        private int _key;

        private Application_DAO _application;

        private string _userURL;

        private string _referringServerURL;

        private string _parameters;

        [PrimaryKey(PrimaryKeyType.Native, "OfferInternetReferrerKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        [Property("UserURL", ColumnType = "String")]
        public virtual string UserURL
        {
            get
            {
                return this._userURL;
            }
            set
            {
                this._userURL = value;
            }
        }

        [Property("ReferringServerURL", ColumnType = "String")]
        public virtual string ReferringServerURL
        {
            get
            {
                return this._referringServerURL;
            }
            set
            {
                this._referringServerURL = value;
            }
        }

        [Property("Parameters", ColumnType = "String")]
        public virtual string Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                this._parameters = value;
            }
        }
    }
}