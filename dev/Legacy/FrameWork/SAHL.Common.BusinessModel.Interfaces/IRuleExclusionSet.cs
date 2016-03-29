using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Groups rule exclusions into a set so that rules can be excluded during validation.  A set
    /// will contain multiple RuleExclusions.
    /// </summary>
    public partial interface IRuleExclusionSet : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the RuleExclusionSet.  This is a unique name.
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Describes what the exclusion set is used for.
        /// </summary>
        System.String Comment
        {
            get;
            set;
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of RuleExclusion objects that fall under the set.
        /// </summary>
        IEventList<IRuleExclusion> RuleExclusions
        {
            get;
        }
    }
}