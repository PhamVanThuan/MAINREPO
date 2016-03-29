using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateThirdPartyInvoiceCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public ThirdPartyInvoiceDataModel ThirdPartyInvoice { get; protected set; }

        public UpdateThirdPartyInvoiceCommand(ThirdPartyInvoiceDataModel thirdPartyInvoice)
        {
            this.ThirdPartyInvoice = thirdPartyInvoice;
        }
    }
}