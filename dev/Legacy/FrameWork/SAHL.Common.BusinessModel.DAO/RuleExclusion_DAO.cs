using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTestAttribute]
    [ActiveRecord("RuleExclusion", Schema = "dbo", Lazy = false)]
    public partial class RuleExclusion_DAO : DB_2AM<RuleExclusion_DAO>
    {
        private int _key;

        private RuleExclusionSet_DAO _ruleExclusionSet;

        private int _ruleItemKey;

        [PrimaryKey(PrimaryKeyType.Native, "RuleExclusionKey", ColumnType = "Int32")]
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

        [BelongsTo("RuleExclusionSetKey", NotNull = true)]
        [ValidateNonEmpty("Rule Exclusion Set is a mandatory field")]
        public virtual RuleExclusionSet_DAO RuleExclusionSet
        {
            get
            {
                return this._ruleExclusionSet;
            }
            set
            {
                this._ruleExclusionSet = value;
            }
        }

        /// <summary>
        /// The key of the <see cref="RuleItem_DAO"/> object.  This is declared as an int for
        /// performance reasons - the RuleItem is not usually required for exclusions as all
        /// we're interested in is the key.  If you require the item, load it.
        /// </summary>
        [Property("RuleItemKey", ColumnType = "Int32", NotNull = true)]
        public virtual int RuleItemKey
        {
            get
            {
                return this._ruleItemKey;
            }
            set
            {
                this._ruleItemKey = value;
            }
        }
    }
}