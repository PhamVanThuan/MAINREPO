using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.Application
{
    public interface IApplicationDataManager
    {
        int SaveApplication(OfferDataModel offer);

        void SaveApplicationMortgageLoan(OfferMortgageLoanDataModel offerMortgageLoan);

        int SaveApplicationInformation(OfferInformationDataModel offerInformation);

        void SaveApplicationInformationVariableLoan(OfferInformationVariableLoanDataModel offerInformationVariableLoan);

        void SaveApplicationInformationInterestOnly(OfferInformationInterestOnlyDataModel offerInformationInterestOnly);

        int GetReservedAccountNumber();

        void SaveExternalOriginatorAttribute(OfferAttributeDataModel offerAttribute);

        bool DoesOpenApplicationExist(int applicationNumber);

        int SaveApplicationMailingAddress(ApplicationMailingAddressModel model, int clientKey, int addressKey);

        IEnumerable<OfferMailingAddressDataModel> GetApplicationMailingAddress(int applicationNumber);

        bool DoesApplicationMailingAddressExist(int applicationNumber);

        bool DoesApplicationExist(int applicationNumber);

        int SaveApplicationDebitOrder(Interfaces.ApplicationDomain.Models.ApplicationDebitOrderModel applicationDebitOrderModel, int bankAccountKey);

        IEnumerable<OfferDebitOrderDataModel> GetApplicationDebitOrder(int applicationNumber);

        OfferInformationVariableLoanDataModel GetApplicationInformationVariableLoan(int applicationInformationNumber);

        OfferInformationDataModel GetLatestApplicationOfferInformation(int applicationNumber);

        void UpdateApplicationInformationVariableLoan(OfferInformationVariableLoanDataModel offerInformationVariableLoanDataModel);

        VendorModel GetExternalVendorForVendorCode(string vendorCode);

        int SaveExternalVendorOfferRole(OfferRoleDataModel offerRole);

        void LinkPropertyToApplication(int applicationNumber, int propertyKey);

        bool DoesOpenApplicationExistForPropertyAndClient(int propertyKey, string clientIDNumber);

        void SaveOfferInformationQuickCash(OfferInformationQuickCashDataModel offerInformationQuickCashDataModel);

        decimal GetMinimumLoanAmountForMortgageLoanPurpose(MortgageLoanPurpose mortgageLoanPurpose);

        OfferDataModel GetApplication(int applicationNumber);
    }
}