using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.X2;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CreateThirdPartyInvoiceWorkflowCase
{
    public class when_creating_workflow_case_fails : WithFakes
    {
        private static IServiceUrlConfiguration serviceUrlConfiguration;
        private static IX2Service client;
        private static CreateThirdPartyInvoiceWorkflowCaseCommand command;
        private static CreateThirdPartyInvoiceWorkflowCaseCommandHandler handler;
        private static IServiceRequestMetadata metadata;
        private static Dictionary<string, string> fieldInputList;
        private static int thirdPartyInvoiceKey, loanNumber;
        private static int thirdPartyTypeKey;
        private static X2Response response;
        private static ISystemMessageCollection messages;
        private static string missingNodeSupportingThirdPartInvoicesErrMessage;
        private static string receivedFrom;

        private Establish context = () =>
        {
            serviceUrlConfiguration = An<IServiceUrlConfiguration>();
            metadata = An<IServiceRequestMetadata>();
            fieldInputList = new Dictionary<string, string>();
            thirdPartyInvoiceKey = 1234323;
            loanNumber = 324233;
            thirdPartyTypeKey = (int)ThirdPartyType.Attorney;
            receivedFrom = "accounts@straussdaly.com";

            client = An<IX2Service>();
            command = new CreateThirdPartyInvoiceWorkflowCaseCommand(thirdPartyInvoiceKey, loanNumber, thirdPartyTypeKey, receivedFrom);
            handler = new CreateThirdPartyInvoiceWorkflowCaseCommandHandler(serviceUrlConfiguration, client);

            fieldInputList.Add("ThirdPartyInvoiceKey", command.ThirdPartyInvoiceKey.ToString());
            fieldInputList.Add("AccountKey", command.AccountKey.ToString());

            missingNodeSupportingThirdPartInvoicesErrMessage = "No routes available for Third Party Invoices to service the request.";
            response = new X2Response(Guid.NewGuid(), missingNodeSupportingThirdPartInvoicesErrMessage, 0, true);

            client.WhenToldTo(x => x.PerformCommand(Param.IsAny<X2CreateInstanceRequest>())).Return<X2CreateInstanceRequest>(y =>
            {
                y.Result = response;
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}