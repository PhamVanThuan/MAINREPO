using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferConditionToken", Schema = "dbo")]
    public partial class ApplicationConditionToken_DAO : DB_2AM<ApplicationConditionToken_DAO>
    {
        private Token_DAO _token;

        private TranslatableItem_DAO _translatableItem;

        private string _tokenValue;

        private int _key;

        private ApplicationCondition_DAO _applicationCondition;

        [BelongsTo("TokenKey", NotNull = true)]
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
