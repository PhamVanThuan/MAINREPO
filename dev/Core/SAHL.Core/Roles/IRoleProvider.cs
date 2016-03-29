using System.Collections.Generic;

namespace SAHL.Core.Roles
{
    public interface IRoleProvider
    {
        IEnumerable<string> GetRoles(string username);
    }
}