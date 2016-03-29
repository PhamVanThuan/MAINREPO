using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.Rules;
using System;

namespace SAHL.Services.LifeDomain.CommandHandlers
{
    public class ApproveDisabilityClaimCommandHandler : IServiceCommandHandler<ApproveDisabilityClaimCommand>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private IDomainRuleManager<IDisabilityClaimApproveModel> domainRuleManager;
        private IDomainQueryServiceClient domainQueryClient;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory uowFactory;

        public ApproveDisabilityClaimCommandHandler(ILifeDomainDataManager lifeDomainDataManager, IDomainQueryServiceClient domainQueryClient, 
            IDomainRuleManager<IDisabilityClaimApproveModel> domainRuleManager, IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.domainRuleManager = domainRuleManager;
            this.domainQueryClient = domainQueryClient;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;

            this.domainRuleManager.RegisterPartialRule<IDisabilityClaimApproveModel>(new DisabilityClaimAuthorisedInstalmentsMaximumRule(lifeDomainDataManager));
            this.domainRuleManager.RegisterPartialRule<IDisabilityClaimApproveModel>(new DisabilityClaimAuthorisedInstalmentsCannotExceedBondTermRule(lifeDomainDataManager, domainQueryClient));
        }

        public ISystemMessageCollection HandleCommand(ApproveDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            domainRuleManager.ExecuteRules(messages, command);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var uow = uowFactory.Build())
            {
                lifeDomainDataManager.UpdateDisabilityClaimStatus(command.DisabilityClaimKey, DisabilityClaimStatus.Approved);

                lifeDomainDataManager.UpdateDisabilityClaimPaymentDates(command.DisabilityClaimKey, command.PaymentStartDate, command.NumberOfInstalmentsAuthorised, command.PaymentEndDate);

                // call the halo api to produce the payment schedule                
                var createPaymentScheduleResult = lifeDomainDataManager.CreateDisabilityClaimPaymentSchedule(command.DisabilityClaimKey, metadata.UserName);

                if (!string.IsNullOrEmpty(createPaymentScheduleResult))
                {
                    messages.AddMessage(new SystemMessage(createPaymentScheduleResult, SystemMessageSeverityEnum.Error));
                }
                else
                {
                    eventRaiser.RaiseEvent(DateTime.Now, new DisabilityClaimApprovedEvent(DateTime.Now, command.PaymentStartDate, command.NumberOfInstalmentsAuthorised, command.PaymentEndDate), 
                        command.DisabilityClaimKey, (int)GenericKeyType.DisabilityClaim, metadata);
                    uow.Complete();
                }
            }
            return messages;
        }
    }
}