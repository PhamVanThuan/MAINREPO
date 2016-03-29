using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules.Models;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.UpdateInactiveNaturalPersonClient
{
    public class when_the_rules_return_messages : WithCoreFakes
    {
        private static IClientDataManager clientDataManager;
        private static UpdateInactiveNaturalPersonClientCommand command;
        private static UpdateInactiveNaturalPersonClientCommandHandler handler;
        private static NaturalPersonClientModel model;
        private static int clientKey;
        private static IDomainRuleManager<NaturalPersonClientRuleModel> domainRuleManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<NaturalPersonClientRuleModel>>();
            clientDataManager = An<IClientDataManager>();
            validationUtils = An<IValidationUtils>();
            clientKey = 99;
            model = new NaturalPersonClientModel
                        ("8211045229080", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", 
                        Gender.Male, MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African,
                        CitizenType.SACitizen, DateTime.Now.AddYears(-25), Language.English, 
                        CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
                        string.Empty, "test@sahomeloans.com");
            command = new UpdateInactiveNaturalPersonClientCommand(clientKey, model);
            handler = new UpdateInactiveNaturalPersonClientCommandHandler(clientDataManager, eventRaiser, domainRuleManager, validationUtils);
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Arg.Is<NaturalPersonClientRuleModel>(
                c => c.ClientKey == command.ClientKey &&
                    c.IDNumber == command.NaturalPersonClient.IDNumber &&
                    c.PassportNumber == command.NaturalPersonClient.PassportNumber &&
                    c.Salutation == command.NaturalPersonClient.Salutation &&
                    c.FirstName == command.NaturalPersonClient.FirstName &&
                    c.Surname == command.NaturalPersonClient.Surname &&
                    c.PreferredName == command.NaturalPersonClient.PreferredName &&
                    c.Gender == command.NaturalPersonClient.Gender &&
                    c.MaritalStatus == command.NaturalPersonClient.MaritalStatus &&
                    c.PopulationGroup == command.NaturalPersonClient.PopulationGroup &&
                    c.CitizenshipType == command.NaturalPersonClient.CitizenshipType &&
                    c.DateOfBirth == command.NaturalPersonClient.DateOfBirth &&
                    c.HomeLanguage == command.NaturalPersonClient.HomeLanguage &&
                    c.CorrespondenceLanguage == command.NaturalPersonClient.CorrespondenceLanguage &&
                    c.Education == command.NaturalPersonClient.Education
                )))
                .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("A rule failed.", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_the_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("A rule failed.");
        };

        private It should_not_find_the_existing_client = () =>
        {
            clientDataManager.WasNotToldTo
             (x => x.FindExistingClient(Param.IsAny<int>()));
        };

        private It should_not_update_the_existing_client = () =>
        {
            clientDataManager.WasNotToldTo
             (x => x.UpdateLegalEntity(Param.IsAny<LegalEntityDataModel>()));
        };
    }
}