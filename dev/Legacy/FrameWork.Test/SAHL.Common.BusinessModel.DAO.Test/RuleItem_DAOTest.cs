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
    /// Class for testing the <see cref="RuleItem_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class RuleItem_DAOTest : TestBase
    {

        #region Static helper methods

        public static RuleItem_DAO CreateRuleItem()
        {
            RuleItem_DAO RuleItem = new RuleItem_DAO();
            RuleItem.Name = "Test Name";
            RuleItem.AssemblyName = "Test Assembly";
            // RuleItem..BusinessRuleArea = BusinessRuleArea_DAO.FindFirst();
            return RuleItem;
        }

        #endregion

    }
}
