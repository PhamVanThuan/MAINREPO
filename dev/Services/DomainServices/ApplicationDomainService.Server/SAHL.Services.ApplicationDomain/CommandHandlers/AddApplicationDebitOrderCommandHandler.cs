using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddApplicationDebitOrderCommandHandler : IDomainServiceCommandHandler<AddApplicationDebitOrderCommand, DebitOrderAddedToApplicationEvent>
    {
        private IApplicationDataManager applicationDataManager;
        private IApplicantDataManager applicantDataManager;
        private IDomainRuleManager<ApplicationDebitOrderModel> domainRuleContext;
        private IDomainQueryServiceClient domainQueryService;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory uowFactory;


        public AddApplicationDebitOrderCommandHandler(IApplicationDataManager applicationDataManager, IDomainRuleManager<ApplicationDebitOrderModel> domainRuleContext, 
            IDomainQueryServiceClient domainQueryService, IEventRaiser eventRaiser, IApplicantDataManager applicantDataManager, IUnitOfWorkFactory uowFactory)
        {
            this.applicationDataManager = applicationDataManager;
            this.applicantDataManager = applicantDataManager;
            this.domainQueryService = domainQueryService;
            this.domainRuleContext = domainRuleContext;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;

            this.domainRuleContext.RegisterRule(new ClientBankAccountMustBelongToApplicantOnApplicationRule(applicantDataManager));
            this.domainRuleContext.RegisterRule(new DebitOrderDayCannotBeAfterThe28thDayRule());
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddApplicationDebitOrderCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                IEnumerable<OfferDebitOrderDataModel> applicationDebitOrder = applicationDataManager.GetApplicationDebitOrder(command.ApplicationDebitOrderModel.ApplicationNumber);
                if (applicationDebitOrder.Any())
                {
                    messages.AddMessage(new SystemMessage("An existing application debit order is in place.", SystemMessageSeverityEnum.Error));
                }

                var getClientBankAccountQuery = new GetClientBankAccountQuery(command.ApplicationDebitOrderModel.ClientBankAccountKey);
                domainQueryService.PerformQuery(getClientBankAccountQuery);
                if (!getClientBankAccountQuery.Result.Results.Any() || getClientBankAccountQuery.Result.Results.First().BankAccountKey == 0)
                {
                    messages.AddMessage(new SystemMessage("The client bank account provided does not exist.", SystemMessageSeverityEnum.Error));
                }

                domainRuleContext.ExecuteRules(messages, command.ApplicationDebitOrderModel);

                if (!messages.HasErrors)
                {
                    var OfferDebitOrderKey = applicationDataManager.SaveApplicationDebitOrder(command.ApplicationDebitOrderModel, getClientBankAccountQuery.Result.Results.First().BankAccountKey);

                    eventRaiser.RaiseEvent(DateTime.Now,
                        new DebitOrderAddedToApplicationEvent(DateTime.Now, command.ApplicationDebitOrderModel.PaymentType, command.ApplicationDebitOrderModel.DebitOrderDay, 
                            command.ApplicationDebitOrderModel.ClientBankAccountKey, command.ApplicationDebitOrderModel.ApplicationNumber),
                        OfferDebitOrderKey, (int)GenericKeyType.OfferDebitOrder, metadata);
                }

                uow.Complete();
            }

            return messages;
        }
    }
}