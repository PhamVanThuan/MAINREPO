using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ConditionToken", Schema = "dbo")]
    public partial class ConditionToken_DAO : DB_2AM<ConditionToken_DAO>
    {
        private int _key;

        private Condition_DAO _condition;

        private Token_DAO _token;

        [PrimaryKey(PrimaryKeyType.Native, "ConditionTokenKey", ColumnType = "Int32")]
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

        [BelongsTo("TokenKey", NotNull = true)]
        [ValidateNonEmpty("Token is a mandatory field")]
        public virtual Token_DAO Token
        {
            get
            {
                return this._token;
            }
            set
            {
                this._token = value;
            }
        }
    }
}