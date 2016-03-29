using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// OfferCondition_DAO is instantiated in order to find the Conditions which are associated to a particular Application.
    /// </summary>
    [ActiveRecord("OfferCondition", Schema = "dbo", Lazy = true)]
    public partial class ApplicationCondition_DAO : DB_2AM<ApplicationCondition_DAO>
    {
        private Condition_DAO _condition;

        private TranslatableItem_DAO _translatableItem;

        private int _key;

        private IList<ApplicationConditionToken_DAO> _applicationConditionTokens;

        private Application_DAO _application;

        private ConditionSet_DAO _conditionSet;

        /// <summary>
        /// The Condition which is associated to the Application.
        /// </summary>
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

        //TODO :
        /// <summary>
        /// Each Condition is related to a TranslatableItem, from which the translated version of the condition can be retrieved.
        /// </summary>
        [BelongsTo("TranslatableItemKey", NotNull = true)]
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

        /// <summary>
        /// Primary Key
        /// </summary>
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

        [HasMany(typeof(ApplicationConditionToken_DAO), ColumnKey = "OfferConditionKey", Table = "OfferConditionToken", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
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

        /// <summary>
        /// The Application Number from the Offer table
        /// </summary>
        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
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

        [BelongsTo("ConditionSetKey")]
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
    }
}