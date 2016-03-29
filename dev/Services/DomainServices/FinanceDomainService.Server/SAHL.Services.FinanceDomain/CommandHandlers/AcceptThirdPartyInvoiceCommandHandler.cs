using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.CommandHandlers
{
    public class AcceptThirdPartyInvoiceCommandHandler
        : IDomainServiceCommandHandler<AcceptThirdPartyInvoiceCommand, ThirdPartyInvoiceSubmissionAcceptedEvent>
    {
        private IUnitOfWorkFactory UnitOfWorkFactory;
        private IDomainRuleManager<IAccountRuleModel> DomainRuleManager;
        private IDomainQueryServiceClient DomainQueryServiceClient;
        private IServiceCommandRouter ServiceCommandRouter;
        private ILinkedKeyManager LinkedKeyManager;
        private IEventRaiser EventRaiser;
        private IServiceCoordinator ServiceCoordinator;
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public AcceptThirdPartyInvoiceCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IDomainRuleManager<IAccountRuleModel> domainRuleManager,
            IDomainQueryServiceClient domainQueryServiceClient, IServiceCommandRouter serviceCommandRouter, ILinkedKeyManager linkedKeyManager,
            IEventRaiser eventRaiser, IServiceCoordinator serviceCoordinator, IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.UnitOfWorkFactory = unitOfWorkFactory;
            this.DomainRuleManager = domainRuleManager;
            this.DomainQueryServiceClient = domainQueryServiceClient;
            this.ServiceCommandRouter = serviceCommandRouter;
            this.LinkedKeyManager = linkedKeyManager;
            this.EventRaiser = eventRaiser;
            this.ServiceCoordinator = serviceCoordinator;
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
            domainRuleManager.RegisterPartialRule<IAccountRuleModel>(new TheInvoicesAccountNumberMustBeAValidSAHLAccountRule(domainQueryServiceClient));
        }

        public ISystemMessageCollection HandleCommand(AcceptThirdPartyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            int thirdPartyInvoiceKey = 0;
            ISystemMessageCollection rulesMessages = new SystemMessageCollection();
            DomainRuleManager.ExecuteRules(rulesMessages, command);
            if (!rulesMessages.HasErrors)
            {
                using (var uow = UnitOfWorkFactory.Build())
                {
                    Guid emptyInvoiceGuid = CombGuid.Instance.Generate();

                    CreateEmptyInvoiceCommand emptyInvoiceCommand = new CreateEmptyInvoiceCommand(command.AccountNumber, emptyInvoiceGuid, command.InvoiceDocument.FromEmailAddress,
                        command.InvoiceDocument.DateReceived);
                    messages.Aggregate(ServiceCommandRouter.HandleCommand(emptyInvoiceCommand, metadata));
                    if (messages.HasErrors)
                    {
                        uow.Dispose();
                        return messages;
                    }

                    thirdPartyInvoiceKey = LinkedKeyManager.RetrieveLinkedKey(emptyInvoiceGuid);

                    LinkedKeyManager.DeleteLinkedKey(emptyInvoiceGuid);

                    string sahlReference = thirdPartyInvoiceDataManager.RetrieveSAHLReference(thirdPartyInvoiceKey);

                    AddInvoiceAttachmentCommand addInvoiceAttachmentCommand = new AddInvoiceAttachmentCommand(command.InvoiceDocument, thirdPartyInvoiceKey);

                    CreateThirdPartyInvoiceWorkflowCaseCommand createThirdPartyInvoiceWorkflowCaseCommand = new CreateThirdPartyInvoiceWorkflowCaseCommand(thirdPartyInvoiceKey,
                        command.AccountNumber, command.ThirdPartyTypeKey, command.InvoiceDocument.FromEmailAddress);

                    CompensateAddInvoiceAttachmentCommand compensateAddInvoiceAttachmentCommand = new CompensateAddInvoiceAttachmentCommand(thirdPartyInvoiceKey);

                    var serviceCoordinatorSequence = ServiceCoordinator.StartSequence(
                                     () => { return ServiceCommandRouter.HandleCommand(addInvoiceAttachmentCommand, metadata); },
                                     () => { return ServiceCommandRouter.HandleCommand(compensateAddInvoiceAttachmentCommand, metadata); }).
                                Then(() => { return ServiceCommandRouter.HandleCommand(createThirdPartyInvoiceWorkflowCaseCommand, metadata); },
                                     () => { return SystemMessageCollection.Empty(); }).
                             EndSequence();
                    messages.Aggregate(serviceCoordinatorSequence.Run());

                    if (!messages.HasErrors)
                    {
                        EventRaiser.RaiseEvent(DateTime.Now, new ThirdPartyInvoiceSubmissionAcceptedEvent(DateTime.Now, sahlReference, command.AccountNumber, command.InvoiceDocument),
                         thirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, metadata);
                        uow.Complete();
                    }
                }
            }
            else
            {
                EventRaiser.RaiseEvent(DateTime.Now, new ThirdPartyInvoiceSubmissionRejectedEvent(DateTime.Now, command.AccountNumber, command.InvoiceDocument
                        , rulesMessages.ErrorMessages().Select(x => x.Message).ToList()), command.AccountNumber, (int)GenericKeyType.Account, metadata);
            }

            return messages;
        }
    }
}
