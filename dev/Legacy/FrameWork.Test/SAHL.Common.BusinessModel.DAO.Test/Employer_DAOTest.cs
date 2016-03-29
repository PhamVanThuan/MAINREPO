using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="Employer_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class Employer_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the static FindByPrefix method.
        /// </summary>
        [Test]
        public void FindByPrefix()
        {
            string prefix = "a";
            IList<Employer_DAO> results = Employer_DAO.FindByPrefix(prefix, 10);
            if (results.Count == 0)
                Assert.Ignore("Unable to find any employers to test Employer_DAOTest.FindByPrefix.");
            else
            {
                foreach (Employer_DAO e in results)
                {
                    Assert.IsTrue(e.Name.ToLower().StartsWith(prefix));
                }
            }

            Assert.LessOrEqual(results.Count, 10);

        }

    }
}
