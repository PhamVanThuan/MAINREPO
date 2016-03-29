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
    /// Class for testing the <see cref="StatementParameter_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class StatementParameter_DAOTest : TestBase
    {
        #region Static helper methods

        public static StatementParameter_DAO CreateStatementParameter()
        {
            StatementParameter_DAO StatementParameter = new StatementParameter_DAO();
            StatementParameter.StatementDefinition = StatementDefinition_DAO.FindFirst();
            StatementParameter.DisplayName = "Test Displayname";
            StatementParameter.ParameterLength = 1;
            StatementParameter.ParameterName = "Test Param Name";
            StatementParameter.ParameterType = ParameterType_DAO.FindFirst();
            StatementParameter.PopulationStatementDefinition = StatementDefinition_DAO.FindFirst();
            StatementParameter.Required = true;
            StatementParameter.StatementDefinition = StatementDefinition_DAO.FindFirst();            
            return StatementParameter;
        }

        #endregion
    }
}
