using System.Collections.Generic;

namespace SAHL.Core.Identity
{
    public interface IUserDetails : IAdUserDetails, IUserRoles
    {
    }

    public interface IAdUserDetails
    {
        string FullADUsername { get; }

        string UserName { get; }

        string Domain { get; }

        string DisplayName { get; }

        string EmailAddress { get; }

        byte[] UserPhoto { get; }
    }

    public interface IUserRoles
    {
        IUserRole ActiveRole { get; }

        IEnumerable<IUserRole> UserRoles { get; }
    }
}