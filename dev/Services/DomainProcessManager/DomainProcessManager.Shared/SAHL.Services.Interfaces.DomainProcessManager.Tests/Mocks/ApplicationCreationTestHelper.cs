using System;
using System.Collections.Generic;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests.Mocks;

namespace SAHL.Services.Interfaces.DomainProcessManager.Tests
{
    public static class ApplicationCreationTestHelper
    {
        public static ApplicationCreationModel PopulateApplicationCreationModel(OfferType offerType,
            EmploymentType employmentType = EmploymentType.Salaried)
        {
            return PopulateApplicationCreationModel(offerType, "", "AN388344", employmentType);
        }

        public static ApplicationCreationModel PopulateApplicationCreationModel(OfferType offerType, string idNumber, string passportNumber,
            EmploymentType employmentType = EmploymentType.Salaried)
        {
            var bankAccount = PopulateBankAccountModel();
            List<BankAccountModel> bankAccounts = new List<BankAccountModel> { bankAccount };
            List<EmploymentModel> employments = new List<EmploymentModel>();

            if (employmentType == EmploymentType.Salaried)
            {
                employments.Add(PopulateSalariedEmploymentModel());
            }
            else if (employmentType == EmploymentType.SalariedwithDeduction)
            {
                employments.Add(PopulateSalaryDeductionEmploymentModel());
            }
            else if (employmentType == EmploymentType.Unemployed)
            {
                employments.Add(PopulateUnemployedEmploymentModel());
            }

            List<AddressModel> addresses = new List<AddressModel>();
            var residentialAddress = PopulateFreeTextResidentialAddressModel();
            addresses.Add(residentialAddress);
            var postalAddress = PopulateFreeTextPostalAddressModel();
            addresses.Add(postalAddress);
            var propertyResidentialAddress = PopulatePropertyAddressAsResidential();
            addresses.Add(propertyResidentialAddress);

            var affordabilities = new List<ApplicantAffordabilityModel>();
            affordabilities.Add(new ApplicantAffordabilityModel(AffordabilityType.BasicSalary, "Description", 20000));
            affordabilities.Add(new ApplicantAffordabilityModel(AffordabilityType.Childsupport, "Description", 10000));

            var applicationDeclarations = PopulateApplicantDeclarations();

            var applicantAssetLiabilities = new List<ApplicantAssetLiabilityModel>();

            ApplicantModel applicant = new ApplicantModel("8001045100005",
                passportNumber,
                SalutationType.Mr,
                "Bob",
                "Murry",
                "B",
                "Martin Murry",
                Gender.Male,
                MaritalStatus.Single,
                PopulationGroup.Unknown,
                CitizenType.SACitizen,
                DateTime.Now.AddYears(-35),
                Language.English,
                CorrespondenceLanguage.English,
                "031",
                "2115648",
                string.Empty,
                string.Empty,
                "0825544785",
                string.Empty,
                string.Empty,
                string.Empty,
                LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant,
                bankAccounts,
                employments,
                addresses,
                affordabilities,
                applicationDeclarations,
                applicantAssetLiabilities,
                PopulateApplicantMarketingOptions(),
                Education.Unknown,
                true);
            List<ApplicantModel> applicants = new List<ApplicantModel> { applicant };

            return PopulateApplicationCreationModelWithApplicants(offerType, applicants);
        }

        public static PropertyAddressModel CreatePropertyAddressModel()
        {
            return new PropertyAddressModel(
                "",
                "",
                "",
                "7",
                "Maryland Avenue",
                "Durban North",
                "Durban",
                "Kwazulu-Natal",
                "4051",
                "123456",
                "789456",
                "South Africa",
                true
                );
        }

        public static ApplicationCreationModel PopulateApplicationCreationModelWithApplicants(OfferType offerType, List<ApplicantModel> applicants)
        {
            var factory = new PurchaseApplicationFactory();
            return factory.Create(offerType, applicants);
        }

        public static ApplicantModel PopulateApplicantModel(List<AddressModel> addresses)
        {
            var bankAccounts = new List<BankAccountModel> { PopulateBankAccountModel() };
            var employments = new List<EmploymentModel> { PopulateSalariedEmploymentModel() };
            var affordabilities = new List<ApplicantAffordabilityModel>();
            affordabilities.Add(new ApplicantAffordabilityModel(AffordabilityType.BasicSalary, "Description", 20000));
            affordabilities.Add(new ApplicantAffordabilityModel(AffordabilityType.Childsupport, "Description", 10000));

            return PopulateApplicantModel(bankAccounts,
                employments,
                addresses,
                affordabilities,
                PopulateApplicantDeclarations(),
                PopulateApplicantAssetLiabilities(),
                PopulateApplicantMarketingOptions());
        }

        public static ApplicantModel PopulateApplicantModel(List<BankAccountModel> bankAccounts, List<EmploymentModel> employments,
            List<AddressModel> addresses,
            List<ApplicantAffordabilityModel> affordabilities, List<ApplicantDeclarationsModel> applicantDeclarations,
            List<ApplicantAssetLiabilityModel> applicantAssetLiabilities, List<ApplicantMarketingOptionModel> marketingOptions)
        {
            return new ApplicantModel("8001045000007",
                "",
                SalutationType.Mr,
                "bob",
                "smith",
                "b",
                "bobby smith",
                Gender.Male,
                MaritalStatus.Single,
                PopulationGroup.Unknown,
                CitizenType.SACitizen,
                DateTime.Now.AddYears(-35),
                Language.English,
                CorrespondenceLanguage.English,
                "031",
                "2233415",
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant,
                bankAccounts,
                employments,
                addresses,
                affordabilities,
                applicantDeclarations,
                applicantAssetLiabilities,
                marketingOptions,
                Education.Unknown,
                true);
        }

        public static ResidentialAddressModel PopulatePropertyAddressModel(bool isDomicilium)
        {
            return new ResidentialAddressModel("25",
                "Mandela Street",
                null,
                "25",
                "WestWing",
                "La Lucia",
                "Umhlanga",
                "Kwazulu-Natal",
                "4051",
                "South Africa",
                isDomicilium);
        }

        public static FreeTextAddressModel PopulateFreeTextResidentialAddressModel()
        {
            return new FreeTextAddressModel(
                AddressType.Residential,
                "25",
                "Mandela Street",
                "25 WestWing, La Lucia",
                "Umhlanga",
                "Kwazulu-Natal, 4051",
                "South Africa"
                );
        }

        public static FreeTextAddressModel PopulateFreeTextPostalAddressModel()
        {
            return new FreeTextAddressModel(
                AddressType.Postal,
                "256",
                "Post Office",
                "Glenwood, 4001",
                "Durban",
                "Kwazulu-Natal",
                "South Africa"
                );
        }

        public static ApplicationMailingAddressModel PopulateApplicationMailingAddressModel(AddressModel address)
        {
            return new ApplicationMailingAddressModel(address,
                CorrespondenceLanguage.English,
                OnlineStatementFormat.HTML,
                CorrespondenceMedium.Email,
                null,
                true);
        }

        public static BankAccountModel PopulateBankAccountModel()
        {
            return new BankAccountModel("051001", "STANDARD BANK SOUTH AFRICA", "302879056", ACBType.Current, "Account Name", "System", true);
        }

        public static SalariedEmploymentModel PopulateSalariedEmploymentModel()
        {
            EmployerModel employer = new EmployerModel(null,
                "Employer Name",
                "031",
                "5551234",
                "Contact Person",
                "email@contact.com",
                EmployerBusinessType.Company,
                EmploymentSector.FinancialServices);
            return new SalariedEmploymentModel(50000,
                25,
                employer,
                DateTime.Now.AddYears(-1),
                SalariedRemunerationType.Salaried,
                EmploymentStatus.Current,
                new List<EmployeeDeductionModel>
                {
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 111),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 222),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 333),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 444)
                });
        }

        public static SalaryDeductionEmploymentModel PopulateSalaryDeductionEmploymentModel()
        {
            EmployerModel employer = new EmployerModel(null,
                "Employer Name",
                "031",
                "5551234",
                "Contact Person",
                "email@contact.com",
                EmployerBusinessType.Company,
                EmploymentSector.FinancialServices);
            return new SalaryDeductionEmploymentModel(50000,
                5000,
                25,
                employer,
                DateTime.Now.AddYears(-1),
                SalaryDeductionRemunerationType.Salaried,
                EmploymentStatus.Current,
                new List<EmployeeDeductionModel>
                {
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 111),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 222),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 333),
                    new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 444)
                });
        }

        public static UnemployedEmploymentModel PopulateUnemployedEmploymentModel()
        {
            EmployerModel employer = new EmployerModel(null,
                "Unemployed",
                "031",
                "5551234",
                null,
                null,
                EmployerBusinessType.Unknown,
                EmploymentSector.FinancialServices);
            return new UnemployedEmploymentModel(50000,
                25,
                employer,
                null,
                UnemployedRemunerationType.InvestmentIncome
                ,
                EmploymentStatus.Current);
        }

        public static ComcorpApplicationPropertyDetailsModel PopulateComcorpPropertyDetailsModel()
        {
            return new ComcorpApplicationPropertyDetailsModel("14567890123",
                "",
                "",
                "",
                "",
                "Hathaway heights",
                "42",
                "StreetOne",
                "DurbanVille",
                "Durban",
                "Kwazulu-Natal",
                "4451",
                "0725564785",
                "Red John",
                "",
                "1234",
                "5678");
        }

        public static PropertyAddressModel PopulatePropertyAddressModel()
        {
            var propertyResidentialAddress = PopulatePropertyAddressAsResidential();
            var comcorpPropertyDetailsModel = PopulateComcorpPropertyDetailsModel();
            return new PropertyAddressModel(
                propertyResidentialAddress.UnitNumber,
                propertyResidentialAddress.BuildingName,
                propertyResidentialAddress.BuildingNumber,
                propertyResidentialAddress.StreetNumber,
                propertyResidentialAddress.StreetName,
                propertyResidentialAddress.Suburb,
                propertyResidentialAddress.City,
                propertyResidentialAddress.Province,
                propertyResidentialAddress.PostalCode,
                comcorpPropertyDetailsModel.StandErfNo,
                comcorpPropertyDetailsModel.PortionNo,
                propertyResidentialAddress.Country,
                propertyResidentialAddress.IsDomicilium);
        }

        public static ResidentialAddressModel PopulatePropertyAddressAsResidential()
        {
            return new ResidentialAddressModel(
                "7",
                "Maryland Avenue",
                "",
                "",
                "",
                "Durban North",
                "Durban",
                "Kwazulu-Natal",
                "4051",
                "South Africa",
                true
                );
        }

        public static List<ApplicantDeclarationsModel> PopulateApplicantDeclarations()
        {
            var applicantDeclarations = new List<ApplicantDeclarationsModel>();

            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey,
                OfferDeclarationAnswer.Yes,
                null));
            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey,
                OfferDeclarationAnswer.Date,
                DateTime.Now.AddYears(-1)));

            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey,
                OfferDeclarationAnswer.No,
                null));
            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.AdministrationOrderDateRescindedQuestionKey,
                OfferDeclarationAnswer.Date,
                null));

            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey,
                OfferDeclarationAnswer.No,
                null));
            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey,
                OfferDeclarationAnswer.No,
                null));

            applicantDeclarations.Add(new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey,
                OfferDeclarationAnswer.Yes,
                DateTime.Now));

            return applicantDeclarations;
        }

        public static List<ApplicantAssetLiabilityModel> PopulateApplicantAssetLiabilities()
        {
            var assetsAndLiabilities = new List<ApplicantAssetLiabilityModel>();
            var assets = PopulateApplicantAssets();
            assetsAndLiabilities.AddRange(assets);
            var liabilities = PopulateApplicantLiabilities();
            assetsAndLiabilities.AddRange(liabilities);
            return assetsAndLiabilities;
        }

        public static List<ApplicantAssetLiabilityModel> PopulateApplicantAssets()
        {
            var assets = new List<ApplicantAssetLiabilityModel>();

            var address = new FreeTextAddressModel(
                AddressType.Residential,
                "29",
                "Thabo Street",
                "25 WestWing, La Lucia",
                "Umhlanga",
                "Kwazulu-Natal, 4051",
                "South Africa"
                );

            assets.Add(new ApplicantFixedPropertyAssetModel(address, 300000, 20000, DateTime.Now.AddYears(-5)));
            assets.Add(new ApplicantListedInvestmentAssetModel(10000, "company name"));
            assets.Add(new ApplicantUnListedInvestmentAssetModel(10000, "company name"));
            assets.Add(new ApplicantOtherAssetModel(10000, 0, "description"));
            return assets;
        }

        public static List<ApplicantAssetLiabilityModel> PopulateApplicantLiabilities()
        {
            var liabitilies = new List<ApplicantAssetLiabilityModel>();
            liabitilies.Add(new ApplicantLifeAssuranceAssetModel(1000000, "company name"));
            liabitilies.Add(new ApplicantLiabilityLoanModel(AssetLiabilitySubType.PersonalLoan, "fin institution", 3000, 100, DateTime.Now.AddYears(3)));
            liabitilies.Add(new ApplicantLiabilitySuretyModel(50000, 5000, "description"));
            liabitilies.Add(new ApplicantFixedLongTermLiabilityModel(10000, "company name"));
            return liabitilies;
        }

        public static List<ApplicantMarketingOptionModel> PopulateApplicantMarketingOptions()
        {
            var applicantMarketingOptions = new List<ApplicantMarketingOptionModel>
            {
                new ApplicantMarketingOptionModel(MarketingOption.CustomerLists, GeneralStatus.Inactive),
                new ApplicantMarketingOptionModel(MarketingOption.Email, GeneralStatus.Active),
                new ApplicantMarketingOptionModel(MarketingOption.Marketing, GeneralStatus.Inactive),
                new ApplicantMarketingOptionModel(MarketingOption.SMS, GeneralStatus.Inactive),
                new ApplicantMarketingOptionModel(MarketingOption.Telemarketing, GeneralStatus.Inactive)
            };

            return applicantMarketingOptions;
        }

        public static List<ApplicantAffordabilityModel> PopulateIncomeAndExpenses()
        {
            var applicantIncomeAndExpenses = new List<ApplicantAffordabilityModel>();
            var incomes = PopulateIncomes();
            applicantIncomeAndExpenses.AddRange(incomes);
            var expenses = PopulateExpenses();
            applicantIncomeAndExpenses.AddRange(expenses);
            return applicantIncomeAndExpenses;
        }

        public static List<ApplicantAffordabilityModel> PopulateIncomes()
        {
            return new List<ApplicantAffordabilityModel> { new ApplicantAffordabilityModel(AffordabilityType.BasicSalary, string.Empty, 60000) };
        }

        public static List<ApplicantAffordabilityModel> PopulateExpenses()
        {
            return new List<ApplicantAffordabilityModel>
            {
                new ApplicantAffordabilityModel(AffordabilityType.Foodandgroceries, string.Empty, 20000)
            };
        }

        public static List<EmploymentModel> PopulateEmployments()
        {
            var employment = PopulateSalariedEmploymentModel();
            return new List<EmploymentModel> { employment };
        }

        public static List<AddressModel> PopulateAddresses()
        {
            var postalAddress = PopulateFreeTextPostalAddressModel();
            var residentialAddress = PopulateFreeTextResidentialAddressModel();
            return new List<AddressModel> { postalAddress, residentialAddress };
        }

        public static List<BankAccountModel> PopulateBankAccounts()
        {
            var debitOrderBankAccount = PopulateBankAccountModel();
            debitOrderBankAccount.IsDebitOrderBankAccount = true;
            var nonDebitOrderBankAccount = PopulateBankAccountModel();
            nonDebitOrderBankAccount.IsDebitOrderBankAccount = false;
            return new List<BankAccountModel> { debitOrderBankAccount, nonDebitOrderBankAccount };
        }

        public static List<ApplicantMarketingOptionModel> PopulateMarketingOptions()
        {
            return new List<ApplicantMarketingOptionModel>
            {
                new ApplicantMarketingOptionModel(MarketingOption.SMS, GeneralStatus.Active)
            };
        }
    }
}
