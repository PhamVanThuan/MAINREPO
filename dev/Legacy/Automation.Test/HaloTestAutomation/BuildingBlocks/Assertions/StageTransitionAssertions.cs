using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class StageTransitionAssertions
    {
        private static readonly IStageTransitionService stageTransitionService;

        static StageTransitionAssertions()
        {
            stageTransitionService = ServiceLocator.Instance.GetService<IStageTransitionService>();
        }

        /// <summary>
        /// Assert that a StageTransition for the given stageDefinitionName was written for the debt counselling.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="SDSDGKey"></param>
        /// <param name="comment">If passed through to the assertion, the method will check that the comment matches the one provided.</param>
        public static void AssertStageTransitionCreated(int genericKey, StageDefinitionStageDefinitionGroupEnum SDSDGKey, string comment = "")
        {
            Logger.LogAction(string.Format(@"Asserting that SDSDGKey: '{0}' exists against GenericKey: {1}", (int)SDSDGKey, genericKey));
            WatiN.Core.UtilityClasses.SimpleTimer timer = new WatiN.Core.UtilityClasses.SimpleTimer(new TimeSpan(0, 0, 30));
            IList<Automation.DataModels.StageTransition> stageTransitions = null;
            do
            {
                stageTransitions = stageTransitionService.GetStageTransition(genericKey, SDSDGKey);
                if (stageTransitions.Count > 0)
                    break;
            } while (!timer.Elapsed);

            Assert.True(stageTransitions.Count > 0, String.Format("SDSDGKey:{0} was not written for GenericKey:{1}", (int)SDSDGKey, genericKey));
            if (!string.IsNullOrEmpty(comment))
            {
                var transition = (from st in stageTransitions select st).First();
                StringAssert.AreEqualIgnoringCase(comment, transition.Comments);
            }
        }
        public static void AssertStageTransitionEndDate(int genericKey, StageDefinitionStageDefinitionGroupEnum SDSDGKey, DateTime expectedEndDate)
        {
            var stageTransition = stageTransitionService.GetStageTransition(genericKey, SDSDGKey).FirstOrDefault();
            Assert.AreEqual(expectedEndDate.Date, stageTransition.EndTransitionDate.Date);
        }

        public static void AssertStageTransitionComment(int genericKey, StageDefinitionStageDefinitionGroupEnum SDSDGKey, string expectedComments)
        {
            var stageTransition = stageTransitionService.GetStageTransition(genericKey, SDSDGKey).FirstOrDefault();
            Assert.AreEqual(expectedComments, stageTransition.Comments);
        }
    }
}