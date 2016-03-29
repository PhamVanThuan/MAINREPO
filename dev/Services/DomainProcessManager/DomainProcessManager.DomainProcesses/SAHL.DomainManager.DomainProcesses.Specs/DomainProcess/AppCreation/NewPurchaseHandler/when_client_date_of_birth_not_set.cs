using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Linq;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Core.SystemMessages;
using System.Collections.Generic;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.NewPurchaseHandler
{
    public class when_client_date_of_birth_not_set : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationAddedEvent newPurchaseApplicationAddedEvent;
        private static ApplicantModel applicant;
        private static ClientDetailsQueryResult existingClient;
        private static int applicationNumber;
        private static int legalEntityKey;

        private Establish context = () =>
        {
            applicationNumber = 150;
            var newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, "8001045000007", "") as NewPurchaseApplicationCreationModel;
            applicant = newPurchaseCreationModel.Applicants.First();

            domainProcess.DataModel = newPurchaseCreationModel;
            domainProcess.ProcessState = applicationStateMachine;
            var mapper = new DomainModelMapper();
            mapper.CreateMap<NewPurchaseApplicationCreationModel, Services.Interfaces.ApplicationDomain.Models.NewPurchaseApplicationModel>();
            Services.Interfaces.ApplicationDomain.Models.NewPurchaseApplicationModel newPurchaseApplicationModel = mapper.Map(newPurchaseCreationModel);
            newPurchaseApplicationAddedEvent = new NewPurchaseApplicationAddedEvent(new DateTime(2014, 01, 01), applicationNumber,
                newPurchaseApplicationModel.ApplicationType, newPurchaseApplicationModel.ApplicationStatus, newPurchaseApplicationModel.ApplicationSourceKey,
                newPurchaseApplicationModel.OriginationSource, newPurchaseApplicationModel.Deposit, newPurchaseApplicationModel.PurchasePrice, newPurchaseApplicationModel.Term, newPurchaseApplicationModel.Product);

            legalEntityKey = 14;
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            clientDomainService.WhenToldTo(cds => cds.PerformQuery(Param.IsAny<ClientHasOpenAccountOrApplicationQuery>()))
                .Return<ClientHasOpenAccountOrApplicationQuery>(query =>
                {
                    query.Result = new ServiceQueryResult<ClientHasOpenAccountOrApplicationQueryResult>(
                        new ClientHasOpenAccountOrApplicationQueryResult[]
                            {
                                new ClientHasOpenAccountOrApplicationQueryResult { ClientIsAlreadyAnSAHLCustomer = true }
                            });

                    return SystemMessageCollection.Empty();
                });

            existingClient = new ClientDetailsQueryResult
            {
                LegalEntityKey = legalEntityKey,
                CitizenTypeKey = (int)applicant.CitizenshipType,
                DateOfBirth = null,
                FirstNames = applicant.FirstName,
                Surname = applicant.Surname,
                GenderKey = (int)applicant.Gender,
                IDNumber = applicant.IDNumber,
                MaritalStatusKey = (int)applicant.MaritalStatus,
                PassportNumber = applicant.PassportNumber,
                PopulationGroupKey = (int)applicant.PopulationGroup
            };
            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult>() { existingClient });
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

        private It should_check_if_the_applicant_has_an_open_account_or_offer = () =>
        {
            clientDomainService.WasToldTo(cds => cds.PerformQuery(Param<ClientHasOpenAccountOrApplicationQuery>
                .Matches(m =>
                   m.ClientKey == legalEntityKey
             )));
        };

        private It should_update_the_active_client_including_supplied_date_of_birth = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(
            Param<UpdateActiveNaturalPersonClientCommand>.Matches(m =>
                m.ClientKey == legalEntityKey &&
                m.ActiveNaturalPersonClient.Cellphone == applicant.CellPhone &&
                m.ActiveNaturalPersonClient.CorrespondenceLanguage == applicant.CorrespondenceLanguage &&
                m.ActiveNaturalPersonClient.Education == applicant.Education &&
                m.ActiveNaturalPersonClient.EmailAddress == applicant.EmailAddress &&
                m.ActiveNaturalPersonClient.FaxCode == applicant.FaxCode &&
                m.ActiveNaturalPersonClient.FaxNumber == applicant.FaxNumber &&
                m.ActiveNaturalPersonClient.HomeLanguage == applicant.HomeLanguage &&
                m.ActiveNaturalPersonClient.DateOfBirth == applicant.DateOfBirth &&
                m.ActiveNaturalPersonClient.WorkPhone == applicant.WorkPhone),
                
             Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_the_applicant_to_the_application = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
            Param<AddLeadApplicantToApplicationCommand>.Matches(m =>
                m.ApplicationNumber == applicationNumber &&
                m.ClientKey == legalEntityKey),
            Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}