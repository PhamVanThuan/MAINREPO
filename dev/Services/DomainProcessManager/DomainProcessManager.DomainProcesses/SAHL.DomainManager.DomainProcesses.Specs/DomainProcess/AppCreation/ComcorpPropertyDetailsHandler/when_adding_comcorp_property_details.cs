using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ComcorpPropertyDetailsHandler
{
    public class when_adding_comcorp_property_details : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static ComcorpApplicationPropertyDetailsModel comcorpPropertyDetails;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;

        private Establish context = () =>
        {
            applicationNumber = 12;

            int clientKey = 100;

            var address = ApplicationCreationTestHelper.PopulateFreeTextResidentialAddressModel();
            var postalAddress = ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel();

            comcorpPropertyDetails = ApplicationCreationTestHelper.PopulateComcorpPropertyDetailsModel();
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            var applicant = applicationCreationModel.Applicants.First();
            applicant.Addresses = new List<AddressModel> { address, postalAddress };

            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());

            var clientCollection = new Dictionary<string, int> { { applicant.IDNumber, clientKey } };

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
        };

        private Because of = () =>
        {
            domainProcess.AddComCorpPropertyDetails();
        };

        private It should_add_comcorp_property_details = () =>
        {
            propertyDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddComcorpOfferPropertyDetailsCommand>.Matches(m =>
                    m.ApplicationNumber == applicationNumber &&
                    m.ComcorpOfferPropertyDetails.StandErfNo == comcorpPropertyDetails.StandErfNo &&
                    m.ComcorpOfferPropertyDetails.ComplexName == comcorpPropertyDetails.ComplexName &&
                    m.ComcorpOfferPropertyDetails.PortionNo == comcorpPropertyDetails.PortionNo &&
                    m.ComcorpOfferPropertyDetails.StreetNo == comcorpPropertyDetails.StreetNo),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_not_return_error_messages = () =>
        {
            applicationStateMachine.SystemMessages.HasErrors.ShouldBeFalse();
        };
    }
}