using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ITC", Schema = "dbo")]
    public partial class ITC_DAO : DB_2AM<ITC_DAO>
    {
        private System.DateTime _changeDate;

        private string _responseXML;

        private string _responseStatus;

        private string _userID;

        private string _requestXML;

        private int _Key;

        private AccountSequence_DAO _reservedaccount;

        private LegalEntity_DAO _legalEntity;

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
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

        [Property("ResponseXML", ColumnType = "StringClob")]
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

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "ITCKey", ColumnType = "Int32")]
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
        [ValidateNonEmpty("Reserved Account is a mandatory field")]
        public virtual AccountSequence_DAO ReservedAccount
        {
            get
            {
                return this._reservedaccount;
            }
            set
            {
                this._reservedaccount = value;
            }
        }

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
    }
}