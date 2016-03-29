using SAHL.Common.Security;
using SAHL.Common.UserProfiles;

namespace SAHL.Common.Service.Interfaces
{
    public interface IUserProfileService
    {
        UserProfile GetUserProfile(SAHLPrincipal principal);

        void PersistUserProfile(SAHLPrincipal principal, UserProfile UserProfile);
    }
}