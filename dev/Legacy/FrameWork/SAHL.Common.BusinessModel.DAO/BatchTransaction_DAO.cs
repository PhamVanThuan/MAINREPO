using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BatchTransaction", Lazy = true, Schema = "dbo")]
    public partial class BatchTransaction_DAO : DB_2AM<BatchTransaction_DAO>
    {
        private int _transactionTypeNumber;

        private System.DateTime _effectiveDate;

        private double _amount;

        private string _reference;

        private string _userID;

        private int _key;

        private IList<BatchLoanTransaction_DAO> _batchLoanTransactions;

        private Account_DAO _account;

        private BatchTransactionStatus_DAO _batchTransactionStatus;

        private BulkBatch_DAO _bulkBatch;

        private LegalEntity_DAO _legalEntity;

        [BelongsTo("BatchTransactionStatusKey", Update = true, NotNull = true)]
        [ValidateNonEmpty("Batch Transaction Status is a mandatory field")]
        public virtual BatchTransactionStatus_DAO BatchTransactionStatus
        {
            get
            {
                return this._batchTransactionStatus;
            }
            set
            {
                this._batchTransactionStatus = value;
            }
        }

        [Property("TransactionTypeNumber", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Transaction Type Number is a mandatory field")]
        public virtual int TransactionTypeNumber
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

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [Property("Amount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Amount is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "BatchTransactionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(BatchLoanTransaction_DAO), ColumnKey = "BatchTransactionKey", Table = "BatchLoanTransaction", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<BatchLoanTransaction_DAO> BatchLoanTransactions
        {
            get
            {
                return this._batchLoanTransactions;
            }
            set
            {
                this._batchLoanTransactions = value;
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

        [BelongsTo("BulkBatchKey", NotNull = true)]
        [ValidateNonEmpty("Bulk Batch is a mandatory field")]
        public virtual BulkBatch_DAO BulkBatch
        {
            get
            {
                return this._bulkBatch;
            }
            set
            {
                this._bulkBatch = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = false)]
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
    }
}