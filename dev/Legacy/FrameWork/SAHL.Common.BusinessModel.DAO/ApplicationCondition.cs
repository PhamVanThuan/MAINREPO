using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferCondition", Schema = "dbo")]
    public partial class ApplicationCondition_DAO : DB_2AM<ApplicationCondition_DAO>
    {

        private Condition_DAO _condition;

        private TranslatableItem_DAO _translatableItem;

        private int _key;

        private IList<ApplicationConditionToken_DAO> _applicationConditionTokens;

        private Application_DAO _application;

        [BelongsTo("ConditionKey", NotNull = true)]
        public virtual Condition_DAO ConditionKey
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

        [PrimaryKey(PrimaryKeyType.Native, "OfferConditionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationConditionToken_DAO), ColumnKey = "OfferConditionKey", Table = "OfferConditionToken", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationConditionToken_DAO> ApplicationConditionTokens
        {
            get
            {
                return this._applicationConditionTokens;
            }
            set
            {
                this._applicationConditionTokens = value;
            }
        }

        [BelongsTo("OfferKey", NotNull = true)]
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


    }
    
}
