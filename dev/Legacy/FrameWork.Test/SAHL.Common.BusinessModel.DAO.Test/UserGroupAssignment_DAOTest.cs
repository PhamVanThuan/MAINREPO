using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test.DAOHelpers;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="UserGroupAssignment_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class UserGroupAssignment_DAOTest : TestBase
    {

        /// <summary>
        /// Tests the retrieval of a <see cref="UserGroupAssignment_DAO"/> object.
        /// </summary>
        [Test]
        public void Find()
        {

            UserGroupAssignment_DAO D = base.TestFind<UserGroupAssignment_DAO>("UserGroupAssignment", "UserGroupAssignmentKey");

        }



        [Test]
        [Ignore]
        public void Save()
        {

            // create the object and save
            UserGroupAssignmentHelper helper = new UserGroupAssignmentHelper();
            UserGroupAssignment_DAO UserGroupAssignment = helper.CreateUserGroupAssignment();

            try
            {
                UserGroupAssignment.Save();
            }
            finally
            {
                helper.Dispose();
            }

        }


    }
}
