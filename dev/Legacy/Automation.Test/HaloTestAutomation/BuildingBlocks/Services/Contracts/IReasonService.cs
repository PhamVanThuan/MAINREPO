using Automation.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IReasonService
    {
        void RemoveReasonsAgainstGenericKeyByReasonType(int genericKey, GenericKeyTypeEnum genericKeyType, ReasonTypeEnum reasonTypeKey);

        void InsertReason(int genericKey, string reasonDescription, ReasonTypeEnum reasonType, GenericKeyTypeEnum genericKeyType);

        bool ReasonExistsAgainstGenericKey(string reasonDescription, string reasonTypeDescription, int genericKey, GenericKeyTypeEnum genKey);

        Automation.DataModels.ReasonDefinition GetReasonDefinition(ReasonTypeEnum reasonType, string reasonDescription);

        Dictionary<int, string> GetReasonDescriptionsByReasonType(string reasonType, bool allowComment);

        QueryResults GetActiveReasonsByReasonType(string ReasonType);

        QueryResults GetReasonsByGenericKeyAndGenericKeyType(int genericKey, GenericKeyTypeEnum genericKeyType);

        QueryResults GetNotificationReasonsForLegalEntity(int reasonDefinition, int accountKey);
    }
}