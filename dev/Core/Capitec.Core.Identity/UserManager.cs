using Capitec.Core.Identity.Exceptions;
using SAHL.Core.Communication;
using SAHL.Core.Data;
using System;
using System.Collections.Generic;

namespace Capitec.Core.Identity
{
    public class UserManager : IUserManager
    {
        private readonly IUserDataManager userDataManager;
        private readonly IUserCommunicationService userCommunicationService;
        private readonly IPasswordManager passwordManager;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UserManager(IUserDataManager userDataManager, IUserCommunicationService userCommunicationService, IPasswordManager passwordManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.userDataManager = userDataManager;
            this.userCommunicationService = userCommunicationService;
            this.passwordManager = passwordManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Guid InviteUser(string username, string emailAddress, string firstName, string lastName, Guid[] rolesToAssign)
        {
            Guid userId;
            bool userExists = userDataManager.DoesUsernameExist(username);
            if (userExists)
            {
                throw new UsernameAlreadyExistsException(username);
            }

            //using (var uow = new UnitOfWork())
            using (var uow = unitOfWorkFactory.Build())
            {
                // create the user
                userId = userDataManager.CreateInvitedUser(username);

                // add aditional user details
                userDataManager.AddUserDetails(userId, emailAddress, firstName, lastName);

                // assign the supplied roles to the user
                userDataManager.AssignRolesToUser(userId, rolesToAssign);

                // add an invitation record
                userDataManager.AddUserInvitation(userId);

                uow.Complete();
            }

            // notify the user of their invitation
            userCommunicationService.SendUserEmail(emailAddress, "CapitecInvitation", new Dictionary<string, string>());
            return userId;
        }

        public void AcceptInvitation(Guid invitationToken, string password)
        {
            using (var unitOfWork = unitOfWorkFactory.Build())
            {
                // check the token exists and is still valid
                Guid userId = userDataManager.ValididateInvitationToken(invitationToken);

                // update the token to have been accepted
                userDataManager.AcceptInvitationToken(invitationToken);

                // set the users password
                string hashedPassword = passwordManager.HashPassword(password);

                userDataManager.SetUserPasswordAndActivate(userId, hashedPassword);

                unitOfWork.Complete();
            }
        }

        public void ActivateUser(Guid userId)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            userDataManager.ActivateUser(userId);
        }

        public void DeactivateUser(Guid userId)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            userDataManager.DeactivateUser(userId);
        }

        public void LockUserAccount(Guid userId)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            userDataManager.LockUserAccount(userId);
        }

        public void UnlockUserAccount(Guid userId)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            userDataManager.UnlockUserAccount(userId);
        }

        public void ChangePassword(Guid userId, string newPassword)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            string hashedPassword = passwordManager.HashPassword(newPassword);

            userDataManager.ChangeUserPassword(userId, hashedPassword);
        }

        public void AssignUserRole(Guid userId, Guid[] roleIds)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            // assign the roles to the user
            userDataManager.AssignRolesToUser(userId, roleIds);
        }

        public void RemoveUserRole(Guid userId, Guid[] roleIds)
        {
            // check the user exists
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            userDataManager.RemoveRolesFromUser(userId, roleIds);
        }

        public void UpdateUser(Guid userId, bool isActive)
        {
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }
            bool hasUserDataChanged = userDataManager.HasUserDataChanged(userId, isActive);
            if (hasUserDataChanged)
            {
                userDataManager.UpdateUser(userId, isActive);
            }
        }

        public void UpdateUserInformation(Guid userId, string emailAddress, string firstName, string lastName)
        {
            bool userExists = userDataManager.DoesUserIdExist(userId);
            if (!userExists)
            {
                throw new UserDoesNotExistException(userId);
            }

            bool hasUserInformationDataChanged = userDataManager.HasUserInformationDataChanged(userId, emailAddress, firstName, lastName);

            if (!hasUserInformationDataChanged)
            {
                return;
            }
            bool hasEmailChanged = userDataManager.HasUserEmailChanged(userId, emailAddress);

            userDataManager.UpdateUserInformation(userId, emailAddress, firstName, lastName);

            if (hasEmailChanged)
            {
                userCommunicationService.SendUserEmail(emailAddress, "CapitecInvitation",
                    new Dictionary<string, string>());
            }
        }
    }
}