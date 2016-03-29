using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferExpense", Schema = "dbo", Lazy = true)]
    public partial class ApplicationExpense_DAO : DB_2AM<ApplicationExpense_DAO>
    {
        private string _expenseAccountNumber;

        private string _expenseAccountName;

        private string _expenseReference;

        private double _totalOutstandingAmount;

        private double _monthlyPayment;

        private bool _toBeSettled;

        private bool _overRidden;

        private int _key;

        private ExpenseType_DAO _expenseType;

        private LegalEntity_DAO _legalEntity;

        private Application_DAO _application;

        private IList<ApplicationDebtSettlement_DAO> _applicationDebtSettlements;

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

        [PrimaryKey(PrimaryKeyType.Native, "OfferExpenseKey", ColumnType = "Int32")]
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

        [BelongsTo("ExpenseTypeKey")]
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

        [BelongsTo("LegalEntityKey")]
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

        [BelongsTo("OfferKey")]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        [HasMany(typeof(ApplicationDebtSettlement_DAO), ColumnKey = "OfferExpenseKey", Table = "OfferDebtSettlement", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public virtual IList<ApplicationDebtSettlement_DAO> ApplicationDebtSettlements
        {
            get
            {
                return this._applicationDebtSettlements;
            }
            set
            {
                this._applicationDebtSettlements = value;
            }
        }
    }
}