using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Service.Interfaces
{
    /// <summary>
    /// Represents a business rule.
    /// </summary>
    public interface IBusinessRule
    {
        /// <summary>
        /// Starts the nHibernate transaction for the rule to run under
        /// </summary>
        /// <param name="Messages">The domain messages collection.</param>
        /// <param name="rule">A reference to the underlying IRuleItem.</param>
        bool StartRule(IDomainMessageCollection Messages, IRuleItem rule);

        /// <summary>
        /// Runs the rule
        /// </summary>
        /// <param name="Messages">The domain message collection - this will be populated when the rule executes.</param>
        /// <param name="Parameters">Any parameters passed into the rule.</param>
        int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters);

        /// <summary>
        /// Completes the nHibernate transaction for the rule to run under
        /// </summary>
        void CompleteRule();
    }
}