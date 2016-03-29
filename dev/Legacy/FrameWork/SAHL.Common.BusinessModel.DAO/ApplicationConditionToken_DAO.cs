using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferConditionToken", Schema = "dbo", Lazy = true)]
    public partial class ApplicationConditionToken_DAO : DB_2AM<ApplicationConditionToken_DAO>
    {
        private Token_DAO _token;

        private TranslatableItem_DAO _translatableItem;

        private string _tokenValue;

        private int _key;

        private ApplicationCondition_DAO _applicationCondition;

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

        //TODO:
        //[Property("TranslatableItemKey", ColumnType = "Int32", NotNull = true)]
        [BelongsTo("TranslatableItemKey", NotNull = false)]
        public virtual TranslatableItem_DAO TranslatableItem
        {
            get
            {
                return this._translatableItem;
            }
            set
            {
                this._translatableItem = value;
            }
        }

        [Property("TokenValue", ColumnType = "String")]
        public virtual string TokenValue
        {
            get
            {
                return this._tokenValue;
            }
            set
            {
                this._tokenValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferConditionTokenKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferConditionKey", NotNull = true)]
        [ValidateNonEmpty("Application Condition is a mandatory field")]
        public virtual ApplicationCondition_DAO ApplicationCondition
        {
            get
            {
                return this._applicationCondition;
            }
            set
            {
                this._applicationCondition = value;
            }
        }
    }
}