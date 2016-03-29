using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountExpense", Schema = "RCS", Lazy = true)]
    public partial class AccountExpense_DAO : DB_2AM<AccountExpense_DAO>
    {
        private string _expenseAccountNumber;

        private string _expenseAccountName;

        private string _expenseReference;

        private double _totalOutstandingAmount;

        private double _monthlyPayment;

        private bool _toBeSettled;

        private bool _overRidden;

        private int _Key;

        private IList<AccountDebtSettlement_DAO> _accountDebtSettlements;

        private Account_DAO _account;

        private ExpenseType_DAO _expenseType;

        private LegalEntity_DAO _legalEntity;

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

        [Property("OverRidden", ColumnType = "Boolean")]
        public virtual bool OverRidden
        {
            get
            {
                return this._overRidden;
            }
            set
            {
                this._overRidden = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ExpenseKey", ColumnType = "Int32")]
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

        [HasMany(typeof(AccountDebtSettlement_DAO), ColumnKey = "ExpenseKey", Table = "AccountDebtSettlement", Schema = "RCS", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<AccountDebtSettlement_DAO> AccountDebtSettlements
        {
            get
            {
                return this._accountDebtSettlements;
            }
            set
            {
                this._accountDebtSettlements = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = false)]
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

        [BelongsTo("ExpenseTypeKey", NotNull = false)]
        public virtual ExpenseType_DAO ExpenseType
        {
            get
            {
                return this._expenseType;
            }
            set
            {
                this._expenseType = value;
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