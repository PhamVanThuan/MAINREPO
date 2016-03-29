using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresActiveClientRole : IDomainCommandCheck
    {
        int ApplicationRoleKey { get; }
    }
}
