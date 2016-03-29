using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UserProfileSetting_DAO"/> domain entity.
    /// </summary>
    public class UserProfileSettingHelper : BaseHelper<UserProfileSetting_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UserProfileSetting_DAO"/> entity.
        /// </summary>
        /// <returns>A new UserProfileSetting_DAO entity (not yet persisted).</returns>
        public UserProfileSetting_DAO CreateUserProfileSetting()
        {
            UserProfileSetting_DAO UserProfileSetting = new UserProfileSetting_DAO();
            UserProfileSetting.ADUser = ADUser_DAO.FindFirst();
            UserProfileSetting.SettingName = "Test Setting name";
            UserProfileSetting.SettingType = "Test Setting Type";
            UserProfileSetting.SettingValue = "Test Setting Value";
            CreatedEntities.Add(UserProfileSetting);

            return UserProfileSetting;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UserProfileSetting_DAO UserProfileSetting in CreatedEntities)
            {
                if (UserProfileSetting.Key > 0)
                    TestBase.DeleteRecord("UserProfileSetting", "UserProfileSettingKey", UserProfileSetting.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

