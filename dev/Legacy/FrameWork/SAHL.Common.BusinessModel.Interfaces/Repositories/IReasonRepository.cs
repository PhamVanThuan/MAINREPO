using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IReasonRepository
    {
        IReason GetReasonByKey(int ReasonKey);

        IReadOnlyEventList<IReason> GetReasonByGenericTypeAndKey(int GenericKeyTypeKey, int GenericKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="ReasonGroupTypeKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GetReasonByGenericKeyAndReasonGroupTypeKey(int GenericKey, int ReasonGroupTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKeyList"></param>
        /// <param name="ReasonTypeGroupKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GetReasonByGenericKeyListAndReasonTypeGroupKey(List<int> GenericKeyList, int ReasonTypeGroupKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKeyList"></param>
        /// <param name="ReasonTypeKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GetReasonByGenericKeyListAndReasonTypeKey(List<int> GenericKeyList, int ReasonTypeKey);

        IReadOnlyEventList<IReasonType> GetReasonTypeByReasonTypeGroup(int[] ReasonTypeGroupKeys);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IReason CreateEmptyReason();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReasonDescription CreateEmptyReasonDescription();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReasonDefinition CreateEmptyReasonDefinition();

        /// <summary>
        ///
        /// </summary>
        /// <param name="reason"></param>
        void SaveReason(IReason reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasondescription"></param>
        void SaveReasonDescription(IReasonDescription reasondescription);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasondefinition"></param>
        void SaveReasonDefinition(IReasonDefinition reasondefinition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="reasonType"></param>
        /// <returns></returns>
        IReasonDefinition AddNewReasonDefinition(string description, ReasonTypes reasonType);

        /// <summary>
        /// Gets an IReasonDefinition for a given ReasonDefinitionKey
        /// </summary>
        /// <param name="reasonDefinitionKey"></param>
        /// <returns>IReasonDefinition</returns>
        IReasonDefinition GetReasonDefinitionByKey(int reasonDefinitionKey);

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonType
        /// </summary>
        /// <param name="reasonType">SAHL.Common.Globals.ReasonTypes</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonType(Globals.ReasonTypes reasonType);

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonTypeKey
        /// </summary>
        /// <param name="reasonTypeKey"></param>
        /// <param name="sortByDescription">Order the reasons by the ReasonDescription ASC</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonTypeKey(int reasonTypeKey, bool sortByDescription);

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonTypeKey
        /// </summary>
        /// <param name="reasonTypeKey"></param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonTypeKey(int reasonTypeKey);

        /// <summary>
        /// Get the next Managed Reason Description key so we can insert a new one
        /// </summary>
        /// <returns></returns>
        int GetNextReasonDescriptionKey();

        /// <summary>
        /// Gets a reason type given a reasontypekey
        /// </summary>
        /// <param name="ReasonTypeKey"></param>
        /// <returns></returns>
        IReasonType GetReasonTypeByKey(int ReasonTypeKey);

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonType and ReasonDescription
        /// </summary>
        /// <param name="reasonType">SAHL.Common.Globals.ReasonTypes</param>
        /// <param name="reasonDescription">string</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonDescription(Globals.ReasonTypes reasonType, string reasonDescription);

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonType and ReasonDescription
        /// </summary>s
        /// <param name="reasonType">SAHL.Common.Globals.ReasonTypes</param>
        /// <param name="reasonDescriptionKey">int</param>
        /// <returns>IReasonDefinition</returns>
        IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes reasonType, int reasonDescriptionKey);

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonTypeGroup
        /// </summary>
        /// <param name="reasonTypeGroup">SAHL.Common.Globals.ReasonTypeGroups</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonTypeGroup(Globals.ReasonTypeGroups reasonTypeGroup);

        /// <summary>
        /// Get a complete list of reasons for display
        /// </summary>
        /// <param name="GenericKeys"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GeReasonsByGenericKeys(int[] GenericKeys);

        /// <summary>
        /// Get Reasons by Reason Type and generic Key type and Generic Key
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="ReasonTypeKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GetReasonByGenericKeyAndReasonTypeKey(int GenericKey, int ReasonTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="reasonTypeKey"></param>
        /// <returns></returns>
        IReason GetLatestReasonByGenericKeyAndReasonTypeKey(int genericKey, int reasonTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <returns></returns>
        int GetLatestReasonDescriptionKeyForGenericKey(int genericKey, int genericKeyTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="reason"></param>
        void DeleteReason(IReason reason);

        /// <summary>
        /// Get a complete list of reasons per GenericKeyType and GenericKeys for display
        /// </summary>
        /// <param name="dtGenericKeysAndTypes"></param>
        /// <returns></returns>
        IEventList<IReason> GetReasonsByGenericKeyTypeAndKeys(DataTable dtGenericKeysAndTypes);

        /// <summary>
        /// Get Reasons by Stage Transition Keys
        /// </summary>
        /// <param name="stageTransitionKeys"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GetReasonsByStageTransitionKeys(int[] stageTransitionKeys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="reasonDefinition"></param>
        /// <returns></returns>
        IReadOnlyEventList<IReason> GetReasonByGenericKeyAndReasonDefinitionKey(int genericKey, ReasonDefinitions reasonDefinition);
    }
}