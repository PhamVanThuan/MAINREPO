using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AssetsEventHandler
{
    public class when_client_details_captured_and_has_unlisted_investment : WithNewPurchaseDomainProcess
    {
        private static ApplicantUnListedInvestmentAssetModel unlistedInvestment;

        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static FreeTextPostalAddressLinkedToClientEvent addressLinkedEvent;
        private static IEnumerable<ApplicantAssetLiabilityModel> applicantAssetLiabilities;
        private static Dictionary<string, int> clientCollection;
        private static int clientKey, applicationNumber;

        private Establish context = () =>
        {
            unlistedInvestment = new ApplicantUnListedInvestmentAssetModel(500000, "ACME co");
            applicantAssetLiabilities = new List<ApplicantAssetLiabilityModel> { unlistedInvestment };

            clientKey = 100;
            applicationNumber = 173;

            addressLinkedEvent = new FreeTextPostalAddressLinkedToClientEvent(new DateTime(2014, 1, 1),
               "12", "32", "Mayville", "Kwazulu Natal", "Durban", "4001", AddressFormat.FreeText, clientKey, 14);
            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            newPurchaseCreationModel.Applicants.First().ApplicantAssetLiabilities = applicantAssetLiabilities;
            domainProcess.Start(newPurchaseCreationModel, typeof(PostalAddressLinkedToClientEvent).Name);

            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(105);
            clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(addressLinkedEvent, serviceRequestMetadata);
        };

        private It should_add_the_listed_investment_to_client = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddInvestmentAssetToClientCommand>.Matches(m =>
                m.InvestmentAsset.AssetValue == unlistedInvestment.AssetValue &&
                m.InvestmentAsset.CompanyName == unlistedInvestment.CompanyName &&
                m.InvestmentAsset.InvestmentType == AssetInvestmentType.UnlistedInvestments
                ), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}