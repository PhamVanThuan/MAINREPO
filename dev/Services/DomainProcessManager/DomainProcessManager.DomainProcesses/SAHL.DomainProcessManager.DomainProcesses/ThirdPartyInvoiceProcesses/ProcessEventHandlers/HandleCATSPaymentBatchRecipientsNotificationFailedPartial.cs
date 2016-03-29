using SAHL.Core.DomainProcess;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public abstract partial class PayThirdPartyInvoiceDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<CATSPaymentBatchRecipientsNotifiedEvent>
        where T : PayThirdPartyInvoiceProcessModel
    {
        public void Handle(CATSPaymentBatchRecipientsNotifiedEvent recipientNotifiedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var hasItems = recipientNotifiedEvent.UnSuccessfulNotifications == null ? false : recipientNotifiedEvent.UnSuccessfulNotifications.Any();
            if (hasItems)
            {
                string[] SAHLReferencesForFailedRecipientNotifications = recipientNotifiedEvent.UnSuccessfulNotifications.Select(y => y.SahlReferenceNumber).ToArray();
                communicationManager.SendFailureToNotifyRecipientsEmail(SAHLReferencesForFailedRecipientNotifications);
            }
            
        }
    }
}