using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("MailingAddress", Schema = "dbo", Lazy = true)]
    public partial class MailingAddress_DAO : DB_2AM<MailingAddress_DAO>
    {
        private bool _onlineStatement;

        private OnlineStatementFormat_DAO _onlineStatementFormat;

        private int _Key;

        private Account_DAO _account;

        private Address_DAO _address;

        private Language_DAO _language;

        private CorrespondenceMedium_DAO _correspondenceMedium;

        private LegalEntity_DAO _legalEntity;

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

        [PrimaryKey(PrimaryKeyType.Native, "MailingAddressAccountKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

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