using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddEmployer
{
    public class when_an_existing_employer_is_found : WithCoreFakes
    {
        private static IEmploymentDataManager dataManager;
        private static AddEmployerCommand command;
        private static AddEmployerCommandHandler handler;
        private static Guid employerId;
        private static EmployerModel employerModel;
        private static IEnumerable<EmployerDataModel> existingEmployer;
        private static int employerKey;

        Establish context = () =>
        {
            employerKey = 1234;
            dataManager = An<IEmploymentDataManager>();
            existingEmployer = new EmployerDataModel[] { new EmployerDataModel(1234, "Google", "", "", "", "", "", "", "", "", "", 1, "", null, 1) };
            employerId = CombGuid.Instance.Generate();
            employerModel = new EmployerModel
                (null, "Google", "031", "1234567", "Bob", "bob@employer.co.za", Core.BusinessModel.Enums.EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            command = new AddEmployerCommand(employerId, employerModel);
            handler = new AddEmployerCommandHandler(dataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser);
            dataManager.WhenToldTo(x => x.FindExistingEmployer(Param.IsAny<EmployerModel>())).Return(existingEmployer);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_check_if_the_employer_exists = () =>
        {
            dataManager.WasToldTo(x => x.FindExistingEmployer(employerModel));
        };

        It should_not_add_the_employer_to_the_database = () =>
        {
            dataManager.WasNotToldTo(x => x.SaveEmployer(employerModel));
        };

        It should_not_link_the_employer_key_to_the_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(employerKey, command.EmployerId));
        };

        It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Google is an existing employer and could not be added.");
        };

        It should_not_raise_an_employerAdded_event = () =>
        {
            eventRaiser.WasNotToldTo(er => er.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

    }
}