using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedUnemployedEmploymentToClient
{
    public class when_adding_unemployed_employment_to_a_client : WithCoreFakes
    {
        private static AddUnconfirmedUnemployedEmploymentToClientCommandHandler handler;
        private static AddUnconfirmedUnemployedEmploymentToClientCommand command;
        private static UnemployedEmploymentModel model;
        private static int clientKey;
        private static EmployerModel employer;
        private static IEmploymentDataManager employmentDataManager;
        private static IClientDataManager clientDataManager;
        private static IDomainRuleManager<UnemployedEmploymentModel> domainRuleManager;
        private static SystemMessage ruleMessage;
        private static int employmentKey;

        private Establish context = () =>
        {
            employer = new EmployerModel(1, "EmployerName", "031", "7658521", "Bob", "bob@employer.co.za", EmployerBusinessType.Unknown, EmploymentSector.Other);
            model = new UnemployedEmploymentModel(10000, 1, employer, DateTime.Now.AddDays(-50), UnemployedRemunerationType.InvestmentIncome, EmploymentStatus.Current);
            employmentKey = 9999;
            clientKey = 1234;
            messages = SystemMessageCollection.Empty();
            ruleMessage = new SystemMessage("Rules Failed.", SystemMessageSeverityEnum.Error);
            employmentDataManager = An<IEmploymentDataManager>();
            clientDataManager = An<IClientDataManager>();
            domainRuleManager = An<IDomainRuleManager<UnemployedEmploymentModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<UnemployedEmploymentModel>()))
                .Callback<ISystemMessageCollection>(y=>y.Clear());
            employmentDataManager.WhenToldTo(x=>x.SaveUnemployedEmployment(clientKey, model)).Return(employmentKey);
            command = new AddUnconfirmedUnemployedEmploymentToClientCommand(model, clientKey, OriginationSource.SAHomeLoans);
            handler = new AddUnconfirmedUnemployedEmploymentToClientCommandHandler(employmentDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_domain_rules_against_the_model = () =>
        {
            domainRuleManager.Received().ExecuteRules(Arg.Any<ISystemMessageCollection>(), model);
        };

        private It should_add_the_employment_record = () =>
        {
            employmentDataManager.Received().SaveUnemployedEmployment(clientKey, model);
        };

        private It should_complete_the_unit_of_work = () =>
        {
            unitOfWork.Received().Complete();
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.Received().RaiseEvent(Arg.Any<DateTime>(), 
                Arg.Is<UnconfirmedUnemployedEmploymentAddedToClientEvent>(y=>y.BasicIncome == model.BasicIncome 
                    && y.ClientKey == clientKey 
                    && y.EmployerName == employer.EmployerName
                    && y.EmploymentStatus == model.EmploymentStatus
                    && y.StartDate == model.StartDate
                    && y.SalaryPaymentDay == model.SalaryPaymentDay),
                employmentKey, (int)GenericKeyType.Employment, serviceRequestMetaData);
        };
    }
}