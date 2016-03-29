using System;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CreateInstanceCommandHandler : IServiceCommandHandler<CreateInstanceCommand>
    {
        public ISystemMessageCollection HandleCommand(CreateInstanceCommand command, IServiceRequestMetadata metadata)
        {
            try
            {
                using (var db = new Db().InWorkflowContext())
                {
                    Activity activity = db.SelectOne<Activity>(new ActivityByNameAndWorkflowNameSqlStatement(command.ActivityName, command.WorkflowName));

                    WorkFlowDataModel workflowDataModel = db.SelectOne<WorkFlowDataModel>(UIStatements.workflowdatamodel_selectbykey, new { PrimaryKey = activity.WorkflowId });

                    InstanceDataModel instance = new InstanceDataModel(activity.WorkflowId, command.ParentInstanceID, workflowDataModel.Name, workflowDataModel.DefaultSubject,
                        command.WorkflowProviderName, null, command.UserName, DateTime.Now, null, null, DateTime.Now, command.UserName, activity.ActivityID, 9,
                        command.SourceInstanceId, command.ReturnActivityId);
                    db.Insert<InstanceDataModel>(instance);
                    db.Complete();
                    command.NewlyCreatedInstanceId = instance.ID;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new SystemMessageCollection();
        }
    }
}