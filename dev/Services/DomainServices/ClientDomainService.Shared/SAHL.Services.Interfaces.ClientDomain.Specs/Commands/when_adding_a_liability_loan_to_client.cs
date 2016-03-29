using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_a_liability_loan_to_client : WithFakes
    {
        private static AddLiabilityLoanToClientCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddLiabilityLoanToClientCommand(1111, new LiabilityLoanModel(AssetLiabilitySubType.PersonalLoan, "Financial institute", DateTime.Now, 1d, 1d));
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}