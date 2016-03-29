using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AddressFormat", Schema = "dbo", Lazy = true)]
    public partial class AddressFormat_DAO : DB_2AM<AddressFormat_DAO>
    {
        private string _description;

        private int _key;

        //commented, this is a lookup.
        // private IList<Address> _addresses;

        //private IList<SubsidyProvider> _subsidyProviders;

        /// <summary>
        /// The description of the Address Format (Street/Cluster Box etc)
        /// </summary>
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "AddressFormatKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(Address), ColumnKey = "AddressFormatKey", Table = "Address")]
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

        // commented, this is a lookup, and this table should not contain addresses.
        //[HasMany(typeof(SubsidyProvider), ColumnKey = "AddressFormatKey", Table = "SubsidyProvider")]
        //public virtual IList<SubsidyProvider> SubsidyProviders
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
    }
}