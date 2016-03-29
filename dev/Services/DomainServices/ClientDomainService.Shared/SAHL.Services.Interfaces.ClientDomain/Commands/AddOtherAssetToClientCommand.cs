using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddOtherAssetToClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public OtherAssetModel OtherAsset { get; protected set; }

        public AddOtherAssetToClientCommand(int clientKey, OtherAssetModel otherAsset )
        {
            this.ClientKey = clientKey;
            this.OtherAsset = otherAsset;
        }
    }
}