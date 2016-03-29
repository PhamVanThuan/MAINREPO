using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Text;
using SAHL.Test;
using Castle.ActiveRecord;
using NUnit.Framework;
using System.Reflection;
using SAHL.Common.Security;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Common.UI.Test
{
    [TestFixture]
    public class CBOManagerTest : TestBase
    {
        private CBOManager _cboManager;

        public CBOManager CBOManager
        {
            get
            {
                if (_cboManager == null)
                    _cboManager = new CBOManager();

                return _cboManager;
            }
        }

        [Test]
        public void GetMenuNodesCBO()
        {
            using (new SessionScope())
            {

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                List<CBONode> nodes = CBOManager.GetMenuNodes(SAHLPrincipal.GetCurrent(), CBONodeSetType.CBO);

                Assert.IsTrue(nodes.Count > 0);
            }
        }

        [Test]
        public void GetMenuNodesX2()
        {
            using (new SessionScope())
            {

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                List<CBONode> nodes = CBOManager.GetMenuNodes(SAHLPrincipal.GetCurrent(), CBONodeSetType.X2);

                Assert.IsTrue(nodes.Count > 0);
            }
        }
    }
}
