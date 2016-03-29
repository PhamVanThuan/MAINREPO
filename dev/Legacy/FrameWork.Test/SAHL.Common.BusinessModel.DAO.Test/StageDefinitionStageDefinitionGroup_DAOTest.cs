using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class StageDefinitionStageDefinitionGroup_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the loading and saving of StageDefinitionStageDefinitionGroup_DAO objects.  This is not covered by 
        /// the generic LoadSaveLoad test as there is a unique key on the columns in the table which can randomly cause 
        /// the test to fail.
        /// </summary>
         // Saving SDSDG is not done in the app. SD & SDG are managed keys and do not have identity increment.
        [Test]
        public void LoadSaveLoad()
        {
            int sdsdgKey = 0;
            int stageDefinitionKey = 0;
            int stageDefinitionGroupKey = 0;

            // load one up
            using (new SessionScope())
            {
                StageDefinitionStageDefinitionGroup_DAO sdsdgLoad1 = StageDefinitionStageDefinitionGroup_DAO.FindFirst();
                Assert.IsNotNull(sdsdgLoad1);
            }

            // create items for the test and try and save a new object
            using (new SessionScope())
            {
                StageDefinition_DAO stageDefinition = DAODataConsistancyChecker.GetDAO<StageDefinition_DAO>();
                stageDefinition.SaveAndFlush();
                stageDefinitionKey = stageDefinition.Key;

                StageDefinitionGroup_DAO stageDefinitionGroup = DAODataConsistancyChecker.GetDAO<StageDefinitionGroup_DAO>();
                stageDefinitionGroup.SaveAndFlush();
                stageDefinitionGroupKey = stageDefinitionGroup.Key;

                // create a new one and save it
                StageDefinitionStageDefinitionGroup_DAO sdsdg = new StageDefinitionStageDefinitionGroup_DAO();
                sdsdg.StageDefinition = stageDefinition;
                sdsdg.StageDefinitionGroup = stageDefinitionGroup;
                sdsdg.SaveAndFlush();
                sdsdgKey = sdsdg.Key;
            }

            using (new SessionScope())
            {

                // now try and load it in a new session
                StageDefinitionStageDefinitionGroup_DAO sdsdgLoad2 = StageDefinitionStageDefinitionGroup_DAO.Find(sdsdgKey);
                Assert.IsNotNull(sdsdgLoad2);

            }

            // delete all the data
            using (new SessionScope())
            {
                StageDefinitionStageDefinitionGroup_DAO SDSDG = StageDefinitionStageDefinitionGroup_DAO.Find(sdsdgKey);
                SDSDG.DeleteAndFlush();
                //StageDefinitionStageDefinitionGroup_DAO.DeleteAll(new int[] { sdsdgKey });
                StageDefinitionGroup_DAO SDG = StageDefinitionGroup_DAO.Find(stageDefinitionGroupKey);
                SDG.DeleteAndFlush();
                //StageDefinitionGroup_DAO.DeleteAll(new int[] { stageDefinitionGroupKey });
                StageDefinition_DAO SD = StageDefinition_DAO.Find(stageDefinitionKey);
                SD.DeleteAndFlush();
                //StageDefinition_DAO.DeleteAll(new int[] { stageDefinitionKey });
            }



        }
    }
}
