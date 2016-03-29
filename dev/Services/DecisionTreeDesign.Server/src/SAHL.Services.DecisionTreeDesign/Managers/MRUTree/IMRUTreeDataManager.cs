using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree
{
    public interface IMRUTreeDataManager
    {
        void TrimMRUTreeDataForUser(string username);

        void RemoveMRUDecisionTree(Guid decisionTreeVersionId, string username);

        bool DoesMRUTreeIdUserExist(Guid decisionTreeVersionId, string username);

        void UpdateMRUTree(Guid decisionTreeVersionId, string username);

        void InsertMRUTree(Guid decisionTreeVersionId, string username);

        void SaveMRUTreePinStatus(Guid decisionTreeVersionId, string username, bool pinned);
    }
}