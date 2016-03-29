using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveEmptyThirdPartyInvoiceCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public RemoveEmptyThirdPartyInvoiceCommand(int ThirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = ThirdPartyInvoiceKey;
        }
    }
}
