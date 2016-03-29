using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateVariableLoanApplicationInformationCommand : ServiceCommand, IFrontEndTestCommand
    {
        public UpdateVariableLoanApplicationInformationCommand(OfferInformationVariableLoanDataModel variableLoanApplicationInformation)
        {
            this.VariableLoanApplicationInformation = variableLoanApplicationInformation;
        }

        [Required]
        public OfferInformationVariableLoanDataModel VariableLoanApplicationInformation { get; protected set; }
    }
}