using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("PostOffice", Schema = "dbo")]
    public partial class PostOffice_DAO : DB_2AM<PostOffice_DAO>
    {
        private string _description;

        private string _postalCode;

        private int _key;

        //private IList<Address_DAO> _addresses;

        //private IList<SubsidyProvider_DAO> _subsidyProviders;

        private City_DAO _city;

        [Property("Description", ColumnType = "String")]
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

        [PrimaryKey(PrimaryKeyType.Native, "PostOfficeKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(Address_DAO), ColumnKey = "PostOfficeKey", Table = "Address", Lazy=true)]
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

        // commented, this is a meeting.
        //[HasMany(typeof(SubsidyProvider_DAO), ColumnKey = "PostOfficeKey", Table = "SubsidyProvider", Lazy=true)]
        //public virtual IList<SubsidyProvider_DAO> SubsidyProviders
        //{
        //    get
        //    {
        //        return this._subsidyProviders;
        //    }
        //    set
        //    {
        //        this._subsidyProviders = value;
        //    }
        //}

        [BelongsTo("CityKey", NotNull = false)]
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