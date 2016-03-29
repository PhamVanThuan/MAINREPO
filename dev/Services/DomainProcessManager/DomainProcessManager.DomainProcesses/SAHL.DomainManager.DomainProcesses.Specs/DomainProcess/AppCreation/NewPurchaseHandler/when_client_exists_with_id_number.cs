using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.NewPurchaseHandler
{
    public class when_client_exists_with_id_number : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationAddedEvent newPurchaseApplicationAddedEvent;
        private static ApplicantModel applicant;
        private static ClientDetailsQueryResult existingClient;
        private static int applicationNumber;
        private static int legalEntityKey;

        private Establish context = () =>
        {
            applicationNumber = 150;
            var newPurchaseCreationModel =
                ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, "8001045000007", "") as
                    NewPurchaseApplicationCreationModel;
            applicant = newPurchaseCreationModel.Applicants.First();

            domainProcess.DataModel = newPurchaseCreationModel;
            domainProcess.ProcessState = applicationStateMachine;
            var mapper = new DomainModelMapper();
            mapper.CreateMap<NewPurchaseApplicationCreationModel, Services.Interfaces.ApplicationDomain.Models.NewPurchaseApplicationModel>();
            Services.Interfaces.ApplicationDomain.Models.NewPurchaseApplicationModel newPurchaseApplicationModel = mapper.Map(newPurchaseCreationModel);
            newPurchaseApplicationAddedEvent = new NewPurchaseApplicationAddedEvent(new DateTime(2014, 01, 01),
                applicationNumber,
                newPurchaseApplicationModel.ApplicationType,
                newPurchaseApplicationModel.ApplicationStatus,
                newPurchaseApplicationModel.ApplicationSourceKey,
                newPurchaseApplicationModel.OriginationSource,
                newPurchaseApplicationModel.Deposit,
                newPurchaseApplicationModel.PurchasePrice,
                newPurchaseApplicationModel.Term,
                newPurchaseApplicationModel.Product);

            legalEntityKey = 14;
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            clientDomainService.WhenToldTo(cds => cds.PerformQuery(Param.IsAny<ClientHasOpenAccountOrApplicationQuery>()))
                .Return<ClientHasOpenAccountOrApplicationQuery>(query =>
                {
                    query.Result = new ServiceQueryResult<ClientHasOpenAccountOrApplicationQueryResult>(
                        new[]
                        {
                            new ClientHasOpenAccountOrApplicationQueryResult { ClientIsAlreadyAnSAHLCustomer = false }
                        });

                    return SystemMessageCollection.Empty();
                });

            existingClient = new ClientDetailsQueryResult
            {
                LegalEntityKey = legalEntityKey,
                CitizenTypeKey = (int)applicant.CitizenshipType,
                DateOfBirth = new DateTime(2010, 1, 1),
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

        private It should_update_the_inactive_client = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<UpdateInactiveNaturalPersonClientCommand>.Matches(m =>
                    m.ClientKey == legalEntityKey &&
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
