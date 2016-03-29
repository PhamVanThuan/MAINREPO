using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ITC", Schema = "Archive")]
    public partial class ITCArchive_DAO : DB_2AM<ITCArchive_DAO>
    {
        private int _legalEntityKey;

        private int? _accountKey;

        private System.DateTime _changeDate;

        private string _responseXML;

        private string _responseStatus;

        private string _userID;

        private string _archiveUser;

        private System.DateTime _archiveDate;

        private string _requestXML;

        private int _Key;

        [Property("LegalEntityKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Key is a mandatory field")]
        public virtual int LegalEntityKey
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

        [Property("AccountKey", ColumnType = "Int32", NotNull = false)]
        [ValidateNonEmpty("Account Key is a mandatory field")]
        public virtual int? AccountKey
        {
            get
            {
                return this._accountKey;
            }
            set
            {
                this._accountKey = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime ChangeDate
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

        [Property("ResponseXML", ColumnType = "String")]
        public virtual string ResponseXML
        {
            get
            {
                return this._responseXML;
            }
            set
            {
                this._responseXML = value;
            }
        }

        [Property("ResponseStatus", ColumnType = "String", Length = 10)]
        public virtual string ResponseStatus
        {
            get
            {
                return this._responseStatus;
            }
            set
            {
                this._responseStatus = value;
            }
        }

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

        [Property("ArchiveUser", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Archive User is a mandatory field")]
        public virtual string ArchiveUser
        {
            get
            {
                return this._archiveUser;
            }
            set
            {
                this._archiveUser = value;
            }
        }

        [Property("ArchiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Archive Date is a mandatory field")]
        public virtual System.DateTime ArchiveDate
        {
            get
            {
                return this._archiveDate;
            }
            set
            {
                this._archiveDate = value;
            }
        }

        [Property("RequestXML", ColumnType = "String")]
        public virtual string RequestXML
        {
            get
            {
                return this._requestXML;
            }
            set
            {
                this._requestXML = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "ITCKey", ColumnType = "Int32")]
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
    }
}