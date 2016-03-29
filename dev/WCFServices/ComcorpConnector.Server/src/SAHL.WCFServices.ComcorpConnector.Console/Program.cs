using SAHL.Core.BusinessModel.Enums;
using SAHL.WCFServices.ComcorpConnector.Console.SAHLIntegration;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SAHL.WCFServices.ComcorpConnector.Console
{
    internal class Program
    {
        public enum ComcorpApplicantType
        {
            MainApplicant, Spouse, CoApplicant
        }

        private static void Main(string[] args)
        {
            TestApplicationStuff();

            //TestDocumentStuff();
        }

        private static void TestApplicationStuff()
        {
            SAHLIntegrationServiceClient client = new SAHLIntegrationServiceClient();

            try
            {
                // setup a basic application
                Application application = new Application();
                application.ConsultantFirstName = "firstname";
                application.ConsultantSurname = "surname";

                application.SahlLoanPurpose = "New Purchase Loan";
                application.SAHLVendorCode = "Vendor001";
                application.SAHLOccupancyType = "Owner Occupied";
                application.SAHLPropertyType = "Cluster Home";
                application.SAHLTitleType = "Sectional Title";

                application.PropertyPurchasePrice = 1000000;
                application.DepositCash = 100000;

                // setup a main applicant
                MainApplicant mainApplicant = PopulateComcorpApplicant(ComcorpApplicantType.MainApplicant, MaritalStatus.Divorced, false, false, false, false) as MainApplicant;
                mainApplicant.AssetItems = PopulateAssetItems().ToArray();
                mainApplicant.LiabilityItems = PopulateLiabilityItems().ToArray();
                mainApplicant.IncomeItems = PopulateIncomeItems().ToArray();
                mainApplicant.ExpenditureItems = PopulateExpenditureItems().ToArray();
                mainApplicant.BankAccounts = PopulateBankAccounts().ToArray();

                // with a spouse
                Spouse spouse = PopulateComcorpApplicant(ComcorpApplicantType.Spouse, MaritalStatus.Divorced, false, false, false, false) as Spouse;
                spouse.AssetItems = PopulateAssetItems().ToArray();
                spouse.LiabilityItems = PopulateLiabilityItems().ToArray();
                spouse.IncomeItems = PopulateIncomeItems().ToArray();
                spouse.ExpenditureItems = PopulateExpenditureItems().ToArray();
                spouse.BankAccounts = PopulateBankAccounts().ToArray();

                mainApplicant.Spouse = spouse;

                // setup a surety
                CoApplicant coApplicant = PopulateComcorpApplicant(ComcorpApplicantType.CoApplicant, MaritalStatus.Married_AnteNuptualContract, true, true, true, true) as CoApplicant;
                coApplicant.AssetItems = PopulateAssetItems().ToArray();
                coApplicant.LiabilityItems = PopulateLiabilityItems().ToArray();
                coApplicant.IncomeItems = PopulateIncomeItems().ToArray();
                coApplicant.ExpenditureItems = PopulateExpenditureItems().ToArray();
                coApplicant.BankAccounts = PopulateBankAccounts().ToArray();

                // add main applicant to the application
                application.MainApplicant = mainApplicant;

                // add co applicant to the application
                System.Collections.Generic.List<CoApplicant> CoApplicants = new System.Collections.Generic.List<CoApplicant>();
                CoApplicants.Add(coApplicant);
                application.CoApplicants = CoApplicants.ToArray();

                // add property to applicatuion
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

                property.UsePropertyAddressAsLoanMailingAddress = true;
                property.UsePropertyAddressAsDomiciliumAddress = true;

                application.Property = property;

                string response = client.Submit(application);
                System.Console.WriteLine(response);
            }
            catch (FaultException<SAHLFault> validationFault)
            {
                System.Console.WriteLine(validationFault.Detail.FaultDescription);
            }
        }

        private static void TestDocumentStuff()
        {
            SAHLDocumentServiceClient documentClient = new SAHLDocumentServiceClient();

            var mainApplicantResponse = documentClient.ProcessMainApplicantMessage(GetImagingMainApplicantRequest());
            var coApplicantResponse = documentClient.ProcessCoApplicantMessage(GetImagingCoApplicantRequest());
            var applicationResponse = documentClient.ProcessApplicationMessage(GetImagingApplicationRequest());

            System.Console.WriteLine("Main Applicant Status: {0}", mainApplicantResponse.ReplyHeader.RequestStatus);
            System.Console.WriteLine("Co Applicant Status: {0}", coApplicantResponse.ReplyHeader.RequestStatus);
            System.Console.WriteLine("Application Status: {0}", applicationResponse.ReplyHeader.RequestStatus);
            System.Console.ReadLine();
        }

        public static ImagingCoApplicantRequest GetImagingCoApplicantRequest()
        {
            return new ImagingCoApplicantRequest
            {
                RequestHeader = new CoApplicantHeaderType
                {
                    ApplicationReference = "1498180",
                    BankReference = "1498180",
                    ImagingReference = "",
                    RequestAction = headerTypeRequestAction.New,
                    RequestDateTime = new DateTime(2014, 1, 1),
                    RequestMac = "inMj/1JVsnUNu9VgzvviILNVv1tTpmo9oLu5K+nM2DWb" + 
                    "VVH2i2OqefRs0RYqhmH/X3wxzKEB28KBdo/JUNwKaNy/2uwgPShFa85tG" + 
                    "k//y3UJmMdDmxDZuL0oFvGhRQArO4k4h26OYfSsnjm8gp4Yfe8C8bmHkg" + 
                    "T5esl+kCV8w26MG7UM4WJL1mdeAdaAYASFZCFsnvKMdbXJMVLhBL3GoJG" + 
                    "uj4ArjkE9bzW6fCDjLOg/Y82BK+Sb9G8F56M0NzVr99ZXtPhEXfEoLjjp" + 
                    "CxY4dNCarhYFhFQsFKD1ULcNcJHypi12lmzaLaeQbbBfYNtX27dOd5sEU" + 
                    "bw9iF9ktS23Cg==",
                    ServiceVersion = 1
                },
                SupportingDocuments = new CoApplicantSupportingDocuments
                {
                    CoApplicantDocuments = new CoApplicantType
                    { 
                        ApplicantFirstName = "Bob",
                        ApplicantIdentityNumber = "1234567890123",
                        ApplicantReference = 123,
                        ApplicantSurname = "Smith",
                        SpouseDocuments = new CoApplicantSpouseType
                        {
                            ApplicantFirstName = "Jane",
                            ApplicantSurname = "Smith",
                            ApplicantReference = 235,
                            ApplicantIdentityNumber = "9876543210123",
                            SupportingDocument = new CoApplicantDocumentType[1]
                            {
                                new CoApplicantDocumentType
                                {
                                    DocumentComments = "Blah",
                                    DocumentDescription = "No description",
                                    DocumentImage = document,
                                    DocumentReference = 1,
                                    DocumentType = "ID Documents"
                                }
                            }
                        },
                        SupportingDocument = new CoApplicantDocumentType[1]
                        {
                            new CoApplicantDocumentType
                            {
                                DocumentComments= "Blah",
                                DocumentDescription= "ID Documents",
                                DocumentImage= document,
                                DocumentReference= 5,
                                DocumentType= "General",
                            }
                        }
                    }
                }
            };
        }

        private static ImagingApplicationRequest GetImagingApplicationRequest()
        {
            return new ImagingApplicationRequest
            {
                RequestHeader = new ApplicationHeaderType
                {
                    ApplicationReference = "1498180",
                    BankReference = "1498180",
                    RequestAction = headerTypeRequestAction.New,
                    RequestDateTime = new DateTime(2014, 1, 1),
                    RequestMac = "CkdKiAhSokdamnOSFUcNUUxMyzgAWhR6fJ6dFYl759Ij7nJXBed9WKFq" + 
                    "YRGItv6YPgQG9xen6PnPGiQDoz9fBDUQklsYWzwIxT4IJbPMeyqWubZ8uqCL3sfbOy41k" + 
                    "LrRE8zi2xVCV5mic1FRharr/yPIa9xS1PnDVCEq3cLvwHM4dd96XdVE3XhUKNayJ34OHe" + 
                    "91zZQMNtRqHqU0hnHiag7fpBQQKYH/JcOuThEX/a6jT/lrbq/6/FMfBBi51SxBUzcU2TB" + 
                    "ch34w0f01tKFou9qpFvSDRiy2NNAvnKtShkjfxjab8zdw9Zs/mCm59c+X5TjNw+p3OsHa" + 
                    "5og8eavAjw==",
                    RequestMessages = 5,
                    ServiceVersion = 2.1m
                },
                SupportingDocuments = new ApplicationSupportingDocuments
                {
                    ApplicationDocuments = new documentType[1] {
                        new documentType
                        {
                            DocumentComments= "Blah",
                            DocumentDescription= "General",
                            DocumentImage= document,
                            DocumentReference= 1,
                            DocumentType= "General",
                        }
                    }
                }
            };
        }

        private static ImagingMainApplicantRequest GetImagingMainApplicantRequest()
        {
            return new ImagingMainApplicantRequest
            {
                RequestHeader = new MainApplicantHeaderType
                {
                    ApplicationReference = "1498180",
                    BankReference = "1498180",
                    RequestAction = headerTypeRequestAction1.New,
                    RequestDateTime = new DateTime(2014, 1, 1),
                    RequestMac = "P9nkwDfIU6Kw+3hr7eMQ8MdcX1HXlKzt1zUbOcZ6d5WBz2j3" + 
                    "NvqeP4gJN7Io8AfqOsrxs4ePE2wxeLwcBEmXCgjdXaabSu5vmOT4btS3NSWo4" + 
                    "Jp2UbgcNUJDehULK38sQog1iuBJ4xlM2Eq5Y46xYeombW1bbhEXGobHqAuP5k" + 
                    "2rDWqcvb03tPxOOjkKeHEG7Kun2F0SpPeFRXJEfoI8iU3ToFuB7FnNlTFoKt+" + 
                    "r05f8WevyHzL6ZaGuMmy7EQ6ONkoEHLr7f8siV4W3Fg9c0ZlyJ81VvGvRFrSR" + 
                    "KkpY6a3oX1jjsJ8VQWWUJu6rVQx+ZnD0Fpieo+7LWQptf3tsgA==",
                    RequestMessages = 5,
                    ServiceVersion = 2.1m
                },
                SupportingDocuments = new MainApplicantSupportingDocuments
                {
                    MainApplicantDocuments = new MainApplicantType
                    {
                        ApplicantFirstName = "Bob",
                        ApplicantIdentityNumber = "1234567890123",
                        ApplicantReference = 123,
                        ApplicantSurname = "Smith",
                        SpouseDocuments = new MainApplicantSpouseType
                        {
                            ApplicantFirstName = "Jane",
                            ApplicantSurname = "Smith",
                            ApplicantReference = 235,
                            ApplicantIdentityNumber = "9876543210123",
                            SupportingDocument = new MainApplicantDocumentType[1]
                            {
                                new MainApplicantDocumentType
                                {
                                    DocumentImage= document,
                                    DocumentReference= 2,
                                    DocumentType= "ID Documents",
                                }
                            }
                        },
                        SupportingDocument = new MainApplicantDocumentType[1]
                        {
                            new MainApplicantDocumentType
                            {
                                DocumentComments= "Blah",
                                DocumentDescription= "General",
                                DocumentImage= document,
                                DocumentReference= 1,
                                DocumentType= "General",
                            }
                        }
                    }
                }
            };
        }

        private static string document = @"iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAMAAAAM7l6QAAAAVFBMVEUAAACfxE+exFGgwlKfxFC
            fxFCexE+t/1ufw1Chw0+fv2CfxFCfxVCfxFD///+lyFqhxlOvzmzl78/w9uTq8tje68LM4KL8/fnX5rWoymH1+e3D2pE/Aj+4AAAADXRSTlMA
            isQ74+50AYBRCPajYTxJkAAAANdJREFUKM+F09u2giAUheGFnFSaGB6wtu//nlvCSIz0v/QbQ5YIlNKcVQIQFeOajrXKIGVUm2sjkSWbHeoaX9U66Q2
            Fbm+vUaze1sWPmtfMsiBLvwAyzK8K6q2dO0ARafOtD2vtAMBo4mX9c1jjxI54n5KCUXWiqEgctA/rPhETlGuXNEa5jquOHVLx5c67
            ooo42mxnDzyHVftNt9FYmGden0/upXfsYnFbluChKVPwbVPdUFKjidTnix7IU7sf6kePPNmeH4fzw3RxFM8P8tU1uL5EF1fwH4RuJb8DTG6xAAAAAElFTkSuQmCC";

        public static Applicant PopulateComcorpApplicant(ComcorpApplicantType comcorpApplicantType, MaritalStatus maritalStatus, bool isSurety, bool declarationsInsolvent, bool declarationsDebtCounselling,
            bool declarationsAdminOrder)
        {
            Applicant applicant = new Applicant();

            switch (comcorpApplicantType)
            {
                case ComcorpApplicantType.MainApplicant:
                    applicant = new MainApplicant();

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

                    break;
                case ComcorpApplicantType.Spouse:
                    applicant = new Spouse();

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
                    break;
                case ComcorpApplicantType.CoApplicant:
                    applicant = new CoApplicant();

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
                    break;
                default:
                    break;
            }

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

            applicant.MayDoCreditBureauEnquiry = true;

            applicant.EmployerName = "Employer Name2";
            applicant.EmployerEmail = "contact@employer.com";
            applicant.DateJoinedEmployer = DateTime.Now.AddYears(-3);
            applicant.CurrentEmploymentType = "Salaried";
            applicant.EmploymentSector = "Financial Services";
            applicant.EmployerBusinessType = "Company";
            applicant.DateSalaryPaid = 29;

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
            applicant.PhysicalCountry = "South Africa";

            applicant.PostalAddressLine1 = "5";
            applicant.PostalAddressLine2 = "Clarendon Drive";
            applicant.PostalAddressSuburb = "Durban North";
            applicant.PostalAddressCity = "Durban";
            applicant.PostalAddressCode = "4051";
            applicant.PostalCountry = "South Africa";

            return applicant;
        }

        public static List<AssetItem> PopulateAssetItems()
        {
            List<AssetItem> assetItems = new List<AssetItem>
            {
                new AssetItem{SAHLAssetDesc="Fixed Property",SAHLAssetValue=200000,DateAssetAcquired=DateTime.Now.AddYears(-2), AssetPhysicalAddressLine1="5",
                    AssetPhysicalAddressLine2="Clarendon Drive", AssetPhysicalAddressSuburb="Durban North", AssetPhysicalAddressCity="Durban", AssetPhysicalAddressCode="4051",
                    AssetPhysicalProvince = "Kwazulu Natal", AssetPhysicalCountry = "South Africa"},
                new AssetItem{SAHLAssetDesc="Listed Investments",AssetCompanyName = "Listed Investment Company Name", SAHLAssetValue=10000},
                new AssetItem{SAHLAssetDesc="UnListed Investments",AssetCompanyName = "UnListed Investment Company Name", SAHLAssetValue=4000},
                new AssetItem{SAHLAssetDesc="Other Asset", AssetDescription = "Other Asset Description",SAHLAssetValue=1000, AssetOutstandingLiability=500},
                new AssetItem{SAHLAssetDesc="Life Assurance",AssetCompanyName = "Life Assurance Company Name", SAHLAssetValue=15000}
            };

            return assetItems;
        }

        public static List<LiabilityItem> PopulateLiabilityItems()
        {
            List<LiabilityItem> liabilityItems = new List<LiabilityItem>
            {
                new LiabilityItem{SAHLLiabilityDesc="Liability Loan", LiabilityLoanType="Personal Loan", LiabilityCompanyName = "Personal Loan Company Name", SAHLLiabilityValue = 15000,
                    LiabilityDateRepayable=DateTime.Now.AddYears(1), SAHLLiabilityCost = 500},
                new LiabilityItem{SAHLLiabilityDesc="Liability Loan", LiabilityLoanType="Student Loan", LiabilityCompanyName = "Student Loan Company Name", SAHLLiabilityValue = 5000,
                    LiabilityDateRepayable=DateTime.Now.AddYears(2), SAHLLiabilityCost = 300},
                new LiabilityItem{SAHLLiabilityDesc="Liability Surety", LiabilityDescription = "Liability Surety Description", SAHLLiabilityValue = 1000, LiabilityAssetValue = 1500},
                new LiabilityItem{SAHLLiabilityDesc="Fixed Long Term Investment", LiabilityCompanyName = "Fixed Long Term Investment Company Name", SAHLLiabilityValue = 1000}
            };

            return liabilityItems;
        }

        public static List<IncomeItem> PopulateIncomeItems()
        {
            List<IncomeItem> incomeItems = new List<IncomeItem>()
            {
                new IncomeItem(){IncomeType = 1, IncomeDesc="Basic Salary" , IncomeAmount = 1000} ,
                new IncomeItem(){IncomeType = 1, IncomeDesc="Commission and Overtime" , IncomeAmount = 1000},
                new IncomeItem(){IncomeType = 1, IncomeDesc="Rental" , IncomeAmount = 1000},
                new IncomeItem(){IncomeType = 1, IncomeDesc="Income from Investments" , IncomeAmount = 1000, CapturedDescription = "Income from Investments Description"},
                new IncomeItem(){IncomeType = 1, IncomeDesc="Other Income 1" , IncomeAmount = 1000},
                new IncomeItem(){IncomeType = 1, IncomeDesc="Other Income 2" , IncomeAmount = 1000, CapturedDescription = "Other Income 2 Description"},
            };

            return incomeItems;
        }

        public static List<ExpenditureItem> PopulateExpenditureItems()
        {
            List<ExpenditureItem> expenditureItems = new List<ExpenditureItem>()
            {
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Planned Savings", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Other Instalments", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Clothing", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Credit Card", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Domestic worker wage/garden services", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Education Fees", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Food and Groceries", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Insurance/funeral policies", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Planned Savings", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Child support", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Bond Payments", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="All Car Repayments", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Other", ExpenditureAmount = 200, CapturedDescription = "Other Description"},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Other Instalments", ExpenditureAmount = 200, CapturedDescription = "Other Instalments Description"},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="other debt repayment", ExpenditureAmount = 500, CapturedDescription = "other debt repayment description"},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Overdraft", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Personal Loans", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Transport/petrol costs", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Rates and Taxes", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Retail Accounts", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Water, Lights,  Refuse Removal", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Rental repayment", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Credit line repayment", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Medical Expenses", ExpenditureAmount = 500},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Telephone", ExpenditureAmount = 500}
            };

            return expenditureItems;
        }

        public static List<BankAccount> PopulateBankAccounts()
        {
            List<BankAccount> bankAccounts = new List<BankAccount>()
            {
                new BankAccount(){AccountName="Test Account Name", AccountNumber="12345678", AccountBranch="Fourways", STDAccountBranchCode="1234", AccountType="Current",
                    AccountInstitution = "ABSA", IsBusinessAccount=false, isMainAccount =true },
                new BankAccount(){AccountName="Test Account Name 2", AccountNumber="12345988", AccountBranch="Fourways", STDAccountBranchCode="1234", AccountType="Bond",
                    AccountInstitution = "ABSA", IsBusinessAccount=false, isMainAccount=false }
            };

            return bankAccounts;
        }
    }
}