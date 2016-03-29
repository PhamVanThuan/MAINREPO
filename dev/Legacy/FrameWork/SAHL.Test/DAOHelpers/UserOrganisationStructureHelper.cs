using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UserOrganisationStructure_DAO"/> domain entity.
    /// </summary>
    public class UserOrganisationStructureHelper : BaseHelper<UserOrganisationStructure_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UserOrganisationStructure_DAO"/> entity.
        /// </summary>
        /// <returns>A new UserOrganisationStructure_DAO entity (not yet persisted).</returns>
        public UserOrganisationStructure_DAO CreateUserOrganisationStructure()
        {
            UserOrganisationStructure_DAO UserOrganisationStructure = new UserOrganisationStructure_DAO();
            UserOrganisationStructure.OrganisationStructure = OrganisationStructure_DAO.FindFirst();
            UserOrganisationStructure.ADUser = ADUser_DAO.FindFirst();
            CreatedEntities.Add(UserOrganisationStructure);
            return UserOrganisationStructure;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UserOrganisationStructure_DAO UserOrganisationStructure in CreatedEntities)
            {
                if (UserOrganisationStructure.Key > 0)
                    TestBase.DeleteRecord("UserOrganisationStructure", "UserOrganisationStructureKey", UserOrganisationStructure.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

