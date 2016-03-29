using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_updating_an_active_natural_person_client : WithFakes
    {
        private static UpdateActiveNaturalPersonClientCommand command;
        private static ActiveNaturalPersonClientModel model;
        private static int clientKey = 99;

        private Establish context = () =>
        {
            model = new ActiveNaturalPersonClientModel(SalutationType.Mr, "1", Language.English, CorrespondenceLanguage.English, Education.Unknown, "", "", "", "", "", "", "", "");
        };

        private Because of = () =>
        {
            command = new UpdateActiveNaturalPersonClientCommand(clientKey, model);
        };

        private It should_create_a_valid_command = () =>
        {
            command.ActiveNaturalPersonClient.ShouldNotBeNull();
            command.ActiveNaturalPersonClient.Salutation.Equals(SalutationType.Mr);
            command.ActiveNaturalPersonClient.PreferredName.Equals("1");
            command.ActiveNaturalPersonClient.HomeLanguage.Equals(Language.English);
            command.ActiveNaturalPersonClient.CorrespondenceLanguage.Equals(CorrespondenceLanguage.English);
            command.ActiveNaturalPersonClient.Education.Equals(Education.Unknown);
        };

        private It should_check_client_exists = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}