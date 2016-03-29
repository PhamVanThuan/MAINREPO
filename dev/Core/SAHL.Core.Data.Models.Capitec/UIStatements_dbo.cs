using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string capiteccreditcriteriadatamodel_selectwhere = "SELECT CreditCriteriaKey, NewBusinessIndicator, MarginValue, MaxLoanAmount, LTV, EmploymentTypeKey, MinIncomeAmount, MaxIncomeAmount, CategoryKey, MortgageLoanPurposeKey FROM [Capitec].[dbo].[CapitecCreditCriteria] WHERE";
        public const string capiteccreditcriteriadatamodel_selectbykey = "SELECT CreditCriteriaKey, NewBusinessIndicator, MarginValue, MaxLoanAmount, LTV, EmploymentTypeKey, MinIncomeAmount, MaxIncomeAmount, CategoryKey, MortgageLoanPurposeKey FROM [Capitec].[dbo].[CapitecCreditCriteria] WHERE CreditCriteriaKey = @PrimaryKey";
        public const string capiteccreditcriteriadatamodel_delete = "DELETE FROM [Capitec].[dbo].[CapitecCreditCriteria] WHERE CreditCriteriaKey = @PrimaryKey";
        public const string capiteccreditcriteriadatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[CapitecCreditCriteria] WHERE";
        public const string capiteccreditcriteriadatamodel_insert = "INSERT INTO [Capitec].[dbo].[CapitecCreditCriteria] (CreditCriteriaKey, NewBusinessIndicator, MarginValue, MaxLoanAmount, LTV, EmploymentTypeKey, MinIncomeAmount, MaxIncomeAmount, CategoryKey, MortgageLoanPurposeKey) VALUES(@CreditCriteriaKey, @NewBusinessIndicator, @MarginValue, @MaxLoanAmount, @LTV, @EmploymentTypeKey, @MinIncomeAmount, @MaxIncomeAmount, @CategoryKey, @MortgageLoanPurposeKey); ";
        public const string capiteccreditcriteriadatamodel_update = "UPDATE [Capitec].[dbo].[CapitecCreditCriteria] SET CreditCriteriaKey = @CreditCriteriaKey, NewBusinessIndicator = @NewBusinessIndicator, MarginValue = @MarginValue, MaxLoanAmount = @MaxLoanAmount, LTV = @LTV, EmploymentTypeKey = @EmploymentTypeKey, MinIncomeAmount = @MinIncomeAmount, MaxIncomeAmount = @MaxIncomeAmount, CategoryKey = @CategoryKey, MortgageLoanPurposeKey = @MortgageLoanPurposeKey WHERE CreditCriteriaKey = @CreditCriteriaKey";



        public const string capitecfeesdatamodel_selectwhere = "SELECT FeeRange, FeeBondStamps, FeeBondConveyancingNoFICA, FeeBondNoFICAVAT, FeeBondConveyancing80Pct, FeeBondVAT80Pct, FeeBondConveyancing, FeeBondVAT, FeeCancelDuty, FeeCancelConveyancing, FeeCancelVAT FROM [Capitec].[dbo].[CapitecFees] WHERE";
        public const string capitecfeesdatamodel_selectbykey = "SELECT FeeRange, FeeBondStamps, FeeBondConveyancingNoFICA, FeeBondNoFICAVAT, FeeBondConveyancing80Pct, FeeBondVAT80Pct, FeeBondConveyancing, FeeBondVAT, FeeCancelDuty, FeeCancelConveyancing, FeeCancelVAT FROM [Capitec].[dbo].[CapitecFees] WHERE FeeRange = @PrimaryKey";
        public const string capitecfeesdatamodel_delete = "DELETE FROM [Capitec].[dbo].[CapitecFees] WHERE FeeRange = @PrimaryKey";
        public const string capitecfeesdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[CapitecFees] WHERE";
        public const string capitecfeesdatamodel_insert = "INSERT INTO [Capitec].[dbo].[CapitecFees] (FeeRange, FeeBondStamps, FeeBondConveyancingNoFICA, FeeBondNoFICAVAT, FeeBondConveyancing80Pct, FeeBondVAT80Pct, FeeBondConveyancing, FeeBondVAT, FeeCancelDuty, FeeCancelConveyancing, FeeCancelVAT) VALUES(@FeeRange, @FeeBondStamps, @FeeBondConveyancingNoFICA, @FeeBondNoFICAVAT, @FeeBondConveyancing80Pct, @FeeBondVAT80Pct, @FeeBondConveyancing, @FeeBondVAT, @FeeCancelDuty, @FeeCancelConveyancing, @FeeCancelVAT); ";
        public const string capitecfeesdatamodel_update = "UPDATE [Capitec].[dbo].[CapitecFees] SET FeeRange = @FeeRange, FeeBondStamps = @FeeBondStamps, FeeBondConveyancingNoFICA = @FeeBondConveyancingNoFICA, FeeBondNoFICAVAT = @FeeBondNoFICAVAT, FeeBondConveyancing80Pct = @FeeBondConveyancing80Pct, FeeBondVAT80Pct = @FeeBondVAT80Pct, FeeBondConveyancing = @FeeBondConveyancing, FeeBondVAT = @FeeBondVAT, FeeCancelDuty = @FeeCancelDuty, FeeCancelConveyancing = @FeeCancelConveyancing, FeeCancelVAT = @FeeCancelVAT WHERE FeeRange = @FeeRange";



        public const string controldatamodel_selectwhere = "SELECT ControlNumber, ControlDescription, ControlNumeric, ControlText FROM [Capitec].[dbo].[Control] WHERE";
        public const string controldatamodel_selectbykey = "SELECT ControlNumber, ControlDescription, ControlNumeric, ControlText FROM [Capitec].[dbo].[Control] WHERE ControlNumber = @PrimaryKey";
        public const string controldatamodel_delete = "DELETE FROM [Capitec].[dbo].[Control] WHERE ControlNumber = @PrimaryKey";
        public const string controldatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[Control] WHERE";
        public const string controldatamodel_insert = "INSERT INTO [Capitec].[dbo].[Control] (ControlNumber, ControlDescription, ControlNumeric, ControlText) VALUES(@ControlNumber, @ControlDescription, @ControlNumeric, @ControlText); ";
        public const string controldatamodel_update = "UPDATE [Capitec].[dbo].[Control] SET ControlNumber = @ControlNumber, ControlDescription = @ControlDescription, ControlNumeric = @ControlNumeric, ControlText = @ControlText WHERE ControlNumber = @ControlNumber";



        public const string marketratedatamodel_selectwhere = "SELECT MarketRateKey, Value, Description FROM [Capitec].[dbo].[MarketRate] WHERE";
        public const string marketratedatamodel_selectbykey = "SELECT MarketRateKey, Value, Description FROM [Capitec].[dbo].[MarketRate] WHERE MarketRateKey = @PrimaryKey";
        public const string marketratedatamodel_delete = "DELETE FROM [Capitec].[dbo].[MarketRate] WHERE MarketRateKey = @PrimaryKey";
        public const string marketratedatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[MarketRate] WHERE";
        public const string marketratedatamodel_insert = "INSERT INTO [Capitec].[dbo].[MarketRate] (MarketRateKey, Value, Description) VALUES(@MarketRateKey, @Value, @Description); ";
        public const string marketratedatamodel_update = "UPDATE [Capitec].[dbo].[MarketRate] SET MarketRateKey = @MarketRateKey, Value = @Value, Description = @Description WHERE MarketRateKey = @MarketRateKey";



        public const string addressdatamodel_selectwhere = "SELECT Id, AddressFormatEnumId, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbId, PostOfficeId, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5 FROM [Capitec].[dbo].[Address] WHERE";
        public const string addressdatamodel_selectbykey = "SELECT Id, AddressFormatEnumId, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbId, PostOfficeId, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5 FROM [Capitec].[dbo].[Address] WHERE Id = @PrimaryKey";
        public const string addressdatamodel_delete = "DELETE FROM [Capitec].[dbo].[Address] WHERE Id = @PrimaryKey";
        public const string addressdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[Address] WHERE";
        public const string addressdatamodel_insert = "INSERT INTO [Capitec].[dbo].[Address] (Id, AddressFormatEnumId, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbId, PostOfficeId, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5) VALUES(@Id, @AddressFormatEnumId, @BoxNumber, @UnitNumber, @BuildingNumber, @BuildingName, @StreetNumber, @StreetName, @SuburbId, @PostOfficeId, @SuiteNumber, @FreeText1, @FreeText2, @FreeText3, @FreeText4, @FreeText5); ";
        public const string addressdatamodel_update = "UPDATE [Capitec].[dbo].[Address] SET Id = @Id, AddressFormatEnumId = @AddressFormatEnumId, BoxNumber = @BoxNumber, UnitNumber = @UnitNumber, BuildingNumber = @BuildingNumber, BuildingName = @BuildingName, StreetNumber = @StreetNumber, StreetName = @StreetName, SuburbId = @SuburbId, PostOfficeId = @PostOfficeId, SuiteNumber = @SuiteNumber, FreeText1 = @FreeText1, FreeText2 = @FreeText2, FreeText3 = @FreeText3, FreeText4 = @FreeText4, FreeText5 = @FreeText5 WHERE Id = @Id";



        public const string persondatamodel_selectwhere = "SELECT Id, SalutationEnumId, FirstName, Surname, IdentityNumber FROM [Capitec].[dbo].[Person] WHERE";
        public const string persondatamodel_selectbykey = "SELECT Id, SalutationEnumId, FirstName, Surname, IdentityNumber FROM [Capitec].[dbo].[Person] WHERE Id = @PrimaryKey";
        public const string persondatamodel_delete = "DELETE FROM [Capitec].[dbo].[Person] WHERE Id = @PrimaryKey";
        public const string persondatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[Person] WHERE";
        public const string persondatamodel_insert = "INSERT INTO [Capitec].[dbo].[Person] (Id, SalutationEnumId, FirstName, Surname, IdentityNumber) VALUES(@Id, @SalutationEnumId, @FirstName, @Surname, @IdentityNumber); ";
        public const string persondatamodel_update = "UPDATE [Capitec].[dbo].[Person] SET Id = @Id, SalutationEnumId = @SalutationEnumId, FirstName = @FirstName, Surname = @Surname, IdentityNumber = @IdentityNumber WHERE Id = @Id";



        public const string applicantdatamodel_selectwhere = "SELECT Id, PersonID, MainContact FROM [Capitec].[dbo].[Applicant] WHERE";
        public const string applicantdatamodel_selectbykey = "SELECT Id, PersonID, MainContact FROM [Capitec].[dbo].[Applicant] WHERE Id = @PrimaryKey";
        public const string applicantdatamodel_delete = "DELETE FROM [Capitec].[dbo].[Applicant] WHERE Id = @PrimaryKey";
        public const string applicantdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[Applicant] WHERE";
        public const string applicantdatamodel_insert = "INSERT INTO [Capitec].[dbo].[Applicant] (Id, PersonID, MainContact) VALUES(@Id, @PersonID, @MainContact); ";
        public const string applicantdatamodel_update = "UPDATE [Capitec].[dbo].[Applicant] SET Id = @Id, PersonID = @PersonID, MainContact = @MainContact WHERE Id = @Id";



        public const string applicationdatamodel_selectwhere = "SELECT Id, ApplicationDate, ApplicationPurposeEnumId, ApplicationNumber, UserId, AddressId, ApplicationStageTypeEnumId, ApplicationStatusEnumId, LastStatusChangeDate, ConsultantName, ConsultantContactNumber, CaptureStartTime, CaptureEndTime, BranchId FROM [Capitec].[dbo].[Application] WHERE";
        public const string applicationdatamodel_selectbykey = "SELECT Id, ApplicationDate, ApplicationPurposeEnumId, ApplicationNumber, UserId, AddressId, ApplicationStageTypeEnumId, ApplicationStatusEnumId, LastStatusChangeDate, ConsultantName, ConsultantContactNumber, CaptureStartTime, CaptureEndTime, BranchId FROM [Capitec].[dbo].[Application] WHERE Id = @PrimaryKey";
        public const string applicationdatamodel_delete = "DELETE FROM [Capitec].[dbo].[Application] WHERE Id = @PrimaryKey";
        public const string applicationdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[Application] WHERE";
        public const string applicationdatamodel_insert = "INSERT INTO [Capitec].[dbo].[Application] (Id, ApplicationDate, ApplicationPurposeEnumId, ApplicationNumber, UserId, AddressId, ApplicationStageTypeEnumId, ApplicationStatusEnumId, LastStatusChangeDate, ConsultantName, ConsultantContactNumber, CaptureStartTime, CaptureEndTime, BranchId) VALUES(@Id, @ApplicationDate, @ApplicationPurposeEnumId, @ApplicationNumber, @UserId, @AddressId, @ApplicationStageTypeEnumId, @ApplicationStatusEnumId, @LastStatusChangeDate, @ConsultantName, @ConsultantContactNumber, @CaptureStartTime, @CaptureEndTime, @BranchId); ";
        public const string applicationdatamodel_update = "UPDATE [Capitec].[dbo].[Application] SET Id = @Id, ApplicationDate = @ApplicationDate, ApplicationPurposeEnumId = @ApplicationPurposeEnumId, ApplicationNumber = @ApplicationNumber, UserId = @UserId, AddressId = @AddressId, ApplicationStageTypeEnumId = @ApplicationStageTypeEnumId, ApplicationStatusEnumId = @ApplicationStatusEnumId, LastStatusChangeDate = @LastStatusChangeDate, ConsultantName = @ConsultantName, ConsultantContactNumber = @ConsultantContactNumber, CaptureStartTime = @CaptureStartTime, CaptureEndTime = @CaptureEndTime, BranchId = @BranchId WHERE Id = @Id";



        public const string applicationloandetaildatamodel_selectwhere = "SELECT Id, ApplicationId, EmploymentTypeID, OccupancyTypeEnumID, HouseholdIncome, Instalment, InterestRate, LoanAmount, LTV, PTI, Fees, TermInMonths, CapitaliseFees FROM [Capitec].[dbo].[ApplicationLoanDetail] WHERE";
        public const string applicationloandetaildatamodel_selectbykey = "SELECT Id, ApplicationId, EmploymentTypeID, OccupancyTypeEnumID, HouseholdIncome, Instalment, InterestRate, LoanAmount, LTV, PTI, Fees, TermInMonths, CapitaliseFees FROM [Capitec].[dbo].[ApplicationLoanDetail] WHERE Id = @PrimaryKey";
        public const string applicationloandetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicationLoanDetail] WHERE Id = @PrimaryKey";
        public const string applicationloandetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicationLoanDetail] WHERE";
        public const string applicationloandetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicationLoanDetail] (Id, ApplicationId, EmploymentTypeID, OccupancyTypeEnumID, HouseholdIncome, Instalment, InterestRate, LoanAmount, LTV, PTI, Fees, TermInMonths, CapitaliseFees) VALUES(@Id, @ApplicationId, @EmploymentTypeID, @OccupancyTypeEnumID, @HouseholdIncome, @Instalment, @InterestRate, @LoanAmount, @LTV, @PTI, @Fees, @TermInMonths, @CapitaliseFees); ";
        public const string applicationloandetaildatamodel_update = "UPDATE [Capitec].[dbo].[ApplicationLoanDetail] SET Id = @Id, ApplicationId = @ApplicationId, EmploymentTypeID = @EmploymentTypeID, OccupancyTypeEnumID = @OccupancyTypeEnumID, HouseholdIncome = @HouseholdIncome, Instalment = @Instalment, InterestRate = @InterestRate, LoanAmount = @LoanAmount, LTV = @LTV, PTI = @PTI, Fees = @Fees, TermInMonths = @TermInMonths, CapitaliseFees = @CapitaliseFees WHERE Id = @Id";



        public const string newpurchaseapplicationloandetaildatamodel_selectwhere = "SELECT Id, Deposit, PurchasePrice FROM [Capitec].[dbo].[NewPurchaseApplicationLoanDetail] WHERE";
        public const string newpurchaseapplicationloandetaildatamodel_selectbykey = "SELECT Id, Deposit, PurchasePrice FROM [Capitec].[dbo].[NewPurchaseApplicationLoanDetail] WHERE Id = @PrimaryKey";
        public const string newpurchaseapplicationloandetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[NewPurchaseApplicationLoanDetail] WHERE Id = @PrimaryKey";
        public const string newpurchaseapplicationloandetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[NewPurchaseApplicationLoanDetail] WHERE";
        public const string newpurchaseapplicationloandetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[NewPurchaseApplicationLoanDetail] (Id, Deposit, PurchasePrice) VALUES(@Id, @Deposit, @PurchasePrice); ";
        public const string newpurchaseapplicationloandetaildatamodel_update = "UPDATE [Capitec].[dbo].[NewPurchaseApplicationLoanDetail] SET Id = @Id, Deposit = @Deposit, PurchasePrice = @PurchasePrice WHERE Id = @Id";



        public const string switchapplicationloandetaildatamodel_selectwhere = "SELECT Id, CashRequired, CurrentBalance, EstimatedMarketValueOfTheHome, InterimInterest FROM [Capitec].[dbo].[SwitchApplicationLoanDetail] WHERE";
        public const string switchapplicationloandetaildatamodel_selectbykey = "SELECT Id, CashRequired, CurrentBalance, EstimatedMarketValueOfTheHome, InterimInterest FROM [Capitec].[dbo].[SwitchApplicationLoanDetail] WHERE Id = @PrimaryKey";
        public const string switchapplicationloandetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[SwitchApplicationLoanDetail] WHERE Id = @PrimaryKey";
        public const string switchapplicationloandetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[SwitchApplicationLoanDetail] WHERE";
        public const string switchapplicationloandetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[SwitchApplicationLoanDetail] (Id, CashRequired, CurrentBalance, EstimatedMarketValueOfTheHome, InterimInterest) VALUES(@Id, @CashRequired, @CurrentBalance, @EstimatedMarketValueOfTheHome, @InterimInterest); ";
        public const string switchapplicationloandetaildatamodel_update = "UPDATE [Capitec].[dbo].[SwitchApplicationLoanDetail] SET Id = @Id, CashRequired = @CashRequired, CurrentBalance = @CurrentBalance, EstimatedMarketValueOfTheHome = @EstimatedMarketValueOfTheHome, InterimInterest = @InterimInterest WHERE Id = @Id";



        public const string contactdetaildatamodel_selectwhere = "SELECT Id, ContactDetailTypeEnumId FROM [Capitec].[dbo].[ContactDetail] WHERE";
        public const string contactdetaildatamodel_selectbykey = "SELECT Id, ContactDetailTypeEnumId FROM [Capitec].[dbo].[ContactDetail] WHERE Id = @PrimaryKey";
        public const string contactdetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[ContactDetail] WHERE Id = @PrimaryKey";
        public const string contactdetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ContactDetail] WHERE";
        public const string contactdetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[ContactDetail] (Id, ContactDetailTypeEnumId) VALUES(@Id, @ContactDetailTypeEnumId); ";
        public const string contactdetaildatamodel_update = "UPDATE [Capitec].[dbo].[ContactDetail] SET Id = @Id, ContactDetailTypeEnumId = @ContactDetailTypeEnumId WHERE Id = @Id";



        public const string applicantcontactdetaildatamodel_selectwhere = "SELECT Id, ApplicantId, ContactDetailId FROM [Capitec].[dbo].[ApplicantContactDetail] WHERE";
        public const string applicantcontactdetaildatamodel_selectbykey = "SELECT Id, ApplicantId, ContactDetailId FROM [Capitec].[dbo].[ApplicantContactDetail] WHERE Id = @PrimaryKey";
        public const string applicantcontactdetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicantContactDetail] WHERE Id = @PrimaryKey";
        public const string applicantcontactdetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicantContactDetail] WHERE";
        public const string applicantcontactdetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicantContactDetail] (Id, ApplicantId, ContactDetailId) VALUES(@Id, @ApplicantId, @ContactDetailId); ";
        public const string applicantcontactdetaildatamodel_update = "UPDATE [Capitec].[dbo].[ApplicantContactDetail] SET Id = @Id, ApplicantId = @ApplicantId, ContactDetailId = @ContactDetailId WHERE Id = @Id";



        public const string emailaddresscontactdetaildatamodel_selectwhere = "SELECT Id, EmailAddressContactDetailTypeEnumId, EmailAddress FROM [Capitec].[dbo].[EmailAddressContactDetail] WHERE";
        public const string emailaddresscontactdetaildatamodel_selectbykey = "SELECT Id, EmailAddressContactDetailTypeEnumId, EmailAddress FROM [Capitec].[dbo].[EmailAddressContactDetail] WHERE Id = @PrimaryKey";
        public const string emailaddresscontactdetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[EmailAddressContactDetail] WHERE Id = @PrimaryKey";
        public const string emailaddresscontactdetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[EmailAddressContactDetail] WHERE";
        public const string emailaddresscontactdetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[EmailAddressContactDetail] (Id, EmailAddressContactDetailTypeEnumId, EmailAddress) VALUES(@Id, @EmailAddressContactDetailTypeEnumId, @EmailAddress); ";
        public const string emailaddresscontactdetaildatamodel_update = "UPDATE [Capitec].[dbo].[EmailAddressContactDetail] SET Id = @Id, EmailAddressContactDetailTypeEnumId = @EmailAddressContactDetailTypeEnumId, EmailAddress = @EmailAddress WHERE Id = @Id";



        public const string phonenumbercontactdetaildatamodel_selectwhere = "SELECT Id, PhoneNumberContactDetailTypeEnumId, PhoneCode, PhoneNumber FROM [Capitec].[dbo].[PhoneNumberContactDetail] WHERE";
        public const string phonenumbercontactdetaildatamodel_selectbykey = "SELECT Id, PhoneNumberContactDetailTypeEnumId, PhoneCode, PhoneNumber FROM [Capitec].[dbo].[PhoneNumberContactDetail] WHERE Id = @PrimaryKey";
        public const string phonenumbercontactdetaildatamodel_delete = "DELETE FROM [Capitec].[dbo].[PhoneNumberContactDetail] WHERE Id = @PrimaryKey";
        public const string phonenumbercontactdetaildatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[PhoneNumberContactDetail] WHERE";
        public const string phonenumbercontactdetaildatamodel_insert = "INSERT INTO [Capitec].[dbo].[PhoneNumberContactDetail] (Id, PhoneNumberContactDetailTypeEnumId, PhoneCode, PhoneNumber) VALUES(@Id, @PhoneNumberContactDetailTypeEnumId, @PhoneCode, @PhoneNumber); ";
        public const string phonenumbercontactdetaildatamodel_update = "UPDATE [Capitec].[dbo].[PhoneNumberContactDetail] SET Id = @Id, PhoneNumberContactDetailTypeEnumId = @PhoneNumberContactDetailTypeEnumId, PhoneCode = @PhoneCode, PhoneNumber = @PhoneNumber WHERE Id = @Id";



        public const string itcdatamodel_selectwhere = "SELECT Id, ITCDate, ITCData FROM [Capitec].[dbo].[ITC] WHERE";
        public const string itcdatamodel_selectbykey = "SELECT Id, ITCDate, ITCData FROM [Capitec].[dbo].[ITC] WHERE Id = @PrimaryKey";
        public const string itcdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ITC] WHERE Id = @PrimaryKey";
        public const string itcdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ITC] WHERE";
        public const string itcdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ITC] (Id, ITCDate, ITCData) VALUES(@Id, @ITCDate, @ITCData); ";
        public const string itcdatamodel_update = "UPDATE [Capitec].[dbo].[ITC] SET Id = @Id, ITCDate = @ITCDate, ITCData = @ITCData WHERE Id = @Id";



        public const string applicantdeclarationdatamodel_selectwhere = "SELECT ID, ApplicantId, DeclarationId FROM [Capitec].[dbo].[ApplicantDeclaration] WHERE";
        public const string applicantdeclarationdatamodel_selectbykey = "SELECT ID, ApplicantId, DeclarationId FROM [Capitec].[dbo].[ApplicantDeclaration] WHERE ID = @PrimaryKey";
        public const string applicantdeclarationdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicantDeclaration] WHERE ID = @PrimaryKey";
        public const string applicantdeclarationdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicantDeclaration] WHERE";
        public const string applicantdeclarationdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicantDeclaration] (ID, ApplicantId, DeclarationId) VALUES(@ID, @ApplicantId, @DeclarationId); ";
        public const string applicantdeclarationdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicantDeclaration] SET ID = @ID, ApplicantId = @ApplicantId, DeclarationId = @DeclarationId WHERE ID = @ID";



        public const string applicationapplicantdatamodel_selectwhere = "SELECT Id, ApplicationId, ApplicantId FROM [Capitec].[dbo].[ApplicationApplicant] WHERE";
        public const string applicationapplicantdatamodel_selectbykey = "SELECT Id, ApplicationId, ApplicantId FROM [Capitec].[dbo].[ApplicationApplicant] WHERE Id = @PrimaryKey";
        public const string applicationapplicantdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicationApplicant] WHERE Id = @PrimaryKey";
        public const string applicationapplicantdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicationApplicant] WHERE";
        public const string applicationapplicantdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicationApplicant] (Id, ApplicationId, ApplicantId) VALUES(@Id, @ApplicationId, @ApplicantId); ";
        public const string applicationapplicantdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicationApplicant] SET Id = @Id, ApplicationId = @ApplicationId, ApplicantId = @ApplicantId WHERE Id = @Id";



        public const string applicantaddressdatamodel_selectwhere = "SELECT Id, ApplicantId, AddressId, AddressTypeEnumId FROM [Capitec].[dbo].[ApplicantAddress] WHERE";
        public const string applicantaddressdatamodel_selectbykey = "SELECT Id, ApplicantId, AddressId, AddressTypeEnumId FROM [Capitec].[dbo].[ApplicantAddress] WHERE Id = @PrimaryKey";
        public const string applicantaddressdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicantAddress] WHERE Id = @PrimaryKey";
        public const string applicantaddressdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicantAddress] WHERE";
        public const string applicantaddressdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicantAddress] (Id, ApplicantId, AddressId, AddressTypeEnumId) VALUES(@Id, @ApplicantId, @AddressId, @AddressTypeEnumId); ";
        public const string applicantaddressdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicantAddress] SET Id = @Id, ApplicantId = @ApplicantId, AddressId = @AddressId, AddressTypeEnumId = @AddressTypeEnumId WHERE Id = @Id";



        public const string applicantapplicationaddressdatamodel_selectwhere = "SELECT Id, ApplicantAddressId, ApplicationId FROM [Capitec].[dbo].[ApplicantApplicationAddress] WHERE";
        public const string applicantapplicationaddressdatamodel_selectbykey = "SELECT Id, ApplicantAddressId, ApplicationId FROM [Capitec].[dbo].[ApplicantApplicationAddress] WHERE Id = @PrimaryKey";
        public const string applicantapplicationaddressdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicantApplicationAddress] WHERE Id = @PrimaryKey";
        public const string applicantapplicationaddressdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicantApplicationAddress] WHERE";
        public const string applicantapplicationaddressdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicantApplicationAddress] (Id, ApplicantAddressId, ApplicationId) VALUES(@Id, @ApplicantAddressId, @ApplicationId); ";
        public const string applicantapplicationaddressdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicantApplicationAddress] SET Id = @Id, ApplicantAddressId = @ApplicantAddressId, ApplicationId = @ApplicationId WHERE Id = @Id";



        public const string applicantemploymentdatamodel_selectwhere = "SELECT Id, ApplicantId, EmploymentTypeEnumId, BasicIncome, ThreeMonthAverageCommission, HousingAllowance FROM [Capitec].[dbo].[ApplicantEmployment] WHERE";
        public const string applicantemploymentdatamodel_selectbykey = "SELECT Id, ApplicantId, EmploymentTypeEnumId, BasicIncome, ThreeMonthAverageCommission, HousingAllowance FROM [Capitec].[dbo].[ApplicantEmployment] WHERE Id = @PrimaryKey";
        public const string applicantemploymentdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicantEmployment] WHERE Id = @PrimaryKey";
        public const string applicantemploymentdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicantEmployment] WHERE";
        public const string applicantemploymentdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicantEmployment] (Id, ApplicantId, EmploymentTypeEnumId, BasicIncome, ThreeMonthAverageCommission, HousingAllowance) VALUES(@Id, @ApplicantId, @EmploymentTypeEnumId, @BasicIncome, @ThreeMonthAverageCommission, @HousingAllowance); ";
        public const string applicantemploymentdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicantEmployment] SET Id = @Id, ApplicantId = @ApplicantId, EmploymentTypeEnumId = @EmploymentTypeEnumId, BasicIncome = @BasicIncome, ThreeMonthAverageCommission = @ThreeMonthAverageCommission, HousingAllowance = @HousingAllowance WHERE Id = @Id";



        public const string reservedapplicationnumberdatamodel_selectwhere = "SELECT ApplicationNumber, IsUsed FROM [Capitec].[dbo].[ReservedApplicationNumber] WHERE";
        public const string reservedapplicationnumberdatamodel_selectbykey = "SELECT ApplicationNumber, IsUsed FROM [Capitec].[dbo].[ReservedApplicationNumber] WHERE ApplicationNumber = @PrimaryKey";
        public const string reservedapplicationnumberdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ReservedApplicationNumber] WHERE ApplicationNumber = @PrimaryKey";
        public const string reservedapplicationnumberdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ReservedApplicationNumber] WHERE";
        public const string reservedapplicationnumberdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ReservedApplicationNumber] (ApplicationNumber, IsUsed) VALUES(@ApplicationNumber, @IsUsed); ";
        public const string reservedapplicationnumberdatamodel_update = "UPDATE [Capitec].[dbo].[ReservedApplicationNumber] SET ApplicationNumber = @ApplicationNumber, IsUsed = @IsUsed WHERE ApplicationNumber = @ApplicationNumber";



        public const string addressformatenumdatamodel_selectwhere = "SELECT Id, Name, IsActive, SAHLAddressFormatKey FROM [Capitec].[dbo].[AddressFormatEnum] WHERE";
        public const string addressformatenumdatamodel_selectbykey = "SELECT Id, Name, IsActive, SAHLAddressFormatKey FROM [Capitec].[dbo].[AddressFormatEnum] WHERE Id = @PrimaryKey";
        public const string addressformatenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[AddressFormatEnum] WHERE Id = @PrimaryKey";
        public const string addressformatenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[AddressFormatEnum] WHERE";
        public const string addressformatenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[AddressFormatEnum] (Id, Name, IsActive, SAHLAddressFormatKey) VALUES(@Id, @Name, @IsActive, @SAHLAddressFormatKey); ";
        public const string addressformatenumdatamodel_update = "UPDATE [Capitec].[dbo].[AddressFormatEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive, SAHLAddressFormatKey = @SAHLAddressFormatKey WHERE Id = @Id";



        public const string addresstypeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive, SAHLAddressTypeKey FROM [Capitec].[dbo].[AddressTypeEnum] WHERE";
        public const string addresstypeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive, SAHLAddressTypeKey FROM [Capitec].[dbo].[AddressTypeEnum] WHERE Id = @PrimaryKey";
        public const string addresstypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[AddressTypeEnum] WHERE Id = @PrimaryKey";
        public const string addresstypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[AddressTypeEnum] WHERE";
        public const string addresstypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[AddressTypeEnum] (Id, Name, IsActive, SAHLAddressTypeKey) VALUES(@Id, @Name, @IsActive, @SAHLAddressTypeKey); ";
        public const string addresstypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[AddressTypeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive, SAHLAddressTypeKey = @SAHLAddressTypeKey WHERE Id = @Id";



        public const string applicationpurposeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive, SAHLMortgageLoanPurposeKey FROM [Capitec].[dbo].[ApplicationPurposeEnum] WHERE";
        public const string applicationpurposeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive, SAHLMortgageLoanPurposeKey FROM [Capitec].[dbo].[ApplicationPurposeEnum] WHERE Id = @PrimaryKey";
        public const string applicationpurposeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicationPurposeEnum] WHERE Id = @PrimaryKey";
        public const string applicationpurposeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicationPurposeEnum] WHERE";
        public const string applicationpurposeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicationPurposeEnum] (Id, Name, IsActive, SAHLMortgageLoanPurposeKey) VALUES(@Id, @Name, @IsActive, @SAHLMortgageLoanPurposeKey); ";
        public const string applicationpurposeenumdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicationPurposeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive, SAHLMortgageLoanPurposeKey = @SAHLMortgageLoanPurposeKey WHERE Id = @Id";



        public const string publishmessagefailuredatamodel_selectwhere = "SELECT Id, Message, Date FROM [Capitec].[dbo].[PublishMessageFailure] WHERE";
        public const string publishmessagefailuredatamodel_selectbykey = "SELECT Id, Message, Date FROM [Capitec].[dbo].[PublishMessageFailure] WHERE Id = @PrimaryKey";
        public const string publishmessagefailuredatamodel_delete = "DELETE FROM [Capitec].[dbo].[PublishMessageFailure] WHERE Id = @PrimaryKey";
        public const string publishmessagefailuredatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[PublishMessageFailure] WHERE";
        public const string publishmessagefailuredatamodel_insert = "INSERT INTO [Capitec].[dbo].[PublishMessageFailure] (Id, Message, Date) VALUES(@Id, @Message, @Date); ";
        public const string publishmessagefailuredatamodel_update = "UPDATE [Capitec].[dbo].[PublishMessageFailure] SET Id = @Id, Message = @Message, Date = @Date WHERE Id = @Id";



        public const string creditpricingtreeresultdatamodel_selectwhere = "SELECT Id, Messages, TreeQuery, ApplicationID, QueryDate FROM [Capitec].[dbo].[CreditPricingTreeResult] WHERE";
        public const string creditpricingtreeresultdatamodel_selectbykey = "SELECT Id, Messages, TreeQuery, ApplicationID, QueryDate FROM [Capitec].[dbo].[CreditPricingTreeResult] WHERE Id = @PrimaryKey";
        public const string creditpricingtreeresultdatamodel_delete = "DELETE FROM [Capitec].[dbo].[CreditPricingTreeResult] WHERE Id = @PrimaryKey";
        public const string creditpricingtreeresultdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[CreditPricingTreeResult] WHERE";
        public const string creditpricingtreeresultdatamodel_insert = "INSERT INTO [Capitec].[dbo].[CreditPricingTreeResult] (Id, Messages, TreeQuery, ApplicationID, QueryDate) VALUES(@Id, @Messages, @TreeQuery, @ApplicationID, @QueryDate); ";
        public const string creditpricingtreeresultdatamodel_update = "UPDATE [Capitec].[dbo].[CreditPricingTreeResult] SET Id = @Id, Messages = @Messages, TreeQuery = @TreeQuery, ApplicationID = @ApplicationID, QueryDate = @QueryDate WHERE Id = @Id";



        public const string applicationstagetypeenumdatamodel_selectwhere = "SELECT Id, Name, Reference, IsActive FROM [Capitec].[dbo].[ApplicationStageTypeEnum] WHERE";
        public const string applicationstagetypeenumdatamodel_selectbykey = "SELECT Id, Name, Reference, IsActive FROM [Capitec].[dbo].[ApplicationStageTypeEnum] WHERE Id = @PrimaryKey";
        public const string applicationstagetypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicationStageTypeEnum] WHERE Id = @PrimaryKey";
        public const string applicationstagetypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicationStageTypeEnum] WHERE";
        public const string applicationstagetypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicationStageTypeEnum] (Id, Name, Reference, IsActive) VALUES(@Id, @Name, @Reference, @IsActive); ";
        public const string applicationstagetypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicationStageTypeEnum] SET Id = @Id, Name = @Name, Reference = @Reference, IsActive = @IsActive WHERE Id = @Id";



        public const string applicationstatusenumdatamodel_selectwhere = "SELECT Id, Name, SAHLOfferStatusKey, IsActive FROM [Capitec].[dbo].[ApplicationStatusEnum] WHERE";
        public const string applicationstatusenumdatamodel_selectbykey = "SELECT Id, Name, SAHLOfferStatusKey, IsActive FROM [Capitec].[dbo].[ApplicationStatusEnum] WHERE Id = @PrimaryKey";
        public const string applicationstatusenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicationStatusEnum] WHERE Id = @PrimaryKey";
        public const string applicationstatusenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicationStatusEnum] WHERE";
        public const string applicationstatusenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicationStatusEnum] (Id, Name, SAHLOfferStatusKey, IsActive) VALUES(@Id, @Name, @SAHLOfferStatusKey, @IsActive); ";
        public const string applicationstatusenumdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicationStatusEnum] SET Id = @Id, Name = @Name, SAHLOfferStatusKey = @SAHLOfferStatusKey, IsActive = @IsActive WHERE Id = @Id";



        public const string creditassessmenttreeresultdatamodel_selectwhere = "SELECT Id, Messages, TreeQuery, ApplicantID, QueryDate FROM [Capitec].[dbo].[CreditAssessmentTreeResult] WHERE";
        public const string creditassessmenttreeresultdatamodel_selectbykey = "SELECT Id, Messages, TreeQuery, ApplicantID, QueryDate FROM [Capitec].[dbo].[CreditAssessmentTreeResult] WHERE Id = @PrimaryKey";
        public const string creditassessmenttreeresultdatamodel_delete = "DELETE FROM [Capitec].[dbo].[CreditAssessmentTreeResult] WHERE Id = @PrimaryKey";
        public const string creditassessmenttreeresultdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[CreditAssessmentTreeResult] WHERE";
        public const string creditassessmenttreeresultdatamodel_insert = "INSERT INTO [Capitec].[dbo].[CreditAssessmentTreeResult] (Id, Messages, TreeQuery, ApplicantID, QueryDate) VALUES(@Id, @Messages, @TreeQuery, @ApplicantID, @QueryDate); ";
        public const string creditassessmenttreeresultdatamodel_update = "UPDATE [Capitec].[dbo].[CreditAssessmentTreeResult] SET Id = @Id, Messages = @Messages, TreeQuery = @TreeQuery, ApplicantID = @ApplicantID, QueryDate = @QueryDate WHERE Id = @Id";



        public const string contactdetailtypeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive FROM [Capitec].[dbo].[ContactDetailTypeEnum] WHERE";
        public const string contactdetailtypeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive FROM [Capitec].[dbo].[ContactDetailTypeEnum] WHERE Id = @PrimaryKey";
        public const string contactdetailtypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ContactDetailTypeEnum] WHERE Id = @PrimaryKey";
        public const string contactdetailtypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ContactDetailTypeEnum] WHERE";
        public const string contactdetailtypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ContactDetailTypeEnum] (Id, Name, IsActive) VALUES(@Id, @Name, @IsActive); ";
        public const string contactdetailtypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[ContactDetailTypeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive WHERE Id = @Id";



        public const string applicationstatustypeenumdatamodel_selectwhere = "SELECT Id, Name, Reference, IsActive FROM [Capitec].[dbo].[ApplicationStatusTypeEnum] WHERE";
        public const string applicationstatustypeenumdatamodel_selectbykey = "SELECT Id, Name, Reference, IsActive FROM [Capitec].[dbo].[ApplicationStatusTypeEnum] WHERE Id = @PrimaryKey";
        public const string applicationstatustypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[ApplicationStatusTypeEnum] WHERE Id = @PrimaryKey";
        public const string applicationstatustypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[ApplicationStatusTypeEnum] WHERE";
        public const string applicationstatustypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[ApplicationStatusTypeEnum] (Id, Name, Reference, IsActive) VALUES(@Id, @Name, @Reference, @IsActive); ";
        public const string applicationstatustypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[ApplicationStatusTypeEnum] SET Id = @Id, Name = @Name, Reference = @Reference, IsActive = @IsActive WHERE Id = @Id";



        public const string emailaddresscontactdetailtypeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive FROM [Capitec].[dbo].[EmailAddressContactDetailTypeEnum] WHERE";
        public const string emailaddresscontactdetailtypeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive FROM [Capitec].[dbo].[EmailAddressContactDetailTypeEnum] WHERE Id = @PrimaryKey";
        public const string emailaddresscontactdetailtypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[EmailAddressContactDetailTypeEnum] WHERE Id = @PrimaryKey";
        public const string emailaddresscontactdetailtypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[EmailAddressContactDetailTypeEnum] WHERE";
        public const string emailaddresscontactdetailtypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[EmailAddressContactDetailTypeEnum] (Id, Name, IsActive) VALUES(@Id, @Name, @IsActive); ";
        public const string emailaddresscontactdetailtypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[EmailAddressContactDetailTypeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive WHERE Id = @Id";



        public const string employmenttypeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive, SAHLEmploymentTypeKey FROM [Capitec].[dbo].[EmploymentTypeEnum] WHERE";
        public const string employmenttypeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive, SAHLEmploymentTypeKey FROM [Capitec].[dbo].[EmploymentTypeEnum] WHERE Id = @PrimaryKey";
        public const string employmenttypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[EmploymentTypeEnum] WHERE Id = @PrimaryKey";
        public const string employmenttypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[EmploymentTypeEnum] WHERE";
        public const string employmenttypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[EmploymentTypeEnum] (Id, Name, IsActive, SAHLEmploymentTypeKey) VALUES(@Id, @Name, @IsActive, @SAHLEmploymentTypeKey); ";
        public const string employmenttypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[EmploymentTypeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive, SAHLEmploymentTypeKey = @SAHLEmploymentTypeKey WHERE Id = @Id";



        public const string phonenumbercontactdetailtypeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive FROM [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum] WHERE";
        public const string phonenumbercontactdetailtypeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive FROM [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum] WHERE Id = @PrimaryKey";
        public const string phonenumbercontactdetailtypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum] WHERE Id = @PrimaryKey";
        public const string phonenumbercontactdetailtypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum] WHERE";
        public const string phonenumbercontactdetailtypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum] (Id, Name, IsActive) VALUES(@Id, @Name, @IsActive); ";
        public const string phonenumbercontactdetailtypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive WHERE Id = @Id";



        public const string salutationenumdatamodel_selectwhere = "SELECT Id, Name, IsActive, SAHLSalutationKey FROM [Capitec].[dbo].[SalutationEnum] WHERE";
        public const string salutationenumdatamodel_selectbykey = "SELECT Id, Name, IsActive, SAHLSalutationKey FROM [Capitec].[dbo].[SalutationEnum] WHERE Id = @PrimaryKey";
        public const string salutationenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[SalutationEnum] WHERE Id = @PrimaryKey";
        public const string salutationenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[SalutationEnum] WHERE";
        public const string salutationenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[SalutationEnum] (Id, Name, IsActive, SAHLSalutationKey) VALUES(@Id, @Name, @IsActive, @SAHLSalutationKey); ";
        public const string salutationenumdatamodel_update = "UPDATE [Capitec].[dbo].[SalutationEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive, SAHLSalutationKey = @SAHLSalutationKey WHERE Id = @Id";



        public const string occupancytypeenumdatamodel_selectwhere = "SELECT Id, Name, IsActive, SAHLOccupancyTypeKey FROM [Capitec].[dbo].[OccupancyTypeEnum] WHERE";
        public const string occupancytypeenumdatamodel_selectbykey = "SELECT Id, Name, IsActive, SAHLOccupancyTypeKey FROM [Capitec].[dbo].[OccupancyTypeEnum] WHERE Id = @PrimaryKey";
        public const string occupancytypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[OccupancyTypeEnum] WHERE Id = @PrimaryKey";
        public const string occupancytypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[OccupancyTypeEnum] WHERE";
        public const string occupancytypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[OccupancyTypeEnum] (Id, Name, IsActive, SAHLOccupancyTypeKey) VALUES(@Id, @Name, @IsActive, @SAHLOccupancyTypeKey); ";
        public const string occupancytypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[OccupancyTypeEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive, SAHLOccupancyTypeKey = @SAHLOccupancyTypeKey WHERE Id = @Id";



        public const string declarationtypeenumdatamodel_selectwhere = "SELECT ID, Name, IsActive FROM [Capitec].[dbo].[DeclarationTypeEnum] WHERE";
        public const string declarationtypeenumdatamodel_selectbykey = "SELECT ID, Name, IsActive FROM [Capitec].[dbo].[DeclarationTypeEnum] WHERE ID = @PrimaryKey";
        public const string declarationtypeenumdatamodel_delete = "DELETE FROM [Capitec].[dbo].[DeclarationTypeEnum] WHERE ID = @PrimaryKey";
        public const string declarationtypeenumdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[DeclarationTypeEnum] WHERE";
        public const string declarationtypeenumdatamodel_insert = "INSERT INTO [Capitec].[dbo].[DeclarationTypeEnum] (ID, Name, IsActive) VALUES(@ID, @Name, @IsActive); ";
        public const string declarationtypeenumdatamodel_update = "UPDATE [Capitec].[dbo].[DeclarationTypeEnum] SET ID = @ID, Name = @Name, IsActive = @IsActive WHERE ID = @ID";



        public const string declarationdefinitiondatamodel_selectwhere = "SELECT ID, DeclarationTypeEnumId, DeclarationText FROM [Capitec].[dbo].[DeclarationDefinition] WHERE";
        public const string declarationdefinitiondatamodel_selectbykey = "SELECT ID, DeclarationTypeEnumId, DeclarationText FROM [Capitec].[dbo].[DeclarationDefinition] WHERE ID = @PrimaryKey";
        public const string declarationdefinitiondatamodel_delete = "DELETE FROM [Capitec].[dbo].[DeclarationDefinition] WHERE ID = @PrimaryKey";
        public const string declarationdefinitiondatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[DeclarationDefinition] WHERE";
        public const string declarationdefinitiondatamodel_insert = "INSERT INTO [Capitec].[dbo].[DeclarationDefinition] (ID, DeclarationTypeEnumId, DeclarationText) VALUES(@ID, @DeclarationTypeEnumId, @DeclarationText); ";
        public const string declarationdefinitiondatamodel_update = "UPDATE [Capitec].[dbo].[DeclarationDefinition] SET ID = @ID, DeclarationTypeEnumId = @DeclarationTypeEnumId, DeclarationText = @DeclarationText WHERE ID = @ID";



        public const string declarationdatamodel_selectwhere = "SELECT ID, DeclarationDefinitionId, DeclarationDate FROM [Capitec].[dbo].[Declaration] WHERE";
        public const string declarationdatamodel_selectbykey = "SELECT ID, DeclarationDefinitionId, DeclarationDate FROM [Capitec].[dbo].[Declaration] WHERE ID = @PrimaryKey";
        public const string declarationdatamodel_delete = "DELETE FROM [Capitec].[dbo].[Declaration] WHERE ID = @PrimaryKey";
        public const string declarationdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[Declaration] WHERE";
        public const string declarationdatamodel_insert = "INSERT INTO [Capitec].[dbo].[Declaration] (ID, DeclarationDefinitionId, DeclarationDate) VALUES(@ID, @DeclarationDefinitionId, @DeclarationDate); ";
        public const string declarationdatamodel_update = "UPDATE [Capitec].[dbo].[Declaration] SET ID = @ID, DeclarationDefinitionId = @DeclarationDefinitionId, DeclarationDate = @DeclarationDate WHERE ID = @ID";



        public const string personitcdatamodel_selectwhere = "SELECT Id, CurrentITCId, ITCDate FROM [Capitec].[dbo].[PersonITC] WHERE";
        public const string personitcdatamodel_selectbykey = "SELECT Id, CurrentITCId, ITCDate FROM [Capitec].[dbo].[PersonITC] WHERE Id = @PrimaryKey";
        public const string personitcdatamodel_delete = "DELETE FROM [Capitec].[dbo].[PersonITC] WHERE Id = @PrimaryKey";
        public const string personitcdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[PersonITC] WHERE";
        public const string personitcdatamodel_insert = "INSERT INTO [Capitec].[dbo].[PersonITC] (Id, CurrentITCId, ITCDate) VALUES(@Id, @CurrentITCId, @ITCDate); ";
        public const string personitcdatamodel_update = "UPDATE [Capitec].[dbo].[PersonITC] SET Id = @Id, CurrentITCId = @CurrentITCId, ITCDate = @ITCDate WHERE Id = @Id";



        public const string yesnodeclarationdatamodel_selectwhere = "SELECT ID, YesNoValue, DeclarationDate FROM [Capitec].[dbo].[YesNoDeclaration] WHERE";
        public const string yesnodeclarationdatamodel_selectbykey = "SELECT ID, YesNoValue, DeclarationDate FROM [Capitec].[dbo].[YesNoDeclaration] WHERE ID = @PrimaryKey";
        public const string yesnodeclarationdatamodel_delete = "DELETE FROM [Capitec].[dbo].[YesNoDeclaration] WHERE ID = @PrimaryKey";
        public const string yesnodeclarationdatamodel_deletewhere = "DELETE FROM [Capitec].[dbo].[YesNoDeclaration] WHERE";
        public const string yesnodeclarationdatamodel_insert = "INSERT INTO [Capitec].[dbo].[YesNoDeclaration] (ID, YesNoValue, DeclarationDate) VALUES(@ID, @YesNoValue, @DeclarationDate); ";
        public const string yesnodeclarationdatamodel_update = "UPDATE [Capitec].[dbo].[YesNoDeclaration] SET ID = @ID, YesNoValue = @YesNoValue, DeclarationDate = @DeclarationDate WHERE ID = @ID";



    }
}