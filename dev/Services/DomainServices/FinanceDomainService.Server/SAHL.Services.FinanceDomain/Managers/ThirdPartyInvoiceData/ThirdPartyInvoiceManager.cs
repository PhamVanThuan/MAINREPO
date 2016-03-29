using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData
{
    public class ThirdPartyInvoiceManager : IThirdPartyInvoiceManager
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public ThirdPartyInvoiceManager(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        private bool HasInvoiceLineItemChanged(InvoiceLineItemModel invoiceLineItemModel)
        {
            var existingLine = thirdPartyInvoiceDataManager.GetInvoiceLineItem(invoiceLineItemModel.InvoiceLineItemKey.Value);

            bool amountChanged = invoiceLineItemModel.AmountExcludingVAT != existingLine.Amount;
            bool isVatableChanged = invoiceLineItemModel.IsVATItem != existingLine.IsVATItem;
            bool descriptionChanged = invoiceLineItemModel.InvoiceLineItemDescriptionKey != existingLine.InvoiceLineItemDescriptionKey;

            var invoiceLineItemChanged = amountChanged || isVatableChanged || descriptionChanged;

            return invoiceLineItemChanged;
        }

        public bool HasThirdPartyInvoiceHeaderChanged(ThirdPartyInvoiceModel thirdPartyInvoice)
        {
            var currentInvoiceHeader = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(thirdPartyInvoice.ThirdPartyInvoiceKey);
            var oldthirdPartyInvoiceInvoiceDate = currentInvoiceHeader.InvoiceDate.GetValueOrDefault();
            return thirdPartyInvoice.ThirdPartyId != currentInvoiceHeader.ThirdPartyId
                || thirdPartyInvoice.InvoiceNumber != currentInvoiceHeader.InvoiceNumber
                || thirdPartyInvoice.InvoiceDate != oldthirdPartyInvoiceInvoiceDate
                || thirdPartyInvoice.CapitaliseInvoice != currentInvoiceHeader.CapitaliseInvoice
                || thirdPartyInvoice.PaymentReference != currentInvoiceHeader.PaymentReference;
        }

        public IEnumerable<InvoiceLineItemModel> GetUpdatedInvoicedLineItems(IEnumerable<InvoiceLineItemModel> invoiceLineItems)
        {
            List<InvoiceLineItemModel> updatedLineItems = new List<InvoiceLineItemModel>();
            var existingLineItems = invoiceLineItems.Where(x => x.InvoiceLineItemKey != null);
            foreach (var lineItem in existingLineItems)
            {
                var lineItemChanges = this.HasInvoiceLineItemChanged(lineItem);
                if (lineItemChanges)
                {
                    updatedLineItems.Add(lineItem);
                }
            }
            return updatedLineItems;
        }

        public IEnumerable<InvoiceLineItemDataModel> GetRemovedInvoiceLineItems(IEnumerable<InvoiceLineItemDataModel> existingInvoiceLineItems, IEnumerable<InvoiceLineItemModel> newLineItems)
        {
            var removedItems = existingInvoiceLineItems.Where(x => !newLineItems.Where(y => y.InvoiceLineItemKey == x.InvoiceLineItemKey)
                        .Any());
            return removedItems;
        }

        public ThirdPartyInvoiceModel GetThirdPartyInvoiceModel(int thirdPartyInvoiceKey)
        {
            var thirdPartyInvoiceDataModel = thirdPartyInvoiceDataManager.GetThirdPartyInvoiceByKey(thirdPartyInvoiceKey);
            var invoiceLineItems = thirdPartyInvoiceDataManager.GetInvoiceLineItems(thirdPartyInvoiceKey)
                                    .Select(li => new InvoiceLineItemModel
                                                    (
                                                          li.InvoiceLineItemKey
                                                        , li.ThirdPartyInvoiceKey
                                                        , li.InvoiceLineItemDescriptionKey
                                                        , li.Amount
                                                        , li.IsVATItem
                                                    ));

            var thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(
                                               thirdPartyInvoiceKey
                                             , thirdPartyInvoiceDataModel.ThirdPartyId.HasValue ? thirdPartyInvoiceDataModel.ThirdPartyId.Value : Guid.Empty
                                             , thirdPartyInvoiceDataModel.InvoiceNumber
                                             , thirdPartyInvoiceDataModel.InvoiceDate
                                             , invoiceLineItems
                                             , thirdPartyInvoiceDataModel.CapitaliseInvoice.HasValue
                                             , thirdPartyInvoiceDataModel.PaymentReference);
            return thirdPartyInvoiceModel;
        }

        public IDictionary<string, IEnumerable<ThirdPartyInvoicePaymentModel>>
            GroupThirdPartyPaymentInvoicesByThirdParty(IEnumerable<ThirdPartyInvoicePaymentModel> thirdPartyInvoicePayments)
        {
            var groupedData = thirdPartyInvoicePayments.GroupBy(x => x.FirmName).ToDictionary(g => g.Key, d => d.ToList() as IEnumerable<ThirdPartyInvoicePaymentModel>);
            return groupedData;
        }
    }
}