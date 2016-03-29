using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_updating_an_inactive_natural_person_client : WithFakes
    {
        private static UpdateInactiveNaturalPersonClientCommand command;
        private static NaturalPersonClientModel model;
        private static int clientKey = 99;
        private static string idNumber = "5555";

        private Establish context = () =>
        {
            model = new NaturalPersonClientModel(idNumber, "1", SalutationType.Mr, "1", "1","1", "1", Gender.Male, MaritalStatus.Single, 
                                                 PopulationGroup.Unknown, CitizenType.SACitizen, DateTime.Now, Language.English, 
                                                 CorrespondenceLanguage.English, Education.Unknown, "", "", "", "", "", "", "", "");
        };

        private Because of = () =>
        {
            command = new UpdateInactiveNaturalPersonClientCommand(clientKey, model);
        };

        private It should_create_an_existing_command = () =>
        {
            command.NaturalPersonClient.ShouldNotBeNull();
        };

        private It should_create_a_command_using_an_exisitng_client = () =>
        {
            command.NaturalPersonClient.IDNumber.Equals(idNumber);
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}