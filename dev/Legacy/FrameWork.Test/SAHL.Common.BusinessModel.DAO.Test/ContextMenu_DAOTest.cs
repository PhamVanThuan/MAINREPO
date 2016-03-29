using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="ContextMenu_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class ContextMenu_DAOTest : TestBase
    {
        [Test]
        public void FindAllContextMenuNodes()
        {
            ContextMenu_DAO[] cboMenus = ContextMenu_DAO.FindAll();

            Assert.Greater(cboMenus.Length, 0, "No Context Menu Nodes Found.");
        }
    }
}
