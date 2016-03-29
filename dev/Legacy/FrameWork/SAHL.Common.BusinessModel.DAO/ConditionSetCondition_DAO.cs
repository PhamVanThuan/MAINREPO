using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ConditionSetCondition", Schema = "dbo")]
    public partial class ConditionSetCondition_DAO : DB_2AM<ConditionSetCondition_DAO>
    {
        private bool _requiredCondition;

        private int _key;

        private ConditionSet_DAO _conditionSet;

        private Condition_DAO _condition;

        [Property("RequiredCondition", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Required Condition is a mandatory field")]
        public virtual bool RequiredCondition
        {
            get
            {
                return this._requiredCondition;
            }
            set
            {
                this._requiredCondition = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ConditionSetConditionKey", ColumnType = "Int32")]
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

        [BelongsTo("ConditionSetKey", NotNull = true)]
        [ValidateNonEmpty("Condition Set is a mandatory field")]
        public virtual ConditionSet_DAO ConditionSet
        {
            get
            {
                return this._conditionSet;
            }
            set
            {
                this._conditionSet = value;
            }
        }

        [BelongsTo("ConditionKey", NotNull = true)]
        [ValidateNonEmpty("Condition is a mandatory field")]
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
    }
}