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
    /// Class for testing the <see cref="Employment_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class Employment_DAOTest : TestBase
    {
        [Test]
		[Ignore("bad test, run manually only")]
        public void SaveAndOnSave()
        {
            using (new SessionScope())
            {
                EmploymentSubsidised_DAO e = EmploymentSubsidised_DAO.Find(315855);
                e.UserID = "test4";
                Subsidy_DAO s = e.Subsidies[0];
                s.Paypoint = "test4";

                e.Save();

            }
        }
    }
}
