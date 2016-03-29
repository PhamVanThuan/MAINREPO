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
    /// Class for testing the <see cref="StatementDefinition_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class StatementDefinition_DAOTest : TestBase
    {
        #region Static helper methods

        public static StatementDefinition_DAO CreateStatementDefinition()
        {
            StatementDefinition_DAO StatementDefinition = new StatementDefinition_DAO();
            StatementDefinition.StatementName = "test statement name";
            StatementDefinition.ApplicationName = "Test Application Name";
            StatementDefinition.Description = "Test Description";
            return StatementDefinition;
        }

        #endregion
    }
}
