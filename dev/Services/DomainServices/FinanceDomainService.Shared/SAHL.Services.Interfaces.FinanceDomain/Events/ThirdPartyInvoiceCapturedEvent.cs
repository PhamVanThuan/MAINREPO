using SAHL.Core.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceCapturedEvent : Event
    {
        public ThirdPartyInvoiceModel ThirdPartyInvoiceModel { get; protected set; }

        public ThirdPartyInvoiceCapturedEvent(DateTime date, ThirdPartyInvoiceModel thirdPartyInvoiceModel)
            : base(date)
        {
            this.ThirdPartyInvoiceModel = thirdPartyInvoiceModel;
        }
    }
}