using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTestAttribute]
    [ActiveRecord("ManualDebitOrder", Schema = "deb")]
    public class ManualDebitOrder_DAO : DB_2AM<ManualDebitOrder_DAO>
    {
        private int _key;

        private System.DateTime _insertDate;

        private System.DateTime _actionDate;

        private string _reference;

        private double _amount;

        private string _userID;

        private BankAccount_DAO _bankAccount;

        private FinancialService_DAO _financialService;

        private GeneralStatus_DAO _generalStatus;

        private Memo_DAO _memo;

        private TransactionType_DAO _transactionType;

        private BatchTotal_DAO _batchTotal;

        [PrimaryKey(PrimaryKeyType.Native, "ManualDebitOrderKey", ColumnType = "Int32")]
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

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
            }
        }

        [Property("ActionDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ActionDate
        {
            get
            {
                return this._actionDate;
            }
            set
            {
                this._actionDate = value;
            }
        }

        [Property("Reference", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Reference is a mandatory field")]
        public virtual string Reference
        {
            get
            {
                return this._reference;
            }
            set
            {
                this._reference = value;
            }
        }

        [Property("Amount", ColumnType = "Double", NotNull = true)]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
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

        [BelongsTo("BankAccountKey", NotNull = true)]
        public virtual BankAccount_DAO BankAccount
        {
            get
            {
                return this._bankAccount;
            }
            set
            {
                this._bankAccount = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service is a mandatory field")]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
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

        [BelongsTo("MemoKey")]
        public virtual Memo_DAO Memo
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

        [BelongsTo("TransactionTypeKey", NotNull = true)]
        public virtual TransactionType_DAO TransactionType
        {
            get
            {
                return this._transactionType;
            }
            set
            {
                this._transactionType = value;
            }
        }

        [BelongsTo("BatchTotalKey")]
        public virtual BatchTotal_DAO BatchTotal
        {
            get
            {
                return this._batchTotal;
            }
            set
            {
                this._batchTotal = value;
            }
        }
    }
}