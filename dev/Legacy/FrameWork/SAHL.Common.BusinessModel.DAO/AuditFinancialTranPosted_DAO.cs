using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AuditFinancialTranPosted", Schema = "dbo")]
    public partial class AuditFinancialTranPosted_DAO : DB_2AM<AuditFinancialTranPosted_DAO>
    {
        private int _loanNumber;

        private short _transactionTypeNumber;

        private System.DateTime _transactionEffectiveDate;

        private System.DateTime _transactionInsertDate;

        private double _transactionAmount;

        private string _transactionReference;

        private string _userID;

        private decimal _key;

        [Property("LoanNumber", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Loan Number is a mandatory field")]
        public virtual int LoanNumber
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

        [Property("TransactionTypeNumber", ColumnType = "Int16", NotNull = true)]
        [ValidateNonEmpty("Transaction Type Number is a mandatory field")]
        public virtual short TransactionTypeNumber
        {
            get
            {
                return this._transactionTypeNumber;
            }
            set
            {
                this._transactionTypeNumber = value;
            }
        }

        [Property("TransactionEffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Transaction Effective Date is a mandatory field")]
        public virtual System.DateTime TransactionEffectiveDate
        {
            get
            {
                return this._transactionEffectiveDate;
            }
            set
            {
                this._transactionEffectiveDate = value;
            }
        }

        [Property("TransactionInsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Transaction Insert Date is a mandatory field")]
        public virtual System.DateTime TransactionInsertDate
        {
            get
            {
                return this._transactionInsertDate;
            }
            set
            {
                this._transactionInsertDate = value;
            }
        }

        [Property("TransactionAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Transaction Amount is a mandatory field")]
        public virtual double TransactionAmount
        {
            get
            {
                return this._transactionAmount;
            }
            set
            {
                this._transactionAmount = value;
            }
        }

        [Property("TransactionReference", ColumnType = "String")]
        public virtual string TransactionReference
        {
            get
            {
                return this._transactionReference;
            }
            set
            {
                this._transactionReference = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "AuditNumber", ColumnType = "Decimal")]
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
    }
}