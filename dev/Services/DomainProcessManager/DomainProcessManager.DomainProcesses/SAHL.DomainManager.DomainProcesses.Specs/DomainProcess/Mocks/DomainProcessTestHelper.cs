using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using SAHL.Services.Interfaces.PropertyDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks
{
    public static class DomainProcessTestHelper
    {
        public static void GetNewPurchaseDomainProcessPastCriticalPath(NewPurchaseApplicationDomainProcess domainProcess,
            NewPurchaseApplicationCreationModel newPurchaseCreationModel, int applicationNumber,
            Func<Type, Guid> getGuidForCommandType)
        {
            const int applicationRoleKey = 5;
            const int employmentKey = 7894;
            const int employmentTypeKey = 30;
            const int clientKey = 554;
            const int employerKey = 95;
            const int householdIncome = 60000;

            var testDate = new DateTime(2014, 10, 9);
            var newPurchaseApplicationAddedEvent = GetNewPurchaseApplicationAddedEvent(applicationNumber, newPurchaseCreationModel, testDate);

            var applicant = newPurchaseCreationModel.Applicants.First();
            var naturalPersonClientAddedEvent = GetNaturalPersonClientAddedEvent(clientKey, applicant, testDate);
            var applicantAddedEvent = GetLeadApplicantAddedEvent(applicationNumber, clientKey, applicationRoleKey, testDate);
            var marketingOptionsAddedEvent = GetMarketingOptionsAddedEvent(applicant.ApplicantMarketingOptions.ToList(), testDate);
            var applicantMarkedAsIncomeContributorEvent = GetApplicantMarkedAsIncomeContributorEvent(applicationRoleKey, testDate);

            var employerAddedEvent = GetEmployerAddedEvent(employerKey, applicant.Employments.First().Employer, testDate);

            var unconfirmedSalaryAddedEvent = GetSalariedEmploymentAddedEvent(clientKey,
                employmentKey,
                applicant.Employments.First() as SalariedEmploymentModel,
                testDate);

            var applicationEmploymentSetEvent = GetApplicationEmploymentTypeSetEvent(applicationNumber, employmentTypeKey, testDate);
            var householdIncomeDeterminedEvent = GetApplicationHouseholdIncomeDeterminedEvent(applicationNumber, householdIncome, testDate);

            var newBusinessPricedEvent = GetApplicationPricedEvent(applicationNumber, testDate);
            var newBusinessFundedEvent = GetApplicationFundedEvent(applicationNumber, testDate);

            var metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(AddNewPurchaseApplicationCommand)));
            domainProcess.HandleEvent(newPurchaseApplicationAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(AddNaturalPersonClientCommand))) { { "IdNumberOfAddedClient", applicant.IDNumber } };
            domainProcess.HandleEvent(naturalPersonClientAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(AddLeadApplicantToApplicationCommand)));
            domainProcess.HandleEvent(applicantAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(MakeApplicantAnIncomeContributorCommand)));
            domainProcess.HandleEvent(applicantMarkedAsIncomeContributorEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(AddMarketingOptionsToClientCommand)));
            domainProcess.HandleEvent(marketingOptionsAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, getGuidForCommandType(typeof(AddEmployerCommand)))
            {
                { "EmployeeIdNumber", applicant.IDNumber }
            };
            domainProcess.HandleEvent(employerAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(AddUnconfirmedSalariedEmploymentToClientCommand)));
            domainProcess.HandleEvent(unconfirmedSalaryAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(DetermineApplicationHouseholdIncomeCommand)));
            domainProcess.HandleEvent(householdIncomeDeterminedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(SetApplicationEmploymentTypeCommand)));
            domainProcess.HandleEvent(applicationEmploymentSetEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(PriceNewBusinessApplicationCommand)));
            domainProcess.HandleEvent(newBusinessPricedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                getGuidForCommandType(typeof(FundNewBusinessApplicationCommand)));
            domainProcess.HandleEvent(newBusinessFundedEvent, metadata);
        }

        private static ApplicantMadeIncomeContributorEvent GetApplicantMarkedAsIncomeContributorEvent(int applicationRoleKey, DateTime testDate)
        {
            return new ApplicantMadeIncomeContributorEvent(testDate, applicationRoleKey);
        }

        internal static MarketingOptionsAddedEvent GetMarketingOptionsAddedEvent(List<ApplicantMarketingOptionModel> applicantMarketingOptions,
            DateTime date)
        {
            var marketingOptions = new List<Services.Interfaces.ClientDomain.Models.MarketingOptionModel>();
            foreach (var option in applicantMarketingOptions)
            {
                marketingOptions.Add(new Services.Interfaces.ClientDomain.Models.MarketingOptionModel((int)option.MarketingOption, ""));
            }
            return new MarketingOptionsAddedEvent(date, marketingOptions);
        }

        public static NewPurchaseApplicationAddedEvent GetNewPurchaseApplicationAddedEvent(int applicationNumber,
            NewPurchaseApplicationCreationModel creationModel, DateTime date)
        {
            return new NewPurchaseApplicationAddedEvent(date,
                applicationNumber,
                creationModel.ApplicationType,
                creationModel.ApplicationStatus,
                creationModel.ApplicationSourceKey,
                creationModel.OriginationSource,
                creationModel.Deposit,
                creationModel.PurchasePrice,
                creationModel.Term,
                creationModel.Product);
        }

        public static SwitchApplicationAddedEvent GetSwitchApplicationAddedEvent(int applicationNumber, SwitchApplicationCreationModel creationModel,
            DateTime date)
        {
            return new SwitchApplicationAddedEvent(date,
                applicationNumber,
                creationModel.ApplicationType,
                creationModel.ApplicationStatus,
                creationModel.ApplicationSourceKey,
                creationModel.OriginationSource,
                creationModel.ExistingLoan,
                creationModel.EstimatedPropertyValue,
                creationModel.Term,
                creationModel.CashOut,
                creationModel.Product);
        }

        public static RefinanceApplicationAddedEvent GetRefinanceApplicationAddedEvent(int applicationNumber,
            RefinanceApplicationCreationModel creationModel, DateTime date)
        {
            return new RefinanceApplicationAddedEvent(new DateTime(2014, 10, 9),
                applicationNumber,
                creationModel.ApplicationType,
                creationModel.ApplicationStatus,
                creationModel.ApplicationSourceKey,
                creationModel.OriginationSource,
                creationModel.EstimatedPropertyValue,
                creationModel.Term,
                creationModel.CashOut,
                creationModel.Product);
        }

        public static NaturalPersonClientAddedEvent GetNaturalPersonClientAddedEvent(int clientKey, ApplicantModel applicant, DateTime date)
        {
            return new NaturalPersonClientAddedEvent(date,
                clientKey,
                applicant.MaritalStatus,
                applicant.Gender,
                applicant.PopulationGroup,
                date,
                applicant.Salutation,
                applicant.FirstName,
                applicant.Surname,
                applicant.PreferredName,
                applicant.IDNumber,
                applicant.PassportNumber,
                applicant.DateOfBirth,
                applicant.HomePhoneCode,
                applicant.HomePhone,
                applicant.WorkPhoneCode,
                applicant.WorkPhone,
                applicant.CellPhone,
                applicant.EmailAddress,
                applicant.FaxCode,
                applicant.FaxNumber,
                applicant.CitizenshipType,
                applicant.Education);
        }

        public static MailingAddressAddedToApplicationEvent GetApplicationMailingAddressAddedEvent(int applicationNumber, int clientAddressKey,
            ApplicationMailingAddressModel mailingAddress, DateTime date)
        {
            return new MailingAddressAddedToApplicationEvent(date,
                applicationNumber,
                clientAddressKey,
                mailingAddress.CorrespondenceLanguage,
                mailingAddress.OnlineStatementRequired,
                mailingAddress.OnlineStatementFormat,
                mailingAddress.CorrespondenceMedium,
                mailingAddress.ClientToUseForEmailCorrespondence);
        }

        public static LeadApplicantAddedToApplicationEvent GetLeadApplicantAddedEvent(int applicationNumber, int clientKey, int applicationRoleKey,
            DateTime date)
        {
            return new LeadApplicantAddedToApplicationEvent(date, applicationNumber, clientKey, applicationRoleKey);
        }

        internal static DebitOrderAddedToApplicationEvent GetDebitOrderAddedToApplicationEvent(int applicationNumber, int clientBankAccountKey,
            ApplicationDebitOrderModel debitOrder, DateTime date)
        {
            return new DebitOrderAddedToApplicationEvent(date,
                debitOrder.PaymentType,
                debitOrder.DebitOrderDay,
                clientBankAccountKey,
                applicationNumber);
        }

        internal static ApplicantAffordabilitiesAddedEvent GetApplicantAffordabilitiesAddedEvent(int clientKey, int applicationNumber,
            List<ApplicantAffordabilityModel> affordabilities, DateTime testDate)
        {
            var affordabilityAssessment = new List<Services.Interfaces.ApplicationDomain.Models.AffordabilityTypeModel>();
            foreach (var affordability in affordabilities)
            {
                affordabilityAssessment.Add(new Services.Interfaces.ApplicationDomain.Models.AffordabilityTypeModel(affordability.AffordabilityType,
                    (double)affordability.Amount,
                    affordability.Description));
            }
            return new ApplicantAffordabilitiesAddedEvent(testDate, clientKey, applicationNumber, affordabilityAssessment);
        }

        internal static DeclarationsAddedToApplicantEvent GetApplicantDeclarationsAddedEvent(int clientKey, int applicationNumber,
            IEnumerable<ApplicantDeclarationsModel> declarations, DateTime testDate)
        {
            var declaredInsolvent = declarations.First(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey);
            var adminOrderDeclaration =
                declarations.First(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.AdministrationOrderDateRescindedQuestionKey);
            var debtReview = declarations.First(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey);
            var hasDebtArrangement =
                declarations.First(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey);
            var creditCheckPermission =
                declarations.First(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey);
            return new DeclarationsAddedToApplicantEvent(testDate,
                clientKey,
                applicationNumber,
                testDate,
                declaredInsolvent.DeclarationAnswer,
                declaredInsolvent.DeclarationDate,
                adminOrderDeclaration.DeclarationAnswer,
                adminOrderDeclaration.DeclarationDate,
                debtReview.DeclarationAnswer,
                hasDebtArrangement.DeclarationAnswer == OfferDeclarationAnswer.Yes,
                creditCheckPermission.DeclarationAnswer);
        }

        internal static ComcorpOfferPropertyDetailsAddedEvent GetComcorpPropertyDetailAddedEvent(ComcorpApplicationPropertyDetailsModel property,
            DateTime date)
        {
            return new ComcorpOfferPropertyDetailsAddedEvent(date,
                property.SellerIDNo,
                property.SAHLOccupancyType,
                property.SAHLPropertyType,
                property.SAHLTitleType,
                property.SectionalTitleUnitNo,
                property.ComplexName,
                property.StreetNo,
                property.StreetName,
                property.Suburb,
                property.City,
                property.Province,
                property.PostalCode,
                property.ContactCellphone,
                property.ContactName,
                property.NamePropertyRegistered,
                property.StandErfNo,
                property.PortionNo);
        }

        public static DomiciliumAddressLinkedToApplicantEvent GetDomiciliumLinkedToApplicantEvent(int applicationDomiciliumRoleKey, int clientKey,
            int applicationNumber, int clientDomiciliumKey, DateTime date)
        {
            return new DomiciliumAddressLinkedToApplicantEvent(date, applicationDomiciliumRoleKey, clientKey, applicationNumber, clientDomiciliumKey);
        }

        public static EmployerAddedEvent GetEmployerAddedEvent(int employerKey, EmployerModel employer, DateTime date)
        {
            return new EmployerAddedEvent(date,
                employerKey,
                employer.EmployerName,
                employer.TelephoneCode,
                employer.TelephoneNumber,
                employer.ContactPerson,
                employer.ContactEmail,
                employer.EmployerBusinessType,
                employer.EmploymentSector);
        }

        public static UnconfirmedSalariedEmploymentAddedToClientEvent GetSalariedEmploymentAddedEvent(int clientKey, int employmentKey,
            SalariedEmploymentModel salariedEmployment, DateTime date)
        {
            return new UnconfirmedSalariedEmploymentAddedToClientEvent(date,
                clientKey,
                salariedEmployment.BasicIncome,
                salariedEmployment.StartDate.Value,
                salariedEmployment.EmploymentStatus,
                salariedEmployment.SalaryPaymentDay,
                salariedEmployment.Employer.EmployerName,
                salariedEmployment.Employer.TelephoneCode,
                salariedEmployment.Employer.TelephoneNumber,
                salariedEmployment.Employer.ContactPerson,
                salariedEmployment.Employer.ContactEmail,
                salariedEmployment.Employer.EmployerBusinessType,
                salariedEmployment.Employer.EmploymentSector,
                employmentKey);
        }

        public static ApplicationEmploymentTypeSetEvent GetApplicationEmploymentTypeSetEvent(int applicationNumber, int employmentTypeKey,
            DateTime date)
        {
            return new ApplicationEmploymentTypeSetEvent(date, applicationNumber, employmentTypeKey);
        }

        public static ApplicationHouseholdIncomeDeterminedEvent GetApplicationHouseholdIncomeDeterminedEvent(int applicationNumber,
            double householdIncome, DateTime date)
        {
            return new ApplicationHouseholdIncomeDeterminedEvent(date, applicationNumber, householdIncome);
        }

        public static NewBusinessApplicationPricedEvent GetApplicationPricedEvent(int applicationNumber, DateTime date)
        {
            return new NewBusinessApplicationPricedEvent(date, applicationNumber);
        }

        public static NewBusinessApplicationFundedEvent GetApplicationFundedEvent(int applicationNumber, DateTime date)
        {
            return new NewBusinessApplicationFundedEvent(date, applicationNumber);
        }

        public static ExternalVendorLinkedToApplicationEvent GetExternalVendorLinkedEvent(int applicationNumber,
            Services.Interfaces.ApplicationDomain.Models.VendorModel vendor, DateTime date)
        {
            return new ExternalVendorLinkedToApplicationEvent(applicationNumber,
                date,
                vendor.VendorKey,
                vendor.VendorCode,
                vendor.OrganisationStructureKey,
                vendor.LegalEntityKey,
                vendor.GeneralStatusKey);
        }

        public static FreeTextResidentialAddressLinkedToClientEvent GetFreeTextResidentialAddressLinkedToClientEvent(int clientKey,
            int clientAddressKey, FreeTextAddressModel freeTextAddressModel, DateTime date)
        {
            return new FreeTextResidentialAddressLinkedToClientEvent(date,
                freeTextAddressModel.FreeText1,
                freeTextAddressModel.FreeText2,
                freeTextAddressModel.FreeText3,
                freeTextAddressModel.FreeText4,
                freeTextAddressModel.FreeText5,
                freeTextAddressModel.Country,
                freeTextAddressModel.AddressFormat,
                clientKey,
                clientAddressKey);
        }

        public static FreeTextPostalAddressLinkedToClientEvent GetFreeTextPostalAddressLinkedToClientEvent(int clientKey, int clientAddressKey,
            FreeTextAddressModel freeTextAddressModel, DateTime date)
        {
            return new FreeTextPostalAddressLinkedToClientEvent(date,
                freeTextAddressModel.FreeText1,
                freeTextAddressModel.FreeText2,
                freeTextAddressModel.FreeText3,
                freeTextAddressModel.FreeText4,
                freeTextAddressModel.FreeText5,
                freeTextAddressModel.Country,
                freeTextAddressModel.AddressFormat,
                clientKey,
                clientAddressKey);
        }

        public static ResidentialStreetAddressLinkedToClientEvent GetResidentialStreeAddressLinkedToClientEventFromPropertyAddress(int clientKey,
            int clientAddressKey, PropertyAddressModel address, DateTime date)
        {
            return new ResidentialStreetAddressLinkedToClientEvent(date,
                address.UnitNumber,
                address.BuildingNumber,
                address.BuildingName,
                address.StreetNumber,
                address.StreetName,
                address.Suburb,
                address.City,
                address.Province,
                address.PostalCode,
                clientKey,
                clientAddressKey);
        }

        public static ClientAddressAsPendingDomiciliumAddedEvent GetClientDomiciliumAddedEvent(int clientAddressKey, int clientDomiciliumKey,
            DateTime date)
        {
            return new ClientAddressAsPendingDomiciliumAddedEvent(date, clientAddressKey, clientDomiciliumKey);
        }

        public static BankAccountLinkedToClientEvent GetBankAccountLinkedToClientEvent(int bankAccountKey, int clientKey, int clientBankAccountKey,
            BankAccountModel bankAccount, DateTime date)
        {
            return new BankAccountLinkedToClientEvent(date,
                bankAccountKey,
                clientKey,
                clientBankAccountKey,
                bankAccount.AccountName,
                bankAccount.AccountNumber,
                bankAccount.BranchCode,
                bankAccount.BranchName);
        }

        internal static LiabilitySuretyAddedToClientEvent GetLiabilitySuretyAddedEvent(ApplicantAssetLiabilityModel surety, int suretyLiabilityKey,
            int clientSuretyLiabilityKey, DateTime testDate)
        {
            return new LiabilitySuretyAddedToClientEvent(testDate,
                suretyLiabilityKey,
                clientSuretyLiabilityKey,
                surety.AssetValue.Value,
                surety.LiabilityValue.Value,
                surety.Description);
        }

        internal static OtherAssetAddedToClientEvent GetOtherAssetAddedToClientEvent(ApplicantAssetLiabilityModel otherAsset,
            int clientAssetLiabilityKey, DateTime testDate)
        {
            return new OtherAssetAddedToClientEvent(testDate,
                clientAssetLiabilityKey,
                otherAsset.Description,
                otherAsset.AssetValue.Value,
                otherAsset.LiabilityValue.Value);
        }

        internal static FixedPropertyAssetAddedToClientEvent GetFixedPropertyAssetAddedEvent(
            ApplicantAssetLiabilityModel applicantAssetLiabilityModel, int addressKey, DateTime testDate)
        {
            return new FixedPropertyAssetAddedToClientEvent(testDate,
                applicantAssetLiabilityModel.Date.Value,
                addressKey,
                applicantAssetLiabilityModel.AssetValue.Value,
                applicantAssetLiabilityModel.LiabilityValue.Value);
        }

        internal static InvestmentAssetAddedToClientEvent GetInvestmentAssetAddedEvent(ApplicantAssetLiabilityModel applicantAssetLiabilityModel,
            AssetInvestmentType investmentType, DateTime testDate)
        {
            return new InvestmentAssetAddedToClientEvent(testDate,
                investmentType,
                applicantAssetLiabilityModel.CompanyName,
                applicantAssetLiabilityModel.AssetValue.Value);
        }

        internal static LifeAssuranceAssetAddedToClientEvent GetLifeAssuranceAddedEvent(ApplicantAssetLiabilityModel applicantAssetLiabilityModel,
            DateTime testDate)
        {
            return new LifeAssuranceAssetAddedToClientEvent(testDate,
                applicantAssetLiabilityModel.CompanyName,
                applicantAssetLiabilityModel.AssetValue.Value);
        }

        internal static LiabilityLoanAddedToClientEvent GetLiabilityLoanAddedEvent(ApplicantAssetLiabilityModel liabilityLoan, DateTime testDate)
        {
            return new LiabilityLoanAddedToClientEvent(testDate,
                liabilityLoan.AssetLiabilitySubType.Value,
                liabilityLoan.CompanyName,
                liabilityLoan.Date.Value,
                liabilityLoan.Cost.Value,
                liabilityLoan.LiabilityValue.Value);
        }

        internal static FixedLongTermInvestmentLiabilityAddedToClientEvent GetFixedLongTermInvestmentAddedEvent(
            ApplicantAssetLiabilityModel fixedInvestment, DateTime testDate)
        {
            return new FixedLongTermInvestmentLiabilityAddedToClientEvent(testDate, fixedInvestment.CompanyName, fixedInvestment.LiabilityValue.Value);
        }
    }
}
