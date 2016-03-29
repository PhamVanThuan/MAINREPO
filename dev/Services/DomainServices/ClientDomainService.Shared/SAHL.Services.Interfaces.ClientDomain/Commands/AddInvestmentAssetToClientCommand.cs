using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddInvestmentAssetToClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public InvestmentAssetModel InvestmentAsset { get; protected set; }

        public AddInvestmentAssetToClientCommand(int clientKey, InvestmentAssetModel investmentAsset)
        {
            this.ClientKey = clientKey;
            this.InvestmentAsset = investmentAsset;
        }
    }
}