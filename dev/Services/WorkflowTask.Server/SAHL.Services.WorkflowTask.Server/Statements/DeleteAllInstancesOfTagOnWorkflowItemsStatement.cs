using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;

namespace SAHL.Services.WorkflowTask.Server.CommandHandlers.Statements
{
    public class DeleteAllInstancesOfTagOnWorkflowItemsStatement : ISqlStatement<WorkflowItemUserTagsDataModel>
    {
        public Guid TagId { get; private set; }

        public DeleteAllInstancesOfTagOnWorkflowItemsStatement(Guid tagId)
        {
            TagId = tagId;
        }

        public string GetStatement()
        {
            return @"DELETE FROM [2am].[tag].[WorkflowItemUserTags] where TagId = @TagId";
        }
    }
}