using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Services.FrontEndTest.Managers.Statements;
using SAHL.Services.FrontEndTest.QueryStatements;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers
{
    public class TestDataManager : ITestDataManager
    {
        private IDbFactory dbFactory;

        public TestDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void UpdateHouseholdIncomeToZero(int applicationNumber)
        {
            var statement = new UpdateHouseholdIncomeStatement(applicationNumber, 0);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<OfferInformationVariableLoanDataModel>(statement);
                db.Complete();
            }
        }

        public void UpdateVariableLoanApplicationInformationStatement(OfferInformationVariableLoanDataModel model)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new UpdateVariableLoanApplicationInformationStatement(model.OfferInformationKey, model.LoanAmountNoFees, model.LoanAgreementAmount);
                db.Update<OfferInformationVariableLoanDataModel>(statement);
                db.Complete();
            }
        }

        public void InsertComcorpOfferPropertyDetails(ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetails)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(comcorpOfferPropertyDetails);
                db.Complete();
            }
        }

        public void UpdateApplicationEmploymentType(int applicationNumber, EmploymentType employmentType)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new UpdateApplicationEmploymentTypeStatement(applicationNumber, (int)employmentType);
                db.Update<OfferInformationVariableLoanDataModel>(statement);
                db.Complete();
            }
        }

        public void SetClientAddressToInactive(int clientAddressKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new SetClientAddressToInactiveStatement(clientAddressKey);
                db.Update<LegalEntityAddressDataModel>(statement);
                db.Complete();
            }
        }

        public void SetOfferInformationSPV(int applicationInformationKey, int spvKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new SetOfferInformationSPVStatement(applicationInformationKey, spvKey);
                db.Update<OfferInformationVariableLoanDataModel>(statement);
                db.Complete();
            }
        }

        public void RemoveApplicationMailingAddress(int applicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveApplicationMailingAddressStatement(applicationNumber);
                db.Delete(statement);
                db.Complete();
            }
        }

        public void RemoveActiveNewBusinessApplicantRecord(int offerRoleKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveActiveNewBusinessApplicantStatement(offerRoleKey);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void RemoveOpenNewBusinessApplicationCommand(int applicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveOpenNewBusinessApplicationStatement(applicationNumber);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void InsertDisabilityClaimRecord(DisabilityClaimDataModel disabilityClaimDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(disabilityClaimDataModel);
                db.Complete();
            }
        }

        public void RemoveDisabilityClaimRecord(int disabilityClaimKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveDisabilityClaimRecordStatement(disabilityClaimKey);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public string CreateDisabilityClaimPaymentSchedule(int disabilityClaimKey, string adUserName)
        {
            var query = new CreateDisabilityClaimPaymentScheduleStatement(disabilityClaimKey, adUserName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<string>(query);
            }
        }

        public void RemoveApplicationRole(int offerRoleKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveApplicationRoleStatement(offerRoleKey);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void RemoveDisabilityPaymentRecord(int disabilityClaimKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveDisabilityPaymentRecordStatement(disabilityClaimKey);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public GetLossControlAttorneyInvoiceStorDataQueryResult GetLossControlAttorneyInvoiceStorData(int thirdPartyInvoiceKey)
        {
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetLossControlAttorneyInvoiceStorDataQueryStatement(thirdPartyInvoiceKey);
                return dbContext.SelectOne(statement);
            }
        }

        public void RemoveEmptyThirdPartyInvoice(int thirdPartyInvoiceKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new RemoveEmptyThirdPartyInvoiceStatement(thirdPartyInvoiceKey);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void UpdateThirdPartyInvoice(ThirdPartyInvoiceDataModel thirdPartyInvoice)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(thirdPartyInvoice);
                dbContext.Complete();
            }
        }

        public void InsertInvoiceLineItems(IEnumerable<InvoiceLineItemDataModel> invoiceLineItems)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(invoiceLineItems);
                dbContext.Complete();
            }
        }

        public IEnumerable<GetFinancialTransactionsQueryResult> GetFinancialTransactions(int financialServiceKey)
        {
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetFinancialTransactionsStatement(financialServiceKey);
                return dbContext.Select(statement);
            }
        }

        public string GetGuidForFirstThirdPartyInvoice()
        {
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetFirstThirdPartyInvoiceWithDocumentStatement();
                return dbContext.Select(statement).FirstOrDefault();
            }
        }

        public void InsertAddress(AddressDataModel address)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(address);
                dbContext.Complete();
            }
        }

        public void InsertLegalEntityAddress(LegalEntityAddressDataModel legalEntityAddress)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(legalEntityAddress);
                dbContext.Complete();
            }
        }

        public void UpdateAddress(AddressDataModel address)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(address);
                dbContext.Complete();
            }
        }

        public void InsertClient(LegalEntityDataModel legalEntity)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(legalEntity);
                dbContext.Complete();
            }
        }

        public void LinkClientToOffer(OfferRoleDataModel offerRole)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(offerRole);
                dbContext.Complete();
            }
        }

        public void UpdateClient(LegalEntityDataModel legalEntity)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(legalEntity);
                dbContext.Complete();
            }
        }

        public void InsertLoanDetail(DetailDataModel loanDetail)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(loanDetail);
                dbContext.Complete();
            }
        }

        public void InsertValuation(ValuationDataModel valuation)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(valuation);
                dbContext.Complete();
            }
        }

        public void UpdateValuator(ValuatorDataModel valuator)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(valuator);
                dbContext.Complete();
            }
        }

        public void UpdateAttorney(AttorneyDataModel attorney)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(attorney);
                dbContext.Complete();
            }
        }

        public void InsertValuer(ValuatorDataModel valuer)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(valuer);
                dbContext.Complete();
            }
        }

        public void RemoveValuer(int valuatorKey)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.DeleteByKey<ValuatorDataModel, int>(valuatorKey);
                dbContext.Complete();
            }
        }

        public void InsertAttorney(AttorneyDataModel attorney)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(attorney);
                dbContext.Complete();
            }
        }

        public void RemoveAttorney(int attorney)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.DeleteByKey<AttorneyDataModel, int>(attorney);
                dbContext.Complete();
            }
        }

        public void UpdateThirdParty(LegalEntityDataModel thirdparty)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(thirdparty);
                dbContext.Complete();
            }
        }

        public void InsertProperty(PropertyDataModel property)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(property);
                dbContext.Complete();
            }
        }

        public void UpdateProperty(PropertyDataModel property)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(property);
                dbContext.Complete();
            }
        }

        public void LinkOfferMortgageLoanProperty(int offerKey, int propertyKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new LinkOfferMortgageLoanPropertyStatement(offerKey, propertyKey);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void UpdateThirdPartyInvoiceEmailAddress(ThirdPartyInvoiceDataModel thirdPartyInvoice)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(thirdPartyInvoice);
                dbContext.Complete();
            }
        }

        public void RemoveOpenMortgageLoanAccount(int accountNumber)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.DeleteWhere<OpenMortgageLoanAccountsDataModel>(string.Format("AccountKey = {0}", accountNumber));
                db.Complete();
            }
        }

        public void InsertCatsPaymentBatch(CATSPaymentBatchDataModel catsPaymentBatch)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(catsPaymentBatch);
                dbContext.Complete();
            }
        }

        public void RemoveCATSPaymentBatch(int catsPaymentBatchKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.DeleteByKey<CATSPaymentBatchDataModel, int>(catsPaymentBatchKey);
                db.Complete();
            }
        }

        public void UpdateCATSPaymentBatch(CATSPaymentBatchDataModel catsPaymentBatchDataModel)
        {
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Update(catsPaymentBatchDataModel);
                dbContext.Complete();
            }
        }
    }
}