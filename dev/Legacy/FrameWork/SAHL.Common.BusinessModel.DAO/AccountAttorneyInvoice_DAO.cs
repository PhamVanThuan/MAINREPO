using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountAttorneyInvoice", Schema = "dbo")]
    public partial class AccountAttorneyInvoice_DAO : DB_2AM<AccountAttorneyInvoice_DAO>
    {
        private int _key;

        private int _accountKey;

        private int _attorneyKey;

        private string _invoiceNumber;

        private DateTime _invoiceDate;

        private decimal _amount;

        private decimal _vatAmount;

        private decimal _totalAmount;

        private string _comment;

        private DateTime _changeDate;

        [PrimaryKey(PrimaryKeyType.Native, "AccountAttorneyInvoiceKey", ColumnType = "Int32")]
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

        [ValidateNonEmpty("Account is a mandatory field")]
        [Property("AccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountKey
        {
            get
            {
                return this._accountKey;
            }
            set
            {
                this._accountKey = value;
            }
        }

        [ValidateNonEmpty("Attorney is a mandatory field")]
        [Property("AttorneyKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AttorneyKey
        {
            get
            {
                return this._attorneyKey;
            }
            set
            {
                this._attorneyKey = value;
            }
        }

        [ValidateNonEmpty("Invoice Number is a mandatory field")]
        [Property("InvoiceNumber", ColumnType = "String", NotNull = true)]
        public virtual string InvoiceNumber
        {
            get
            {
                return this._invoiceNumber;
            }
            set
            {
                this._invoiceNumber = value;
            }
        }

        [ValidateNonEmpty("Invoice Date is a mandatory field")]
        [Property("InvoiceDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual DateTime InvoiceDate
        {
            get
            {
                return this._invoiceDate;
            }
            set
            {
                this._invoiceDate = value;
            }
        }

        [ValidateNonEmpty("Amount is a mandatory field")]
        [Property("Amount", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal Amount
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

        [Property("VatAmount", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal VatAmount
        {
            get
            {
                return this._vatAmount;
            }
            set
            {
                this._vatAmount = value;
            }
        }

        [Property("TotalAmount", ColumnType = "Decimal", NotNull = true)]
        public virtual decimal TotalAmount
        {
            get
            {
                return this._totalAmount;
            }
            set
            {
                this._totalAmount = value;
            }
        }

        [Property("Comment", ColumnType = "String")]
        public virtual string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }

        [Property("ChangeDate", "Timestamp")]
        public virtual DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }
    }
}