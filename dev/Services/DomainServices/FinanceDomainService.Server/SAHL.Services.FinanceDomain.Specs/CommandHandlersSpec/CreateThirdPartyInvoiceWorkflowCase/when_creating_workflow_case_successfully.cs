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
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CreateThirdPartyInvoiceWorkflowCase
{
    public class when_creating_workflow_case_successfully : WithFakes
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

            response = new X2Response(Guid.NewGuid(), string.Empty, 12123, false);

            client.WhenToldTo(x => x.PerformCommand(Param.IsAny<X2CreateInstanceRequest>())).Return<X2CreateInstanceRequest>(y =>
            {
                y.Result = response;
                return new SystemMessageCollection();
            });

            client.WhenToldTo(x => x.PerformCommand(Param.IsAny<X2RequestForExistingInstance>())).Return<X2RequestForExistingInstance>(y =>
            {
                y.Result = response;
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_create_instance_with_complete_request = () =>
        {
            client.WasToldTo(x => x.PerformCommand(
                  Param<X2CreateInstanceRequest>.Matches(
                    m => m.MapVariables.Keys.Contains("ThirdPartyInvoiceKey")
                      && m.MapVariables["ThirdPartyInvoiceKey"].Equals(command.ThirdPartyInvoiceKey.ToString(), StringComparison.Ordinal)
                      && m.MapVariables.Keys.Contains("AccountKey")
                      && m.MapVariables["AccountKey"].Equals(command.AccountKey.ToString(), StringComparison.Ordinal)
                  )));
        };

        private It should_complete_the_request = () =>
        {
            client.WasToldTo(x => x.PerformCommand(
                  Param<X2RequestForExistingInstance>.Matches(
                    m => m.MapVariables.Keys.Contains("ThirdPartyInvoiceKey")
                      && m.MapVariables["ThirdPartyInvoiceKey"].Equals(command.ThirdPartyInvoiceKey.ToString(), StringComparison.Ordinal)
                      && m.MapVariables.Keys.Contains("AccountKey")
                      && m.MapVariables["AccountKey"].Equals(command.AccountKey.ToString(), StringComparison.Ordinal)
                      && m.InstanceId == response.InstanceId
                      && (string)m.Data == receivedFrom
                  )));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}