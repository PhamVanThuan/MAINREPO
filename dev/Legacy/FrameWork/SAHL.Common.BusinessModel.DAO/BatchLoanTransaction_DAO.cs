using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BatchLoanTransaction", Lazy = true, Schema = "dbo")]
    public partial class BatchLoanTransaction_DAO : DB_2AM<BatchLoanTransaction_DAO>
    {
        private int _loanTransactionNumber;

        private int _key;

        private BatchTransaction_DAO _batchTransaction;

        [Property("LoanTransactionNumber", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Loan Transaction Number is a mandatory field")]
        public virtual int LoanTransactionNumber
        {
            get
            {
                return this._loanTransactionNumber;
            }
            set
            {
                this._loanTransactionNumber = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "BatchLoanTransactionKey", ColumnType = "Int32")]
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

        [BelongsTo("BatchTransactionKey", NotNull = true)]
        [ValidateNonEmpty("Batch Transaction is a mandatory field")]
        public virtual BatchTransaction_DAO BatchTransaction
        {
            get
            {
                return this._batchTransaction;
            }
            set
            {
                this._batchTransaction = value;
            }
        }
    }
}