using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Guarantee", Schema = "dbo")]
    public partial class Guarantee_DAO : DB_2AM<Guarantee_DAO>
    {
        private double _limitedAmount;

        private System.DateTime _issueDate;

        private byte _statusNumber;

        private System.DateTime? _cancelledDate;

        private int _Key;

        private Account_DAO _account;

        [Property("LimitedAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Limited Amount is a mandatory field")]
        public virtual double LimitedAmount
        {
            get
            {
                return this._limitedAmount;
            }
            set
            {
                this._limitedAmount = value;
            }
        }

        [Property("IssueDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Issue Date is a mandatory field")]
        public virtual System.DateTime IssueDate
        {
            get
            {
                return this._issueDate;
            }
            set
            {
                this._issueDate = value;
            }
        }

        [Property("StatusNumber", ColumnType = "Byte", NotNull = true)]
        [ValidateNonEmpty("Status Number is a mandatory field")]
        public virtual byte StatusNumber
        {
            get
            {
                return this._statusNumber;
            }
            set
            {
                this._statusNumber = value;
            }
        }

        [Property("CancelledDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CancelledDate
        {
            get
            {
                return this._cancelledDate;
            }
            set
            {
                this._cancelledDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "GuaranteeKey", ColumnType = "Int32")]
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
    }
}