using SAHL.Core.Data;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument;
using SAHL.Services.DecisionTreeDesign.Managers.MRUTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree
{
    public class DecisionTreeManager : IDecisionTreeManager
    {
        private IDecisionTreeDataManager decisionTreeDataManager;
        private ICurrentlyOpenDocumentManager lockScheduleManager;
        private IMRUTreeManager mruTreeManager;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public DecisionTreeManager(IDecisionTreeDataManager decisionTreeDataManager, ICurrentlyOpenDocumentManager lockScheduleManager, IMRUTreeManager mruTreeManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.decisionTreeDataManager = decisionTreeDataManager;
            this.lockScheduleManager = lockScheduleManager;
            this.mruTreeManager = mruTreeManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void DeleteDecisionTree(Guid decisionTreeId, Guid decisionTreeVersionId, string username)
        {
            if (!this.decisionTreeDataManager.DoesTreeExist(decisionTreeId))
            {
                throw new InvalidOperationException("No such tree");
            }
            if (!this.decisionTreeDataManager.DoesTreeVersionExist(decisionTreeVersionId))
            {
                throw new InvalidOperationException("This Version does not exist");
            }
            if (this.decisionTreeDataManager.HasTreeVersionBeenPublished(decisionTreeVersionId))
            {
                throw new InvalidOperationException("This tree has already been published. It cannot be deleted.");
            }

            using (var uow = this.unitOfWorkFactory.Build())
            {
                // first remove the current lock
                this.lockScheduleManager.CloseDocument(decisionTreeVersionId);

                // remove the MRU item for the user
                this.mruTreeManager.RemoveMRUDecisionTree(decisionTreeVersionId, username);

                // delete the tree
                this.decisionTreeDataManager.DeleteDecisionTree(decisionTreeId, decisionTreeVersionId);
                uow.Complete();
            }
        }

        public void SaveDecisionTreeAs(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username)
        {
            // check if the tree id does not already exist
            if (this.decisionTreeDataManager.DoesTreeExist(decisionTreeId))
            {
                throw new InvalidOperationException("The tree id is not unique.");
            }
            // check if the tree version id does not already exist
            if (this.decisionTreeDataManager.DoesTreeVersionExist(decisionTreeVersionId))
            {
                throw new InvalidOperationException("The tree version id is not unique.");
            }

            // check that the name is not already taken
            if (this.decisionTreeDataManager.DoesTreeWithSameNameExist(name))
            {
                throw new InvalidOperationException("The tree name already exists.");
            }

            this.decisionTreeDataManager.SaveDecisionTreeAs(decisionTreeId, name, description, isActive, thumbnail, decisionTreeVersionId, 1, data, username);
        }

        public void SaveDecisionTreeVersion(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string username)
        {
            // check that this version is not published

            if (this.decisionTreeDataManager.HasTreeVersionBeenPublished(decisionTreeVersionId))
            {
                throw new InvalidOperationException("This tree has already been published");
            }

            this.decisionTreeDataManager.SaveDecisionTreeVersion(decisionTreeId, name, description, isActive, thumbnail,decisionTreeVersionId, data, username);
        }

        public void SaveDecisionTreeThumbnail(Guid decisionTreeId, string thumbnail)
        {
            this.decisionTreeDataManager.SaveDecisionTreeThumbnail(decisionTreeId, thumbnail);
        }

        public void SaveNewDecisionTreeVersion(Guid decisionTreeVersionId, Guid decisionTreeId, int version, string data, string username)
        {
            // check if the tree id does not already exist
            if (!this.decisionTreeDataManager.DoesTreeExist(decisionTreeId))
            {
                throw new InvalidOperationException("There is no tree with this ID");
            }
            // check if the tree version id does not already exist
            if (this.decisionTreeDataManager.DoesTreeVersionExist(decisionTreeVersionId))
            {
                throw new InvalidOperationException("The tree version id is not unique.");
            }
            this.decisionTreeDataManager.SaveNewDecisionTreeVersion(decisionTreeVersionId, decisionTreeId, version, data, username);
        }

        public void SaveAndPublishDecisionTree(Guid decisionTreeId, string name, string description, bool isActive, string thumbnail, Guid decisionTreeVersionId, string data, string publisher, bool saveFirst)
        {
            if (this.decisionTreeDataManager.HasTreeVersionBeenPublished(decisionTreeVersionId))
            {
                throw new InvalidOperationException("This tree has already been published");
            }

            this.decisionTreeDataManager.SaveAndPublishDecisionTree(decisionTreeId,name,description,isActive, thumbnail, decisionTreeVersionId, data, publisher, saveFirst);
        }


        public string GetTreeJson(string name, int version)
        {
            return this.decisionTreeDataManager.GetTreeJson(name, version);
        }
    }
}