using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ClientEmail", Schema = "dbo")]
    public partial class ClientEmail_DAO : DB_SAHL<ClientEmail_DAO>
    {
        private string _emailTo;

        private string _emailCC;

        private string _emailBCC;

        private string _emailSubject;

        private string _emailBody;

        private string _emailAttachment1;

        private string _emailAttachment2;

        private string _emailAttachment3;

        private string _cellphone;

        private decimal _loanNumber;

        private string _emailFrom;

        private System.DateTime _eMailInsertDate;

        private int _contentTypeKey;

        private decimal _key;

        private string _additionalAttachmentsDelimited;

        [Property("EmailTo", ColumnType = "String")]
        public virtual string EmailTo
        {
            get
            {
                return this._emailTo;
            }
            set
            {
                this._emailTo = value;
            }
        }

        [Property("EmailCC", ColumnType = "String")]
        public virtual string EmailCC
        {
            get
            {
                return this._emailCC;
            }
            set
            {
                this._emailCC = value;
            }
        }

        [Property("EmailBCC", ColumnType = "String")]
        public virtual string EmailBCC
        {
            get
            {
                return this._emailBCC;
            }
            set
            {
                this._emailBCC = value;
            }
        }

        [Property("EmailSubject", ColumnType = "String")]
        public virtual string EmailSubject
        {
            get
            {
                return this._emailSubject;
            }
            set
            {
                this._emailSubject = value;
            }
        }

        [Property("EmailBody", ColumnType = "String", Length = 8000)]
        public virtual string EmailBody
        {
            get
            {
                return this._emailBody;
            }
            set
            {
                this._emailBody = value;
            }
        }

        [Property("EmailAttachment1", ColumnType = "String", Length = 150)]
        public virtual string EmailAttachment1
        {
            get
            {
                return this._emailAttachment1;
            }
            set
            {
                this._emailAttachment1 = value;
            }
        }

        [Property("EmailAttachment2", ColumnType = "String", Length = 150)]
        public virtual string EmailAttachment2
        {
            get
            {
                return this._emailAttachment2;
            }
            set
            {
                this._emailAttachment2 = value;
            }
        }

        [Property("EmailAttachment3", ColumnType = "String", Length = 150)]
        public virtual string EmailAttachment3
        {
            get
            {
                return this._emailAttachment3;
            }
            set
            {
                this._emailAttachment3 = value;
            }
        }

        [Property("Cellphone", ColumnType = "String", Length = 15)]
        public virtual string Cellphone
        {
            get
            {
                return this._cellphone;
            }
            set
            {
                this._cellphone = value;
            }
        }

        [Property("LoanNumber", ColumnType = "Decimal", Length = 18)]
        public virtual decimal LoanNumber
        {
            get
            {
                return this._loanNumber;
            }
            set
            {
                this._loanNumber = value;
            }
        }

        [Property("EmailFrom", ColumnType = "String")]
        public virtual string EmailFrom
        {
            get
            {
                return this._emailFrom;
            }
            set
            {
                this._emailFrom = value;
            }
        }

        [Property("EMailInsertDate", ColumnType = "Timestamp")]
        public virtual System.DateTime EMailInsertDate
        {
            get
            {
                return this._eMailInsertDate;
            }
            set
            {
                this._eMailInsertDate = value;
            }
        }

        [Property("ContentTypeKey", ColumnType = "Int32", NotNull = false)]
        public virtual int ContentTypeKey
        {
            get
            {
                return this._contentTypeKey;
            }
            set
            {
                this._contentTypeKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "EmailNumber", ColumnType = "Decimal", Length = 18)]
        public virtual decimal Key
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

        [Property("AdditionalAttachmentsDelimited", ColumnType = "String", Length = 1024)]
        public virtual string AdditionalAttachmentsDelimited
        {
            get
            {
                return this._additionalAttachmentsDelimited;
            }
            set
            {
                this._additionalAttachmentsDelimited = value;
            }
        }
    }
}