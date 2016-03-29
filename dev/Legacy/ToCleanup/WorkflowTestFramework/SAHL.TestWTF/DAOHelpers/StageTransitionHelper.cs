using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="StageTransition_DAO"/> domain entity.
    /// </summary>
    public class StageTransitionHelper : BaseHelper<StageTransition_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="StageTransition_DAO"/> entity.
        /// </summary>
        /// <returns>A new StageTransition_DAO entity (not yet persisted).</returns>
        public StageTransition_DAO CreateStageTransition()
        {
            StageTransition_DAO stageTransition = new StageTransition_DAO();
            stageTransition.ADUser = ADUser_DAO.FindFirst();
            stageTransition.Comments = "Test Comments";
            stageTransition.GenericKey = 9876789;
            stageTransition.StageDefinitionStageDefinitionGroup = StageDefinitionStageDefinitionGroup_DAO.FindFirst();
            stageTransition.TransitionDate = System.DateTime.Now;

            CreatedEntities.Add(stageTransition);

            return stageTransition;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (StageTransition_DAO stageTransition in CreatedEntities)
            {
                if (stageTransition.Key > 0)
                    TestBase.DeleteRecord("StageTransition", "StageTransitionKey", stageTransition.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

