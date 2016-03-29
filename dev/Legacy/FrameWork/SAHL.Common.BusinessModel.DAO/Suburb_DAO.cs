using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Suburb", Schema = "dbo", Lazy = true)]
    public partial class Suburb_DAO : DB_2AM<Suburb_DAO>
    {
        private string _description;

        private string _postalCode;

        private int _key;

        // commented, this is a lookup.
        //private IList<Address_DAO> _addresses;

        private City_DAO _city;

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

        [Property("PostalCode", ColumnType = "String")]
        public virtual string PostalCode
        {
            get
            {
                return this._postalCode;
            }
            set
            {
                this._postalCode = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "SuburbKey", ColumnType = "Int32")]
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

        // Commented, this is a lookup.
        //[HasMany(typeof(Address_DAO), ColumnKey = "SuburbKey", Table = "Address", Lazy=true)]
        //public virtual IList<Address_DAO> Addresses
        //{
        //    get
        //    {
        //        return this._addresses;
        //    }
        //    set
        //    {
        //        this._addresses = value;
        //    }
        //}

        [BelongsTo("CityKey", NotNull = true)]
        [ValidateNonEmpty("City is a mandatory field")]
        public virtual City_DAO City
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
            }
        }
    }
}