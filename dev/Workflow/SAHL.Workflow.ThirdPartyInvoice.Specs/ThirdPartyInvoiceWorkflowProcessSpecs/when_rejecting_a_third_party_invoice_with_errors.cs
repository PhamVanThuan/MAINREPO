using System.Collections.Generic;
using System.Linq;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    using Machine.Fakes;
    using Machine.Specifications;
    using NSubstitute;
    using SAHL.Core.Services;
    using SAHL.Core.SystemMessages;
    using SAHL.Services.Interfaces.FinanceDomain;
    using SAHL.Services.Interfaces.FinanceDomain.Commands;
    using SAHL.Workflow.ThirdPartyInvoices;

    public class when_rejecting_a_third_party_invoice_with_errors : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static int thirdPartyInvoiceKey;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static IThirdPartyInvoiceWorkflowProcess thirdPartyInvoiceWorkflowProcess;

        private static bool result;
        private static IServiceRequestMetadata metadata;
        private static IFinanceDomainCommand command;
        private static string rejectionComments;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            financeDomainServiceClient = Substitute.For<IFinanceDomainServiceClient>();
            metadata = new ServiceRequestMetadata();

            thirdPartyInvoiceKey = 123;
            rejectionComments = "this is a comment";

            command = new RejectThirdPartyInvoiceCommand(thirdPartyInvoiceKey, rejectionComments);
            thirdPartyInvoiceWorkflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            var returnMessages = new SystemMessageCollection();
            returnMessages.AddMessage(new SystemMessage("message", SystemMessageSeverityEnum.Error));
            financeDomainServiceClient
                .WhenToldTo(x => x.PerformCommand(Param.IsAny<RejectThirdPartyInvoiceCommand>(), Param.IsAny<ServiceRequestMetadata>()))
                .Return(returnMessages);
        };

        private Because of = () =>
        {
            result = thirdPartyInvoiceWorkflowProcess.RejectThirdPartyInvoiceByThirdPartyInvoiceKey(messages, thirdPartyInvoiceKey, rejectionComments, metadata);
        };

        private It should_send_a_reject_third_party_invoice_command = () =>
        {
            financeDomainServiceClient.WasToldTo(x =>
                x.PerformCommand(Param<RejectThirdPartyInvoiceCommand>.Matches(y =>
                    y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey &&
               y.RejectionComments.Contains(rejectionComments.First())), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_no_errors = () =>
        {
            result.ShouldBeFalse();
        };

    }
}