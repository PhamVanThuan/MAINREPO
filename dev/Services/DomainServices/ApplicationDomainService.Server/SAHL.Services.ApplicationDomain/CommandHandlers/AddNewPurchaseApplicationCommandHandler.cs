using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddNewPurchaseApplicationCommandHandler : IDomainServiceCommandHandler<AddNewPurchaseApplicationCommand, NewPurchaseApplicationAddedEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IApplicationDataManager applicationDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;
        private IApplicationManager applicationManager;
        private IDomainRuleManager<NewPurchaseApplicationModel> domainRuleContext;

        public AddNewPurchaseApplicationCommandHandler(IServiceCommandRouter serviceCommandRouter, IApplicationDataManager applicationDataManager, ILinkedKeyManager linkedKeyManager,
            IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory, IApplicationManager applicationManager, IDomainRuleManager<NewPurchaseApplicationModel> domainRuleContext)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.applicationDataManager = applicationDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
            this.applicationManager = applicationManager;
            this.domainRuleContext = domainRuleContext;

            domainRuleContext.RegisterRule(new NewBusinessRequestedLoanAmountMustBeAboveMinimumRequiredRule(this.applicationDataManager));
        }

        public ISystemMessageCollection HandleCommand(AddNewPurchaseApplicationCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            domainRuleContext.ExecuteRules(messages, command.NewPurchaseApplication);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var uow = uowFactory.Build())
            {
                // reserve an accountkey
                int reservedAccountKey = applicationDataManager.GetReservedAccountNumber();

                // save the application
                int applicationNumber = applicationManager.SaveApplication(command.NewPurchaseApplication.ApplicationType,
                                                                        command.NewPurchaseApplication.ApplicationStatus,
                                                                        DateTime.Now, command.NewPurchaseApplication.ApplicationSourceKey,
                                                                        reservedAccountKey,
                                                                        SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans,
                                                                        command.NewPurchaseApplication.Reference,
                                                                        command.NewPurchaseApplication.ApplicantCount);

                applicationManager.SaveApplicationMortgageLoan(applicationNumber,
                                                               MortgageLoanPurpose.Newpurchase,
                                                               command.NewPurchaseApplication.ApplicantCount,
                                                               command.NewPurchaseApplication.PurchasePrice,
                                                               null, 
                                                               command.NewPurchaseApplication.TransferAttorney);

                int offerInformationKey = applicationManager.SaveApplicationInformation(DateTime.Now, applicationNumber, OfferInformationType.OriginalOffer, command.NewPurchaseApplication.Product);

                applicationManager.SaveNewPurchaseApplicationInformationVariableLoan(offerInformationKey, command.NewPurchaseApplication.Term, command.NewPurchaseApplication.Deposit, 
                    command.NewPurchaseApplication.PurchasePrice, command.NewPurchaseApplication.LoanAmountNoFees);

                applicationManager.SaveApplicationInformationInterestOnly(offerInformationKey);

                // set the external originator attribute for non sahl origination sources
                if (command.NewPurchaseApplication.OriginationSource != SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans)
                {
                    SetExternalOriginatorAttributeCommand setExternalOriginatorAttributeCommand = new SetExternalOriginatorAttributeCommand(applicationNumber, 
                        command.NewPurchaseApplication.OriginationSource);
                    messages = serviceCommandRouter.HandleCommand(setExternalOriginatorAttributeCommand, metadata);
                    if (messages.HasErrors)
                    {
                        uow.Complete();
                        return messages;
                    }
                }

                // we need to link the guid to the application
                linkedKeyManager.LinkKeyToGuid(applicationNumber, command.ApplicationId);

                // populate the event
                var date = DateTime.Now;
                eventRaiser.RaiseEvent(date, new NewPurchaseApplicationAddedEvent(date, applicationNumber, command.NewPurchaseApplication.ApplicationType,
                    command.NewPurchaseApplication.ApplicationStatus, command.NewPurchaseApplication.ApplicationSourceKey, command.NewPurchaseApplication.OriginationSource
                    , command.NewPurchaseApplication.Deposit, command.NewPurchaseApplication.PurchasePrice, command.NewPurchaseApplication.Term, command.NewPurchaseApplication.Product)
                    , applicationNumber, (int)GenericKeyType.Offer, metadata);

                uow.Complete();
            }

            return messages;
        }
    }
}