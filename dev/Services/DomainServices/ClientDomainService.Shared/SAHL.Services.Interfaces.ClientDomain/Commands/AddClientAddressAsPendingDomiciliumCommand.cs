using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddClientAddressAsPendingDomiciliumCommand : ServiceCommand, IClientDomainCommand
    {
        [Required]
        public ClientAddressAsPendingDomiciliumModel ClientAddressAsPendingDomiciliumModel { get; protected set; }

        [Required]
        public Guid ClientDomiciliumGuid { get; protected set; }

        [Required]
        public string AdUserName { get; protected set; }

        public AddClientAddressAsPendingDomiciliumCommand(ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomiciliumModel, Guid clientDomiciliumGuid)
        {
            this.ClientAddressAsPendingDomiciliumModel = clientAddressAsPendingDomiciliumModel;
            this.ClientDomiciliumGuid = clientDomiciliumGuid;
            this.AdUserName = "System";
        }
    }
}