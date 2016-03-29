using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IWorkflowRuleSet : IEntityValidation, IBusinessModelObject
    {
        List<string> RulesToRun { get; }

        IList<int> RuleKeys { get; }
    }
}