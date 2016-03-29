using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The LegalEntityAddress_DAO class specifies the Addresses related to a Legal Entity.
    /// </summary>
    [ActiveRecord("LegalEntityAddress", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityAddress_DAO : DB_2AM<LegalEntityAddress_DAO>
    {
        private Address_DAO _address;

        private AddressType_DAO _addressType;

        // initialise to minimum value - this will force people to set the value as there is a rule that checks
        // this
        private System.DateTime _effectiveDate = DateTime.MinValue;

        private GeneralStatus_DAO _generalStatus;

        private int _key;

        private LegalEntity_DAO _legalEntity;

		private IList<LegalEntityDomicilium_DAO> _legalEntityDomiciliums;

        /// <summary>
        /// Foreign key reference to the Address table where the Address information is stored.
        /// </summary>
        [BelongsTo("AddressKey", NotNull = true)]
        [ValidateNonEmpty("Address is a mandatory field")]
        public virtual Address_DAO Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

        /// <summary>
        /// Foreign key reference to the AddressType table where the AddressType information is stored. An address can belong
        /// to a single Address Type. e.g. (Residential or Postal)
        /// </summary>
        [BelongsTo("AddressTypeKey", NotNull = true)]
        [ValidateNonEmpty("Address Type is a mandatory field")]
        public virtual AddressType_DAO AddressType
        {
            get
            {
                return this._addressType;
            }
            set
            {
                this._addressType = value;
            }
        }

        /// <summary>
        /// The date on which the address record should come into effect.  If this is set to a value
        /// greater than the current date, it will also set the General Status to inactive.  There is
        /// a database job that sets these to active once the effective date is reached.
        /// </summary>
        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [Lurker]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        /// <summary>
        /// Whether or not the address is active.  If the effective date is greater than today's date, this
        /// will always be inactive.
        /// </summary>
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        [Lurker]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityAddressKey", ColumnType = "Int32")]
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
        /// The Foreign Key reference to the Legal Entity table. The relationship between a specific AddressKey and a LegalEntityKey
        /// can only exist once.
        /// </summary>
        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

		[HasMany(typeof(LegalEntityDomicilium_DAO), ColumnKey = "LegalEntityAddressKey", Table = "LegalEntityDomicilium", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
		public virtual IList<LegalEntityDomicilium_DAO> LegalEntityDomiciliums
		{
			get
			{
				return this._legalEntityDomiciliums;
			}
			set
			{
				this._legalEntityDomiciliums = value;
			}
		}
    }
}