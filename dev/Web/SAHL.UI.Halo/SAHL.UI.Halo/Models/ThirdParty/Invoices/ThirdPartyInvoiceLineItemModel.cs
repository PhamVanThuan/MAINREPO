using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Models.ThirdParty.Invoices
{
    public class ThirdPartyInvoiceLineItemModel: IHaloTileModel
    {
        public string lineItemType
        { get; set; }
        public string lineItemDesc
        { get; set; }
        public decimal lineItemAmount
        { get; set; }
        public decimal lineItemVatAmount
        { get; set; }
        public decimal lineItemTotalAmtInclVAT
        { get; set; }
    }
}
