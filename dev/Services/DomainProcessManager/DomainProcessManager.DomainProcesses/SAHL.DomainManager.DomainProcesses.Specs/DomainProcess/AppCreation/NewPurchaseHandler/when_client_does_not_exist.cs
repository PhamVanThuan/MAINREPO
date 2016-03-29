using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.NewPurchaseHandler
{
    public class when_client_does_not_exist : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationAddedEvent newPurchaseApplicationAddedEvent;
        private static ApplicantModel applicant;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationNumber = 150;
            var newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            applicant = newPurchaseCreationModel.Applicants.First();
            domainProcess.DataModel = newPurchaseCreationModel;
            domainProcess.ProcessState = applicationStateMachine;
            var mapper = new DomainModelMapper();
            mapper.CreateMap<NewPurchaseApplicationCreationModel, Services.Interfaces.ApplicationDomain.Models.NewPurchaseApplicationModel>();
            Services.Interfaces.ApplicationDomain.Models.NewPurchaseApplicationModel newPurchaseApplicationModel = mapper.Map(newPurchaseCreationModel);
            newPurchaseApplicationAddedEvent = new NewPurchaseApplicationAddedEvent(new DateTime(2014, 01, 01), applicationNumber,
                newPurchaseApplicationModel.ApplicationType, newPurchaseApplicationModel.ApplicationStatus, newPurchaseApplicationModel.ApplicationSourceKey,
                newPurchaseApplicationModel.OriginationSource, newPurchaseApplicationModel.Deposit, newPurchaseApplicationModel.PurchasePrice, newPurchaseApplicationModel.Term, newPurchaseApplicationModel.Product);

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult>() { null });
                return new SystemMessageCollection();
            });

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByPassportNumberQuery>())).Return<FindClientByPassportNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult> { null });
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newPurchaseApplicationAddedEvent, serviceRequestMetadata);
        };

        private It should_delete_the_linked_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(serviceRequestMetadata.CommandCorrelationId));
        };

        private It should_fire_the_basic_application_created_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.TriggerBasicApplicationCreated(newPurchaseApplicationAddedEvent.Id, applicationNumber));
        };

        private It should_add_the_applicant = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(
            Param<AddNaturalPersonClientCommand>.Matches(m =>
                m.NaturalPersonClient.Cellphone == applicant.CellPhone &&
                m.NaturalPersonClient.CitizenshipType == applicant.CitizenshipType &&
                m.NaturalPersonClient.CorrespondenceLanguage == applicant.CorrespondenceLanguage &&
                m.NaturalPersonClient.DateOfBirth == applicant.DateOfBirth &&
                m.NaturalPersonClient.Education == applicant.Education &&
                m.NaturalPersonClient.EmailAddress == applicant.EmailAddress &&
                m.NaturalPersonClient.FaxCode == applicant.FaxCode &&
                m.NaturalPersonClient.FaxNumber == applicant.FaxNumber &&
                m.NaturalPersonClient.FirstName == applicant.FirstName &&
                m.NaturalPersonClient.Gender == applicant.Gender &&
                m.NaturalPersonClient.HomeLanguage == applicant.HomeLanguage &&
                m.NaturalPersonClient.IDNumber == applicant.IDNumber &&
                m.NaturalPersonClient.Surname == applicant.Surname &&
                m.NaturalPersonClient.WorkPhone == applicant.WorkPhone),
            Param<DomainProcessServiceRequestMetadata>.Matches(m =>
                m.Contains(new KeyValuePair<string, string>("IdNumberOfAddedClient", applicant.IDNumber)))));
        };
    }
}