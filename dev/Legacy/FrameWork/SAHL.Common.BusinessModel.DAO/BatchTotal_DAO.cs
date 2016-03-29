using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTestAttribute]
    [ActiveRecord("BatchTotal", Schema = "deb")]
    public partial class BatchTotal_DAO : DB_2AM<BatchTotal_DAO>
    {
        private int _key;

        //private int _accountKey;

        private double _amount;

        private System.DateTime _debitOrderDate;

        private System.DateTime _actionDate;

        private int _transactionTypeKey;

        private IList<ManualDebitOrder_DAO> _manualDebitOrders;

        private Account_DAO _account;

        private TransactionType_DAO _transactionType;

        [PrimaryKey(PrimaryKeyType.Native, "BatchTotalKey", ColumnType = "Int32")]
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

        [Property("Amount", ColumnType = "Double")]
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

        [Property("DebitOrderDate", ColumnType = "Timestamp")]
        public virtual System.DateTime DebitOrderDate
        {
            get
            {
                return this._debitOrderDate;
            }
            set
            {
                this._debitOrderDate = value;
            }
        }

        [Property("ActionDate", ColumnType = "Timestamp")]
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

        [Property("TransactionTypeKey", ColumnType = "Int32")]
        public virtual int TransactionTypeKey
        {
            get
            {
                return this._transactionTypeKey;
            }
            set
            {
                this._transactionTypeKey = value;
            }
        }

        [HasMany(typeof(ManualDebitOrder_DAO), ColumnKey = "BatchTotalKey", Table = "ManualDebitOrder")]
        public virtual IList<ManualDebitOrder_DAO> ManualDebitOrders
        {
            get
            {
                return this._manualDebitOrders;
            }
            set
            {
                this._manualDebitOrders = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = true)]
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
    }
}