using SAHL.Core.ActiveDirectory.Credentials;

namespace SAHL.Core.ActiveDirectory.Query
{
    public interface IActiveDirectoryQueryFactory
    {
        IActiveDirectoryQuery Create(ICredentials credentials, string filter);
    }
}