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
    /// Class for testing the <see cref="RuleParameter_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class RuleParameter_DAOTest : TestBase
    {

        #region Static helper methods

        public static RuleParameter_DAO CreateRuleParameter()
        {
            RuleParameter_DAO RuleParameter = new RuleParameter_DAO();
            RuleParameter.Name = "Test Name";
            RuleParameter.RuleItem = RuleItem_DAO.FindFirst();
            RuleParameter.RuleParameterType = ParameterType_DAO.FindFirst();
            return RuleParameter;
        }

        #endregion

    }
}
