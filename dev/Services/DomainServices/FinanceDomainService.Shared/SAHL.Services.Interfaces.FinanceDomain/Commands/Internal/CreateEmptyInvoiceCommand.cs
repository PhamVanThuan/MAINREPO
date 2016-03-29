using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class CreateEmptyInvoiceCommand : ServiceCommand, IFinanceDomainInternalCommand, IRequiresAccount
    {
        [Required]
        public int AccountKey { get; protected set; }

        [Required]
        public Guid EmptyInvoiceGuid { get; protected set; }

        [Required]
        public string ReceivedFromEmailAddress { get; protected set; }

        [Required]
        public DateTime ReceivedDate { get; protected set; }

        public CreateEmptyInvoiceCommand(int accountKey, Guid emptyInvoiceGuid, string receivedFromEmailAddress, DateTime receivedDate)
        {
            this.AccountKey = accountKey;
            this.ReceivedFromEmailAddress = receivedFromEmailAddress;
            this.EmptyInvoiceGuid = emptyInvoiceGuid;
            this.ReceivedDate = receivedDate;
        }
    }
}