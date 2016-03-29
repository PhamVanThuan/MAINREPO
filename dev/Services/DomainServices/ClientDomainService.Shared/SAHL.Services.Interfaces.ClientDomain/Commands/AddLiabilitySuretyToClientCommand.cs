using SAHL.Core.Services;
using SAHL.Core.Validation;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddLiabilitySuretyToClientCommand : ServiceCommand, IClientDomainCommand, IRequiresClient
    {
        [Required]
        public LiabilitySuretyModel LiabilitySuretyModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        public AddLiabilitySuretyToClientCommand(LiabilitySuretyModel liabilitySuretyModel, int clientkey)
        {
            this.LiabilitySuretyModel = liabilitySuretyModel;
            this.ClientKey = clientkey;
        }
    }
}