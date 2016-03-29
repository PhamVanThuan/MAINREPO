using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UserGroupMapping_DAO"/> domain entity.
    /// </summary>
    public class UserGroupMappingHelper : BaseHelper<UserGroupMapping_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UserGroupMapping_DAO"/> entity.
        /// </summary>
        /// <returns>A new UserGroupMapping_DAO entity (not yet persisted).</returns>
        public UserGroupMapping_DAO CreateUserGroupMapping()
        {
            UserGroupMapping_DAO UserGroupMapping = new UserGroupMapping_DAO();
            UserGroupMapping.FunctionalGroupDefinition = FunctionalGroupDefinition_DAO.FindFirst();
            UserGroupMapping.OrganisationStructure = OrganisationStructure_DAO.FindFirst();
            CreatedEntities.Add(UserGroupMapping);
            return UserGroupMapping;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UserGroupMapping_DAO UserGroupMapping in CreatedEntities)
            {
                if (UserGroupMapping.Key > 0)
                    TestBase.DeleteRecord("UserGroupMapping", "UserGroupMappingKey", UserGroupMapping.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

