using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using System.Reflection;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the CBOMenu_DAO entity.
    /// </summary>
    [TestFixture]
    public class CBOMenu_DAOTest : TestBase
    {
        [Test]
        public void FindAllCBOMenuNodes()
        {
            CBOMenu_DAO[] cboMenus = CBOMenu_DAO.FindAll();

            Assert.Greater(cboMenus.Length, 0, "No CBO Menu Nodes Found.");
        }
    }
}
