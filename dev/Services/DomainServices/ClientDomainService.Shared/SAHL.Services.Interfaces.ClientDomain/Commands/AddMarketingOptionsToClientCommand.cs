using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddMarketingOptionsToClientCommand : ServiceCommand, IClientDomainCommand
    {
        [Required]
        public IEnumerable<MarketingOptionModel> ClientMarketingOptionsCollection { get; protected set;}

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        public AddMarketingOptionsToClientCommand(IEnumerable<MarketingOptionModel> clientMarketingOptionsCollection, int clientKey)
        {
            this.ClientMarketingOptionsCollection = clientMarketingOptionsCollection;
            this.ClientKey = clientKey;
        }
    }
}
