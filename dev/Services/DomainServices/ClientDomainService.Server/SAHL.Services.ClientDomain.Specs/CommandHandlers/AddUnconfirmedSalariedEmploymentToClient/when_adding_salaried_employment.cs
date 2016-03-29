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
    public class when_adding_salaried_employment : WithCoreFakes
    {
        private static AddUnconfirmedSalariedEmploymentToClientCommand command;
        private static AddUnconfirmedSalariedEmploymentToClientCommandHandler handler;
        private static SalariedEmploymentModel salariedEmployment;
        private static IEmploymentDataManager employmentDataManager;
        private static IClientDataManager clientDataManager;
        private static int clientKey, employmentKey;
        private static OriginationSource originationSource;
        private static IDomainRuleManager<SalariedEmploymentModel> domainRuleContext;

        private Establish context = () =>
        {
            employmentDataManager = An<IEmploymentDataManager>();
            clientDataManager = An<IClientDataManager>();
            domainRuleContext = An<IDomainRuleManager<SalariedEmploymentModel>>();
            clientKey = 5;
            employmentKey = 1234;
            originationSource = OriginationSource.SAHomeLoans;
            salariedEmployment = new SalariedEmploymentModel
                (10000, 25, new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za"
               , EmployerBusinessType.Company, EmploymentSector.FinancialServices), DateTime.MinValue, SalariedRemunerationType.Salaried,
                 EmploymentStatus.Current, null);
            handler = new AddUnconfirmedSalariedEmploymentToClientCommandHandler
                (employmentDataManager, clientDataManager, domainRuleContext, unitOfWorkFactory, eventRaiser);
            command = new AddUnconfirmedSalariedEmploymentToClientCommand
                (salariedEmployment, clientKey, originationSource);

            var existingClient = new LegalEntityDataModel
                (clientKey, null, null, null, null, DateTime.Now, null, "Bob", "BB", "Builder", "", "1234567890123", "", "", ""
                 , "", "", null, "", "", "", "", "", "", "", "", "", null, null, "", null, "", null, null, null, 1, null);
            clientDataManager.WhenToldTo(x => x.FindExistingClient(clientKey)).Return(existingClient);
            employmentDataManager.WhenToldTo
                (x => x.SaveSalariedEmployment(Param.IsAny<int>(), Param.IsAny<SalariedEmploymentModel>
                    ())).Return(employmentKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_create_the_employment_record_for_the_client = () =>
        {
            employmentDataManager.WasToldTo
                (x => x.SaveSalariedEmployment(clientKey, Arg.Is<SalariedEmploymentModel>
                    (y => y.BasicIncome == salariedEmployment.BasicIncome)));
        };

        It should_raise_an_unconfirmed_salaried_employment_added_to_client_event = () =>
        {
            eventRaiser.WasToldTo
              (er => er.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<UnconfirmedSalariedEmploymentAddedToClientEvent>
                  (y => y.ClientKey == clientKey && y.BasicIncome == salariedEmployment.BasicIncome)
                    , employmentKey, (int)GenericKeyType.Employment, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}