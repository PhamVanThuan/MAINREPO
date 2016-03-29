using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree
{
    public interface IMRUTreeManager
    {
        void RemoveMRUDecisionTree(Guid decisionTreeVersionId, string username);

        void UpdateMRUDecisionTrees(Guid decisionTreeVersionId, string username);

        void SaveMRUDecisionTreesPinStatus(Guid decisionTreeVersionId, string username, bool pinned);
    }
}