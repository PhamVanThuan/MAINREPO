using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules.Models;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddNaturalPersonClient
{
    public class when_updating_an_inactive_natural_person_client : WithCoreFakes
    {
        private static IClientDataManager clientDataManager;
        private static UpdateInactiveNaturalPersonClientCommand command;
        private static UpdateInactiveNaturalPersonClientCommandHandler handler;
        private static NaturalPersonClientModel naturalPersonClientModel;
        private static LegalEntityDataModel legalEntityDataModel;
        private static int clientKey;
        private static string idNumber = "1234567890123";
        private static string passportNumber = "pass56";
        private static string firstNames = "Bob";
        private static string surname = "Builder";
        private static string emailAddress = "bob@employer.co.za";
        private static SalutationType salutation = SalutationType.Mr;
        private static string preferredName = "BB";
        private static Gender gender = Gender.Male;
        private static MaritalStatus maritalStatus = MaritalStatus.Single;
        private static PopulationGroup populationGroup = PopulationGroup.Unknown;
        private static CitizenType citizenType = CitizenType.SACitizen;
        private static DateTime dateOfBirth = DateTime.Now.AddYears(-30);
        private static Language language = Language.English;
        private static CorrespondenceLanguage correspondenceLanguage = CorrespondenceLanguage.English;
        private static Education education = Education.Unknown;
        private static string homePhoneCode = "031";
        private static string homePhoneNumber = "5555555";
        private static string workPhoneCode = "021";
        private static string workPhoneNumber = "4444444";
        private static string faxPhoneCode = "011";
        private static string faxPhoneNumber = "3333333";
        private static string cellNumber = "0885555555";
        private static IDomainRuleManager<NaturalPersonClientRuleModel> domainRuleManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<NaturalPersonClientRuleModel>>();
            clientDataManager = An<IClientDataManager>();
            validationUtils = An<IValidationUtils>();
            clientKey = 99;
            naturalPersonClientModel = new NaturalPersonClientModel
                                      (idNumber, passportNumber, salutation, firstNames, surname, null, preferredName
                                     , gender, maritalStatus, populationGroup, citizenType, dateOfBirth, language
                                     , correspondenceLanguage, education, homePhoneCode, homePhoneNumber, workPhoneCode
                                     , workPhoneNumber, faxPhoneCode, faxPhoneNumber, cellNumber, emailAddress);
            legalEntityDataModel = new LegalEntityDataModel
                                  (-1, -1, -1, -1, DateTime.Now, -1, "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", 
                                  DateTime.Now, "s", "s", "s", "s", "s", "s", "s", "s", "s", -1, -1, "s", -1, "s", 
                                  DateTime.Now, -1, -1, -1, -1);
            command = new UpdateInactiveNaturalPersonClientCommand(clientKey, naturalPersonClientModel);
            handler = new UpdateInactiveNaturalPersonClientCommandHandler(clientDataManager, eventRaiser, domainRuleManager, validationUtils);
            clientDataManager.WhenToldTo(x => x.FindExistingClient(clientKey)).Return(legalEntityDataModel);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_run_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(),
                Arg.Is<NaturalPersonClientRuleModel>(c =>
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
                    c.Education == command.NaturalPersonClient.Education)));
        };

        private It should_find_the_existing_client = () =>
        {
            clientDataManager.WasToldTo(x => x.FindExistingClient(clientKey));
        };

        private It should_add_the_new_legal_entity_to_the_database = () =>
        {
            clientDataManager.WasToldTo(x => x.UpdateLegalEntity(Arg.Is<LegalEntityDataModel>
                (c => c.IDNumber == idNumber &&
                    c.FirstNames == firstNames &&
                    c.Surname == surname &&
                    c.PassportNumber == passportNumber &&
                    c.EmailAddress == emailAddress &&
                    c.Salutationkey == (int)salutation &&
                    c.GenderKey == (int)gender &&
                    c.MaritalStatusKey == (int)maritalStatus &&
                    c.PopulationGroupKey == (int)populationGroup &&
                    c.CitizenTypeKey == (int)citizenType &&
                    c.PreferredName == preferredName &&
                    c.DateOfBirth == dateOfBirth &&
                    c.HomeLanguageKey == (int)language &&
                    c.DocumentLanguageKey == (int)correspondenceLanguage &&
                    c.EducationKey == (int)education &&
                    c.HomePhoneCode == homePhoneCode &&
                    c.HomePhoneNumber == homePhoneNumber &&
                    c.WorkPhoneCode == workPhoneCode &&
                    c.WorkPhoneNumber == workPhoneNumber &&
                    c.FaxCode == faxPhoneCode &&
                    c.FaxNumber == faxPhoneNumber &&
                    c.CellPhoneNumber == cellNumber
                )));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_raise_an_InactiveNaturalPersonClientUpdatedEvent_event = () =>
        {
            eventRaiser.WasToldTo
             (er => er.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<InactiveNaturalPersonClientUpdatedEvent>()
             , clientKey, (int)GenericKeyType.LegalEntity, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}