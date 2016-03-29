using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("PropertyTitleDeed", Schema = "dbo")]
    public partial class PropertyTitleDeed_DAO : DB_2AM<PropertyTitleDeed_DAO>
    {
        private string _titleDeedNumber;

        private int _key;

        private Property_DAO _property;

        private DeedsOffice_DAO _deedsOffice;

        [Property("TitleDeedNumber", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Title Deed Number is a mandatory field")]
        public virtual string TitleDeedNumber
        {
            get
            {
                return this._titleDeedNumber;
            }
            set
            {
                this._titleDeedNumber = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "PropertyTitleDeedKey", ColumnType = "Int32")]
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

        [BelongsTo("PropertyKey", NotNull = true)]
        [ValidateNonEmpty("Property is a mandatory field")]
        public virtual Property_DAO Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
            }
        }

        /// <summary>
        /// Gets/sets the deeds office to which the title deed belongs.
        /// </summary>
        [BelongsTo("DeedsOfficeKey", NotNull = false)]
        public virtual DeedsOffice_DAO DeedsOffice
        {
            get
            {
                return this._deedsOffice;
            }
            set
            {
                this._deedsOffice = value;
            }
        }
    }
}