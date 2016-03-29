using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportAccountExpense", Schema = "dbo")]
    public partial class ImportAccountExpense_DAO : DB_2AM<ImportAccountExpense_DAO>
    {

        private string _expenseTypeKey;

        private string _expenseAccountNumber;

        private string _expenseAccountName;

        private string _expenseReference;

        private double _totalOutstandingAmount;

        private double _monthlyPayment;

        private bool _toBeSettled;

        private int _key;

        private ImportApplication_DAO _importApplication;

        [Property("ExpenseTypeKey", ColumnType = "String")]
        public virtual string ExpenseTypeKey
        {
            get
            {
                return this._expenseTypeKey;
            }
            set
            {
                this._expenseTypeKey = value;
            }
        }

        [Property("ExpenseAccountNumber", ColumnType = "String")]
        public virtual string ExpenseAccountNumber
        {
            get
            {
                return this._expenseAccountNumber;
            }
            set
            {
                this._expenseAccountNumber = value;
            }
        }

        [Property("ExpenseAccountName", ColumnType = "String")]
        public virtual string ExpenseAccountName
        {
            get
            {
                return this._expenseAccountName;
            }
            set
            {
                this._expenseAccountName = value;
            }
        }

        [Property("ExpenseReference", ColumnType = "String")]
        public virtual string ExpenseReference
        {
            get
            {
                return this._expenseReference;
            }
            set
            {
                this._expenseReference = value;
            }
        }

        [Property("TotalOutstandingAmount", ColumnType = "Double")]
        public virtual double TotalOutstandingAmount
        {
            get
            {
                return this._totalOutstandingAmount;
            }
            set
            {
                this._totalOutstandingAmount = value;
            }
        }

        [Property("MonthlyPayment", ColumnType = "Double")]
        public virtual double MonthlyPayment
        {
            get
            {
                return this._monthlyPayment;
            }
            set
            {
                this._monthlyPayment = value;
            }
        }

        [Property("ToBeSettled", ColumnType = "Boolean")]
        public virtual bool ToBeSettled
        {
            get
            {
                return this._toBeSettled;
            }
            set
            {
                this._toBeSettled = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AccountExpenseKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferKey", NotNull = true)]
        public virtual ImportApplication_DAO ImportApplication
        {
            get
            {
                return this._importApplication;
            }
            set
            {
                this._importApplication = value;
            }
        }
    }
}
