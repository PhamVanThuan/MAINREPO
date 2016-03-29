using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.Services.Interfaces.X2;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow
{
    public class X2WorkflowManager : IX2WorkflowManager
    {
        private readonly IX2Service x2Service;

        public X2WorkflowManager(IX2Service x2Service)
        {
            this.x2Service = x2Service;
        }

        public ISystemMessageCollection CreateWorkflowCase(int applicationNumber, DomainProcessServiceRequestMetadata domainServiceMetadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            Dictionary<string, string> inputs = new Dictionary<string, string>();
            inputs.Add("ApplicationKey", applicationNumber.ToString());
            inputs.Add("isEstateAgentApplication", "False");

            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata(domainServiceMetadata);

            X2CreateInstanceRequest createInstanceRequest =
                new X2CreateInstanceRequest(domainServiceMetadata.CommandCorrelationId,
                    X2Constants.WorkFlowActivities.ApplicationCreate, // "Create Instance"
                    X2Constants.WorkflowProcesses.Origination, // "Origination",
                    X2Constants.WorkFlowNames.ApplicationCapture, // "Application Capture",
                    serviceRequestMetadata, // Don't set a consultant to assign to.
                    false,
                    null,
                    null,
                    inputs);

            messages.Aggregate(x2Service.PerformCommand(createInstanceRequest));

            X2Response response = createInstanceRequest.Result;
            if (response.IsErrorResponse)
            {
                messages.AddMessage(new SystemMessage(response.Message, SystemMessageSeverityEnum.Error));
                return messages;
            }

            X2RequestForExistingInstance createCompleteRequest =
                new X2RequestForExistingInstance(domainServiceMetadata.CommandCorrelationId,
                    response.InstanceId,
                    X2RequestType.CreateComplete,
                    serviceRequestMetadata,
                    createInstanceRequest.ActivityName,
                    createInstanceRequest.IgnoreWarnings,
                    createInstanceRequest.MapVariables,
                    createInstanceRequest.Data);

            messages.Aggregate(this.x2Service.PerformCommand(createCompleteRequest));

            return messages;
        }

        public ISystemMessageCollection ProcessThirdPartyInvoicePayment(long instanceId, int accountKey, int thirdPartyInvoiceKey, DomainProcessServiceRequestMetadata domainServiceMetadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata(domainServiceMetadata);

            var startProcessPaymentActivityRequest = new X2RequestForExistingInstance(
                      domainServiceMetadata.CommandCorrelationId
                    , instanceId
                    , X2RequestType.UserStart
                    , serviceRequestMetadata
                    , X2Constants.WorkFlowActivities.ThirdPartyInvoicesProcessPayment
                    , false
            );
            systemMessages.Aggregate(x2Service.PerformCommand(startProcessPaymentActivityRequest));

            X2Response startPayActivityResponse = startProcessPaymentActivityRequest.Result;
            if (startPayActivityResponse.IsErrorResponse)
            {
                systemMessages.AddMessage(new SystemMessage(startPayActivityResponse.Message, SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            var completeProcessPaymentActivityRequest = new X2RequestForExistingInstance(
                  domainServiceMetadata.CommandCorrelationId
                , instanceId
                , X2RequestType.UserComplete
                , serviceRequestMetadata
                , X2Constants.WorkFlowActivities.ThirdPartyInvoicesProcessPayment
                , false
            );
            systemMessages.Aggregate(x2Service.PerformCommand(completeProcessPaymentActivityRequest));

            X2Response completePayActivityResponse = completeProcessPaymentActivityRequest.Result;
            if (completePayActivityResponse.IsErrorResponse)
            {
                systemMessages.AddMessage(new SystemMessage(completePayActivityResponse.Message, SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            return systemMessages;
        }

        public ISystemMessageCollection ArchiveThirdPartyInvoice(long instanceId, int accountKey, int thirdPartyInvoiceKey, DomainProcessServiceRequestMetadata domainServiceMetadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata(domainServiceMetadata);

            var startFirePaidFlagRequest = new X2RequestForExistingInstance(
                      domainServiceMetadata.CommandCorrelationId
                    , instanceId
                    , X2RequestType.UserStart
                    , serviceRequestMetadata
                    , X2Constants.WorkFlowActivities.ThirdPartyInvoicesPaid
                    , false
            );
            systemMessages.Aggregate(x2Service.PerformCommand(startFirePaidFlagRequest));

            X2Response startPayActivityResponse = startFirePaidFlagRequest.Result;
            if (startPayActivityResponse.IsErrorResponse)
            {
                systemMessages.AddMessage(new SystemMessage(startPayActivityResponse.Message, SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            var completeFirePaidFlagRequest = new X2RequestForExistingInstance(
                  domainServiceMetadata.CommandCorrelationId
                , instanceId
                , X2RequestType.UserComplete
                , serviceRequestMetadata
                , X2Constants.WorkFlowActivities.ThirdPartyInvoicesPaid
                , false
            );
            systemMessages.Aggregate(x2Service.PerformCommand(completeFirePaidFlagRequest));

            X2Response completePayActivityResponse = completeFirePaidFlagRequest.Result;
            if (completePayActivityResponse.IsErrorResponse)
            {
                systemMessages.AddMessage(new SystemMessage(completePayActivityResponse.Message, SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            return systemMessages;
        }

        public ISystemMessageCollection ReversePayment(long instanceId, int accountKey, int thirdPartyInvoiceKey, DomainProcessServiceRequestMetadata domainServiceMetadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata(domainServiceMetadata);

            var startReversePaymentActivity = new X2RequestForExistingInstance(
                      domainServiceMetadata.CommandCorrelationId
                    , instanceId
                    , X2RequestType.UserStart
                    , serviceRequestMetadata
                    , X2Constants.WorkFlowActivities.ThirdPartyInvoicesReversePayment
                    , false
            );
            systemMessages.Aggregate(x2Service.PerformCommand(startReversePaymentActivity));

            X2Response startReversePaymentActivityResponse = startReversePaymentActivity.Result;
            if (startReversePaymentActivityResponse.IsErrorResponse)
            {
                systemMessages.AddMessage(new SystemMessage(startReversePaymentActivityResponse.Message, SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            var completeReversePaymentActivityRequest = new X2RequestForExistingInstance(
                  domainServiceMetadata.CommandCorrelationId
                , instanceId
                , X2RequestType.UserComplete
                , serviceRequestMetadata
                , X2Constants.WorkFlowActivities.ThirdPartyInvoicesReversePayment
                , false
            );
            systemMessages.Aggregate(x2Service.PerformCommand(completeReversePaymentActivityRequest));

            X2Response completeReversePaymentActivityResponse = completeReversePaymentActivityRequest.Result;
            if (completeReversePaymentActivityResponse.IsErrorResponse)
            {
                systemMessages.AddMessage(new SystemMessage(completeReversePaymentActivityResponse.Message, SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            return systemMessages;
        }
    }
}