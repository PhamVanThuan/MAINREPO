using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
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
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedSalaryDeductionEmploymentToClient
{
    public class when_rule_fails_while_handling_command : WithCoreFakes
    {
        private static AddUnconfirmedSalaryDeductionEmploymentToClientCommand command;
        private static AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler handler;
        private static SalaryDeductionEmploymentModel salaryDeductionEmployment;
        private static IEmploymentDataManager employmentDataManager;
        private static IClientDataManager clientDataManager;
        private static int clientKey;
        private static OriginationSource originationSource;
        private static IDomainRuleManager<SalaryDeductionEmploymentModel> domainRuleContext;
        private static string errorMessage;

        private Establish context = () =>
        {
            employmentDataManager = An<IEmploymentDataManager>();
            clientDataManager = An<IClientDataManager>();
            domainRuleContext = An<IDomainRuleManager<SalaryDeductionEmploymentModel>>();
            clientKey = 5;
            errorMessage = "A rule failed";
            originationSource = OriginationSource.SAHomeLoans;
            salaryDeductionEmployment = new SalaryDeductionEmploymentModel
                                       (10000, 2500, 25, new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za",
                                        EmployerBusinessType.Company, EmploymentSector.FinancialServices), DateTime.MinValue
                                      , SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
            handler = new AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler
                         (employmentDataManager, clientDataManager, domainRuleContext, unitOfWorkFactory, eventRaiser);
            command = new AddUnconfirmedSalaryDeductionEmploymentToClientCommand
                         (salaryDeductionEmployment, clientKey, originationSource);

            domainRuleContext.WhenToldTo
                (x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.SalaryDeductionEmploymentModel))
                    .Callback<ISystemMessageCollection>(a => { a.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Error)); });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules_on_command = () =>
        {
            domainRuleContext.WasToldTo
                (x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.SalaryDeductionEmploymentModel));
        };

        private It should_return_the_error_messages_from_the_rules = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain(errorMessage);
        };

        private It should_not_check_for_existing_clients = () =>
        {
            clientDataManager.WasNotToldTo
                (x => x.FindExistingClient(Param.IsAny<int>()));
        };

        private It should_not_save_an_employment = () =>
        {
            employmentDataManager.WasNotToldTo
                (x => x.SaveSalaryDeductionEmployment(Param.IsAny<int>(), Param.IsAny<SalaryDeductionEmploymentModel>()));
        };

        It should_not_rais_an_unconfirmed_salary_deduction_employment_added_to_client_event = () =>
        {
            eventRaiser.WasNotToldTo(er => er.RaiseEvent(Param.IsAny<DateTime>(), 
                Param.IsAny<UnconfirmedSalaryDeductionEmploymentAddedToClientEvent>(),
                Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}