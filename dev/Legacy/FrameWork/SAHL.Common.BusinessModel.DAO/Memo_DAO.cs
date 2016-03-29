using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Memo", Schema = "dbo")]
    public partial class Memo_DAO : DB_2AM<Memo_DAO>
    {
        private int _genericKey;

        private System.DateTime _insertedDate;

        private string _memo;

        private ADUser_DAO _aDUser;

        private System.DateTime? _reminderDate;

        private System.DateTime? _expiryDate;

        private int _key;

        private GeneralStatus_DAO _generalStatus;

        private GenericKeyType_DAO _genericKeyType;

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Key is a mandatory field")]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [Property("InsertedDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Inserted Date is a mandatory field")]
        public virtual System.DateTime InsertedDate
        {
            get
            {
                return this._insertedDate;
            }
            set
            {
                this._insertedDate = value;
            }
        }

        [Property("ReminderDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? ReminderDate
        {
            get
            {
                return this._reminderDate;
            }
            set
            {
                this._reminderDate = value;
            }
        }

        [Property("ExpiryDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? ExpiryDate
        {
            get
            {
                return this._expiryDate;
            }
            set
            {
                this._expiryDate = value;
            }
        }

        [Property("Memo", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Memo is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._memo;
            }
            set
            {
                this._memo = value;
            }
        }

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User is a mandatory field")]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "MemoKey", ColumnType = "Int32")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
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
    }
}