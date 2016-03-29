using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Validation
{
    public interface IEntityRuleProvider
    {
        List<string> Rules { get; }
    }
}