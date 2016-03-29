using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2
{
    public class X2RequestInterrogator : IX2RequestInterrogator
    {
        private IWorkflowDataProvider workflowDataProvider;

        public X2RequestInterrogator(IWorkflowDataProvider workflowDataProvider)
        {
            this.workflowDataProvider = workflowDataProvider;
        }

        public X2Workflow GetRequestWorkflow(IX2Request request)
        {
            X2Workflow x2Workflow = null;
            if (request is IX2CreateRequest)
            {
                var createRequest = request as IX2CreateRequest;
                x2Workflow = new X2Workflow(createRequest.ProcessName, createRequest.WorkflowName);
                return x2Workflow;
            }
            else if (request is IX2ExternalActivityRequest)
            {
                var externalActivityRequest = (IX2ExternalActivityRequest)request;
                if ((externalActivityRequest.ActivatingInstanceId.HasValue == false) || (externalActivityRequest.ActivatingInstanceId.Value <= 0))
                {
                    var workflowDataModel = workflowDataProvider.GetWorkflowById(externalActivityRequest.WorkflowId);
                    var processDataModel = workflowDataProvider.GetProcessById(workflowDataModel.ProcessID);
                    return new X2Workflow(processDataModel.Name, workflowDataModel.Name);
                }
                else
                {
                    WorkFlowDataModel workflow = workflowDataProvider.GetWorkflowById(externalActivityRequest.WorkflowId);
                    ProcessDataModel process = workflowDataProvider.GetProcessById(workflow.ProcessID);
                    x2Workflow = new X2Workflow(process.Name, workflow.Name);
                    return x2Workflow;
                }
            }
            else if (request is IX2RequestForSecurityRecalc)
            {
                IX2RequestForSecurityRecalc recalc = request as IX2RequestForSecurityRecalc;
                InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(recalc.InstanceId);
                if (instance != null)
                {
                    WorkFlowDataModel workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);
                    ProcessDataModel process = workflowDataProvider.GetProcessById(workflow.ProcessID);
                    return new X2Workflow(process.Name, workflow.Name);
                }
                else
                    return new X2Workflow("", "");
            }
            else
            {
                var existingInstanceRequest = request as IX2RequestForExistingInstance;
                InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(existingInstanceRequest.InstanceId);
                if (instance != null)
                {
                    WorkFlowDataModel workflow = workflowDataProvider.GetWorkflow(instance);
                    ProcessDataModel process = workflowDataProvider.GetProcessById(workflow.ProcessID);
                    return new X2Workflow(process.Name, workflow.Name);
                }
                else
                    return new X2Workflow("", "");
            }
        }

        public bool IsRequestMonitored(IX2Request request)
        {
            switch (request.RequestType)
            {
                case X2RequestType.UserStart:
                case X2RequestType.UserComplete:
                case X2RequestType.UserCancel:
                case X2RequestType.UserCreate:
                case X2RequestType.UserCreateWithComplete:
                case X2RequestType.CreateComplete:
                case X2RequestType.SecurityRecalc:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}