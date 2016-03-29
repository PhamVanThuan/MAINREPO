using System;

namespace Capitec.Core.Identity
{
    public interface IUserManager
    {
        void AcceptInvitation(Guid invitationToken, string password);

        void ActivateUser(Guid userId);

        void ChangePassword(Guid userId, string newPassword);

        void DeactivateUser(Guid userId);

        Guid InviteUser(string username, string emailAddress, string firstName, string lastName, Guid[] rolesToAssign);

        void LockUserAccount(Guid userId);

        void AssignUserRole(Guid userId, Guid[] roleIds);

        void RemoveUserRole(Guid userId, Guid[] roleIds);

        void UnlockUserAccount(Guid userId);

        void UpdateUser(Guid userId, bool status);

        void UpdateUserInformation(Guid userId, string emailAddress, string firstName, string lastName);
    }
}