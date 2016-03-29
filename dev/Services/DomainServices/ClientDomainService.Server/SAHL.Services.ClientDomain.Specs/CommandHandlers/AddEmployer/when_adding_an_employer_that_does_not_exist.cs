using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
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
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddEmployer
{
    public class when_adding_an_employer_that_does_not_exist : WithCoreFakes
    {
        private static IEmploymentDataManager dataManager;
        private static IDomainRuleManager<EmployerModel> domainRuleContext;
        private static AddEmployerCommand command;
        private static AddEmployerCommandHandler handler;
        private static Guid employerId;
        private static EmployerModel employerModel;
        private static IEnumerable<EmployerDataModel> emptyCollection;
        private static int employerKey;

        private Establish context = () =>
        {
            employerKey = 1234;
            dataManager = An<IEmploymentDataManager>();
            emptyCollection = Enumerable.Empty<EmployerDataModel>();
            employerId = CombGuid.Instance.Generate();
            employerModel = new EmployerModel(null, "EmployerName", "031", "1234567", "Bob", "bob@employer.co.za", 
                                              Core.BusinessModel.Enums.EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            command = new AddEmployerCommand(employerId, employerModel);
            handler = new AddEmployerCommandHandler(dataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser);
            dataManager.WhenToldTo
                (x => x.FindExistingEmployer(Param.IsAny<EmployerModel>())).Return(emptyCollection);
            dataManager.WhenToldTo
                (x => x.SaveEmployer(Param.IsAny<EmployerModel>())).Return(employerKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_employer_exists = () =>
        {
            dataManager.WasToldTo(x => x.FindExistingEmployer(employerModel));
        };

        private It should_add_the_employer_to_the_database = () =>
        {
            dataManager.WasToldTo(x => x.SaveEmployer(employerModel));
        };

        private It should_link_the_employer_key_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(employerKey, command.EmployerId));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_provide_an_employer_added_event = () =>
        {
            eventRaiser.WasToldTo(er => er.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<EmployerAddedEvent>(y => y.EmployerKey == employerKey)
                , employerKey, (int)GenericKeyType.Employer, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}