using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinancialDomain.Commands
{
    public class PriceNewBusinessApplicationCommand : ServiceCommand, IFinancialDomainCommand, IRequiresOpenApplication, IRequiresOpenLatestApplicationInformation, IApplicationModel
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid Application Number.")]
        public int ApplicationNumber { get; protected set; }

        public PriceNewBusinessApplicationCommand(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
        }
    }
}