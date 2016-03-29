using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddLifeAssuranceAssetToClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public LifeAssuranceAssetModel LifeAssuranceAsset { get; protected set; }

        public AddLifeAssuranceAssetToClientCommand(int clientKey, LifeAssuranceAssetModel lifeAssuranceAsset)
        {
            this.ClientKey = clientKey;
            this.LifeAssuranceAsset = lifeAssuranceAsset;
        }
    }
}
