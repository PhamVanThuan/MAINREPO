using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.EmployerAddedHandler
{
    public class when_adding_salary_employment : WithNewPurchaseDomainProcess
    {
        private static EmployerAddedEvent employerAddedEvent;
        private static int applicationNumber;
        private static int clientKey;
        private static string clientIdNumber;
        private static int employerKey;
        private static string employerName;
        private static Services.Interfaces.ClientDomain.Models.EmployerModel employer;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static IDictionary<string, int> clientCollection;

        private Establish context = () =>
        {
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            clientIdNumber = applicationCreationModel.Applicants.First().IDNumber;
            clientKey = 9834;
            serviceRequestMetadata.Add("EmployeeIdNumber", clientIdNumber);

            applicationNumber = 150;
            domainProcess.ProcessState = applicationStateMachine;

            employerKey = 13;
            employerName = "Employer Name";
            employer = new Services.Interfaces.ClientDomain.Models.EmployerModel(null, employerName, "031", "11224455", "", "", EmployerBusinessType.CloseCorporation, EmploymentSector.Manufacturing);
            employerAddedEvent = new EmployerAddedEvent(new DateTime(2014, 10, 9), employerKey, employer.EmployerName, employer.TelephoneCode, employer.TelephoneNumber, employer.ContactPerson,
                employer.ContactEmail, employer.EmployerBusinessType, employer.EmploymentSector);
            applicationStateMachine.WhenToldTo(a => a.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(a => a.ClientEmploymentsFullyCaptured()).Return(true);

            clientCollection = MockRepository.GenerateMock<IDictionary<string, int>>();
            clientCollection.Expect(d => d[clientIdNumber]).Return(clientKey);
            applicationStateMachine.WhenToldTo(a => a.ClientCollection).Return(clientCollection);
            clientDataManager.WhenToldTo(x => x.GetEmployerKey(employerName)).Return(employerKey);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(employerAddedEvent, serviceRequestMetadata);
        };

        private It should_clean_up_the_linked_key_record = () =>
        {
            linkedKeyManager.WasToldTo(l => l.DeleteLinkedKey(serviceRequestMetadata.CommandCorrelationId));
        };

        private It should_find_the_correct_employee_for_added_employment = () =>
        {
            clientCollection.AssertWasCalled(d => d[clientIdNumber]);
        };

        private It should_add_correct_unconfirmed_salaried_employment = () =>
        {
            clientDomainService.WasToldTo(c => c.PerformCommand(
                  Param<AddUnconfirmedSalariedEmploymentToClientCommand>.Matches(e => e.ClientKey == clientKey
                      && e.SalariedEmploymentModel.Employer.EmployerName == employer.EmployerName
                      && e.SalariedEmploymentModel.Employer.EmployerKey == employerKey)
                , Param.IsAny<DomainProcessServiceRequestMetadata>()
                ));
        };
    }
}