using System.Collections.Generic;
using System.Drawing;

namespace SAHL.Core.UI.UserState.Models
{
    public interface IUserDetails : IUserPrincipal
    {
        string DisplayName { get; }

        Image UserPhoto { get; }

        IEnumerable<UserRole> UserRoles { get; }

        void UpdatePhoto(Image photoToUpdate);

        void AddUserRole(string organisationArea, string roleName, string url);

        void ChangeActiveRole(string organisationArea, string roleName);
    }
}