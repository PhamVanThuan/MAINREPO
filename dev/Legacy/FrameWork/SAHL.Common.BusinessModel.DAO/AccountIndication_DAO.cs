using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountIndication", Schema = "dbo", Lazy = true)]
    public partial class AccountIndication_DAO : DB_2AM<AccountIndication_DAO>
    {
        private int _accountIndicator;

        private char _indicator;

        private string _userID;

        private System.DateTime _dateChange;

        private int _Key;

        private AccountIndicationType_DAO _accountIndicationType;

        [Property("AccountIndicator", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Account Indicator is a mandatory field")]
        public virtual int AccountIndicator
        {
            get
            {
                return this._accountIndicator;
            }
            set
            {
                this._accountIndicator = value;
            }
        }

        [Property("Indicator", ColumnType = "Char", NotNull = true)]
        [ValidateNonEmpty("Indicator is a mandatory field")]
        public virtual char Indicator
        {
            get
            {
                return this._indicator;
            }
            set
            {
                this._indicator = value;
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
        [ValidateNonEmpty("Date Change is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "AccountIndicationKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountIndicationTypeKey", NotNull = true)]
        [ValidateNonEmpty("Account Indication Type is a mandatory field")]
        public virtual AccountIndicationType_DAO AccountIndicationType
        {
            get
            {
                return this._accountIndicationType;
            }
            set
            {
                this._accountIndicationType = value;
            }
        }
    }
}