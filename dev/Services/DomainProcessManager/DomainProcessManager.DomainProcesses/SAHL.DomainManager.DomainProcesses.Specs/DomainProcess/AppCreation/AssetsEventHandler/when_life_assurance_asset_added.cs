using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AssetsEventHandler
{
    public class when_life_assurance_asset_added : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static LifeAssuranceAssetAddedToClientEvent lifeAssuranceAssetAddedEvent;

        private Establish context = () =>
        {
            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);
            lifeAssuranceAssetAddedEvent = new LifeAssuranceAssetAddedToClientEvent(new DateTime(2014, 1, 1), "ACME life co.", 1000000);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(lifeAssuranceAssetAddedEvent, serviceRequestMetadata);
        };

        private It should_fire_a_life_asset_added_to_client_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, lifeAssuranceAssetAddedEvent.Id));
        };
    }
}