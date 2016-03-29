using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System.Collections.Generic;

namespace SAHL.Services.FrontEndTest.Managers
{
    public interface ITestDataManager
    {
        void UpdateHouseholdIncomeToZero(int applicationNumber);

        void UpdateVariableLoanApplicationInformationStatement(OfferInformationVariableLoanDataModel model);

        void InsertComcorpOfferPropertyDetails(ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetails);

        void UpdateApplicationEmploymentType(int applicationNumber, EmploymentType employmentType);

        void SetClientAddressToInactive(int clientAddressKey);

        void SetOfferInformationSPV(int applicationInformationKey, int spvKey);

        void UpdateClient(LegalEntityDataModel legalEntity);

        void RemoveApplicationMailingAddress(int applicationNumber);

        void RemoveActiveNewBusinessApplicantRecord(int offerRoleKey);

        void RemoveOpenNewBusinessApplicationCommand(int applicationNumber);

        void InsertDisabilityClaimRecord(DisabilityClaimDataModel disabilityClaimDataModel);

        void RemoveDisabilityClaimRecord(int disabilityClaimKey);

        string CreateDisabilityClaimPaymentSchedule(int disabilityClaimKey, string adUserName);

        void RemoveDisabilityPaymentRecord(int disabilityClaimKey);

        void RemoveApplicationRole(int offerRoleKey);

        GetLossControlAttorneyInvoiceStorDataQueryResult GetLossControlAttorneyInvoiceStorData(int thirdPartyInvoiceKey);

        void RemoveEmptyThirdPartyInvoice(int thirdPartyInvoice);

        void UpdateThirdPartyInvoice(ThirdPartyInvoiceDataModel thirdPartyInvoice);

        void InsertInvoiceLineItems(System.Collections.Generic.IEnumerable<InvoiceLineItemDataModel> invoiceLineItems);

        IEnumerable<GetFinancialTransactionsQueryResult> GetFinancialTransactions(int financialServiceKey);

        string GetGuidForFirstThirdPartyInvoice();

        void InsertAddress(AddressDataModel address);

        void InsertLegalEntityAddress(LegalEntityAddressDataModel legalEntityAddress);

        void UpdateAddress(AddressDataModel address);

        void InsertClient(LegalEntityDataModel legalEntity);

        void LinkClientToOffer(OfferRoleDataModel offerRole);

        void InsertLoanDetail(DetailDataModel loanDetail);

        void InsertValuation(ValuationDataModel valuation);

        void UpdateValuator(ValuatorDataModel valuator);

        void UpdateAttorney(AttorneyDataModel attorney);

        void InsertValuer(ValuatorDataModel valuer);

        void RemoveValuer(int valuatorKey);

        void InsertAttorney(AttorneyDataModel attorney);

        void RemoveAttorney(int attorney);

        void UpdateThirdParty(LegalEntityDataModel legalEntity);

        void InsertProperty(PropertyDataModel property);

        void UpdateProperty(PropertyDataModel property);

        void LinkOfferMortgageLoanProperty(int offerKey, int propertyKey);

        void UpdateThirdPartyInvoiceEmailAddress(ThirdPartyInvoiceDataModel thirdPartyInvoice);

        void RemoveOpenMortgageLoanAccount(int accountNumber);

        void InsertCatsPaymentBatch(CATSPaymentBatchDataModel catsPaymentBatch);

        void RemoveCATSPaymentBatch(int catsPaymentBatchKey);

        void UpdateCATSPaymentBatch(CATSPaymentBatchDataModel catsPaymentBatchDataModel);
    }
}