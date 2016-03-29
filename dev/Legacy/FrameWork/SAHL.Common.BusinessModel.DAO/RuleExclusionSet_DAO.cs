using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Groups rule exclusions into a set so that rules can be excluded during validation.  A set
    /// will contain multiple RuleExclusions.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("RuleExclusionSet", Schema = "dbo", Lazy = false)]
    public partial class RuleExclusionSet_DAO : DB_2AM<RuleExclusionSet_DAO>
    {
        private string _description;

        private string _comment;

        private int _key;

        private IList<RuleExclusion_DAO> _ruleExclusions;

        /// <summary>
        /// The description of the RuleExclusionSet.  This is a unique name.
        /// </summary>
        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// Describes what the exclusion set is used for.
        /// </summary>
        [Property("Comment", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Comment is a mandatory field")]
        public virtual string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "RuleExclusionSetKey", ColumnType = "Int32")]
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

        /// <summary>
        /// Gets a list of RuleExclusion objects that fall under the set.
        /// </summary>
        /// <remarks>
        /// <strong>Developers:</strong> Don't make this lazy loaded - we want these always when the set is
        /// loaded as the exclusion sets are cached.
        /// </remarks>
        [HasMany(typeof(RuleExclusion_DAO), ColumnKey = "RuleExclusionSetKey", Table = "RuleExclusion")]
        public virtual IList<RuleExclusion_DAO> RuleExclusions
        {
            get
            {
                return this._ruleExclusions;
            }
            set
            {
                this._ruleExclusions = value;
            }
        }
    }
}