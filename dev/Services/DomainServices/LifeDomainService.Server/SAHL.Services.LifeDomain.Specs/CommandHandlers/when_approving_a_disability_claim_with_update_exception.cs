using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Managers.Models;
using System;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec
{
    public class when_approving_a_disability_claim_with_update_exception : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainQueryServiceClient domainQueryClient;
        private static IDomainRuleManager<DisabilityClaimModel> domainRuleManager;
        private static ApproveDisabilityClaimCommand command;
        private static ApproveDisabilityClaimCommandHandler handler;
        private static DisabilityClaimModel disabilityClaimModel;
        private static ApproveDisabilityClaimResultModel approveDisabilityClaimResultModel;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            serviceCommandRouter = An<IServiceCommandRouter>();
            domainQueryClient = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<DisabilityClaimModel>>();
            eventRaiser = An<IEventRaiser>();
            disabilityClaimModel = new DisabilityClaimModel(0, 0, 0, DateTime.Now.AddDays(-5), DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(-1), "Professional slacker",
                (int)DisabilityType.Other, "Slacker syndrome", null, (int)DisabilityClaimStatus.Repudiated, null, null, null);

            approveDisabilityClaimResultModel = new ApproveDisabilityClaimResultModel();

            command = new ApproveDisabilityClaimCommand(disabilityClaimModel);
            handler = new ApproveDisabilityClaimCommandHandler(serviceCommandRouter, lifeDomainDataManager, domainQueryClient, domainRuleManager, eventRaiser);

            ISystemMessageCollection serviceMessageCollection = SystemMessageCollection.Empty();
            serviceMessageCollection.AddMessage(new SystemMessage("Error updating Disability Claim Status", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand<UpdateDisabilityClaimCommand>(Param.IsAny<UpdateDisabilityClaimCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(serviceMessageCollection);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_error_messages = () =>
        {
            messages.AllMessages.ShouldNotBeEmpty();
        };

        private It should_update_the_status_of_the_disability_claim_to_approved = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.UpdateDisabilityClaimStatus(Param.IsAny<int>(), DisabilityClaimStatus.Approved));
        };

        private It should_not_call_the_halo_api_to_produce_the_payment_schedule = () =>
        {
            lifeDomainDataManager.WasNotToldTo(x => x.ApproveDisabilityClaim(Param.IsAny<DisabilityClaimModel>(), Param.IsAny<string>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DisabilityClaimApprovedEvent>(),
                     0, (int)GenericKeyType.DisabilityClaim, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}