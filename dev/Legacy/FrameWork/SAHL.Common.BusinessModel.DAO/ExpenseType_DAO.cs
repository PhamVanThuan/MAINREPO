using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ExpenseType", Schema = "dbo")]
    public partial class ExpenseType_DAO : DB_2AM<ExpenseType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<AccountExpense_DAO> _accountExpenses;

        //private IList<LegalEntityAffordabilityExpense_DAO> _legalEntityAffordabilityExpenses;
        // todo: Uncomment when OriginationSourceProductExpenseType implemented
        //private IList<OriginationSourceProductExpenseType> _originationSourceProductExpenseTypes;

        private ExpenseTypeGroup_DAO _expenseTypeGroup;

        private PaymentType_DAO _paymentType;

        [Property("Description", ColumnType = "String")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ExpenseTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(AccountExpense_DAO), ColumnKey = "ExpenseTypeKey", Table = "AccountExpense")]
        //public virtual IList<AccountExpense_DAO> AccountExpenses
        //{
        //    get
        //    {
        //        return this._accountExpenses;
        //    }
        //    set
        //    {
        //        this._accountExpenses = value;
        //    }
        //}

        //[HasMany(typeof(LegalEntityAffordabilityExpense_DAO), ColumnKey = "ExpenseTypeKey", Table = "LegalEntityAffordabilityExpense")]
        //public virtual IList<LegalEntityAffordabilityExpense_DAO> LegalEntityAffordabilityExpenses
        //{
        //    get
        //    {
        //        return this._legalEntityAffordabilityExpenses;
        //    }
        //    set
        //    {
        //        this._legalEntityAffordabilityExpenses = value;
        //    }
        //}

        // todo: Uncomment when OriginationSourceProductExpenseType implemented
        //[HasMany(typeof(OriginationSourceProductExpenseType), ColumnKey = "ExpenseTypeKey", Table = "OriginationSourceProductExpenseType")]
        //public virtual IList<OriginationSourceProductExpenseType> OriginationSourceProductExpenseTypes
        //{
        //    get
        //    {
        //        return this._originationSourceProductExpenseTypes;
        //    }
        //    set
        //    {
        //        this._originationSourceProductExpenseTypes = value;
        //    }
        //}

        [BelongsTo("ExpenseTypeGroupKey", NotNull = false)]
        public virtual ExpenseTypeGroup_DAO ExpenseTypeGroup
        {
            get
            {
                return this._expenseTypeGroup;
            }
            set
            {
                this._expenseTypeGroup = value;
            }
        }

        [BelongsTo("PaymentTypeKey", NotNull = false)]
        public virtual PaymentType_DAO PaymentType
        {
            get
            {
                return this._paymentType;
            }
            set
            {
                this._paymentType = value;
            }
        }
    }
}