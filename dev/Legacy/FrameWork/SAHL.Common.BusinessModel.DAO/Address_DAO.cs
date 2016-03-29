using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Address_DAO is the base class from which the format specific addresses are derived. The discrimination is based on the
    /// Address Format (Street, Box etc).
    /// </summary>
    /// <seealso cref="AddressBox_DAO"/>
    /// <seealso cref="AddressClusterBox_DAO"/>
    /// <seealso cref="AddressFreeText_DAO"/>
    /// <seealso cref="AddressPostnetSuite_DAO"/>
    /// <seealso cref="AddressPrivateBag_DAO"/>
    /// <seealso cref="AddressStreet_DAO"/>
    /// <seealso cref="AddressType_DAO"/>
    /// <seealso cref="AddressFormat_DAO"/>
    ///
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Address", Schema = "dbo", DiscriminatorColumn = "AddressFormatKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true)]
    public partial class Address_DAO : DB_2AM<Address_DAO>
    {
        private string _userID;

        private System.DateTime? _changeDate;

        private int _key;

        private string _rRR_CountryDescription;

        private string _rRR_ProvinceDescription;

        private string _rRR_CityDescription;

        private string _rRR_SuburbDescription;

        private string _rRR_PostalCode;

        //private IList<AssetLiability_DAO> _assetLiabilities;

        //private IList<Attorney_DAO> _attorneys;

        //private IList<MailingAddress_DAO> _mailingAddresses;

        //private IList<Property_DAO> _properties;

        //private IList<Address_DAO> _valuatorAddresses;

        //private IList<LegalEntityAddress_DAO> _legalEntityAddresses;

        private AddressFormat_DAO _addressFormat;

        /// <summary>
        /// The UserID of the last person who updated information on the Address.
        /// </summary>
        [Property("UserID", ColumnType = "String", Length = 25)]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        /// <summary>
        /// The date when the Address record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "AddressKey", ColumnType = "Int32")]
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

        /// <summary>
        /// The format which the address is in (Street/Box/Postnet Suite etc). The Address_DAO base class is discriminated according
        /// to this column.
        /// </summary>
        [Lurker]
        [BelongsTo("AddressFormatKey", NotNull = true, Access = PropertyAccess.FieldCamelcaseUnderscore, Insert = false, Update = false)]
        public virtual AddressFormat_DAO AddressFormat
        {
            get
            {
                return this._addressFormat;
            }
        }

        /// <summary>
        /// The Country property of an address.
        /// </summary>
        [Property("RRR_CountryDescription", ColumnType = "String", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual string RRR_CountryDescription
        {
            get
            {
                return this._rRR_CountryDescription;
            }
        }

        /// <summary>
        /// The Province property of an address.
        /// </summary>
        [Property("RRR_ProvinceDescription", ColumnType = "String", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual string RRR_ProvinceDescription
        {
            get
            {
                return this._rRR_ProvinceDescription;
            }
        }

        /// <summary>
        /// The City property of an address.
        /// </summary>
        [Property("RRR_CityDescription", ColumnType = "String", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual string RRR_CityDescription
        {
            get
            {
                return this._rRR_CityDescription;
            }
        }

        /// <summary>
        /// The Suburb property of an address.
        /// </summary>
        [Property("RRR_SuburbDescription", ColumnType = "String", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual string RRR_SuburbDescription
        {
            get
            {
                return this._rRR_SuburbDescription;
            }
        }

        /// <summary>
        /// The Postal Code property of an address.
        /// </summary>
        [Property("RRR_PostalCode", ColumnType = "String", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual string RRR_PostalCode
        {
            get
            {
                return this._rRR_PostalCode;
            }
        }

        #region collections taken out as they are absurb, re introduce at your own peril.

        /*
        [HasMany(typeof(AssetLiability_DAO), Lazy = true, ColumnKey = "AddressKey", Table = "AssetLiability")]
        public virtual IList<AssetLiability_DAO> AssetLiabilities
        {
            get
            {
                return this._assetLiabilities;
            }
            set
            {
                this._assetLiabilities = value;
            }
        }

        [HasMany(typeof(Attorney_DAO), Lazy = true, ColumnKey = "AddressKey", Table = "Attorney")]
        public virtual IList<Attorney_DAO> Attorneys
        {
            get
            {
                return this._attorneys;
            }
            set
            {
                this._attorneys = value;
            }
        }

        [HasMany(typeof(LegalEntityAddress_DAO), Lazy = true, ColumnKey = "AddressKey", Table = "LegalEntityAddress")]
        public virtual IList<LegalEntityAddress_DAO> LegalEntityAddresses
        {
            get
            {
                return this._legalEntityAddresses;
            }
            set
            {
                this._legalEntityAddresses = value;
            }
        }

        [HasMany(typeof(MailingAddress_DAO), Lazy = true, ColumnKey = "AddressKey", Table = "MailingAddress")]
        public virtual IList<MailingAddress_DAO> MailingAddresses
        {
            get
            {
                return this._mailingAddresses;
            }
            set
            {
                this._mailingAddresses = value;
            }
        }

        [HasMany(typeof(Property_DAO), Lazy = true, ColumnKey = "AddressKey", Table = "Property")]
        public virtual IList<Property_DAO> Properties
        {
            get
            {
                return this._properties;
            }
            set
            {
                this._properties = value;
            }
        }

        [HasAndBelongsToMany(typeof(Address_DAO), Table = "ValuatorAddresses", ColumnKey = "AddressKey", ColumnRef = "ValuationKey", Inverse = true,Lazy= true)]
        public virtual IList<Address_DAO> ValuatorAddresses
        {
            get { return _valuatorAddresses; }
            set { _valuatorAddresses = value; }
        }

         */

        #endregion collections taken out as they are absurb, re introduce at your own peril.
    }
}