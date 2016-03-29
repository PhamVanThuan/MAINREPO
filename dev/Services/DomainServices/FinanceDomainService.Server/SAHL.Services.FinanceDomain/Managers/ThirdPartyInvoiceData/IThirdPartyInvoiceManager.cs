using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData
{
    public interface IThirdPartyInvoiceManager
    {
        bool HasThirdPartyInvoiceHeaderChanged(ThirdPartyInvoiceModel newInvoice);

        IEnumerable<InvoiceLineItemModel> GetUpdatedInvoicedLineItems(IEnumerable<InvoiceLineItemModel> invoiceLineItems);

        IEnumerable<InvoiceLineItemDataModel> GetRemovedInvoiceLineItems(IEnumerable<InvoiceLineItemDataModel> existingInvoiceLineItems, IEnumerable<InvoiceLineItemModel> newLineItems);

        ThirdPartyInvoiceModel GetThirdPartyInvoiceModel(int thirdPartyInvoiceKey);

        IDictionary<string, IEnumerable<ThirdPartyInvoicePaymentModel>> GroupThirdPartyPaymentInvoicesByThirdParty(IEnumerable<ThirdPartyInvoicePaymentModel> thirdPartyInvoicePayments);

    }
}