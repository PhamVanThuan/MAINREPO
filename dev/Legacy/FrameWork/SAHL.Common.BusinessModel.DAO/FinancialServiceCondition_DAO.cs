using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FinancialServiceCondition", Schema = "dbo", Lazy = true)]
    public partial class FinancialServiceCondition_DAO : DB_2AM<FinancialServiceCondition_DAO>
    {
        private string _userDefinedConditionText;

        private int _Key;

        private Condition_DAO _condition;

        private ConditionType_DAO _conditionType;

        private FinancialService_DAO _financialService;

        [Property("UserDefinedConditionText", ColumnType = "String", Length = 800)]
        public virtual string UserDefinedConditionText
        {
            get
            {
                return this._userDefinedConditionText;
            }
            set
            {
                this._userDefinedConditionText = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FinancialServiceConditionKey", ColumnType = "Int32")]
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

        [BelongsTo("ConditionKey", NotNull = false)]
        public virtual Condition_DAO Condition
        {
            get
            {
                return this._condition;
            }
            set
            {
                this._condition = value;
            }
        }

        [BelongsTo("ConditionTypeKey", NotNull = false)]
        public virtual ConditionType_DAO ConditionType
        {
            get
            {
                return this._conditionType;
            }
            set
            {
                this._conditionType = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = false)]
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
    }
}