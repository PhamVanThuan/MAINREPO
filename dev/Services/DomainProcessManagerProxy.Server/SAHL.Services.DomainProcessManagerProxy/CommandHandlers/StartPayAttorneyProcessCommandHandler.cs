using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainProcessManagerProxy.DpmServiceReference;
using SAHL.Services.Interfaces.DomainProcessManagerProxy.Commands;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DomainProcessManagerProxy.CommandHandlers
{
    public class StartPayAttorneyProcessCommandHandler : IServiceCommandHandler<StartPayAttorneyProcessCommand>
    {
        private IDomainProcessManagerService domainProcessManagerServiceClient;

        public StartPayAttorneyProcessCommandHandler(IDomainProcessManagerService domainProcessManagerServiceClient)
        {
            this.domainProcessManagerServiceClient = domainProcessManagerServiceClient;
        }

        public ISystemMessageCollection HandleCommand(StartPayAttorneyProcessCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var dpmCommand = new StartDomainProcessCommand();
            var payThirdPartyInvoiceProcessModel = new PayThirdPartyInvoiceProcessModel();
            var invoiceCollection = new List<PayThirdPartyInvoiceModel>();
            foreach (var invoice in command.ThirdPartyPayments)
            {
                invoiceCollection.Add(new PayThirdPartyInvoiceModel
                {
                    AccountNumber = invoice.AccountNumber,
                    InstanceId = invoice.InstanceId,
                    SAHLReference = invoice.SAHLReference,
                    StepInProcess = 0,
                    ThirdPartyInvoiceKey = invoice.ThirdPartyInvoiceKey
                });
            }
            payThirdPartyInvoiceProcessModel.InvoiceCollection = invoiceCollection.ToArray();
            payThirdPartyInvoiceProcessModel.UserName = metadata.UserName;
            dpmCommand.DataModel = payThirdPartyInvoiceProcessModel;
            dpmCommand.StartEventToWaitFor = "ThirdPartyInvoiceAddedToBatchEvent";

            try
            {
                domainProcessManagerServiceClient.StartDomainProcess(dpmCommand);
            }
            catch (Exception ex)
            {
                messages.AddMessage(new SystemMessage(ex.Message, SystemMessageSeverityEnum.Error));
            }
            
            return messages;
        }
    }
}
