using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;

namespace SAHL.Services.Interfaces.FinanceDomain.Specs.Commands.Internal
{
    public class when_performing_the_create_workflow_case_command : WithFakes
    {
        private static CreateThirdPartyInvoiceWorkflowCaseCommand command;
        private static int thirdPartyInvoiceKey;
        private static int accountKey;
        private static string receivedFrom;

        private Establish context = () =>
        {
            thirdPartyInvoiceKey = 1;
            accountKey = 1234567;
            receivedFrom = "accounts@straussdaly.com";
        };

        private Because of = () =>
        {
            command = new CreateThirdPartyInvoiceWorkflowCaseCommand(thirdPartyInvoiceKey, accountKey, (int)ThirdPartyType.Attorney, receivedFrom);
        };

        private It should_require_a_valid_thirdPartyInvoiceKey = () =>
        {
            command.ShouldBeAssignableTo<IRequiresThirdPartyInvoice>();
        };

        private It should_require_a_valid_accountKey = () =>
        {
            command.ShouldBeAssignableTo<IRequiresAccount>();
        };
    }
}
