using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.Services.CommandPersistence
{
    public class HostContextHelper : IHostContextHelper
    {
        private readonly IActiveDirectoryProvider provider;

        public HostContextHelper(IActiveDirectoryProvider provider)
        {
            this.provider = provider;
        }

        public IHostContext CreateHostContextFromUser(string forUsername, IEnumerable<KeyValuePair<string, string>> contextDetails)
        {
            var hostContextFromUser = new ThreadHostContext();
            hostContextFromUser.Initialise(GetUser(forUsername), GetRolesForUsername(forUsername).ToArray(), contextDetails);
            return hostContextFromUser;
        }

        private GenericIdentity GetUser(string forUsername)
        {
            return string.IsNullOrEmpty(forUsername) ? new GenericIdentity("") : new GenericIdentity(forUsername);
        }

        public virtual IEnumerable<string> GetRolesForUsername(string username)
        {
            return provider.GetRoles(username);
        }
    }
}