using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RuleParameter", Schema = "dbo", Lazy = false)]
    public partial class RuleParameter_DAO : DB_2AM<RuleParameter_DAO>
    {
        private string _name;

        private string _value;

        private int _ruleParameterKey;

        private RuleItem_DAO _ruleItem;

        private ParameterType_DAO _ruleParameterType;

        [Property("Name", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Name is a mandatory field")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [Property("Value", ColumnType = "String")]
        public virtual string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "RuleParameterKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._ruleParameterKey;
            }
            set
            {
                this._ruleParameterKey = value;
            }
        }

        [BelongsTo("RuleItemKey", NotNull = true)]
        [ValidateNonEmpty("Rule Item is a mandatory field")]
        public virtual RuleItem_DAO RuleItem
        {
            get
            {
                return this._ruleItem;
            }
            set
            {
                this._ruleItem = value;
            }
        }

        [BelongsTo("ParameterTypeKey", NotNull = true)]
        [ValidateNonEmpty("Parameter Type is a mandatory field")]
        public virtual ParameterType_DAO RuleParameterType
        {
            get
            {
                return this._ruleParameterType;
            }
            set
            {
                this._ruleParameterType = value;
            }
        }
    }
}