using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
  public interface IBusinessRule
  {
    bool ExecuteRule(IDomainMessageCollection Messages, IRuleItem RuleItem);
  }
}
