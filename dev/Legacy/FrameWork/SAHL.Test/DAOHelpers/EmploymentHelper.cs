using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="Employment_DAO"/> domain entity.
    /// </summary>
    public class EmploymentHelper : BaseHelper<Employment_DAO>
    {

        /// <summary>
        /// Creates a new <see cref="Employment_DAO"/> entity.
        /// </summary>
        /// <returns>A new Employment_DAO entity (not yet persisted).</returns>
        public Employment_DAO CreateEmployment(Employer_DAO employer, LegalEntity_DAO legalEntity)
        {
            Employment_DAO employment = new Employment_DAO();
            employment.ChangeDate = DateTime.Now;
            employment.Employer = employer;
            // Commented by SS (24/12) - causing Framework Compile to Fail - this would
            // have to be re-written since employment is now being disciminated by Type
            //employment.EmploymentType = EmploymentType_DAO.FindFirst();
            employment.RemunerationType = RemunerationType_DAO.FindFirst();
            employment.EmploymentStatus = EmploymentStatus_DAO.FindFirst();
            employment.UserID = "Test ID";
            
            employment.LegalEntity = legalEntity;
            

            CreatedEntities.Add(employment);

            return employment;
        }

        /// <summary>
        /// Ensures that all employment records created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (Employment_DAO employment in CreatedEntities)
            {
                if (employment.Key > 0)
                    TestBase.DeleteRecord("Employment", "EmploymentKey", employment.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
