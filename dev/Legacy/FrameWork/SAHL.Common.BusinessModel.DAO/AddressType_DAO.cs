using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AddressType", Schema = "dbo", Lazy = true)]
    public partial class AddressType_DAO : DB_2AM<AddressType_DAO>
    {
        private string _description;

        private int _key;

        //Commented, this is a lookup.
        //private IList<LegalEntityAddress> _legalEntityAddresses;
        /// <summary>
        /// The Address Type Description (Residential/Postal)
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
        [PrimaryKey(PrimaryKeyType.Assigned, "AddressTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(LegalEntityAddress), ColumnKey = "AddressTypeKey", Table = "LegalEntityAddress")]
        //public virtual IList<LegalEntityAddress> LegalEntityAddresses
        //{
        //    get
        //    {
        //        return this._legalEntityAddresses;
        //    }
        //    set
        //    {
        //        this._legalEntityAddresses = value;
        //    }
        //}
    }
}