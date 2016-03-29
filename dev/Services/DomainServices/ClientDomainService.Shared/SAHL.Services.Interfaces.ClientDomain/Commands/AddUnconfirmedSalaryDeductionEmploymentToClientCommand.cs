using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Commands
{
    public class AddUnconfirmedSalaryDeductionEmploymentToClientCommand : ServiceCommand, IRequiresClient, IClientDomainCommand
    {
        [Required]
        public SalaryDeductionEmploymentModel SalaryDeductionEmploymentModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public OriginationSource OriginationSource { get; protected set; }

        public AddUnconfirmedSalaryDeductionEmploymentToClientCommand(SalaryDeductionEmploymentModel salaryDeductionEmploymentModel, int clientKey, OriginationSource originationSource)
        {
            this.SalaryDeductionEmploymentModel = salaryDeductionEmploymentModel;
            this.ClientKey = clientKey;
            this.OriginationSource = originationSource;
        }
    }
}