using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddFixedLongTermInvestmentLiabilityToClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        public FixedLongTermInvestmentLiabilityModel FixedLongTermInvestmentLiabilityModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey, ClientKey should be in a range of [1, 2 147 483 648].")]
        public int ClientKey { get; protected set; }

        public AddFixedLongTermInvestmentLiabilityToClientCommand(FixedLongTermInvestmentLiabilityModel fixedLongTermInvestmentLiabilityModel, int clientKey)
        {
            this.FixedLongTermInvestmentLiabilityModel = fixedLongTermInvestmentLiabilityModel;
            this.ClientKey = clientKey;
        }
    }
}