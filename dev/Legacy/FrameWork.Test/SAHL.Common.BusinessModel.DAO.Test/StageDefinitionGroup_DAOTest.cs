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
    /// Class for testing the <see cref="StageDefinitionGroup_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class StageDefinitionGroup_DAOTest : TestBase
    {

        #region Static helper methods

        public static StageDefinitionGroup_DAO CreateStageDefinitionGroup()
        {
            StageDefinitionGroup_DAO StageDefinitionGroup = new StageDefinitionGroup_DAO();            
            StageDefinitionGroup.Description = "Test Description";
            StageDefinitionGroup.GenericKeyType = GenericKeyType_DAO.FindFirst();
            StageDefinitionGroup.GeneralStatus = GeneralStatus_DAO.FindFirst();

            return StageDefinitionGroup;
        }

        #endregion

    }
}
