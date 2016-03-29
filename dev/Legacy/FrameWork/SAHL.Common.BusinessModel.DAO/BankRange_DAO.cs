using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BankRange", Schema = "dbo", Lazy = true)]
    public partial class BankRange_DAO : DB_2AM<BankRange_DAO>
    {
        private int _rangeStart;

        private int _rangeEnd;

        private string _userID;

        private System.DateTime _dateChange;

        private int _Key;

        private ACBBank_DAO _aCBBank;

        [Property("RangeStart", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Range Start is a mandatory field")]
        public virtual int RangeStart
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

        [Property("RangeEnd", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Range End is a mandatory field")]
        public virtual int RangeEnd
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

        [Property("DateChange", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("DateChange is a mandatory field")]
        public virtual System.DateTime DateChange
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

        [PrimaryKey(PrimaryKeyType.Native, "BankRangeKey", ColumnType = "Int32")]
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
    }
}