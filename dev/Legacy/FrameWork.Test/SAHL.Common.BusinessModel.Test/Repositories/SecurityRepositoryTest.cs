using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class SecurityRepositoryTest : TestBase
    {
        [Test]
        public void GetADUserByPrincipal()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from ADUser";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                IADUser aduser = secRepo.GetADUserByPrincipal(DT.Rows[0]["ADUserName"].ToString());

                Assert.That(aduser.Key.ToString() == DT.Rows[0]["ADUserKey"].ToString());
            }
        }
    }
}
