using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddUnconfirmedSalariedEmploymentToClientCommand : ServiceCommand,IRequiresClient, IClientDomainCommand
    {
        [Required]
        public SalariedEmploymentModel SalariedEmploymentModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public OriginationSource OriginationSource { get; protected set; }

        public AddUnconfirmedSalariedEmploymentToClientCommand(SalariedEmploymentModel salariedEmploymentModel, int clientKey, OriginationSource originationSource)
        {
            this.SalariedEmploymentModel = salariedEmploymentModel;
            this.ClientKey = clientKey;
            this.OriginationSource = originationSource;
        }
    }
}