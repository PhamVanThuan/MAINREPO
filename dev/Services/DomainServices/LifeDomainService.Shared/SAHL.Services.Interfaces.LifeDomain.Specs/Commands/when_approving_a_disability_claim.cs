using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Commands
{
    public class when_approving_a_disability_claim : WithFakes
    {
        private static ApproveDisabilityClaimCommand command;
        private static int disabilityClaimKey;
        private static int loanNumber;
        private static DateTime paymentStartDate;
        private static int numberOfInstalmentsAuthorized;
        private static DateTime paymentEndDate;

        private Establish context = () =>
        {
            disabilityClaimKey = 1111;
            loanNumber = 2222;
            paymentStartDate = DateTime.Now.AddDays(8d);
            numberOfInstalmentsAuthorized = 10;
            paymentEndDate = DateTime.Now.AddDays(45d);            
        };

        private Because of = () =>
        {
            command = new ApproveDisabilityClaimCommand(disabilityClaimKey, loanNumber, paymentStartDate, numberOfInstalmentsAuthorized, paymentEndDate);
        };

        private It should_be_assignable_to_ILifeDomainCommand = () =>
        {
            command.ShouldBeAssignableTo(typeof(ILifeDomainCommand));
        };
    }
}