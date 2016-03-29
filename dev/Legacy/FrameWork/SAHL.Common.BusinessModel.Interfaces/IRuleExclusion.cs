using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO
    /// </summary>
    public partial interface IRuleExclusion : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO.RuleExclusionSet
        /// </summary>
        IRuleExclusionSet RuleExclusionSet
        {
            get;
            set;
        }

        /// <summary>
        /// The key of the  object.  This is declared as an int for
        /// performance reasons - the RuleItem is not usually required for exclusions as all
        /// we're interested in is the key.  If you require the item, load it.
        /// </summary>
        System.Int32 RuleItemKey
        {
            get;
            set;
        }
    }
}