using SAHL.Core.Data.Models.DecisionTree;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree
{
    public interface IDecisionTreeDataManager
    {
        bool DoesTreeExist(Guid decisionTreeId);

        bool DoesTreeVersionExist(Guid decisionTreeVersionId);

        bool DoesTreeWithSameNameExist(string decisionTreeName);

        int GetMaxTreeVersion(Guid decisionTreeVersionId);

        bool DoesTreeWithSameNameExist(Guid decisionTreeId, string decisionTreeName);

        bool HasTreeVersionBeenPublished(Guid decisionTreeVersionId);

        void SaveDecisionTreeAs(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, int version, string data, string username);

        void SaveDecisionTreeVersion(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username);

        void SaveNewDecisionTreeVersion(Guid decisionTreeVersionId, Guid decisionTreeId, int version, string data, string username);

        void SaveAndPublishDecisionTree(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string publisher, bool saveFirst);

        void SaveDecisionTreeThumbnail(Guid decisionTreeId, string Thumbnail);

        void DeleteDecisionTree(Guid decisionTreeId, Guid decisionTreeVersionId);

        string GetTreeJson(string name, int version);
    }
}