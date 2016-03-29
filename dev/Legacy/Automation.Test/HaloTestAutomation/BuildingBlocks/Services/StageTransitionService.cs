using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class StageTransitionService : _2AMDataHelper, IStageTransitionService
    {
        /// <summary>
        /// Checks if a stage transition record exists against a generic key
        /// </summary>
        /// <param name="genericKey">genericKey</param>
        /// <param name="sdsdgkey">StageDefinitionStageDefinitionGroupKey</param>
        /// <returns></returns>
        public bool CheckIfTransitionExists(int genericKey, int sdsdgkey)
        {
            var results = base.GetStageTransitionsByGenericKey(genericKey);
            var exists = (from r in results
                          where r.SDSDGKey == (int)sdsdgkey
                          select r).Any();
            return exists;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="sdsdgKey"></param>
        /// <returns></returns>
        public Automation.DataModels.StageTransition GetLatestStageTransitionByGenericKeyAndSDSDGKey(int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdgKey)
        {
            var results = base.GetStageTransitionsByGenericKey(genericKey);
            var row = (from r in results where r.SDSDGKey == (int)sdsdgKey select r).First();
            return row;
        }

        /// <summary>
        /// Check if a stage transition exist given the parameters.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="sdsdg"></param>
        /// <returns></returns>
        public List<Automation.DataModels.StageTransition> GetStageTransition(int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdg)
        {
            var stageTransitions = base.GetStageTransitionsByGenericKey(genericKey);
            return (from st in stageTransitions where st.SDSDGKey == (int)sdsdg select st).OrderByDescending(x => x.StageTransitionKey).ToList();
        }
    }
}