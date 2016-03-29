using System.Collections.Generic;
using Castle.ActiveRecord;

using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Condition", Schema = "dbo", Lazy = true)]
    public partial class Condition_DAO : DB_2AM<Condition_DAO>
    {
        private string _conditionPhrase;

        private string _tokenDescriptions;

        private TranslatableItem_DAO _translatableItem;

        private string _conditionName;

        private int _key;

        private IList<ConditionSetCondition_DAO> _conditionSetConditions;

        private IList<ConditionToken_DAO> _conditionTokens;

        private ConditionType_DAO _conditionType;

        [Property("ConditionPhrase", ColumnType = "String")]
        public virtual string ConditionPhrase
        {
            get
            {
                return this._conditionPhrase;
            }
            set
            {
                this._conditionPhrase = value;
            }
        }

        [Property("TokenDescriptions", ColumnType = "String")]
        public virtual string TokenDescriptions
        {
            get
            {
                return this._tokenDescriptions;
            }
            set
            {
                this._tokenDescriptions = value;
            }
        }

        //[Property("TranslatableItemKey", ColumnType = "Int32")]
        [BelongsTo("TranslatableItemKey")]
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

        [Property("ConditionName", ColumnType = "String", NotNull = true)]
        public virtual string ConditionName
        {
            get
            {
                return this._conditionName;
            }
            set
            {
                this._conditionName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ConditionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ConditionSetCondition_DAO), ColumnKey = "ConditionKey", Lazy = true, Table = "ConditionSetCondition", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ConditionSetCondition_DAO> ConditionSetConditions
        {
            get
            {
                return this._conditionSetConditions;
            }
            set
            {
                this._conditionSetConditions = value;
            }
        }

        [HasMany(typeof(ConditionToken_DAO), ColumnKey = "ConditionKey", Lazy = true, Table = "ConditionToken", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ConditionToken_DAO> ConditionTokens
        {
            get
            {
                return this._conditionTokens;
            }
            set
            {
                this._conditionTokens = value;
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
    }
}