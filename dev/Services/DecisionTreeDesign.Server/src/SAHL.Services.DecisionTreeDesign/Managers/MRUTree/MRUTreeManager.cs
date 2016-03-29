using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree
{
    public class MRUTreeManager : IMRUTreeManager
    {
        private IMRUTreeDataManager MRUTreeDataManager;
        public MRUTreeManager(IMRUTreeDataManager MRUTreeDataManager)
        {
            this.MRUTreeDataManager = MRUTreeDataManager;
        }

        public void UpdateMRUDecisionTrees(Guid decisionTreeId, string username)
        {
            if (this.MRUTreeDataManager.DoesMRUTreeIdUserExist(decisionTreeId, username))
            {
                this.MRUTreeDataManager.UpdateMRUTree(decisionTreeId, username);
            }
            else
            {
                this.MRUTreeDataManager.InsertMRUTree(decisionTreeId, username);
                // if we have added a new one trim the list
                this.MRUTreeDataManager.TrimMRUTreeDataForUser(username);
            }
        }

        public void SaveMRUDecisionTreesPinStatus(Guid decisionTreeId, string username, bool pinned)
        {
            this.MRUTreeDataManager.SaveMRUTreePinStatus(decisionTreeId, username, pinned);
        }

        public void RemoveMRUDecisionTree(Guid decisionTreeId, string username)
        {
            this.MRUTreeDataManager.RemoveMRUDecisionTree(decisionTreeId, username);
        }
    }
}