using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddNaturalPersonClient
{
    public class when_adding_a_with_minimum_required_details : WithCoreFakes
    {
        private static IClientDataManager clientDataManager;
        private static AddNaturalPersonClientCommand command;
        private static AddNaturalPersonClientCommandHandler handler;
        private static LegalEntityDataModel legalEntityDataModel;
        private static int clientKey;
        private static string firstNames = "Bob";
        private static string surname = "Builder";
        private static string cellNumber = "0885555555";
        private static IDomainRuleManager<INaturalPersonClientModel> domainRuleManager;
        private static int educationKey;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            clientKey = 99;
            clientDataManager = An<IClientDataManager>();
            validationUtils = An<IValidationUtils>();
            domainRuleManager = An<IDomainRuleManager<INaturalPersonClientModel>>();
            NaturalPersonClientModel naturalPersonClientModel = new NaturalPersonClientModel
                                                               (null, null, null, firstNames, surname, null, null, null, null
                                                              , null, null, DateTime.MinValue, null, null, null, null, null, null
                                                              , null, null, null, cellNumber, null);
            educationKey = naturalPersonClientModel.Education.HasValue ? 
            (int)naturalPersonClientModel.Education.Value : (int)Education.Unknown;
            legalEntityDataModel = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, null, null,
                        null, DateTime.MinValue,
                        null, firstNames, null, surname, null, null, null, null, null, null, null, DateTime.MinValue, null, null,
                        null, null, cellNumber, null, null, null,
                        null, null, (int)LegalEntityStatus.Alive, null, null, null, null, educationKey, null, 
                       (int)CorrespondenceLanguage.English, null);

            command = new AddNaturalPersonClientCommand(naturalPersonClientModel);
            handler = new AddNaturalPersonClientCommandHandler
            (clientDataManager, linkedKeyManager, eventRaiser, domainRuleManager, unitOfWorkFactory, validationUtils);
            clientDataManager.WhenToldTo(x => x.AddNewLegalEntity(Param.IsAny<LegalEntityDataModel>())).Return(clientKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_the_new_legal_entity_to_the_database = () =>
        {
            clientDataManager.WasToldTo(x => x.AddNewLegalEntity(Arg.Is<LegalEntityDataModel>
                (c => c.IDNumber == null &&
                    c.FirstNames == firstNames &&
                    c.Surname == surname &&
                    c.PassportNumber == null &&
                    c.EmailAddress == null &&
                    c.Salutationkey == null &&
                    c.GenderKey == null &&
                    c.MaritalStatusKey == null &&
                    c.PopulationGroupKey == null &&
                    c.CitizenTypeKey == null &&
                    c.PreferredName == null &&
                    c.DateOfBirth == DateTime.MinValue &&
                    c.HomeLanguageKey == null &&
                    c.DocumentLanguageKey == (int)CorrespondenceLanguage.English &&
                    c.EducationKey == educationKey &&
                    c.HomePhoneCode == null &&
                    c.HomePhoneNumber == null &&
                    c.WorkPhoneCode == null &&
                    c.WorkPhoneNumber == null &&
                    c.FaxCode == null &&
                    c.FaxNumber == null &&
                    c.CellPhoneNumber == cellNumber
                )));
        };

        private It should_link_the_client_key_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(clientKey, command.Id));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_raise_a_NaturalPersonClientAddedEvent_event = () =>
        {
            eventRaiser.WasToldTo
             (er => er.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<NaturalPersonClientAddedEvent>()
              , clientKey, (int)GenericKeyType.LegalEntity, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}