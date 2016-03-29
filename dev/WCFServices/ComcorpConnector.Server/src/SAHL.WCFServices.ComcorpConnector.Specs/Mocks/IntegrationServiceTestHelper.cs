using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks
{
    public static class IntegrationServiceTestHelper
    {
        public enum ComcorpApplicantType
        {
            MainApplicant,
            Spouse,
            CoApplicant
        }

        public static Application SetupBasicApplication(MortgageLoanPurpose mortgageLoanPurpose)
        {
            Application comcorpApplication = new Application();

            switch (mortgageLoanPurpose)
            {
                case MortgageLoanPurpose.Newpurchase:
                    comcorpApplication.SahlLoanPurpose = "New Purchase Loan";
                    comcorpApplication.SahlTransferAtt = "Jackie Barrows";
                    break;

                case MortgageLoanPurpose.Refinance:
                    comcorpApplication.SahlLoanPurpose = "Refinance";
                    break;

                case MortgageLoanPurpose.Switchloan:
                    comcorpApplication.SahlLoanPurpose = "Switch Loan";
                    break;

                default:
                    break;
            }

            comcorpApplication.ProductSelection = "New Variable Loan";
            comcorpApplication.SAHLPropertyType = "Cluster Home";
            comcorpApplication.SAHLTitleType = "Sectional Title  ";
            comcorpApplication.SAHLOccupancyType = "Owner Occupied";
            comcorpApplication.ConsultantFirstName = "ConsultantFirstName";
            comcorpApplication.ConsultantSurname = "ConsultantSurname";

            comcorpApplication.LoanAmountRequired = 1000000;

            comcorpApplication.PropertyPurchasePrice = 1000000;

            comcorpApplication.DepositCash = 100000;
            comcorpApplication.TermOfLoan = 20; // comcorp are setting this in years

            comcorpApplication.SAHLVendorCode = "Vender001";

            // main applicant
            MainApplicant mainApplicant = PopulateComcorpApplicant(ComcorpApplicantType.MainApplicant,
                MaritalStatus.Married_AnteNuptualContract,
                false,
                false,
                false,
                false,
                true) as MainApplicant;
            mainApplicant.AssetItems = PopulateAssetItems();
            mainApplicant.LiabilityItems = PopulateLiabilityItems();
            mainApplicant.IncomeItems = PopulateIncomeItems();
            mainApplicant.ExpenditureItems = PopulateExpenditureItems();
            mainApplicant.BankAccounts = PopulateBankAccounts();

            mainApplicant.PreferredContactDelivery = "preferred contact delivery";

            // co applicants
            CoApplicant coApplicant = PopulateComcorpApplicant(ComcorpApplicantType.CoApplicant,
                MaritalStatus.Married_AnteNuptualContract,
                true,
                true,
                true,
                true,
                true) as CoApplicant;

            coApplicant.AssetItems = PopulateAssetItems();
            coApplicant.LiabilityItems = PopulateLiabilityItems();
            coApplicant.IncomeItems = PopulateIncomeItems();
            coApplicant.ExpenditureItems = PopulateExpenditureItems();
            coApplicant.BankAccounts = PopulateBankAccounts();

            Property comcorpProperty = PopulateComcorpPropertyModel(true, true);

            comcorpApplication.MainApplicant = mainApplicant;
            comcorpApplication.CoApplicants = new List<CoApplicant> { coApplicant };
            comcorpApplication.Property = comcorpProperty;

            comcorpApplication.PropertyMarketValue = 500000;

            return comcorpApplication;
        }

        public static Property PopulateComcorpPropertyModel(bool usePropertyAddressAsLoanMailingAddress, bool usePropertyAddressAsDomiciliumAddress)
        {
            Property property = new Property();
            property.PortionNo = "erfPortionNo";
            property.StandErfNo = "erf no";
            property.StreetNo = "15";
            property.StreetName = "StreetName";
            property.AddressSuburb = "Durban North";
            property.AddressCity = "Durban";
            property.PostalCode = "4051";
            property.Province = "Kwazulu Natal";
            property.SellerIDNo = "68120952q9087";

            property.UsePropertyAddressAsLoanMailingAddress = usePropertyAddressAsLoanMailingAddress;
            property.UsePropertyAddressAsDomiciliumAddress = usePropertyAddressAsDomiciliumAddress;

            return property;
        }

        public static Applicant PopulateComcorpApplicant(ComcorpApplicantType comcorpApplicantType, MaritalStatus maritalStatus, bool isSurety,
            bool declarationsInsolvent, bool declarationsDebtCounselling, bool declarationsAdminOrder, bool canDoCreditBureauEnquiry)
        {
            Applicant applicant = CreateApplicant(comcorpApplicantType);

            SetMaritalStatus(maritalStatus, ref  applicant);

            applicant.EthnicGroup = "White";
            applicant.SAHLSACitizenType = "SA Citizen";
            applicant.HomeLanguage = "English";
            applicant.CorrespondenceLanguage = "English";
            applicant.PreferredContactDelivery = "preferred contact delivery";
            applicant.SAHLIsSurety = isSurety;

            // declarations
            applicant.DeclaredInsolvent = declarationsInsolvent;
            if (declarationsInsolvent)
            {
                applicant.DateRehabilitated = DateTime.Now.AddYears(-5);
            }

            applicant.IsUnderAdminOrder = declarationsAdminOrder;
            if (declarationsAdminOrder)
            {
                applicant.AdminRescinded = DateTime.Now.AddYears(-5);
            }

            applicant.IsUnderDebtReview = declarationsDebtCounselling;
            if (declarationsDebtCounselling)
            {
                applicant.DebtRearrangement = true;
            }

            applicant.MayDoCreditBureauEnquiry = canDoCreditBureauEnquiry;

            applicant.EmployerName = "Employer Name2";
            applicant.EmployerEmail = "contact@employer.com";
            applicant.DateJoinedEmployer = DateTime.Now.AddYears(-3);
            applicant.CurrentEmploymentType = "Salaried";
            applicant.EmploymentSector = "Financial Services";
            applicant.EmployerBusinessType = "Company";
            applicant.DateSalaryPaid = 29;
            applicant.EmployerGrossMonthlySalary = 15000;

            applicant.HomePhoneCode = "031";
            applicant.HomePhone = "5551234";
            applicant.WorkPhoneCode = "031";
            applicant.WorkPhone = "5555555";
            applicant.Cellphone = "083555555";
            applicant.EmailAddress = "email@address.com";
            applicant.FaxCode = "031";
            applicant.FaxNo = "5512345";

            applicant.PhysicalAddressLine1 = "5";
            applicant.PhysicalAddressLine2 = "Clarendon Drive";
            applicant.PhysicalAddressSuburb = "Durban North";
            applicant.PhysicalAddressCity = "Durban";
            applicant.PhysicalAddressCode = "4051";
            applicant.PhysicalProvince = "Kwazulu Natal";
            applicant.PhysicalCountry = "South Africa";

            applicant.PostalAddressLine1 = "PO Box";
            applicant.PostalAddressLine2 = "1883";
            applicant.PostalAddressSuburb = "Durban";
            applicant.PostalAddressCity = "Durban";
            applicant.PostalAddressCode = "4001";
            applicant.PhysicalProvince = "Kwazulu Natal";
            applicant.PostalCountry = "South Africa";

            return applicant;
        }

        private static Applicant CreateApplicant(ComcorpApplicantType comcorpApplicantType)
        {
            switch (comcorpApplicantType)
            {
                case ComcorpApplicantType.MainApplicant:
                    return CreateMainApplicant();

                case ComcorpApplicantType.Spouse:
                    return CreateSpouse();

                case ComcorpApplicantType.CoApplicant:
                    return CreateCoApplicant();
                default:
                    return new Applicant();

            }
        }

        private static Applicant CreateCoApplicant()
        {
            var applicant = new CoApplicant();

            applicant.Title = "Mrs";
            applicant.Gender = "Female";
            applicant.FirstName = "Nicolette";
            applicant.Surname = "Hughes";
            applicant.PreferredName = null;

            applicant.DateOfBirth = new DateTime(1971, 9, 16);
            applicant.IdentificationNo = "7109160015089";

            applicant.MarketingTelemarketing = true;
            applicant.MarketingMarketing = false;
            applicant.MarketingConsumerLists = false;
            applicant.MarketingSMS = false;
            applicant.MarketingEmail = false;

            applicant.SAHLHighestQualification = "Unknown";

            applicant.EmployeeUIF = 1000;
            applicant.EmployeePAYE = 2000;
            applicant.EmployeeMedicalAid = 0;
            applicant.EmployeePension = 2500;
            return applicant;
        }

        private static Applicant CreateSpouse()
        {
            var applicant = new Spouse();

            applicant.Title = "Mrs";
            applicant.Gender = "Female";
            applicant.FirstName = "Nicolette";
            applicant.Surname = "Hughes";
            applicant.PreferredName = null;

            applicant.DateOfBirth = new DateTime(1971, 9, 16);
            applicant.IdentificationNo = "7109160015089";

            applicant.MarketingTelemarketing = true;
            applicant.MarketingMarketing = false;
            applicant.MarketingConsumerLists = false;
            applicant.MarketingSMS = false;
            applicant.MarketingEmail = false;

            applicant.SAHLHighestQualification = "Unknown";

            applicant.EmployeeUIF = 1000;
            applicant.EmployeePAYE = 2000;
            applicant.EmployeeMedicalAid = 0;
            applicant.EmployeePension = 2500;
            return applicant;
        }

        private static Applicant CreateMainApplicant()
        {
            var applicant = new MainApplicant();

            applicant.Title = "Mr";
            applicant.Gender = "Male";
            applicant.FirstName = "Craig Graham";
            applicant.Surname = "Fraser";
            applicant.PreferredName = "Craig";

            applicant.DateOfBirth = new DateTime(1968, 12, 9);
            applicant.IdentificationNo = "6812095219087";

            applicant.MarketingTelemarketing = true;
            applicant.MarketingMarketing = true;
            applicant.MarketingConsumerLists = true;
            applicant.MarketingSMS = true;
            applicant.MarketingEmail = true;

            applicant.SAHLHighestQualification = "Matric";

            applicant.EmployeeUIF = 1000;
            applicant.EmployeePAYE = 2000;
            applicant.EmployeeMedicalAid = 1500;
            applicant.EmployeePension = 2500;
            return applicant;
        }

        private static void SetMaritalStatus(MaritalStatus maritalStatus, ref  Applicant applicant)
        {
            switch (maritalStatus)
            {
                case MaritalStatus.Divorced:
                    applicant.MaritalStatus = "Divorced";
                    break;

                case MaritalStatus.Married_AnteNuptualContract:
                    applicant.MaritalStatus = "Married - Ante Nuptual Contract";
                    break;

                case MaritalStatus.Married_CommunityofProperty:
                    applicant.MaritalStatus = "Married - Community of Property";
                    break;

                case MaritalStatus.Married_Foreign:
                    applicant.MaritalStatus = "Married – Foreign";
                    break;

                case MaritalStatus.Single:
                    applicant.MaritalStatus = "Single";
                    break;

                case MaritalStatus.Widowed:
                    applicant.MaritalStatus = "Widowed";
                    break;

                default:
                    break;
            }
        }

        public static List<AssetItem> PopulateAssetItems()
        {
            List<AssetItem> assetItems = new List<AssetItem>
            {
                new AssetItem
                {
                    SAHLAssetDesc = "Fixed Property",
                    SAHLAssetValue = 200000,
                    DateAssetAcquired = DateTime.Now.AddYears(-2),
                    AssetPhysicalAddressLine1 = "5",
                    AssetPhysicalAddressLine2 = "Clarendon Drive",
                    AssetPhysicalAddressSuburb = "Durban North",
                    AssetPhysicalAddressCity = "Durban",
                    AssetPhysicalAddressCode = "4051",
                    AssetPhysicalProvince = "Kwazulu Natal",
                    AssetPhysicalCountry = "South Africa"
                },
                new AssetItem { SAHLAssetDesc = "Listed Investments", AssetCompanyName = "Listed Investment Company Name", SAHLAssetValue = 10000 },
                new AssetItem { SAHLAssetDesc = "UnListed Investments", AssetCompanyName = "UnListed Investment Company Name", SAHLAssetValue = 4000 },
                new AssetItem
                {
                    SAHLAssetDesc = "Other Asset",
                    AssetDescription = "Other Asset Description",
                    SAHLAssetValue = 1000,
                    AssetOutstandingLiability = 500
                },
                new AssetItem { SAHLAssetDesc = "Life Assurance", AssetCompanyName = "Life Assurance Company Name", SAHLAssetValue = 15000 }
            };

            return assetItems;
        }

        public static List<LiabilityItem> PopulateLiabilityItems()
        {
            List<LiabilityItem> liabilityItems = new List<LiabilityItem>
            {
                new LiabilityItem
                {
                    SAHLLiabilityDesc = "Liability Loan",
                    LiabilityLoanType = "Personal Loan",
                    LiabilityCompanyName = "Personal Loan Company Name",
                    SAHLLiabilityValue = 15000,
                    LiabilityDateRepayable = DateTime.Now.AddYears(1),
                    SAHLLiabilityCost = 500
                },
                new LiabilityItem
                {
                    SAHLLiabilityDesc = "Liability Loan",
                    LiabilityLoanType = "Student Loan",
                    LiabilityCompanyName = "Student Loan Company Name",
                    SAHLLiabilityValue = 5000,
                    LiabilityDateRepayable = DateTime.Now.AddYears(2),
                    SAHLLiabilityCost = 300
                },
                new LiabilityItem
                {
                    SAHLLiabilityDesc = "Liability Surety",
                    LiabilityDescription = "Liability Surety Description",
                    SAHLLiabilityValue = 1000,
                    LiabilityAssetValue = 1500
                },
                new LiabilityItem
                {
                    SAHLLiabilityDesc = "Fixed Long Term Investment",
                    LiabilityCompanyName = "Fixed Long Term Investment Company Name",
                    SAHLLiabilityValue = 1000
                }
            };

            return liabilityItems;
        }

        public static List<IncomeItem> PopulateIncomeItems()
        {
            List<IncomeItem> incomeItems = new List<IncomeItem>
            {
                new IncomeItem { IncomeType = 1, IncomeDesc = "Basic Salary", IncomeAmount = 1000 },
                new IncomeItem { IncomeType = 1, IncomeDesc = "Commission and Overtime", IncomeAmount = 1000 },
                new IncomeItem { IncomeType = 1, IncomeDesc = "Rental", IncomeAmount = 1000 },
                new IncomeItem
                {
                    IncomeType = 1,
                    IncomeDesc = "Income from Investments",
                    IncomeAmount = 1000,
                    CapturedDescription = "Income from Investments Description"
                },
                new IncomeItem
                {
                    IncomeType = 1,
                    IncomeDesc = "Other Income 1",
                    IncomeAmount = 4444,
                    CapturedDescription = "Other Income 1 Description"
                },
                new IncomeItem
                {
                    IncomeType = 1,
                    IncomeDesc = "Other Income 2",
                    IncomeAmount = 5555,
                    CapturedDescription = "Other Income 2 Description"
                },
            };

            return incomeItems;
        }

        public static List<ExpenditureItem> PopulateExpenditureItems()
        {
            List<ExpenditureItem> expenditureItems = new List<ExpenditureItem>
            {
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Planned Savings", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Clothing", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Credit Card", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Domestic worker wage/garden services", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Education Fees", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Food and Groceries", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Insurance/funeral policies", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Planned Savings", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Child support", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Bond Payments", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "All Car Repayments", ExpenditureAmount = 500 },
                new ExpenditureItem
                {
                    ExpenditureType = 1,
                    ExpenditureDesc = "Other",
                    ExpenditureAmount = 200,
                    CapturedDescription = "Other Description"
                },
                new ExpenditureItem
                {
                    ExpenditureType = 1,
                    ExpenditureDesc = "Other Instalments",
                    ExpenditureAmount = 200,
                    CapturedDescription = "Other Instalments Description"
                },
                new ExpenditureItem
                {
                    ExpenditureType = 1,
                    ExpenditureDesc = "Other Debt Repayment",
                    ExpenditureAmount = 500,
                    CapturedDescription = "other debt repayment description"
                },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Overdraft", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Personal Loans", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Transport/petrol costs", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Rates and Taxes", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Retail Accounts", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Water, Lights,  Refuse Removal", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Rental repayment", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Credit line repayment", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Medical Expenses", ExpenditureAmount = 500 },
                new ExpenditureItem { ExpenditureType = 1, ExpenditureDesc = "Telephone", ExpenditureAmount = 500 }
            };

            return expenditureItems;
        }

        public static List<BankAccount> PopulateBankAccounts()
        {
            List<BankAccount> bankAccounts = new List<BankAccount>
            {
                new BankAccount
                {
                    AccountName = "Test Account Name",
                    AccountNumber = "12345678",
                    AccountBranch = "Fourways",
                    STDAccountBranchCode = "632005",
                    AccountType = "Current",
                    AccountInstitution = "ABSA",
                    IsBusinessAccount = false,
                    isMainAccount = true
                },
                new BankAccount
                {
                    AccountName = "Test Account Name 2",
                    AccountNumber = "12345988",
                    AccountBranch = "Umhlanga",
                    STDAccountBranchCode = "632005",
                    AccountType = "Savings",
                    AccountInstitution = "ABSA",
                    IsBusinessAccount = false,
                    isMainAccount = false
                }
            };

            return bankAccounts;
        }

        public static ApplicantModel PopulateApplicant(int salaryPaymentDay, List<BankAccountModel> bankAccounts)
        {
            List<EmploymentModel> employments = new List<EmploymentModel>();
            List<AddressModel> addresses = new List<AddressModel>();
            var marketingOptions = ApplicationCreationTestHelper.PopulateMarketingOptions();
            var salariedEmployment = ApplicationCreationTestHelper.PopulateSalariedEmploymentModel();
            salariedEmployment.SalaryPaymentDay = salaryPaymentDay;
            employments.Add(salariedEmployment);
            addresses.Add(ApplicationCreationTestHelper.PopulateFreeTextResidentialAddressModel());
            addresses.Add(ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel());
            List<ApplicantDeclarationsModel> applicantDeclarations = ApplicationCreationTestHelper.PopulateApplicantDeclarations();
            List<ApplicantAffordabilityModel> affordabilities = ApplicationCreationTestHelper.PopulateIncomeAndExpenses();
            List<ApplicantAssetLiabilityModel> assetsAndLiabilities = ApplicationCreationTestHelper.PopulateApplicantAssetLiabilities();
            return ApplicationCreationTestHelper.PopulateApplicantModel(bankAccounts,
                employments,
                addresses,
                affordabilities,
                applicantDeclarations,
                assetsAndLiabilities,
                marketingOptions);
        }
    }
}