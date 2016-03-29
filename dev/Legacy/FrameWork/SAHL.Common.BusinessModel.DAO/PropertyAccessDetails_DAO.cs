using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("PropertyAccessDetails", Schema = "dbo")]
    public partial class PropertyAccessDetails_DAO : DB_2AM<PropertyAccessDetails_DAO>
    {
        private int _key;

        private Property_DAO _property;

        private string _contact1;

        private string _contact1Phone;

        private string _contact1WorkPhone;

        private string _contact1MobilePhone;

        private string _contact2;

        private string _contact2Phone;

        [PrimaryKey(PrimaryKeyType.Native, "PropertyAccessDetailsKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get { return _key; }
            set { _key = value; }
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

        [Property("Contact1", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Contact1 is a mandatory field")]
        public virtual string Contact1
        {
            get
            {
                return this._contact1;
            }
            set
            {
                this._contact1 = value;
            }
        }

        [Property("Contact1Phone", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Contact1 Phone is a mandatory field")]
        public virtual string Contact1Phone
        {
            get
            {
                return this._contact1Phone;
            }
            set
            {
                this._contact1Phone = value;
            }
        }

        [Property("Contact1WorkPhone", ColumnType = "String")]
        public virtual string Contact1WorkPhone
        {
            get
            {
                return this._contact1WorkPhone;
            }
            set
            {
                this._contact1WorkPhone = value;
            }
        }

        [Property("Contact1MobilePhone", ColumnType = "String")]
        public virtual string Contact1MobilePhone
        {
            get
            {
                return this._contact1MobilePhone;
            }
            set
            {
                this._contact1MobilePhone = value;
            }
        }

        [Property("Contact2", ColumnType = "String")]
        public virtual string Contact2
        {
            get
            {
                return this._contact2;
            }
            set
            {
                this._contact2 = value;
            }
        }

        [Property("Contact2Phone", ColumnType = "String")]
        public virtual string Contact2Phone
        {
            get
            {
                return this._contact2Phone;
            }
            set
            {
                this._contact2Phone = value;
            }
        }
    }
}