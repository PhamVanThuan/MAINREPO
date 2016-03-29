using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountDocument", Schema = "RCS", Lazy = true)]
    public partial class AccountDocument_DAO : DB_2AM<AccountDocument_DAO>
    {
        private int? _legalEntityKey;

        private bool? _documentReceived;

        private System.DateTime? _documentReceivedDate;

        private string _documentReceivedBy;

        private string _documentVersionNumber;

        private int _Key;

        private Account_DAO _account;

        private DocumentType_DAO _documentType;

        [Property("LegalEntityKey", ColumnType = "Int32")]
        public virtual int? LegalEntityKey
        {
            get
            {
                return this._legalEntityKey;
            }
            set
            {
                this._legalEntityKey = value;
            }
        }

        [Property("DocumentReceived", ColumnType = "Boolean")]
        public virtual bool? DocumentReceived
        {
            get
            {
                return this._documentReceived;
            }
            set
            {
                this._documentReceived = value;
            }
        }

        [Property("DocumentReceivedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? DocumentReceivedDate
        {
            get
            {
                return this._documentReceivedDate;
            }
            set
            {
                this._documentReceivedDate = value;
            }
        }

        [Property("DocumentReceivedBy", ColumnType = "String")]
        public virtual string DocumentReceivedBy
        {
            get
            {
                return this._documentReceivedBy;
            }
            set
            {
                this._documentReceivedBy = value;
            }
        }

        [Property("DocumentVersionNumber", ColumnType = "String")]
        public virtual string DocumentVersionNumber
        {
            get
            {
                return this._documentVersionNumber;
            }
            set
            {
                this._documentVersionNumber = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AccountDocumentKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountKey", NotNull = false)]
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

        [BelongsTo("DocumentTypeKey", NotNull = false)]
        public virtual DocumentType_DAO DocumentType
        {
            get
            {
                return this._documentType;
            }
            set
            {
                this._documentType = value;
            }
        }
    }
}