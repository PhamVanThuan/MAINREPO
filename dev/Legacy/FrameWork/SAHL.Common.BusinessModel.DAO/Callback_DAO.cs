using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Callback", Schema = "dbo")]
    public partial class Callback_DAO : DB_2AM<Callback_DAO>
    {
        private int _genericKey;
        private GenericKeyType_DAO _genericKeyType;
        private System.DateTime _entryDate;
        private string _entryUser;
        private System.DateTime _callbackDate;
        private string _callbackUser;
        private System.DateTime? _completedDate;
        private string _completedUser;
        //private string _note;
        private int _key;
        private Reason_DAO _reason;

        //[Property("OfferKey", ColumnType = "Int32", NotNull = true)]
        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [Property("GenericKey")]
        public virtual int GenericKey
        {
            get
            {
                return _genericKey;
            }
            set
            {
                _genericKey = value;
            }
        }

        [Property("EntryDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Entry Date is a mandatory field")]
        public virtual System.DateTime EntryDate
        {
            get
            {
                return this._entryDate;
            }
            set
            {
                this._entryDate = value;
            }
        }

        [Property("EntryUser", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Entry User is a mandatory field")]
        public virtual string EntryUser
        {
            get
            {
                return this._entryUser;
            }
            set
            {
                this._entryUser = value;
            }
        }

        [Property("CallbackDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Callback Date is a mandatory field")]
        public virtual System.DateTime CallbackDate
        {
            get
            {
                return this._callbackDate;
            }
            set
            {
                this._callbackDate = value;
            }
        }

        [Property("CallbackUser", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Callback User is a mandatory field")]
        public virtual string CallbackUser
        {
            get
            {
                return this._callbackUser;
            }
            set
            {
                this._callbackUser = value;
            }
        }

        [Property("CompletedDate", NotNull = false)]
        public virtual System.DateTime? CompletedDate
        {
            get
            {
                return this._completedDate;
            }
            set
            {
                this._completedDate = value;
            }
        }

        [Property("CompletedUser", ColumnType = "String")]
        public virtual string CompletedUser
        {
            get
            {
                return this._completedUser;
            }
            set
            {
                this._completedUser = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CallbackKey", ColumnType = "Int32")]
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
        /// The Reason for a callback, which will also provide a link to the object in question.
        /// </summary>
        [BelongsTo("ReasonKey", NotNull = true)]
        [ValidateNonEmpty("Reason is a mandatory field")]
        public virtual Reason_DAO Reason
        {
            get
            {
                return this._reason;
            }
            set
            {
                this._reason = value;
            }
        }
    }
}