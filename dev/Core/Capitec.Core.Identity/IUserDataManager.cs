using SAHL.Core.Data.Models.Capitec;
using System;
using System.Collections.Generic;

namespace Capitec.Core.Identity
{
    public interface IUserDataManager
    {
        bool DoesUsernameExist(string username);

        bool DoesUserIdExist(Guid userId);

        Guid CreateInvitedUser(string username);

        void AddUserDetails(Guid userId, string emailAddress, string firstName, string lastName);

        InvitationDataModel AddUserInvitation(Guid userId);

        void AssignRolesToUser(Guid userId, Guid[] rolesToAssign);

        void RemoveRolesFromUser(Guid userId, Guid[] roleIdsToRemove);

        void ActivateUser(Guid userId);

        void DeactivateUser(Guid userId);

        void LockUserAccount(Guid userId);

        void UnlockUserAccount(Guid userId);

        void ChangeUserPassword(Guid userId, string hashedPassword);

        Guid ValididateInvitationToken(Guid invitationToken);

        void AcceptInvitationToken(Guid invitationToken);

        UserDataModel GetUserFromUsername(string username);

        UserInformationDataModel GetUserInformationFromUser(Guid userId);

        IEnumerable<RoleDataModel> GetRolesFromUser(Guid userId);

        void UpdateUserToken(Guid userId, Guid token, string ipAddress);

        void UpdateUserLoginAndActivity(Guid userId);

        void UpdateUserActivity(Guid userId);

        void RemoveUserToken(Guid userId);

        UserDataModel GetUserFromToken(Guid authToken);

        void SetUserPasswordAndActivate(Guid userId, string hashedPassword);

        bool HasUserEmailChanged(Guid userId, string emailAddress);

        void UpdateUser(Guid userId, bool isActive);

        bool HasUserDataChanged(Guid userId, bool isActive);

        void UpdateUserInformation(Guid userId, string emailAddress, string firstName, string lastName);

        bool HasUserInformationDataChanged(Guid userId, string emailAddress, string firstName, string lastName);
    }
}