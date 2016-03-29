using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddNaturalPersonClient
{
    public class when_updating_an_active_natural_person_client : WithCoreFakes
    {
        private static IDomainRuleManager<ActiveNaturalPersonClientModel> domainRuleManager;
        private static IClientDataManager clientDataManager;
        private static UpdateActiveNaturalPersonClientCommand command;
        private static UpdateActiveNaturalPersonClientCommandHandler handler;
        private static ActiveNaturalPersonClientModel activeNaturalPersonClientModel;
        private static LegalEntityDataModel existingActiveClient;
        private static int clientKey;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<ActiveNaturalPersonClientModel>>();
            clientDataManager = An<IClientDataManager>();
            clientKey = 99;
            activeNaturalPersonClientModel = new ActiveNaturalPersonClientModel(SalutationType.Mr, "Bob", Language.English, CorrespondenceLanguage.English, Education.Diploma, "031", "5647788",
                "031", "5713036", "0861", "9000000", "0825548000", "bob@employer.co.za");
            existingActiveClient = new LegalEntityDataModel(1, 1, 1, 1, DateTime.Now, 1, "Clint", "C", "Speed", "Clint", "8211045229080", string.Empty, "0944500587", string.Empty, string.Empty,
                string.Empty, DateTime.Now.AddYears(-31), "031", "5647788", "031", "5713036", "0825548000", "clintonspeed@gmail.com", "0861", "900000", string.Empty, 1, 1, string.Empty,
                1, @"SAHL\HaloUser", DateTime.Now, 1, 1, 1, 1);
            clientDataManager.WhenToldTo(x => x.FindExistingClient(clientKey)).Return(existingActiveClient);
            command = new UpdateActiveNaturalPersonClientCommand(clientKey, activeNaturalPersonClientModel);
            handler = new UpdateActiveNaturalPersonClientCommandHandler(clientDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_run_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.ActiveNaturalPersonClient));
        };

        private It should_find_the_existing_client = () =>
        {
            clientDataManager.WasToldTo(x => x.FindExistingClient(clientKey));
        };

        private It should_update_the_details_allowed_for_an_active_client = () =>
        {
            clientDataManager.WasToldTo(x => x.UpdateLegalEntity(Arg.Is<LegalEntityDataModel>
                (c =>   c.Salutationkey == (int)command.ActiveNaturalPersonClient.Salutation &&
                        c.EducationKey == (int)command.ActiveNaturalPersonClient.Education &&
                        c.HomeLanguageKey == (int)command.ActiveNaturalPersonClient.HomeLanguage &&
                        c.DocumentLanguageKey == (int)command.ActiveNaturalPersonClient.CorrespondenceLanguage &&
                        c.HomePhoneCode == command.ActiveNaturalPersonClient.HomePhoneCode &&
                        c.HomePhoneNumber == command.ActiveNaturalPersonClient.HomePhone &&
                        c.WorkPhoneCode == command.ActiveNaturalPersonClient.WorkPhoneCode &&
                        c.WorkPhoneNumber == command.ActiveNaturalPersonClient.WorkPhone &&
                        c.FaxCode == command.ActiveNaturalPersonClient.FaxCode &&
                        c.FaxNumber == command.ActiveNaturalPersonClient.FaxNumber &&
                        c.CellPhoneNumber == command.ActiveNaturalPersonClient.Cellphone &&
                        c.EmailAddress == command.ActiveNaturalPersonClient.EmailAddress
                )));
        };

        private It should_keep_the_existing_client_details_that_cannot_be_updated_on_active_client = () =>
        {
            clientDataManager.WasToldTo(x => x.UpdateLegalEntity(Arg.Is<LegalEntityDataModel>
                (c => c.IDNumber == existingActiveClient.IDNumber &&
                    c.FirstNames == existingActiveClient.FirstNames &&
                    c.Surname == existingActiveClient.Surname &&
                    c.PassportNumber == existingActiveClient.PassportNumber &&
                    c.GenderKey == existingActiveClient.GenderKey &&
                    c.MaritalStatusKey == existingActiveClient.MaritalStatusKey &&
                    c.PopulationGroupKey == existingActiveClient.PopulationGroupKey &&
                    c.CitizenTypeKey == existingActiveClient.CitizenTypeKey &&
                    c.DateOfBirth == existingActiveClient.DateOfBirth &&
                    c.PreferredName == existingActiveClient.PreferredName
                )));
        };
        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_raise_an_ActiveNaturalPersonClientUpdatedEvent_event = () =>
        {
            eventRaiser.WasToldTo(er => er.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<ActiveNaturalPersonClientUpdatedEvent>(), clientKey, (int)GenericKeyType.LegalEntity, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}