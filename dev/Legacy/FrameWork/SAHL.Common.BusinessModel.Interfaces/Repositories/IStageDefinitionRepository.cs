using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IStageDefinitionRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <returns></returns>
        IList<IStageTransition> GetStageTransitionList(int genericKey, int genericKeyTypeKey, List<int> stageDefinitionstageDefinitionGroupKey);

        /// <summary>
        /// Returns all the recorded stage transtions for the specified generic key
        /// </summary>
        /// <param name="genericKeyValue"></param>
        /// <returns></returns>
        IList<IStageTransition> GetStageTransitionsByGenericKey(int genericKeyValue);

        /// <summary>
        /// Returns all the recorded stage transtions for the specified generic keys
        /// </summary>
        /// <param name="genericKeyValues"></param>
        /// <returns>IList&lt;IStageTransition&gt;</returns>
        IList<IStageTransition> GetStageTransitionsByGenericKeys(IList<int> genericKeyValues);

        /// <summary>
        /// Returns the stage definition group of the given key value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IStageDefinitionGroup GetStageDefinitionGroupByKey(int key);

        /// <summary>
        /// Returns the IStageDefinition for the given StageDefinition Description
        /// </summary>
        /// <param name="stageDefinitionDescription"></param>
        /// <returns></returns>
        IStageDefinition GetStageDefinitionByDescription(string stageDefinitionDescription);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stageDefinitionGroupKey"></param>
        /// <param name="stageDefinitionKey"></param>
        /// <returns></returns>
        IStageDefinitionStageDefinitionGroup GetStageDefinitionStageDefinitionGroup(int stageDefinitionGroupKey, int stageDefinitionKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stageDefinitionGroupKey"></param>
        /// <param name="stageDefinitionKey"></param>
        /// <returns></returns>
        int GetStageDefinitionStageDefinitionGroupKey(int stageDefinitionGroupKey, int stageDefinitionKey);

        /// <summary>
        /// Creates an empty StageTransition object
        /// </summary>
        /// <returns>IStageTransition</returns>
        IStageTransition CreateEmptyStageTransition();

        /// <summary>
        /// Saves a StageTransition record
        /// </summary>
        /// <param name="stageTransition"></param>
        void SaveStageTransition(IStageTransition stageTransition);

        /// <summary>
        /// Saves a StageTransition record
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="StageDefinitionGroupKey"></param>
        /// <param name="StageDefinitionDescription"></param>
        /// <param name="Comments"></param>
        /// <param name="ADUser"></param>
        IStageTransition SaveStageTransition(int GenericKey, int StageDefinitionGroupKey, string StageDefinitionDescription, string Comments, IADUser ADUser);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stageTransitionKey"></param>
        /// <returns></returns>
        IStageTransition GetStageTransitionByKey(int stageTransitionKey);

        /// <summary>
        /// Checks if an object is in a particular stage.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <returns></returns>
        bool CheckCompositeStageDefinition(int genericKey, int stageDefinitionstageDefinitionGroupKey);

        /// <summary>
        /// Checks if an object has passed a particular stage.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <returns></returns>
        int CountCompositeStageOccurance(int genericKey, int stageDefinitionstageDefinitionGroupKey);

        /// <summary>
        /// In the event we actually want to count all Composites without grouping as these might not be duplicated
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <param name="doGroup"></param>
        /// <returns></returns>
        int CountCompositeStageOccurance(int genericKey, int stageDefinitionstageDefinitionGroupKey, bool doGroup);

        /// <summary>
        /// Get the last Composite occurrence of a list of StageDefinitionStageDefinitionGroups
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionStageDefinitionGroups"></param>
        /// <returns></returns>
        IStageTransitionComposite GetLastStageTransitionComposite(int genericKey, IList<int> stageDefinitionStageDefinitionGroups);

        /// <summary>
        /// Get the Stage Transition prior to the Transition passed in with the matching
        /// StageDefinitionStageDefinitionGroupKey passed in.
        /// This is a HACK because x2 is writing the Transitions and Composites and we
        /// need to get the ADUser that made the decision.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionStageDefinitionGroupKey"></param>
        /// <param name="stageTransitionKey"></param>
        /// <returns></returns>
        IStageTransition GetPreviousTransition(int genericKey, int stageDefinitionStageDefinitionGroupKey, int stageTransitionKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dicGenricKeyTypeAndKeys"></param>
        /// <returns></returns>
        DataTable GetStageTransitionDTByGenericKeyTypeAndKeys(IDictionary<GenericKeyTypes, List<int>> dicGenricKeyTypeAndKeys);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="StageDefinitionStageDefinitionGroup"></param>
        /// <param name="Comments"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        IStageTransition SaveStageTransition(int GenericKey, StageDefinitionStageDefinitionGroups StageDefinitionStageDefinitionGroup, string Comments, string ADUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="StageDefinitionStageDefinitionGroup"></param>
        /// <param name="transitionDate"></param>
        /// <param name="Comments"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        IStageTransition SaveStageTransition(int GenericKey, StageDefinitionStageDefinitionGroups StageDefinitionStageDefinitionGroup, DateTime transitionDate, string Comments, string ADUserName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionGroupKey"></param>
        /// <returns></returns>
        IEnumerable<IStageTransition> GetStageTransitionsByGenericKey(int genericKey, int stageDefinitionGroupKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="sdsdgKey"></param>
        /// <returns></returns>
        IStageTransition GetLastStageTransitionByGenericKeyAndSDSDGKey(int genericKey, int genericKeyTypeKey, int sdsdgKey);
    }
}