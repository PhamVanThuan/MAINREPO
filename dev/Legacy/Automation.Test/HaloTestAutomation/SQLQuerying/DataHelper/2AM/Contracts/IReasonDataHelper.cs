using Common.Enums;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper._2AM.Contracts
{
    public interface IReasonDataHelper : IDataHelper
    {
        QueryResults GetActiveReasonsByReasonType(string ReasonType);

        QueryResults GetReasonsByGenericKeyAndGenericKeyType(int genericKey, GenericKeyTypeEnum genericKeyType);

        void RemoveReasonsAgainstGenericKeyByReasonType(int genericKey, GenericKeyTypeEnum genericKeyType, ReasonTypeEnum reasonTypeKey);

        void InsertReason(int genericKey, string reasonDescription, ReasonTypeEnum reasonType, GenericKeyTypeEnum genericKeyType);

        QueryResults GetNotificationReasonsForLegalEntity(int reasonDefinition, int accountKey);

        IEnumerable<Automation.DataModels.ReasonDefinition> GetReasonDefinition(string reasonDescription, ReasonTypeEnum reasonType);

        void SetStageTransitionOnLatestReason(int genericKeyStageTransition, int genericKeyReason, StageDefinitionStageDefinitionGroupEnum sdsdgKey);
    }
}