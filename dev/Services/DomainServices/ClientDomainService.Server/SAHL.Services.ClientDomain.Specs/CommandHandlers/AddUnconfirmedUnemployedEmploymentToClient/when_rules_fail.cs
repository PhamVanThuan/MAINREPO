using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedUnemployedEmploymentToClient
{
    public class when_rules_fail : WithCoreFakes
    {
        private static AddUnconfirmedUnemployedEmploymentToClientCommandHandler handler;
        private static AddUnconfirmedUnemployedEmploymentToClientCommand command;
        private static UnemployedEmploymentModel model;
        private static int clientKey;
        private static EmployerModel employer;
        private static IEmploymentDataManager employmentDataManager;
        private static IDomainRuleManager<UnemployedEmploymentModel> domainRuleManager;
        private static SystemMessage ruleMessage;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            ruleMessage = new SystemMessage("Rules Failed.", SystemMessageSeverityEnum.Error);
            employmentDataManager = An<IEmploymentDataManager>();
            domainRuleManager = An<IDomainRuleManager<UnemployedEmploymentModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<UnemployedEmploymentModel>()))
                .Callback<ISystemMessageCollection>(y => y.AddMessage(ruleMessage));
            employer = new EmployerModel(1, "EmployerName", "031", "7658521", "Bob", "bob@employer.co.za", EmployerBusinessType.Unknown, EmploymentSector.Other);
            clientKey = 1234;
            model = new UnemployedEmploymentModel(10000, 1, employer, DateTime.Now.AddDays(-50), UnemployedRemunerationType.InvestmentIncome, EmploymentStatus.Current);
            command = new AddUnconfirmedUnemployedEmploymentToClientCommand(model, clientKey, OriginationSource.SAHomeLoans);
            handler = new AddUnconfirmedUnemployedEmploymentToClientCommandHandler(employmentDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_the_messages_from_the_rules = () =>
        {
            messages.ErrorMessages().ShouldContain(ruleMessage);
        };

        private It should_not_add_the_employment_record = () =>
        {
            employmentDataManager.WasNotToldTo(x => x.SaveUnemployedEmployment(Param.IsAny<int>(), Param.IsAny<UnemployedEmploymentModel>()));
        };

        private It should_not_build_a_unit_of_work = () =>
        {
            unitOfWorkFactory.WasNotToldTo(x => x.Build());
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}