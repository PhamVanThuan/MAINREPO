using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferMailingAddress", Schema = "dbo", Lazy = true)]
    public partial class ApplicationMailingAddress_DAO : DB_2AM<ApplicationMailingAddress_DAO>
    {
        private Application_DAO _application;

        private Address_DAO _address;

        private bool _onlineStatement;

        private OnlineStatementFormat_DAO _onlineStatementFormat;

        private Language_DAO _language;

        private CorrespondenceMedium_DAO _correspondenceMedium;

        private LegalEntity_DAO _legalEntity;

        private int _key;

        /// <summary>
        /// The Mailing Address is associated to a particular application. This relationship is defined in the OfferMailingAddress
        /// table where the Offer.OfferKey = OfferMailingAddress.OfferKey.
        /// </summary>
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

        /// <summary>
        /// Each Address is associated an AddressKey. The Address details are retrieved from the Address table based on this key.
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
        /// An indicator as to whether the client would like to receive their Loan Statement electronically.
        /// </summary>
        [Property("OnlineStatement", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Online Statement is a mandatory field")]
        public virtual bool OnlineStatement
        {
            get
            {
                return this._onlineStatement;
            }
            set
            {
                this._onlineStatement = value;
            }
        }

        /// <summary>
        /// The Electronic Format they would like to receive their Loan Statement in.
        /// </summary>
        [BelongsTo("OnlineStatementFormatKey", NotNull = true)]
        [ValidateNonEmpty("Online Statement Format is a mandatory field")]
        public virtual OnlineStatementFormat_DAO OnlineStatementFormat
        {
            get
            {
                return this._onlineStatementFormat;
            }
            set
            {
                this._onlineStatementFormat = value;
            }
        }

        /// <summary>
        /// This determines the Language preference correspondence sent to the client.
        /// </summary>
        [BelongsTo("LanguageKey", NotNull = true)]
        [ValidateNonEmpty("Language is a mandatory field")]
        public virtual Language_DAO Language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "OfferMailingAddressKey", ColumnType = "Int32")]
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

        [BelongsTo("LegalEntityKey")]
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

        [BelongsTo("CorrespondenceMediumKey")]
        public virtual CorrespondenceMedium_DAO CorrespondenceMedium
        {
            get
            {
                return this._correspondenceMedium;
            }
            set
            {
                this._correspondenceMedium = value;
            }
        }
    }
}