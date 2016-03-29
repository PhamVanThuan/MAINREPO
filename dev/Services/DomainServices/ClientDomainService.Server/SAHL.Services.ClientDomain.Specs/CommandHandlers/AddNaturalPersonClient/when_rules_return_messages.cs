using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddNaturalPersonClient
{
    public class when_rules_return_messages : WithCoreFakes
    {
        private static IDomainRuleManager<INaturalPersonClientModel> domainRuleManager;
        private static AddNaturalPersonClientCommandHandler handler;
        private static AddNaturalPersonClientCommand command;
        private static NaturalPersonClientModel model;
        private static IClientDataManager clientDataManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
            {
                validationUtils = An<IValidationUtils>();
                messages = SystemMessageCollection.Empty();
                model = new NaturalPersonClientModel("8211045229080", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
                MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-25), Language.English,
                CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
                 string.Empty, "test@sahomeloans.com");
                command = new AddNaturalPersonClientCommand(model);
                domainRuleManager = An<IDomainRuleManager<INaturalPersonClientModel>>();
                domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(),
                    Param.IsAny<INaturalPersonClientModel>()))
                    .Callback<ISystemMessageCollection>(y => y.AddMessage(new SystemMessage("A rule failed.", SystemMessageSeverityEnum.Error)));
                clientDataManager = An<IClientDataManager>();
                handler = new AddNaturalPersonClientCommandHandler(clientDataManager, linkedKeyManager, eventRaiser, domainRuleManager, unitOfWorkFactory, validationUtils);
            };

        private Because of = () =>
            {
                messages = handler.HandleCommand(command, serviceRequestMetaData);
            };

        private It should_not_add_the_client = () =>
            {
                clientDataManager.WasNotToldTo(x => x.AddNewLegalEntity(Param.IsAny<LegalEntityDataModel>()));
            };

        private It should_return_the_messages = () =>
            {
                messages.ErrorMessages().First().Message.ShouldEqual("A rule failed.");
            };
    }
}