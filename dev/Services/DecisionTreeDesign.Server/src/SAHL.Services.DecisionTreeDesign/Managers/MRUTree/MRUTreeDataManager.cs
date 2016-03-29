using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Identity;
using System;
using SAHL.Services.DecisionTreeDesign.Managers.MRUTree.Statements;

namespace SAHL.Services.DecisionTreeDesign.Managers.MRUTree
{
    public class MRUTreeDataManager : IMRUTreeDataManager
    {
        public bool DoesMRUTreeIdUserExist(Guid decisionTreeVersionId, string username)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                DoesMRUTreeIdUserExistQuery query = new DoesMRUTreeIdUserExistQuery(decisionTreeVersionId, username);
                var tree = db.SelectOne(query);
                if (tree != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public void UpdateMRUTree(Guid decisionTreeVersionId, string username)
        {
            UpdateMRUTreeDataQuery query = new UpdateMRUTreeDataQuery(decisionTreeVersionId, username);
            using (var db = new Db().InAppContext())
            {
                db.Update<UserMRUDecisionTreeDataModel>(query);
                db.Complete();
            }
        }

        public void InsertMRUTree(Guid decisionTreeVersionId, string username)
        {
            var id = CombGuid.Instance.Generate();
            UserMRUDecisionTreeDataModel userMRUDecisionTreeDataModel = new UserMRUDecisionTreeDataModel(id, username, decisionTreeVersionId, DateTime.Now, false);

            using (var db = new Db().InAppContext())
            {
                db.Insert<UserMRUDecisionTreeDataModel>(userMRUDecisionTreeDataModel);
                db.Complete();
            }
        }

        public void SaveMRUTreePinStatus(Guid decisionTreeVersionId, string username, bool pinned)
        {
            SaveMRUTreePinStatusDataQuery query = new SaveMRUTreePinStatusDataQuery(decisionTreeVersionId, username, pinned);
            using (var db = new Db().InAppContext())
            {
                db.Update<UserMRUDecisionTreeDataModel>(query);
                db.Complete();
            }
        }

        public void RemoveMRUDecisionTree(Guid decisionTreeVersionId, string username)
        {
            RemoveDecisionTreeMRUForUserQuery query = new RemoveDecisionTreeMRUForUserQuery(decisionTreeVersionId, username);
            using (var db = new Db().InAppContext())
            {
                db.Delete<UserMRUDecisionTreeDataModel>(query);
                db.Complete();
            }
        }

        public void TrimMRUTreeDataForUser(string username)
        {
            TrimMRUTreeDataForUserQuery query = new TrimMRUTreeDataForUserQuery(username);
            using (var db = new Db().InAppContext())
            {
                db.Delete<UserMRUDecisionTreeDataModel>(query);
                db.Complete();
            }
        }
    }
}