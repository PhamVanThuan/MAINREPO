using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.X2;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class CreateThirdPartyInvoiceWorkflowCaseCommandHandler : IServiceCommandHandler<CreateThirdPartyInvoiceWorkflowCaseCommand>
    {
        private IServiceUrlConfiguration serviceUrlConfiguration;
        private IX2Service client;

        public CreateThirdPartyInvoiceWorkflowCaseCommandHandler(IServiceUrlConfiguration serviceUrlConfiguration, IX2Service client)
        {
            this.serviceUrlConfiguration = serviceUrlConfiguration;
            this.client = client;
        }

        public ISystemMessageCollection HandleCommand(CreateThirdPartyInvoiceWorkflowCaseCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            var fieldInputList = new Dictionary<string, string>();
            fieldInputList.Add("ThirdPartyInvoiceKey", command.ThirdPartyInvoiceKey.ToString());
            fieldInputList.Add("AccountKey", command.AccountKey.ToString());
            fieldInputList.Add("ThirdPartyTypeKey", command.ThirdPartyTypeKey.ToString());

            var activityName = X2Constants.WorkFlowActivities.ThirdPartyInvoicesCreateCase;
            var processName = X2Constants.WorkflowProcesses.ThirdPartyInvoices;
            var workflowName = X2Constants.WorkflowProcesses.ThirdPartyInvoices;
            var ignoreWarnings = false;

            var request = new X2CreateInstanceRequest(command.Id, activityName, processName, workflowName, metadata, ignoreWarnings, null, null, fieldInputList, null);
            client.PerformCommand(request);

            var response = request.Result;

            if (response.InstanceId <= 0 || response.IsErrorResponse)
            {
                messages.AddMessage(new SystemMessage(response.Message, SystemMessageSeverityEnum.Error));
                return messages;
            }

            var completeRequest = new X2RequestForExistingInstance(command.Id, response.InstanceId, X2RequestType.CreateComplete, metadata, activityName, ignoreWarnings, fieldInputList, command.ReceivedFrom);
            client.PerformCommand(completeRequest);

            if (completeRequest.Result.IsErrorResponse)
            {
                messages.AddMessage(new SystemMessage(completeRequest.Result.Message, SystemMessageSeverityEnum.Error));
            }

            return messages;
        }
    }
}