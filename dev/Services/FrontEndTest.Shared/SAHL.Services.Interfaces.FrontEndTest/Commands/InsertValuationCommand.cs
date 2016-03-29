using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data.Models._2AM;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertValuationCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public ValuationDataModel Valuation { get; protected set; }
        [Required]
        public Guid ValuationId { get; protected set; }
        public InsertValuationCommand(ValuationDataModel valuation, Guid valuationId)
        {
            this.Valuation = valuation;
            this.ValuationId = valuationId;
        }

    }
}
