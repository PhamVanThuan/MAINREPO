using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UserGroupAssignment_DAO"/> domain entity.
    /// </summary>
    public class UserGroupAssignmentHelper : BaseHelper<UserGroupAssignment_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UserGroupAssignment_DAO"/> entity.
        /// </summary>
        /// <returns>A new UserGroupMapping_DAO entity (not yet persisted).</returns>
        public UserGroupAssignment_DAO CreateUserGroupAssignment()
        {
            UserGroupAssignment_DAO UserGroupAssignment = new UserGroupAssignment_DAO();
            UserGroupAssignment.ADUser = ADUser_DAO.FindFirst();
            UserGroupAssignment.ChangeDate = DateTime.Now;
            UserGroupAssignment.InsertedDate = DateTime.Now;
            UserGroupAssignment.UserGroupMapping = UserGroupMapping_DAO.FindFirst();
            CreatedEntities.Add(UserGroupAssignment);
            return UserGroupAssignment;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UserGroupAssignment_DAO UserGroupAssignment in CreatedEntities)
            {
                if (UserGroupAssignment.Key > 0)
                    TestBase.DeleteRecord("UserGroupAssignment", "UserGroupAssignmentKey", UserGroupAssignment.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

