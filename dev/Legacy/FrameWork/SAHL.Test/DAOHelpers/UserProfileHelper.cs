using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UserProfile_DAO"/> domain entity.
    /// </summary>
    public class UserProfileHelper : BaseHelper<UserProfile_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UserProfile_DAO"/> entity.
        /// </summary>
        /// <returns>A new UserProfile_DAO entity (not yet persisted).</returns>
        public UserProfile_DAO CreateUserProfile()
        {
            UserProfile_DAO UserProfile = new UserProfile_DAO();
            UserProfile.ProfileType = ProfileType_DAO.FindFirst();
            UserProfile.ADUserName = "Test Name";
            UserProfile.Value = "Test Value";
            CreatedEntities.Add(UserProfile);
            return UserProfile;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UserProfile_DAO UserProfile in CreatedEntities)
            {
                if (UserProfile.Key > 0)
                    TestBase.DeleteRecord("UserProfile", "UserProfileKey", UserProfile.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

