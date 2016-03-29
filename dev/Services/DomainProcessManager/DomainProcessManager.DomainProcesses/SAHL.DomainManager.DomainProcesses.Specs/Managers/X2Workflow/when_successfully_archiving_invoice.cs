using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.Services.Interfaces.X2;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.X2Workflow
{
    public class when_successfully_archiving_invoice : WithFakes
    {
        private static IX2Service x2Service;
        private static X2WorkflowManager x2workflowManger;
        private static long instanceId;
        private static int accountKey;
        private static int thirdPartyInvoiceKey;
        private static ICombGuid combGuidGenerator;
        private static Guid domainProcessId, performProcessedActivityCorrelationId;
        private static DomainProcessServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            x2Service = An<IX2Service>();
            combGuidGenerator = An<ICombGuid>();

            x2workflowManger = new X2WorkflowManager(x2Service);
            instanceId = 84390249504950940;
            accountKey = 1251337;
            thirdPartyInvoiceKey = 6161;
            domainProcessId = combGuidGenerator.Generate();
            performProcessedActivityCorrelationId = combGuidGenerator.Generate();

            serviceRequestMetadata = new DomainProcessServiceRequestMetadata(domainProcessId, performProcessedActivityCorrelationId);

            x2Service.WhenToldTo(x2 => x2.PerformCommand(Param.IsAny<X2RequestForExistingInstance>()))
                .Return<X2RequestForExistingInstance>((request) =>
                {
                    request.Result = new X2Response(Guid.NewGuid(), string.Empty, instanceId);
                    return SystemMessageCollection.Empty();
                }
            );
        };

        private Because of = () =>
        {
            x2workflowManger.ArchiveThirdPartyInvoice(instanceId, accountKey, thirdPartyInvoiceKey, serviceRequestMetadata);
        };

        private It should_start_processed_activity_for_the_given_case = () =>
        {
            x2Service.WasToldTo(x => x.PerformCommand(Param<X2RequestForExistingInstance>.Matches(m =>
                    m.ActivityName == X2Constants.WorkFlowActivities.ThirdPartyInvoicesPaid &&
                    m.CorrelationId == performProcessedActivityCorrelationId &&
                    m.ServiceRequestMetadata.UserName == serviceRequestMetadata.UserName &&
                    !m.IgnoreWarnings &&
                    m.InstanceId == instanceId
                 )
               ));
        };

        private It should_complete_processed_activity_for_the_given_case = () =>
        {
            x2Service.WasToldTo(x => x.PerformCommand(Param<X2RequestForExistingInstance>.Matches(m =>
                   m.ActivityName == X2Constants.WorkFlowActivities.ThirdPartyInvoicesPaid &&
                   m.CorrelationId == performProcessedActivityCorrelationId &&
                   m.ServiceRequestMetadata.UserName == serviceRequestMetadata.UserName &&
                   !m.IgnoreWarnings &&
                   m.InstanceId == instanceId
                )
           ));
        };
    }
}