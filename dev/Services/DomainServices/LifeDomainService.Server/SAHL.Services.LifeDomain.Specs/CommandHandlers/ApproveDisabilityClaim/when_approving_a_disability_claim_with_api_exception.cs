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
using System;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlersSpec.ApproveDisabilityClaim
{
    public class when_approving_a_disability_claim_with_api_exception : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IDomainQueryServiceClient domainQueryClient;
        private static IDomainRuleManager<IDisabilityClaimApproveModel> domainRuleManager;
        private static ApproveDisabilityClaimCommand command;
        private static ApproveDisabilityClaimCommandHandler handler;
        private static int disabilityClaimKey;
        private static DateTime paymentStartDate;
        private static DateTime paymentEndDate;
        private static int numberOfInstalmentsAuthorised;
        private static int loanNumber;
        private static string adUserName;

        private Establish context = () =>
        {
            disabilityClaimKey = 1;
            loanNumber = 2;
            paymentStartDate = DateTime.Now;
            numberOfInstalmentsAuthorised = 6;
            paymentEndDate = paymentStartDate.AddMonths(numberOfInstalmentsAuthorised);
            adUserName = "";

            lifeDomainDataManager = An<ILifeDomainDataManager>();
            domainQueryClient = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<IDisabilityClaimApproveModel>>();
            eventRaiser = An<IEventRaiser>();
  
            command = new ApproveDisabilityClaimCommand(disabilityClaimKey, loanNumber, paymentStartDate, numberOfInstalmentsAuthorised, paymentEndDate);
            handler = new ApproveDisabilityClaimCommandHandler(lifeDomainDataManager, domainQueryClient, domainRuleManager, eventRaiser, unitOfWorkFactory);

            lifeDomainDataManager.WhenToldTo(x => x.CreateDisabilityClaimPaymentSchedule(disabilityClaimKey, adUserName)).Return("API Call Failed");
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_update_the_status_of_the_disability_claim_to_approved = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.UpdateDisabilityClaimStatus(Param.IsAny<int>(), DisabilityClaimStatus.Approved));
        };

        private It should_update_the_disability_claim_payment_dates = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.UpdateDisabilityClaimPaymentDates(disabilityClaimKey, paymentStartDate, numberOfInstalmentsAuthorised, paymentEndDate));
        };

        private It should_call_the_halo_api_to_produce_the_payment_schedule = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.CreateDisabilityClaimPaymentSchedule(disabilityClaimKey, adUserName));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<DisabilityClaimApprovedEvent>(),
                     disabilityClaimKey, (int)GenericKeyType.DisabilityClaim, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_error_messages = () =>
        {
            messages.AllMessages.ShouldNotBeEmpty();
        };
    }
}