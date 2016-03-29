using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData
{
    public class ThirdPartyInvoiceDataManager : IThirdPartyInvoiceDataManager
    {
        private IDbFactory dbFactory;

        public ThirdPartyInvoiceDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int SaveEmptyThirdPartyInvoice(int accountKey, Guid correlationId, string receivedFromEmailAddress, DateTime receivedDate)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                var sahlReferenceQueryStatement = new GetSAHLReferenceStatement();
                string sahlReference = dbContext.Select<string>(sahlReferenceQueryStatement).FirstOrDefault();

                ThirdPartyInvoiceDataModel thirdPartyInvoiceDataModel = new ThirdPartyInvoiceDataModel(sahlReference, (int)InvoiceStatus.Received, accountKey, null,
                    null, null, receivedFromEmailAddress, 0, 0, 0, false, DateTime.Now, string.Empty);
                dbContext.Insert<ThirdPartyInvoiceDataModel>(thirdPartyInvoiceDataModel);
                dbContext.Complete();
                return thirdPartyInvoiceDataModel.ThirdPartyInvoiceKey;
            }
        }

        public string RetrieveSAHLReference(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceDataModel thirdPartyInvoiceDataModel;
            var thirdPartyInvoiceQuery = new GetThirdPartyInvoiceByKeyStatement(thirdPartyInvoiceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                thirdPartyInvoiceDataModel = db.SelectOne<ThirdPartyInvoiceDataModel>(thirdPartyInvoiceQuery);
            }
            return thirdPartyInvoiceDataModel.SahlReference;
        }

        public void AddInvoiceLineItem(InvoiceLineItemModel invoiceLineItemModel)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                var invoiceLineItem = new InvoiceLineItemDataModel(invoiceLineItemModel.ThirdPartyInvoiceKey,
                                      invoiceLineItemModel.InvoiceLineItemDescriptionKey,
                                      invoiceLineItemModel.AmountExcludingVAT,
                                      invoiceLineItemModel.IsVATItem,
                                      invoiceLineItemModel.VATAmount,
                                      invoiceLineItemModel.TotalAmountIncludingVAT);
                dbContext.Insert<InvoiceLineItemDataModel>(invoiceLineItem);
                dbContext.Complete();
            }
        }

        public void AmendThirdPartyInvoiceHeader(ThirdPartyInvoiceModel thirdPartyInvoiceModel)
        {
            var statement = new UpdateThirdPartyInvoiceHeaderStatement(thirdPartyInvoiceModel.ThirdPartyInvoiceKey, thirdPartyInvoiceModel.ThirdPartyId,
                thirdPartyInvoiceModel.InvoiceNumber, thirdPartyInvoiceModel.InvoiceDate, thirdPartyInvoiceModel.CapitaliseInvoice, thirdPartyInvoiceModel.PaymentReference);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ThirdPartyInvoiceDataModel>(statement);
                db.Complete();
            }
        }

        public void AmendInvoiceLineItem(InvoiceLineItemModel invoiceLineItemModel)
        {
            var ammendAttorneyInvoiceLineItemStatement = new AmendInvoiceLineItemStatement(invoiceLineItemModel);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<InvoiceLineItemDataModel>(ammendAttorneyInvoiceLineItemStatement);
                db.Complete();
            }
        }

        public ThirdPartyInvoiceDataModel GetThirdPartyInvoiceByKey(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceDataModel thirdPartyInvoiceDataModel;
            var thirdPartyInvoiceQuery = new GetThirdPartyInvoiceByKeyStatement(thirdPartyInvoiceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                thirdPartyInvoiceDataModel = db.SelectOne<ThirdPartyInvoiceDataModel>(thirdPartyInvoiceQuery);
            }

            return thirdPartyInvoiceDataModel;
        }

        public void UpdateThirdPartyInvoiceStatus(int thirdPartyInvoiceKey, InvoiceStatus newInvoiceStatus)
        {
            var updateThirdPartyInvoiceStatement = new UpdateThirdPartyInvoiceStatusStatement(thirdPartyInvoiceKey, newInvoiceStatus);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ThirdPartyInvoiceDataModel>(updateThirdPartyInvoiceStatement);
                db.Complete();
            }
        }

        public void RemoveInvoiceLineItem(int removedInvoiceLineItemKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.DeleteByKey<InvoiceLineItemDataModel, int>(removedInvoiceLineItemKey);
                db.Complete();
            }
        }

        public InvoiceLineItemDataModel GetInvoiceLineItem(int InvoiceLineItemKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOneWhere<InvoiceLineItemDataModel>(string.Format("[InvoiceLineItemKey] = {0}", InvoiceLineItemKey));
            }
        }

        public IEnumerable<InvoiceLineItemDataModel> GetInvoiceLineItems(int thirdPartyInvoiceKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectWhere<InvoiceLineItemDataModel>(string.Format("[ThirdPartyInvoiceKey] = {0}", thirdPartyInvoiceKey));
            }
        }

        public void AmendInvoiceTotals(int thirdPartyInvoiceKey)
        {
            var lineItems = this.GetInvoiceLineItems(thirdPartyInvoiceKey);
            decimal vatAmount = lineItems.Sum(x => x.VATAmount.GetValueOrDefault());
            decimal invoiceTotal = lineItems.Sum(x => x.TotalAmountIncludingVAT);
            decimal invoiceTotalExVAT = invoiceTotal - vatAmount;

            var amendAttorneyInvoiceTotalsStatement = new UpdateThirdPartyInvoiceTotalsStatement(thirdPartyInvoiceKey, vatAmount, invoiceTotalExVAT, invoiceTotal);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ThirdPartyInvoiceDataModel>(amendAttorneyInvoiceTotalsStatement);
                db.Complete();
            }
        }

        public string GetThirdPartyInvoiceEmailSubject(int thirdPartyInvoiceKey)
        {
            ThirdPartyEmailSubjectModel thirdPartyEmailSubjectModel;
            var getThirdPartyInvoiceEmailSubjectStatement = new GetThirdPartyInvoiceEmailSubjectStatement(thirdPartyInvoiceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                thirdPartyEmailSubjectModel = db.SelectOne<ThirdPartyEmailSubjectModel>(getThirdPartyInvoiceEmailSubjectStatement);
            }
            return thirdPartyEmailSubjectModel != null ? thirdPartyEmailSubjectModel.EmailSubject : string.Empty;
        }


        public List<string> GetUserCapabilitiesByUserOrgStructureKey(int orgStructureKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(new GetUserCapabilitiesByUserOrgStructureKeyStatement(orgStructureKey)).ToList();
            }
        }

        public IEnumerable<ThirdPartyInvoiceDataModel> GetThirdPartyInvoicesByInvoiceNumber(string invoiceNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectWhere<ThirdPartyInvoiceDataModel>(string.Format("[InvoiceNumber] = '{0}'", invoiceNumber));
            }
        }

        public IEnumerable<ThirdPartyInvoicePaymentModel> GetThirdPartyInvoicePaymentInformation(int[] thirdPartyInvoiceKeys)
        {
            var query = new GetThirdPartyInvoicePaymentInformationStatement(thirdPartyInvoiceKeys);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(query);
            }
        }

        public ThirdPartyInvoicePaymentBatchItem GetCatsPaymentBatchItemInformation( int catsPaymentBatchKey, int thirdPartyInvoiceKey)
        {
            var catsPaymentBatchItemInfoStatement = new GetCatsPaymentBatchItemInfoStatement(catsPaymentBatchKey, thirdPartyInvoiceKey);
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return dbContext.SelectOne(catsPaymentBatchItemInfoStatement);
            }
        }


        public bool HasThirdPartyInvoiceBeenApproved(int thirdPartyInvoiceKey)
        {
            var wasInvoiceApproved = false;
            var invoiceApprovalQuery = new GetThirdPartyInvoiceApprovalStateStatement(thirdPartyInvoiceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                wasInvoiceApproved = db.SelectOne<bool>(invoiceApprovalQuery);
            }

            return wasInvoiceApproved;
        }
    }
}