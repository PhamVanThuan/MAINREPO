using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Identity;
using System;
using System.Collections.Generic;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree
{
    public class DecisionTreeDataManager : IDecisionTreeDataManager
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        public DecisionTreeDataManager(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void DeleteDecisionTree(Guid decisionTreeId, Guid decisionTreeVersionId)
        {
            using (IUnitOfWork unitOfWork =this.unitOfWorkFactory.Build())
            {
                //must remove history references due to FK constraint
                DeleteVersionFromHistoryQuery deleteHistory = new DeleteVersionFromHistoryQuery(decisionTreeVersionId);
                DeleteVersionQuery deleteVersion = new DeleteVersionQuery(decisionTreeVersionId);
                
                //must remove specific version
                //if no more versions, delete tree itself
                using (var db = new Db().InAppContext())
                {
                    db.Delete<DecisionTreeHistoryDataModel>(deleteHistory);
                    db.Delete<DecisionTreeVersionDataModel>(deleteVersion);
                    if (!AreVersionsRemaining(decisionTreeId))
                    {
                        //delete tree itself
                        DeleteTreeQuery deleteTree = new DeleteTreeQuery(decisionTreeId);
                        db.Delete<DecisionTreeDataModel>(deleteTree);

                    }
                    db.Complete();
                }
                unitOfWork.Complete();
            }
        }

        public void SaveAndPublishDecisionTree(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string publisher, bool saveFirst)
        {
            using (IUnitOfWork unitOfWork =this.unitOfWorkFactory.Build())
            {
                if (saveFirst)
                {
                    SaveDecisionTreeVersion(decisionTreeId,name,description,isActive, thumbnail,decisionTreeVersionId, data, publisher);
                 }
                PublishedDecisionTreeDataModel publishedDecisionTreeDataModel = new PublishedDecisionTreeDataModel(CombGuid.Instance.Generate(), decisionTreeVersionId, Guid.Parse(PublishStatusEnumDataModel.PUBLISHED), DateTime.Now, publisher);
                using (var db = new Db().InAppContext())
                {
                    db.Insert<PublishedDecisionTreeDataModel>(publishedDecisionTreeDataModel);
                    db.Complete();
                }
                unitOfWork.Complete();
            }
        }

        public void SaveDecisionTreeAs(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, int version, string data, string username)
        {
            DecisionTreeDataModel decisionTreeDataModel = new DecisionTreeDataModel(decisionTreeId, name, description, true, thumbnail);
            DecisionTreeVersionDataModel decisionTreeVersionDataModel = new DecisionTreeVersionDataModel(decisionTreeVersionId, decisionTreeId, version, data);
            DecisionTreeHistoryDataModel decisionTreeHistoryDataModel = new DecisionTreeHistoryDataModel(CombGuid.Instance.Generate(), decisionTreeVersionId, username, DateTime.Now);
            using (var db = new Db().InAppContext())
            {
                db.Insert<DecisionTreeDataModel>(decisionTreeDataModel);
                db.Insert<DecisionTreeVersionDataModel>(decisionTreeVersionDataModel);
                db.Insert<DecisionTreeHistoryDataModel>(decisionTreeHistoryDataModel);
                db.Complete();
            }
        }

        public void SaveNewDecisionTreeVersion(Guid decisionTreeVersionId, Guid decisionTreeId, int version, string data, string username)
        {
            DecisionTreeVersionDataModel decisionTreeVersionDataModel = new DecisionTreeVersionDataModel(decisionTreeVersionId, decisionTreeId, version, data);
            DecisionTreeHistoryDataModel decisionTreeHistoryDataModel = new DecisionTreeHistoryDataModel(CombGuid.Instance.Generate(), decisionTreeVersionId, username, DateTime.Now);
            using (var db = new Db().InAppContext())
            {
                db.Insert<DecisionTreeVersionDataModel>(decisionTreeVersionDataModel);
                db.Insert<DecisionTreeHistoryDataModel>(decisionTreeHistoryDataModel);
                db.Complete();
            }
        }

        public void SaveDecisionTreeVersion(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username)
        {
            UpdateDecisionTreeDataQuery query = new UpdateDecisionTreeDataQuery(decisionTreeVersionId, data);
            UpdateDecisionTreeQuery query2 = new UpdateDecisionTreeQuery(decisionTreeId, name, description, isActive, thumbnail);
            DecisionTreeHistoryDataModel decisionTreeHistoryDataModel = new DecisionTreeHistoryDataModel(CombGuid.Instance.Generate(), decisionTreeVersionId, username, DateTime.Now);
            using (var db = new Db().InAppContext())
            {
                db.Update<DecisionTreeVersionDataModel>(query);
                db.Update<DecisionTreeDataModel>(query2);
                db.Insert<DecisionTreeHistoryDataModel>(decisionTreeHistoryDataModel);
                db.Complete();
            }
        }

        public void SaveDecisionTreeThumbnail(Guid decisionTreeId, string thumbnail)
        {
            UpdateThumbnailDataQuery query = new UpdateThumbnailDataQuery(decisionTreeId, thumbnail);            
            using (var db = new Db().InAppContext())
            {
                db.Update<DecisionTreeDataModel>(query);                
                db.Complete();
            }
        }

        public bool DoesTreeExist(Guid decisionTreeId)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                DoesTreeIdExistQuery query = new DoesTreeIdExistQuery(decisionTreeId);
                var tree = db.SelectOne(query);
                if (tree != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool DoesTreeVersionExist(Guid decisionTreeVersionId)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                DoesTreeVersionIdExistQuery query = new DoesTreeVersionIdExistQuery(decisionTreeVersionId);
                var treeVersion = db.SelectOne(query);
                if (treeVersion != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool DoesTreeWithSameNameExist(string decisionTreeName)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                DoesTreeWithSameNameExistQuery query = new DoesTreeWithSameNameExistQuery(decisionTreeName);
                var tree = db.SelectOne(query);
                if (tree != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool DoesTreeWithSameNameExist(Guid decisionTreeId, string decisionTreeName)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                DoesTreeWithSameNameAndIdExistQuery query = new DoesTreeWithSameNameAndIdExistQuery(decisionTreeId, decisionTreeName);
                var tree = db.SelectOne(query);
                if (tree != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public int GetMaxTreeVersion(Guid decisionTreeVersionId)
        {
            throw new NotImplementedException();
        }

        public bool AreVersionsRemaining(Guid decisionTreeId)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                AreDecisionTreeVersionsLeftQuery query = new AreDecisionTreeVersionsLeftQuery(decisionTreeId);
                var max = db.SelectOne(query);
                if (max != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool HasTreeVersionBeenPublished(Guid decisionTreeVersionId)
        {
            bool result = false;

            using (var db = new Db().InReadOnlyAppContext())
            {
                IsDecisionTreeVersionPublishedQuery query = new IsDecisionTreeVersionPublishedQuery(decisionTreeVersionId);
                var tree = db.SelectOne(query);
                if (tree != null)
                {
                    result = true;
                }
            }
            return result;
        }


        public string GetTreeJson(string name, int version)
        {
            string result = "";
            GetTreeJsonQuery query = new GetTreeJsonQuery(name, version);
            using (var db = new Db().InReadOnlyAppContext())
            {
                DecisionTreeVersionDataModel model = db.SelectOne(query);
                result = model.Data;
            }
            return result;
        }
    }
}