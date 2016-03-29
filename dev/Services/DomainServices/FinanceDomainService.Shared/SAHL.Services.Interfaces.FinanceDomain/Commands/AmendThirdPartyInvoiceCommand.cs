using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class AmendThirdPartyInvoiceCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice, IRequiresThirdParty
    {
        public ThirdPartyInvoiceModel ThirdPartyInvoiceModel { get; protected set; }

        public AmendThirdPartyInvoiceCommand(ThirdPartyInvoiceModel thirdPartyInvoiceModel)
        {
            this.ThirdPartyInvoiceModel = thirdPartyInvoiceModel;
        }

        public int ThirdPartyInvoiceKey
        {
            get { return ThirdPartyInvoiceModel.ThirdPartyInvoiceKey; }
        }

        public Guid ThirdPartyId
        {
            get { return ThirdPartyInvoiceModel.ThirdPartyId; }
        }
    }
}
