using NUnit.Framework;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="FeatureGroup_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class FeatureGroup_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the <see cref="FeatureGroup_DAO.FindAllGroups"/> method.
        /// </summary>
        [Test]
        public void FindAllGroups()
        {
            IPrincipal principal = TestPrincipal;
            String[] arrGroups = FeatureGroup_DAO.FindAllGroups();

            // there needs to be at least one group to perform this test
            Assert.Greater(arrGroups.Length, 0, "No groups found in the FeatureGroup table - unable to complete test.");

            // make sure the list is distinct
            List<string> lstGroups = new List<string>();
            foreach (string group in arrGroups)
            {
                if (lstGroups.Contains(group))
                    Assert.Fail("The group " + group + " featured more than once in FindAllGroups - this should be a distinct list.");

                lstGroups.Add(group);
            }
        }
    }
}