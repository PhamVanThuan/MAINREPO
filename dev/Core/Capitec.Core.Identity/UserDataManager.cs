using System.Linq;
using Capitec.Core.Identity.Exceptions;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Identity;
using SAHL.Core.Strings;
using System;
using System.Collections.Generic;

namespace Capitec.Core.Identity
{
    public class UserDataManager : IUserDataManager
    {
        private readonly IDbFactory dbFactory;

        public UserDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesUsernameExist(string username)
        {
            bool result = false;

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                DoesUsernameExistQuery query = new DoesUsernameExistQuery(username);
                var user = db.SelectOne(query);
                if (user != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool DoesUserIdExist(Guid userId)
        {
            bool result = false;

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                DoesUserIdExistQuery query = new DoesUserIdExistQuery(userId);
                var user = db.SelectOne(query);
                if (user != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public Guid CreateInvitedUser(string username)
        {
            UserDataModel newUser = new UserDataModel(CombGuid.Instance.Generate(), username, "", CombGuid.Instance.Generate(), false, false);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(newUser);
                db.Complete();
                return newUser.Id;
            }
        }

        public void AddUserDetails(Guid userId, string emailAddress, string firstName, string lastName)
        {
            var userInformation = new UserInformationDataModel(CombGuid.Instance.Generate(), userId, emailAddress.ToLower(), firstName.ConvertToTitleCase(), lastName.ConvertToTitleCase());
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(userInformation);
                db.Complete();
            }
        }

        public InvitationDataModel AddUserInvitation(Guid userId)
        {
            InvitationDataModel userInvitation = new InvitationDataModel(CombGuid.Instance.Generate(), userId, CombGuid.Instance.Generate(), DateTime.Now, null);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(userInvitation);
                db.Complete();
            }

            return userInvitation;
        }

        public void AssignRolesToUser(Guid userId, Guid[] roleIdsToAssign)
        {
            List<UserRoleDataModel> roles = roleIdsToAssign
                .Select(roleId => new UserRoleDataModel(CombGuid.Instance.Generate(), userId, roleId))
                .ToList();

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<UserRoleDataModel>(roles);
                db.Complete();
            }
        }

        public void RemoveRolesFromUser(Guid userId, Guid[] roleIdsToRemove)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                foreach (var roleId in roleIdsToRemove)
                {
                    RemoveRoleFromUserQuery removeRoleFromUserQuery = new RemoveRoleFromUserQuery(userId, roleId);
                    db.Delete(removeRoleFromUserQuery);
                }
                db.Complete();
            }
        }

        public void ActivateUser(Guid userId)
        {
            ActivateUserQuery activateUserQuery = new ActivateUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(activateUserQuery);
                db.Complete();
            }
        }

        public void DeactivateUser(Guid userId)
        {
            DeactivateUserQuery deactivateUserQuery = new DeactivateUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(deactivateUserQuery);
                db.Complete();
            }
        }

        public void LockUserAccount(Guid userId)
        {
            LockUserAccountQuery lockUserAccountQuery = new LockUserAccountQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(lockUserAccountQuery);
                db.Complete();
            }
        }

        public void UnlockUserAccount(Guid userId)
        {
            UnlockUserAccountQuery unlockUserAccountQuery = new UnlockUserAccountQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(unlockUserAccountQuery);
                db.Complete();
            }
        }

        public void ChangeUserPassword(Guid userId, string hashedPassword)
        {
            ChangeUserPasswordQuery changeUserPasswordQuery = new ChangeUserPasswordQuery(userId, hashedPassword);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(changeUserPasswordQuery);
                db.Complete();
            }
        }

        public Guid ValididateInvitationToken(Guid invitationToken)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                DoesInvitationTokenExistQuery doesInvitationTokenExistQuery = new DoesInvitationTokenExistQuery(invitationToken);
                var token = db.SelectOne(doesInvitationTokenExistQuery);
                if (token == null)
                {
                    throw new InvitationDoesNotExistException(invitationToken);
                }
                if (token.AcceptedDate.HasValue)
                {
                    throw new InvitationHasAlreadyBeenAcceptedException(token.AcceptedDate.Value);
                }

                return token.UserId;
            }
        }

        public void AcceptInvitationToken(Guid invitationToken)
        {
            AcceptInvitationTokenQuery acceptInvitationTokenQuery = new AcceptInvitationTokenQuery(invitationToken);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<InvitationDataModel>(acceptInvitationTokenQuery);
                db.Complete();
            }
        }

        public UserDataModel GetUserFromUsername(string username)
        {
            GetUserFromUsernameQuery getUserFromUsernameQuery = new GetUserFromUsernameQuery(username);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(getUserFromUsernameQuery);
            }
        }

        public UserInformationDataModel GetUserInformationFromUser(Guid userId)
        {
            GetUserInformationFromUserQuery getUserInformationFromUserQuery = new GetUserInformationFromUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(getUserInformationFromUserQuery);
            }
        }

        public IEnumerable<RoleDataModel> GetRolesFromUser(Guid userId)
        {
            GetRolesFromUserQuery getRolesFromUserQuery = new GetRolesFromUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(getRolesFromUserQuery);
            }
        }

        public void UpdateUserToken(Guid userId, Guid token, string ipAddress)
        {
            GetTokenForUserQuery getTokenForUserQuery = new GetTokenForUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var userToken = db.SelectOne(getTokenForUserQuery);
                if (userToken == null)
                {
                    // insert
                    InsertTokenForUserQuery insertTokenForUserQuery = new InsertTokenForUserQuery(userId, token, ipAddress);
                    db.Insert<TokenDataModel>(insertTokenForUserQuery);
                }
                else
                {
                    //  update
                    UpdateTokenForUserQuery updateTokenForUserQuery = new UpdateTokenForUserQuery(userId, token, ipAddress);
                    db.Update<TokenDataModel>(updateTokenForUserQuery);
                }
                db.Complete();
            }
        }

        public void UpdateUserLoginAndActivity(Guid userId)
        {
            GetActivityForUserQuery getActivityForUserQuery = new GetActivityForUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var userActivity = db.SelectOne(getActivityForUserQuery);
                if (userActivity == null)
                {
                    // insert
                    InsertLoginAndActivityForUserQuery insertLoginAndActivityForUserQuery = new InsertLoginAndActivityForUserQuery(userId);
                    db.Insert<ActivityDataModel>(insertLoginAndActivityForUserQuery);
                }
                else
                {
                    //  update
                    UpdateLoginAndActivityForUserQuery updateLoginAndActivityForUserQuery = new UpdateLoginAndActivityForUserQuery(userId);
                    db.Update<ActivityDataModel>(updateLoginAndActivityForUserQuery);
                }
                db.Complete();
            }
        }

        public void UpdateUserActivity(Guid userId)
        {
            GetActivityForUserQuery getActivityForUserQuery = new GetActivityForUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var userActivity = db.SelectOne(getActivityForUserQuery);
                if (userActivity == null)
                {
                    // insert
                    InsertLoginAndActivityForUserQuery insertLoginAndActivityForUserQuery = new InsertLoginAndActivityForUserQuery(userId);
                    db.Insert<ActivityDataModel>(insertLoginAndActivityForUserQuery);
                }
                else
                {
                    //  update
                    UpdateActivityForUserQuery updateActivityForUserQuery = new UpdateActivityForUserQuery(userId);
                    db.Update<ActivityDataModel>(updateActivityForUserQuery);
                }
                db.Complete();
            }
        }

        public void RemoveUserToken(Guid userId)
        {
            RemoveTokenForUserQuery removeTokenForUserQuery = new RemoveTokenForUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Delete(removeTokenForUserQuery);
                db.Complete();
            }
        }

        public UserDataModel GetUserFromToken(Guid authToken)
        {
            GetUserFromTokenQuery getUserFromTokenQuery = new GetUserFromTokenQuery(authToken);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(getUserFromTokenQuery);
            }
        }

        public void SetUserPasswordAndActivate(Guid userId, string hashedPassword)
        {
            SetUserPasswordAndActivateQuery setUserPasswordAndActivateQuery = new SetUserPasswordAndActivateQuery(userId, hashedPassword);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(setUserPasswordAndActivateQuery);
                db.Complete();
            }
        }

        public bool HasUserEmailChanged(Guid userId, string emailAddress)
        {
            bool result = true;
            HasUserEmailChangedQuery query = new HasUserEmailChangedQuery(userId, emailAddress);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var user = db.SelectOne(query);
                if (user != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool HasUserDataChanged(Guid userId, bool isActive)
        {
            bool result = true;
            HasUserDataChangedQuery query = new HasUserDataChangedQuery(userId, isActive);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var user = db.SelectOne(query);
                if (user != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool HasUserInformationDataChanged(Guid userId, string emailAddress, string firstName, string lastName)
        {
            bool result = true;
            HasUserInformationDataChangedQuery query = new HasUserInformationDataChangedQuery(userId, firstName, lastName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var user = db.SelectOne(query);
                if (user != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public void UpdateUser(Guid userId, bool isActive)
        {
            UpdateUserQuery updateUserQuery = new UpdateUserQuery(userId, isActive);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserDataModel>(updateUserQuery);
                db.Complete();
            }
        }

        public void UpdateUserInformation(Guid userId, string emailAddress, string firstName, string lastName)
        {
            UpdateUserInformationQuery updateUserInformationQuery = new UpdateUserInformationQuery(userId, emailAddress, firstName, lastName);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserInformationDataModel>(updateUserInformationQuery);
                db.Complete();
            }
        }
    }
}