using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_a_natural_person_client : WithFakes
    {
        private static AddNaturalPersonClientCommand command;
        private static NaturalPersonClientModel model;
        private static string idNumber = "5555";

        private Establish context = () =>
        {
            model = new NaturalPersonClientModel(idNumber, "1", SalutationType.Mr, "1", "1","1", "1", Gender.Male, MaritalStatus.Single, 
                                                PopulationGroup.Unknown, CitizenType.SACitizen, DateTime.Now, Language.English, CorrespondenceLanguage.English, 
                                                Education.Unknown, "", "", "", "", "", "", "", "");
        };

        private Because of = () =>
        {
            command = new AddNaturalPersonClientCommand(model);
        };

        private It should_create_a_command_with_a_valid_client = () =>
        {
            command.NaturalPersonClient.ShouldNotBeNull();
        };

        private It should_create_a_command_using_the_correct_ID_number = () =>
        {
            command.NaturalPersonClient.IDNumber.ShouldEqual(idNumber);
        };
    }
}