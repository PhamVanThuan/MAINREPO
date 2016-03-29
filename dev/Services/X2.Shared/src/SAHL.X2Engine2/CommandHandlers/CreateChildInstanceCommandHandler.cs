using System;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CreateChildInstanceCommandHandler : IServiceCommandHandler<CreateChildInstanceCommand>
    {
        public ISystemMessageCollection HandleCommand(CreateChildInstanceCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                WorkFlowDataModel workflowDataModel = db.SelectOne<WorkFlowDataModel>(UIStatements.workflowdatamodel_selectbykey, new { PrimaryKey = command.Activity.WorkflowId });

                InstanceDataModel instance = new InstanceDataModel(workflowDataModel.ID, command.Instance.ID, workflowDataModel.Name, workflowDataModel.DefaultSubject, command.WorkflowProviderName,
                    null, command.UserName, DateTime.Now, null, null, DateTime.Now, command.UserName, command.Activity.ActivityID, 9, null, null);
                db.Insert(instance);
                db.Complete();

                command.CreatedInstance = db.SelectOne<InstanceDataModel>(UIStatements.instancedatamodel_selectbykey, new { PrimaryKey = instance.ID });
            }
            return new SystemMessageCollection();
        }
    }
}