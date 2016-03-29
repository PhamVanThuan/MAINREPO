using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_AddClientToApplicationCommand
    {
        private static AddLeadApplicantToApplicationCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), 1, 1, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
        };

        private It should_implement_the_open_application_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresOpenApplication));
        };

        private It should_implement_the_client_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}