using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData
{
    public interface IThirdPartyInvoiceDataManager
    {
        bool HasThirdPartyInvoiceBeenApproved(int ThirdPartyInvoicekey);

        void AddInvoiceLineItem(InvoiceLineItemModel invoiceLineItemModel);

        void AmendThirdPartyInvoiceHeader(ThirdPartyInvoiceModel invoice);

        void AmendInvoiceLineItem(InvoiceLineItemModel invoiceLineItemModel);

        string RetrieveSAHLReference(int thirdPartyInvoiceKey);

        IEnumerable<InvoiceLineItemDataModel> GetInvoiceLineItems(int thirdPartyInvoiceKey);

        ThirdPartyInvoiceDataModel GetThirdPartyInvoiceByKey(int thirdPartyInvoiceKey);

        int SaveEmptyThirdPartyInvoice(int accountKey, Guid correlationGuid, string receivedFromEmailAddress, DateTime receivedDate);
        string GetThirdPartyInvoiceEmailSubject(int thirdPartyInvoiceKey);


        void UpdateThirdPartyInvoiceStatus(int thirdPartyInvoiceKey, InvoiceStatus newInvoiceStatus);

        void AmendInvoiceTotals(int thirdPartyInvoiceKey);

        InvoiceLineItemDataModel GetInvoiceLineItem(int lineItemKey);

        void RemoveInvoiceLineItem(int invoiceLineItemId);

        List<string> GetUserCapabilitiesByUserOrgStructureKey(int orgStructureKey);

        IEnumerable<ThirdPartyInvoiceDataModel> GetThirdPartyInvoicesByInvoiceNumber(string invoiceNumber);
        IEnumerable<ThirdPartyInvoicePaymentModel> GetThirdPartyInvoicePaymentInformation(int[] thirdPartyInvoiceKeys);

        ThirdPartyInvoicePaymentBatchItem GetCatsPaymentBatchItemInformation(int catsPaymentBatchKey, int thirdPartyInvoiceKey);
    }
}