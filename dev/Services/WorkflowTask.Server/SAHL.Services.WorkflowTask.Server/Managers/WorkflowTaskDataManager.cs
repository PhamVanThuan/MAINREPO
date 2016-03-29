using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.CommandHandlers.Statements;
using SAHL.Services.WorkflowTask.Server.Managers.Statements;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.DataModels;
using SAHL.Services.WorkflowTask.Server.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SAHL.Services.WorkflowTask.Server.Managers
{
    public class WorkflowTaskDataManager : IWorkflowTaskDataManager
    {
        private readonly IDbFactory dbFactory;

        public WorkflowTaskDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<WorkFlowStateItem> RetrieveWorkFlowStatesForWorkListItemsForUsername(string username, List<string> capabilities)
        {
            var statement = new GetWorkFlowStatesForWorkListItemsForUsernameStatement(username, capabilities);

            using (var context = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = context.Select(statement);
                context.Complete();
                return result;
            }
        }

        public void PerformTaskQueries(IEnumerable<ITaskQueryResultCollator> resultCollators)
        {
            var items = resultCollators;

            Parallel.ForEach(items, collator =>
            {
                using (var context = dbFactory.NewDb().InReadOnlyAppContext())
                {
                    var statement = collator.CreateTaskStatement();

                    var results = context
                        .Select(statement)
                        .Cast<IDictionary<string, object>>()
                        .ToList();

                    collator.Results = results;

                    context.Complete();
                }
            }
            );
        }

        public void AddTagForUser(CreateTagForUserCommand command)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var insertTags = new UserTagsDataModel(command.Id,command.Caption,command.UserName, command.BackColour, command.ForeColour, DateTime.Now);
                db.Insert(insertTags);
                db.Complete();
            }
        }

        public void AddTagToWorkFlowItem(AddTagToWorkflowInstanceCommand command)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var toInsert = new WorkflowItemUserTagsDataModel(command.WorkflowInstanceId, command.Username, command.TagId, DateTime.Now);
                db.Insert(toInsert);
                db.Complete();
            }
        }

        public void DeleteAllInstancesOfTagOnWorkflowItems(Guid tagId)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new DeleteAllInstancesOfTagOnWorkflowItemsStatement(tagId);
                db.Delete(statement);
                db.Complete();
            }
        }

        public void DeleteTagForUser(Guid tagId)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.DeleteByKey<UserTagsDataModel, Guid>(tagId);
                db.Complete();
            }
        }

        public int GetWorkflowUserTagKey(Guid tagId, long workflowItemId)
        {
            WorkflowItemUserTagsDataModel item;
            using (var db = dbFactory.NewDb().InAppContext())
            {
                item = db.SelectOneWhere<WorkflowItemUserTagsDataModel>("TagId = @TagId AND WorkFlowItemId = @WorkFlowItemId", new { TagId = tagId, WorkFlowItemId = workflowItemId });
                db.Complete();
            }
            return item.ItemKey;
        }

        public void DeleteWorkflowUserTagByKey(int keyId)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.DeleteByKey<WorkflowItemUserTagsDataModel, int>(keyId);
                db.Complete();
            }
        }

        public void UpdateUserTag(UpdateUserTagCommand command)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var updateUserTags = new UpdateUserTagWithGivenValuesStatement(command.Id, command.Caption,
                    command.BackColour, command.ForeColour);
                
                db.Update<UserTagsDataModel>(updateUserTags);
                db.Complete();
            }
        }

        public Guid FindTagIdForUsernameAndTextCombination(string tagText, string username)
        {
            UserTagsDataModel tag;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                tag = db.SelectOneWhere<UserTagsDataModel>("Username = @Username AND Text = @Text",
                    new { Username = username, Text = tagText });
                db.Complete();
            }
            return tag.Id;
        }

        public IEnumerable<UserTagsDataModel> GetAllTagsForUser(string userName)
        {
            IEnumerable<UserTagsDataModel> tags;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                tags = db.SelectWhere<UserTagsDataModel>("ADUsername = @Username", new { Username = userName });
                db.Complete();
            }
            return tags;
        }

       public Dictionary<Guid, Tag> MapTags(IEnumerable<UserTagsDataModel> tags)
        {
            return tags.ToDictionary(tag => tag.Id, tag => new Tag
            {
                Id = tag.Id,
                Caption = tag.caption,
                Style = new Dictionary<string, string> { { "background-color", tag.BackColour }, { "color", tag.ForeColour } }
            });
        }
    }
}
