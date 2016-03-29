using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
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
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddUnconfirmedSalaryDeductionEmploymentToClient
{
    public class when_adding_salary_deduction_employment : WithCoreFakes
    {
        static AddUnconfirmedSalaryDeductionEmploymentToClientCommand command;
        static AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler handler;
        static SalaryDeductionEmploymentModel salaryDeductionEmploymentModel;
        static IClientDataManager clientDataManager;
        static IEmploymentDataManager employmentDataManager;
        static int clientKey, employmentKey;
        static OriginationSource originationSource;
        static IDomainRuleManager<SalaryDeductionEmploymentModel> domainRuleContext;

        Establish context = () =>
        {
            employmentDataManager = An<IEmploymentDataManager>();
            clientDataManager = An<IClientDataManager>();
            domainRuleContext = An<IDomainRuleManager<SalaryDeductionEmploymentModel>>();
            clientKey = 5;
            employmentKey = 1234;
            originationSource = OriginationSource.SAHomeLoans;
            salaryDeductionEmploymentModel = new SalaryDeductionEmploymentModel
                                            (10000, 0, 25, new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za"
                                           , EmployerBusinessType.Company, EmploymentSector.FinancialServices), DateTime.MinValue,
                                             SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
            handler = new AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler(
                            employmentDataManager, clientDataManager, domainRuleContext, unitOfWorkFactory, eventRaiser);
            command = new AddUnconfirmedSalaryDeductionEmploymentToClientCommand(salaryDeductionEmploymentModel, clientKey, originationSource);
            var existingClient = new LegalEntityDataModel(clientKey, null, null, null, null, DateTime.Now
                                                        , null, "Bob", "BB", "Builder", "", "1234567890123", "", "", ""
                                                        , "", "", null, "", "", "", "", "", "", "", "", "", null, null
                                                        , "", null, "", null, null, null, 1, null);
            clientDataManager.WhenToldTo
                (x => x.FindExistingClient(clientKey)).Return(existingClient);
                    employmentDataManager.WhenToldTo
                        (x => x.SaveSalaryDeductionEmployment
                            (Param.IsAny<int>(), Param.IsAny<SalaryDeductionEmploymentModel>())).Return(employmentKey);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_create_the_employment_record_for_the_client = () =>
        {
            employmentDataManager.WasToldTo(x => x.SaveSalaryDeductionEmployment(clientKey, Arg.Is<SalaryDeductionEmploymentModel>(
                y => y.BasicIncome == salaryDeductionEmploymentModel.BasicIncome)));
        };

        It should_raise_an_unconfirmed_salary_deduction_employment_added_to_client_event = () =>
        {
            eventRaiser.WasToldTo(er => er.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<UnconfirmedSalaryDeductionEmploymentAddedToClientEvent>(y =>
                y.ClientKey == clientKey && y.BasicIncome == salaryDeductionEmploymentModel.BasicIncome)
                , employmentKey, (int)GenericKeyType.Employment, Param.IsAny<IServiceRequestMetadata>()));
        };

        It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}