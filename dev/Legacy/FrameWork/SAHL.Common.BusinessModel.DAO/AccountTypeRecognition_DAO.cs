using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountTypeRecognition", Schema = "dbo", Lazy = true)]
    public partial class AccountTypeRecognition_DAO : DB_2AM<AccountTypeRecognition_DAO>
    {
        private long? _rangeStart;

        private long? _rangeEnd;

        private int? _noOfDigits1;

        private int? _noOfDigits2;

        private int? _digitNo1;

        private int? _mustEqual1;

        private int? _digitNo2;

        private int? _mustEqual2;

        private char _dropDigits;

        private int? _startDropDigits;

        private int? _endDropDigits;

        private string _userID;

        private System.DateTime? _dateChange;

        private int _Key;

        private ACBBank_DAO _aCBBank;

        private ACBType_DAO _aCBType;

        [Property("RangeStart", ColumnType = "Int64")]
        public virtual long? RangeStart
        {
            get
            {
                return this._rangeStart;
            }
            set
            {
                this._rangeStart = value;
            }
        }

        [Property("RangeEnd", ColumnType = "Int64")]
        public virtual long? RangeEnd
        {
            get
            {
                return this._rangeEnd;
            }
            set
            {
                this._rangeEnd = value;
            }
        }

        [Property("NoOfDigits1", ColumnType = "Int32")]
        public virtual int? NoOfDigits1
        {
            get
            {
                return this._noOfDigits1;
            }
            set
            {
                this._noOfDigits1 = value;
            }
        }

        [Property("NoOfDigits2", ColumnType = "Int32")]
        public virtual int? NoOfDigits2
        {
            get
            {
                return this._noOfDigits2;
            }
            set
            {
                this._noOfDigits2 = value;
            }
        }

        [Property("DigitNo1", ColumnType = "Int32")]
        public virtual int? DigitNo1
        {
            get
            {
                return this._digitNo1;
            }
            set
            {
                this._digitNo1 = value;
            }
        }

        [Property("MustEqual1", ColumnType = "Int32")]
        public virtual int? MustEqual1
        {
            get
            {
                return this._mustEqual1;
            }
            set
            {
                this._mustEqual1 = value;
            }
        }

        [Property("DigitNo2", ColumnType = "Int32")]
        public virtual int? DigitNo2
        {
            get
            {
                return this._digitNo2;
            }
            set
            {
                this._digitNo2 = value;
            }
        }

        [Property("MustEqual2", ColumnType = "Int32")]
        public virtual int? MustEqual2
        {
            get
            {
                return this._mustEqual2;
            }
            set
            {
                this._mustEqual2 = value;
            }
        }

        [Property("DropDigits", ColumnType = "Char")]
        public virtual char DropDigits
        {
            get
            {
                return this._dropDigits;
            }
            set
            {
                this._dropDigits = value;
            }
        }

        [Property("StartDropDigits", ColumnType = "Int32")]
        public virtual int? StartDropDigits
        {
            get
            {
                return this._startDropDigits;
            }
            set
            {
                this._startDropDigits = value;
            }
        }

        [Property("EndDropDigits", ColumnType = "Int32")]
        public virtual int? EndDropDigits
        {
            get
            {
                return this._endDropDigits;
            }
            set
            {
                this._endDropDigits = value;
            }
        }

        [Property("UserID", ColumnType = "String")]
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

        [Property("DateChange", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Date Change is a mandatory field")]
        public virtual System.DateTime? DateChange
        {
            get
            {
                return this._dateChange;
            }
            set
            {
                this._dateChange = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AccountTypeRecognitionKey", ColumnType = "Int32")]
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

        [BelongsTo("ACBBankCode", NotNull = true)]
        [ValidateNonEmpty("ACB Bank is a mandatory field")]
        public virtual ACBBank_DAO ACBBank
        {
            get
            {
                return this._aCBBank;
            }
            set
            {
                this._aCBBank = value;
            }
        }

        [BelongsTo("ACBTypeNumber", NotNull = true)]
        [ValidateNonEmpty("ACB Type is a mandatory field")]
        public virtual ACBType_DAO ACBType
        {
            get
            {
                return this._aCBType;
            }
            set
            {
                this._aCBType = value;
            }
        }
    }
}