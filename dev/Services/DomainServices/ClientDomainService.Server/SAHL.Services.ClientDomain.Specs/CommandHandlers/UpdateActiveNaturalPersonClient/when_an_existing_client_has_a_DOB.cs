using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.UpdateActiveNaturalPersonClient
{
    public class when_an_existing_client_has_a_DOB : WithCoreFakes
    {
        private static IDomainRuleManager<ActiveNaturalPersonClientModel> domainRuleManager;
        private static IClientDataManager clientDataManager;
        private static UpdateActiveNaturalPersonClientCommand command;
        private static UpdateActiveNaturalPersonClientCommandHandler handler;
        private static ActiveNaturalPersonClientModel activeNaturalPersonClientModel;
        private static LegalEntityDataModel existingActiveClient;
        private static int clientKey;
        private static DateTime? currentDOB = DateTime.Now.AddYears(-31);
        private static DateTime? newDateOfBirth = DateTime.Now.AddYears(-32);

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<ActiveNaturalPersonClientModel>>();
            clientDataManager = An<IClientDataManager>();
            clientKey = 99;
            activeNaturalPersonClientModel = new ActiveNaturalPersonClientModel(SalutationType.Mr, "Bob", Language.English, CorrespondenceLanguage.English, Education.Diploma, "031", "5647788",
                "031", "5713036", "0861", "9000000", "0825548000", "bob@employer.co.za", newDateOfBirth);
            existingActiveClient = new LegalEntityDataModel(1, 1, 1, 1, DateTime.Now, 1, "Clint", "C", "Speed", "Clint", "8211045229080", string.Empty, "0944500587", string.Empty, string.Empty,
                string.Empty, currentDOB, "031", "5647788", "031", "5713036", "0825548000", "clintonspeed@gmail.com", "0861", "900000", string.Empty, 1, 1, string.Empty,
                1, @"SAHL\HaloUser", DateTime.Now, 1, 1, 1, 1);
            clientDataManager.WhenToldTo(x => x.FindExistingClient(clientKey)).Return(existingActiveClient);
            command = new UpdateActiveNaturalPersonClientCommand(clientKey, activeNaturalPersonClientModel);
            handler = new UpdateActiveNaturalPersonClientCommandHandler(clientDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_update_the_date_of_birth_to_new_value_provided = () =>
        {
            clientDataManager.WasNotToldTo(x => x.UpdateLegalEntity(Arg.Is<LegalEntityDataModel>(y => y.DateOfBirth == activeNaturalPersonClientModel.DateOfBirth)));
        };
    }
}