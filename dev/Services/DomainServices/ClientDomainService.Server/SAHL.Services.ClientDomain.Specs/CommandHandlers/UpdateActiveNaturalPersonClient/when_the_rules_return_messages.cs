using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.UpdateActiveNaturalPersonClient
{
    public class when_the_rules_return_messages : WithCoreFakes
    {
        private static IClientDataManager clientDataManager;
        private static UpdateActiveNaturalPersonClientCommand command;
        private static UpdateActiveNaturalPersonClientCommandHandler handler;
        private static ActiveNaturalPersonClientModel activeNaturalPersonModel;
        private static int clientKey;
        private static IDomainRuleManager<ActiveNaturalPersonClientModel> domainRuleManager;

        private Establish context = () =>
            {
                messages = SystemMessageCollection.Empty();
                clientDataManager = An<IClientDataManager>();
                domainRuleManager = An<IDomainRuleManager<ActiveNaturalPersonClientModel>>();
                clientKey = 99;
                activeNaturalPersonModel = new ActiveNaturalPersonClientModel
                    (SalutationType.Mr, "Bob", Language.Afrikaans, CorrespondenceLanguage.English
                   , Education.Diploma, "031", "7657192", "021", "2211221", "0860", "1122334", "0827702444", "test@sahomeloans.com");
                command = new UpdateActiveNaturalPersonClientCommand(clientKey, activeNaturalPersonModel);
                handler = new UpdateActiveNaturalPersonClientCommandHandler(clientDataManager, eventRaiser, domainRuleManager);
                domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.ActiveNaturalPersonClient))
                 .Callback<ISystemMessageCollection>
                   (y => y.AddMessage(new SystemMessage("A rule failed.", SystemMessageSeverityEnum.Error)));
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