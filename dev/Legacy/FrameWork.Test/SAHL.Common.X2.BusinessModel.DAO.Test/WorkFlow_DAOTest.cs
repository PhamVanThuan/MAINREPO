using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.Security;
using SAHL.Common.Collections;

using Castle.ActiveRecord;
using NUnit.Framework;
using System.Security.Principal;
using SAHL.Common.X2.BusinessModel.DAO;

namespace SAHL.Common.X2.BusinessModel.Test
{
    [TestFixture]
    public class WorkFlow_DAOTest : TestBase
    {
        [Test]
        public void Find()
        {
            base.TestFind<WorkFlow_DAO>("X2.X2.WorkFlow", "ID");
        }

        [Test]
        [Ignore("On the build server this runs as SQLService2 whic is not configured with groups in AD")]
        public void FindByPrincipal()
        {
            var workflows = WorkFlow_DAO.FindByPrincipal(TestPrincipal);

            Assert.IsTrue(workflows.Count > 0, SAHL.Common.CacheData.SAHLPrincipalCache.GetPrincipalCache(TestPrincipal).GetCachedRolesAsStringForQuery(true, true));
        }
    }
}
