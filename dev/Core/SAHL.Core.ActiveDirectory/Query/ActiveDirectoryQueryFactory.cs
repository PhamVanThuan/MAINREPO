using SAHL.Core.ActiveDirectory.Credentials;

namespace SAHL.Core.ActiveDirectory.Query
{
    public class ActiveDirectoryQueryFactory : IActiveDirectoryQueryFactory
    {
        public virtual IActiveDirectoryQuery Create(ICredentials credentials, string filter)
        {
            return new ActiveDirectoryQuery(credentials, filter);
        }
    }
}