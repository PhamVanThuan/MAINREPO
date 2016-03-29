using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddFixedPropertyAssetToClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public FixedPropertyAssetModel FixedPropertyAsset { get; protected set; }

        public AddFixedPropertyAssetToClientCommand(int clientKey, FixedPropertyAssetModel fixedPropertyAsset)
        {
            this.ClientKey = clientKey;
            this.FixedPropertyAsset = fixedPropertyAsset;
        }
    }
}