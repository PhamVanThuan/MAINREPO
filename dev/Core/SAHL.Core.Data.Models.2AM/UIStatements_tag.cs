using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string usertagsdatamodel_selectwhere = "SELECT Id, caption, ADUsername, BackColour, ForeColour, CreateDate FROM [2am].[tag].[UserTags] WHERE";
        public const string usertagsdatamodel_selectbykey = "SELECT Id, caption, ADUsername, BackColour, ForeColour, CreateDate FROM [2am].[tag].[UserTags] WHERE Id = @PrimaryKey";
        public const string usertagsdatamodel_delete = "DELETE FROM [2am].[tag].[UserTags] WHERE Id = @PrimaryKey";
        public const string usertagsdatamodel_deletewhere = "DELETE FROM [2am].[tag].[UserTags] WHERE";
        public const string usertagsdatamodel_insert = "INSERT INTO [2am].[tag].[UserTags] (Id, caption, ADUsername, BackColour, ForeColour, CreateDate) VALUES(@Id, @caption, @ADUsername, @BackColour, @ForeColour, @CreateDate); ";
        public const string usertagsdatamodel_update = "UPDATE [2am].[tag].[UserTags] SET Id = @Id, caption = @caption, ADUsername = @ADUsername, BackColour = @BackColour, ForeColour = @ForeColour, CreateDate = @CreateDate WHERE Id = @Id";



        public const string workflowitemusertagsdatamodel_selectwhere = "SELECT ItemKey, WorkFlowItemId, ADUsername, TagId, CreateDate FROM [2am].[tag].[WorkflowItemUserTags] WHERE";
        public const string workflowitemusertagsdatamodel_selectbykey = "SELECT ItemKey, WorkFlowItemId, ADUsername, TagId, CreateDate FROM [2am].[tag].[WorkflowItemUserTags] WHERE ItemKey = @PrimaryKey";
        public const string workflowitemusertagsdatamodel_delete = "DELETE FROM [2am].[tag].[WorkflowItemUserTags] WHERE ItemKey = @PrimaryKey";
        public const string workflowitemusertagsdatamodel_deletewhere = "DELETE FROM [2am].[tag].[WorkflowItemUserTags] WHERE";
        public const string workflowitemusertagsdatamodel_insert = "INSERT INTO [2am].[tag].[WorkflowItemUserTags] (WorkFlowItemId, ADUsername, TagId, CreateDate) VALUES(@WorkFlowItemId, @ADUsername, @TagId, @CreateDate); select cast(scope_identity() as int)";
        public const string workflowitemusertagsdatamodel_update = "UPDATE [2am].[tag].[WorkflowItemUserTags] SET WorkFlowItemId = @WorkFlowItemId, ADUsername = @ADUsername, TagId = @TagId, CreateDate = @CreateDate WHERE ItemKey = @ItemKey";



    }
}