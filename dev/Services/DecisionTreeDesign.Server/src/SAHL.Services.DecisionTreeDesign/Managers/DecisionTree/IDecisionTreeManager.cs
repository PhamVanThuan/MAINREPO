using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree
{
    public interface IDecisionTreeManager
    {
        void SaveDecisionTreeAs(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username);

        void SaveDecisionTreeVersion(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username);

        void SaveDecisionTreeThumbnail(Guid decisionTreeId, string thumbnail);

        void SaveNewDecisionTreeVersion(Guid decisionTreeVersionId, Guid decisionTreeId, int version, string data, string username);

        void SaveAndPublishDecisionTree(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string publisher, bool saveFirst);

        void DeleteDecisionTree(Guid decisionTreeId,Guid decisionTreeVersionId, string username);

        string GetTreeJson(string name, int version);
    }
}