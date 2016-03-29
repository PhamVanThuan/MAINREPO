using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Application attributes.
    /// </summary>
    /// <remarks>This is a lookup table and so the generic test can randomly fail due to unique
    /// key constraints.  A manual LoadSaveLoad test has been written instead.</remarks>
    [ActiveRecord("OfferAttribute", Schema = "dbo", Lazy = true)]
    public partial class ApplicationAttribute_DAO : DB_2AM<ApplicationAttribute_DAO>
    {
        private Application_DAO _application;

        private int _key;

        private ApplicationAttributeType_DAO _applicationAttributeType;

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

        [PrimaryKey(PrimaryKeyType.Native, "OfferAttributeKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferAttributeTypeKey", NotNull = true)]
        [ValidateNonEmpty("Application Attribute Type is a mandatory field")]
        public virtual ApplicationAttributeType_DAO ApplicationAttributeType
        {
            get
            {
                return this._applicationAttributeType;
            }
            set
            {
                this._applicationAttributeType = value;
            }
        }
    }
}