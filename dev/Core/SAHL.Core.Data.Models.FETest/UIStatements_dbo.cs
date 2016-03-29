using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.FETest
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string openthirdpartyinvoicedatamodel_selectwhere = "SELECT ThirdPartyInvoiceKey, InstanceID, WorkflowState, AssignedUser, InvoiceStatusKey FROM [fetest].[dbo].[OpenThirdPartyInvoice] WHERE";
        public const string openthirdpartyinvoicedatamodel_selectbykey = "SELECT ThirdPartyInvoiceKey, InstanceID, WorkflowState, AssignedUser, InvoiceStatusKey FROM [fetest].[dbo].[OpenThirdPartyInvoice] WHERE  = @PrimaryKey";
        public const string openthirdpartyinvoicedatamodel_delete = "DELETE FROM [fetest].[dbo].[OpenThirdPartyInvoice] WHERE  = @PrimaryKey";
        public const string openthirdpartyinvoicedatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[OpenThirdPartyInvoice] WHERE";
        public const string openthirdpartyinvoicedatamodel_insert = "INSERT INTO [fetest].[dbo].[OpenThirdPartyInvoice] (ThirdPartyInvoiceKey, InstanceID, WorkflowState, AssignedUser, InvoiceStatusKey) VALUES(@ThirdPartyInvoiceKey, @InstanceID, @WorkflowState, @AssignedUser, @InvoiceStatusKey); ";
        public const string openthirdpartyinvoicedatamodel_update = "UPDATE [fetest].[dbo].[OpenThirdPartyInvoice] SET ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey, InstanceID = @InstanceID, WorkflowState = @WorkflowState, AssignedUser = @AssignedUser, InvoiceStatusKey = @InvoiceStatusKey WHERE  = @";



        public const string openmortgageloanaccountsdatamodel_selectwhere = "SELECT Id, AccountKey, ProductKey, HasThirdPartyInvoice FROM [fetest].[dbo].[OpenMortgageLoanAccounts] WHERE";
        public const string openmortgageloanaccountsdatamodel_selectbykey = "SELECT Id, AccountKey, ProductKey, HasThirdPartyInvoice FROM [fetest].[dbo].[OpenMortgageLoanAccounts] WHERE Id = @PrimaryKey";
        public const string openmortgageloanaccountsdatamodel_delete = "DELETE FROM [fetest].[dbo].[OpenMortgageLoanAccounts] WHERE Id = @PrimaryKey";
        public const string openmortgageloanaccountsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[OpenMortgageLoanAccounts] WHERE";
        public const string openmortgageloanaccountsdatamodel_insert = "INSERT INTO [fetest].[dbo].[OpenMortgageLoanAccounts] (AccountKey, ProductKey, HasThirdPartyInvoice) VALUES(@AccountKey, @ProductKey, @HasThirdPartyInvoice); select cast(scope_identity() as int)";
        public const string openmortgageloanaccountsdatamodel_update = "UPDATE [fetest].[dbo].[OpenMortgageLoanAccounts] SET AccountKey = @AccountKey, ProductKey = @ProductKey, HasThirdPartyInvoice = @HasThirdPartyInvoice WHERE Id = @Id";



        public const string clientaddressesdatamodel_selectwhere = "SELECT Id, LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey FROM [fetest].[dbo].[ClientAddresses] WHERE";
        public const string clientaddressesdatamodel_selectbykey = "SELECT Id, LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey FROM [fetest].[dbo].[ClientAddresses] WHERE Id = @PrimaryKey";
        public const string clientaddressesdatamodel_delete = "DELETE FROM [fetest].[dbo].[ClientAddresses] WHERE Id = @PrimaryKey";
        public const string clientaddressesdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[ClientAddresses] WHERE";
        public const string clientaddressesdatamodel_insert = "INSERT INTO [fetest].[dbo].[ClientAddresses] (LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey) VALUES(@LegalEntityAddressKey, @AddressKey, @AddressTypeKey, @AddressFormatKey, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string clientaddressesdatamodel_update = "UPDATE [fetest].[dbo].[ClientAddresses] SET LegalEntityAddressKey = @LegalEntityAddressKey, AddressKey = @AddressKey, AddressTypeKey = @AddressTypeKey, AddressFormatKey = @AddressFormatKey, LegalEntityKey = @LegalEntityKey WHERE Id = @Id";



        public const string activenewbusinessapplicantsdatamodel_selectwhere = "SELECT Id, OfferKey, OfferRoleKey, LegalEntityKey, OfferRoleTypeKey, IsIncomeContributor, HasDeclarations, HasAffordabilityAssessment, HasAssetsLiabilities, HasBankAccount, HasEmployment, HasResidentialAddress, HasPostalAddress, HasDomicilium FROM [fetest].[dbo].[ActiveNewBusinessApplicants] WHERE";
        public const string activenewbusinessapplicantsdatamodel_selectbykey = "SELECT Id, OfferKey, OfferRoleKey, LegalEntityKey, OfferRoleTypeKey, IsIncomeContributor, HasDeclarations, HasAffordabilityAssessment, HasAssetsLiabilities, HasBankAccount, HasEmployment, HasResidentialAddress, HasPostalAddress, HasDomicilium FROM [fetest].[dbo].[ActiveNewBusinessApplicants] WHERE Id = @PrimaryKey";
        public const string activenewbusinessapplicantsdatamodel_delete = "DELETE FROM [fetest].[dbo].[ActiveNewBusinessApplicants] WHERE Id = @PrimaryKey";
        public const string activenewbusinessapplicantsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[ActiveNewBusinessApplicants] WHERE";
        public const string activenewbusinessapplicantsdatamodel_insert = "INSERT INTO [fetest].[dbo].[ActiveNewBusinessApplicants] (OfferKey, OfferRoleKey, LegalEntityKey, OfferRoleTypeKey, IsIncomeContributor, HasDeclarations, HasAffordabilityAssessment, HasAssetsLiabilities, HasBankAccount, HasEmployment, HasResidentialAddress, HasPostalAddress, HasDomicilium) VALUES(@OfferKey, @OfferRoleKey, @LegalEntityKey, @OfferRoleTypeKey, @IsIncomeContributor, @HasDeclarations, @HasAffordabilityAssessment, @HasAssetsLiabilities, @HasBankAccount, @HasEmployment, @HasResidentialAddress, @HasPostalAddress, @HasDomicilium); select cast(scope_identity() as int)";
        public const string activenewbusinessapplicantsdatamodel_update = "UPDATE [fetest].[dbo].[ActiveNewBusinessApplicants] SET OfferKey = @OfferKey, OfferRoleKey = @OfferRoleKey, LegalEntityKey = @LegalEntityKey, OfferRoleTypeKey = @OfferRoleTypeKey, IsIncomeContributor = @IsIncomeContributor, HasDeclarations = @HasDeclarations, HasAffordabilityAssessment = @HasAffordabilityAssessment, HasAssetsLiabilities = @HasAssetsLiabilities, HasBankAccount = @HasBankAccount, HasEmployment = @HasEmployment, HasResidentialAddress = @HasResidentialAddress, HasPostalAddress = @HasPostalAddress, HasDomicilium = @HasDomicilium WHERE Id = @Id";



        public const string alphahousingapplicationsdatamodel_selectwhere = "SELECT Id, OfferKey FROM [fetest].[dbo].[AlphaHousingApplications] WHERE";
        public const string alphahousingapplicationsdatamodel_selectbykey = "SELECT Id, OfferKey FROM [fetest].[dbo].[AlphaHousingApplications] WHERE Id = @PrimaryKey";
        public const string alphahousingapplicationsdatamodel_delete = "DELETE FROM [fetest].[dbo].[AlphaHousingApplications] WHERE Id = @PrimaryKey";
        public const string alphahousingapplicationsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[AlphaHousingApplications] WHERE";
        public const string alphahousingapplicationsdatamodel_insert = "INSERT INTO [fetest].[dbo].[AlphaHousingApplications] (OfferKey) VALUES(@OfferKey); select cast(scope_identity() as int)";
        public const string alphahousingapplicationsdatamodel_update = "UPDATE [fetest].[dbo].[AlphaHousingApplications] SET OfferKey = @OfferKey WHERE Id = @Id";



        public const string foreignnaturalpersonclientsdatamodel_selectwhere = "SELECT Id, LegalEntityKey, CitizenshipTypeKey, PassportNumber FROM [fetest].[dbo].[ForeignNaturalPersonClients] WHERE";
        public const string foreignnaturalpersonclientsdatamodel_selectbykey = "SELECT Id, LegalEntityKey, CitizenshipTypeKey, PassportNumber FROM [fetest].[dbo].[ForeignNaturalPersonClients] WHERE Id = @PrimaryKey";
        public const string foreignnaturalpersonclientsdatamodel_delete = "DELETE FROM [fetest].[dbo].[ForeignNaturalPersonClients] WHERE Id = @PrimaryKey";
        public const string foreignnaturalpersonclientsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[ForeignNaturalPersonClients] WHERE";
        public const string foreignnaturalpersonclientsdatamodel_insert = "INSERT INTO [fetest].[dbo].[ForeignNaturalPersonClients] (LegalEntityKey, CitizenshipTypeKey, PassportNumber) VALUES(@LegalEntityKey, @CitizenshipTypeKey, @PassportNumber); select cast(scope_identity() as int)";
        public const string foreignnaturalpersonclientsdatamodel_update = "UPDATE [fetest].[dbo].[ForeignNaturalPersonClients] SET LegalEntityKey = @LegalEntityKey, CitizenshipTypeKey = @CitizenshipTypeKey, PassportNumber = @PassportNumber WHERE Id = @Id";



        public const string naturalpersonclientdatamodel_selectwhere = "SELECT Id, LegalEntityKey, IsActive, IdNumber FROM [fetest].[dbo].[NaturalPersonClient] WHERE";
        public const string naturalpersonclientdatamodel_selectbykey = "SELECT Id, LegalEntityKey, IsActive, IdNumber FROM [fetest].[dbo].[NaturalPersonClient] WHERE Id = @PrimaryKey";
        public const string naturalpersonclientdatamodel_delete = "DELETE FROM [fetest].[dbo].[NaturalPersonClient] WHERE Id = @PrimaryKey";
        public const string naturalpersonclientdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[NaturalPersonClient] WHERE";
        public const string naturalpersonclientdatamodel_insert = "INSERT INTO [fetest].[dbo].[NaturalPersonClient] (LegalEntityKey, IsActive, IdNumber) VALUES(@LegalEntityKey, @IsActive, @IdNumber); select cast(scope_identity() as int)";
        public const string naturalpersonclientdatamodel_update = "UPDATE [fetest].[dbo].[NaturalPersonClient] SET LegalEntityKey = @LegalEntityKey, IsActive = @IsActive, IdNumber = @IdNumber WHERE Id = @Id";



        public const string opennewbusinessapplicationsdatamodel_selectwhere = "SELECT Id, OfferKey, LTV, SPVKey, PropertyKey, HasDebitOrder, HasMailingAddress, HasProperty, IsAccepted, HouseholdIncome, EmploymentTypeKey FROM [fetest].[dbo].[OpenNewBusinessApplications] WHERE";
        public const string opennewbusinessapplicationsdatamodel_selectbykey = "SELECT Id, OfferKey, LTV, SPVKey, PropertyKey, HasDebitOrder, HasMailingAddress, HasProperty, IsAccepted, HouseholdIncome, EmploymentTypeKey FROM [fetest].[dbo].[OpenNewBusinessApplications] WHERE Id = @PrimaryKey";
        public const string opennewbusinessapplicationsdatamodel_delete = "DELETE FROM [fetest].[dbo].[OpenNewBusinessApplications] WHERE Id = @PrimaryKey";
        public const string opennewbusinessapplicationsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[OpenNewBusinessApplications] WHERE";
        public const string opennewbusinessapplicationsdatamodel_insert = "INSERT INTO [fetest].[dbo].[OpenNewBusinessApplications] (OfferKey, LTV, SPVKey, PropertyKey, HasDebitOrder, HasMailingAddress, HasProperty, IsAccepted, HouseholdIncome, EmploymentTypeKey) VALUES(@OfferKey, @LTV, @SPVKey, @PropertyKey, @HasDebitOrder, @HasMailingAddress, @HasProperty, @IsAccepted, @HouseholdIncome, @EmploymentTypeKey); select cast(scope_identity() as int)";
        public const string opennewbusinessapplicationsdatamodel_update = "UPDATE [fetest].[dbo].[OpenNewBusinessApplications] SET OfferKey = @OfferKey, LTV = @LTV, SPVKey = @SPVKey, PropertyKey = @PropertyKey, HasDebitOrder = @HasDebitOrder, HasMailingAddress = @HasMailingAddress, HasProperty = @HasProperty, IsAccepted = @IsAccepted, HouseholdIncome = @HouseholdIncome, EmploymentTypeKey = @EmploymentTypeKey WHERE Id = @Id";



        public const string compositedefinitionsdatamodel_selectwhere = "SELECT Id, CompositeKey, Description, CompositeTransitions, TransitionGroup, TransitionDefinition, DateIndicator, Sequence FROM [fetest].[dbo].[CompositeDefinitions] WHERE";
        public const string compositedefinitionsdatamodel_selectbykey = "SELECT Id, CompositeKey, Description, CompositeTransitions, TransitionGroup, TransitionDefinition, DateIndicator, Sequence FROM [fetest].[dbo].[CompositeDefinitions] WHERE Id = @PrimaryKey";
        public const string compositedefinitionsdatamodel_delete = "DELETE FROM [fetest].[dbo].[CompositeDefinitions] WHERE Id = @PrimaryKey";
        public const string compositedefinitionsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[CompositeDefinitions] WHERE";
        public const string compositedefinitionsdatamodel_insert = "INSERT INTO [fetest].[dbo].[CompositeDefinitions] (CompositeKey, Description, CompositeTransitions, TransitionGroup, TransitionDefinition, DateIndicator, Sequence) VALUES(@CompositeKey, @Description, @CompositeTransitions, @TransitionGroup, @TransitionDefinition, @DateIndicator, @Sequence); select cast(scope_identity() as int)";
        public const string compositedefinitionsdatamodel_update = "UPDATE [fetest].[dbo].[CompositeDefinitions] SET CompositeKey = @CompositeKey, Description = @Description, CompositeTransitions = @CompositeTransitions, TransitionGroup = @TransitionGroup, TransitionDefinition = @TransitionDefinition, DateIndicator = @DateIndicator, Sequence = @Sequence WHERE Id = @Id";



        public const string emptythirdpartyinvoicesdatamodel_selectwhere = "SELECT Id, ThirdPartyInvoiceKey FROM [fetest].[dbo].[EmptyThirdPartyInvoices] WHERE";
        public const string emptythirdpartyinvoicesdatamodel_selectbykey = "SELECT Id, ThirdPartyInvoiceKey FROM [fetest].[dbo].[EmptyThirdPartyInvoices] WHERE Id = @PrimaryKey";
        public const string emptythirdpartyinvoicesdatamodel_delete = "DELETE FROM [fetest].[dbo].[EmptyThirdPartyInvoices] WHERE Id = @PrimaryKey";
        public const string emptythirdpartyinvoicesdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[EmptyThirdPartyInvoices] WHERE";
        public const string emptythirdpartyinvoicesdatamodel_insert = "INSERT INTO [fetest].[dbo].[EmptyThirdPartyInvoices] (ThirdPartyInvoiceKey) VALUES(@ThirdPartyInvoiceKey); select cast(scope_identity() as int)";
        public const string emptythirdpartyinvoicesdatamodel_update = "UPDATE [fetest].[dbo].[EmptyThirdPartyInvoices] SET ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey WHERE Id = @Id";



        public const string workflowsearchdatamodel_selectwhere = "SELECT Id, InstanceID, Subject, GenericKey, State, Workflow FROM [fetest].[dbo].[WorkflowSearch] WHERE";
        public const string workflowsearchdatamodel_selectbykey = "SELECT Id, InstanceID, Subject, GenericKey, State, Workflow FROM [fetest].[dbo].[WorkflowSearch] WHERE Id = @PrimaryKey";
        public const string workflowsearchdatamodel_delete = "DELETE FROM [fetest].[dbo].[WorkflowSearch] WHERE Id = @PrimaryKey";
        public const string workflowsearchdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[WorkflowSearch] WHERE";
        public const string workflowsearchdatamodel_insert = "INSERT INTO [fetest].[dbo].[WorkflowSearch] (InstanceID, Subject, GenericKey, State, Workflow) VALUES(@InstanceID, @Subject, @GenericKey, @State, @Workflow); select cast(scope_identity() as int)";
        public const string workflowsearchdatamodel_update = "UPDATE [fetest].[dbo].[WorkflowSearch] SET InstanceID = @InstanceID, Subject = @Subject, GenericKey = @GenericKey, State = @State, Workflow = @Workflow WHERE Id = @Id";



        public const string thirdpartysearchdatamodel_selectwhere = "SELECT Id, LegalName, Email FROM [fetest].[dbo].[ThirdPartySearch] WHERE";
        public const string thirdpartysearchdatamodel_selectbykey = "SELECT Id, LegalName, Email FROM [fetest].[dbo].[ThirdPartySearch] WHERE Id = @PrimaryKey";
        public const string thirdpartysearchdatamodel_delete = "DELETE FROM [fetest].[dbo].[ThirdPartySearch] WHERE Id = @PrimaryKey";
        public const string thirdpartysearchdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[ThirdPartySearch] WHERE";
        public const string thirdpartysearchdatamodel_insert = "INSERT INTO [fetest].[dbo].[ThirdPartySearch] (LegalName, Email) VALUES(@LegalName, @Email); select cast(scope_identity() as int)";
        public const string thirdpartysearchdatamodel_update = "UPDATE [fetest].[dbo].[ThirdPartySearch] SET LegalName = @LegalName, Email = @Email WHERE Id = @Id";



        public const string clientsearchdatamodel_selectwhere = "SELECT Id, IdNumber, LegalName, Email, HasMultipleRoles FROM [fetest].[dbo].[ClientSearch] WHERE";
        public const string clientsearchdatamodel_selectbykey = "SELECT Id, IdNumber, LegalName, Email, HasMultipleRoles FROM [fetest].[dbo].[ClientSearch] WHERE Id = @PrimaryKey";
        public const string clientsearchdatamodel_delete = "DELETE FROM [fetest].[dbo].[ClientSearch] WHERE Id = @PrimaryKey";
        public const string clientsearchdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[ClientSearch] WHERE";
        public const string clientsearchdatamodel_insert = "INSERT INTO [fetest].[dbo].[ClientSearch] (IdNumber, LegalName, Email, HasMultipleRoles) VALUES(@IdNumber, @LegalName, @Email, @HasMultipleRoles); select cast(scope_identity() as int)";
        public const string clientsearchdatamodel_update = "UPDATE [fetest].[dbo].[ClientSearch] SET IdNumber = @IdNumber, LegalName = @LegalName, Email = @Email, HasMultipleRoles = @HasMultipleRoles WHERE Id = @Id";



        public const string halousersdatamodel_selectwhere = "SELECT ADUserKey, ADUserName, LegalEntityKey, UserOrganisationStructureKey, OrgStructureDescription, Capabilities FROM [fetest].[dbo].[HaloUsers] WHERE";
        public const string halousersdatamodel_selectbykey = "SELECT ADUserKey, ADUserName, LegalEntityKey, UserOrganisationStructureKey, OrgStructureDescription, Capabilities FROM [fetest].[dbo].[HaloUsers] WHERE  = @PrimaryKey";
        public const string halousersdatamodel_delete = "DELETE FROM [fetest].[dbo].[HaloUsers] WHERE  = @PrimaryKey";
        public const string halousersdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[HaloUsers] WHERE";
        public const string halousersdatamodel_insert = "INSERT INTO [fetest].[dbo].[HaloUsers] (ADUserKey, ADUserName, LegalEntityKey, UserOrganisationStructureKey, OrgStructureDescription, Capabilities) VALUES(@ADUserKey, @ADUserName, @LegalEntityKey, @UserOrganisationStructureKey, @OrgStructureDescription, @Capabilities); ";
        public const string halousersdatamodel_update = "UPDATE [fetest].[dbo].[HaloUsers] SET ADUserKey = @ADUserKey, ADUserName = @ADUserName, LegalEntityKey = @LegalEntityKey, UserOrganisationStructureKey = @UserOrganisationStructureKey, OrgStructureDescription = @OrgStructureDescription, Capabilities = @Capabilities WHERE  = @";



        public const string losscontrolattorneyinvoicedocumentsdatamodel_selectwhere = "SELECT ID, STOR, GUID FROM [fetest].[dbo].[LossControlAttorneyInvoiceDocuments] WHERE";
        public const string losscontrolattorneyinvoicedocumentsdatamodel_selectbykey = "SELECT ID, STOR, GUID FROM [fetest].[dbo].[LossControlAttorneyInvoiceDocuments] WHERE ID = @PrimaryKey";
        public const string losscontrolattorneyinvoicedocumentsdatamodel_delete = "DELETE FROM [fetest].[dbo].[LossControlAttorneyInvoiceDocuments] WHERE ID = @PrimaryKey";
        public const string losscontrolattorneyinvoicedocumentsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[LossControlAttorneyInvoiceDocuments] WHERE";
        public const string losscontrolattorneyinvoicedocumentsdatamodel_insert = "INSERT INTO [fetest].[dbo].[LossControlAttorneyInvoiceDocuments] (ID, STOR, GUID) VALUES(@ID, @STOR, @GUID); ";
        public const string losscontrolattorneyinvoicedocumentsdatamodel_update = "UPDATE [fetest].[dbo].[LossControlAttorneyInvoiceDocuments] SET ID = @ID, STOR = @STOR, GUID = @GUID WHERE ID = @ID";



        public const string spvsdatamodel_selectwhere = "SELECT SPVKey, Description, ReportDescription, ParentSPVKey, ParentSPVDescription, ParentSPVReportDescription, SPVCompanyKey, SPVCompanyDescription, GeneralStatusKey, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType FROM [fetest].[dbo].[SPVs] WHERE";
        public const string spvsdatamodel_selectbykey = "SELECT SPVKey, Description, ReportDescription, ParentSPVKey, ParentSPVDescription, ParentSPVReportDescription, SPVCompanyKey, SPVCompanyDescription, GeneralStatusKey, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType FROM [fetest].[dbo].[SPVs] WHERE SPVKey = @PrimaryKey";
        public const string spvsdatamodel_delete = "DELETE FROM [fetest].[dbo].[SPVs] WHERE SPVKey = @PrimaryKey";
        public const string spvsdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[SPVs] WHERE";
        public const string spvsdatamodel_insert = "INSERT INTO [fetest].[dbo].[SPVs] (SPVKey, Description, ReportDescription, ParentSPVKey, ParentSPVDescription, ParentSPVReportDescription, SPVCompanyKey, SPVCompanyDescription, GeneralStatusKey, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType) VALUES(@SPVKey, @Description, @ReportDescription, @ParentSPVKey, @ParentSPVDescription, @ParentSPVReportDescription, @SPVCompanyKey, @SPVCompanyDescription, @GeneralStatusKey, @BankAccountKey, @BankName, @BranchCode, @BranchName, @AccountName, @AccountNumber, @AccountType); ";
        public const string spvsdatamodel_update = "UPDATE [fetest].[dbo].[SPVs] SET SPVKey = @SPVKey, Description = @Description, ReportDescription = @ReportDescription, ParentSPVKey = @ParentSPVKey, ParentSPVDescription = @ParentSPVDescription, ParentSPVReportDescription = @ParentSPVReportDescription, SPVCompanyKey = @SPVCompanyKey, SPVCompanyDescription = @SPVCompanyDescription, GeneralStatusKey = @GeneralStatusKey, BankAccountKey = @BankAccountKey, BankName = @BankName, BranchCode = @BranchCode, BranchName = @BranchName, AccountName = @AccountName, AccountNumber = @AccountNumber, AccountType = @AccountType WHERE SPVKey = @SPVKey";



        public const string thirdpartiesdatamodel_selectwhere = "SELECT ThirdPartyKey, Id, LegalEntityKey, TradingName, Contact, GeneralStatusKey, GenericKey, GenericKeyTypeKey, GenericKeyTypeDescription, HasBankAccount, PaymentEmailAddress, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType FROM [fetest].[dbo].[ThirdParties] WHERE";
        public const string thirdpartiesdatamodel_selectbykey = "SELECT ThirdPartyKey, Id, LegalEntityKey, TradingName, Contact, GeneralStatusKey, GenericKey, GenericKeyTypeKey, GenericKeyTypeDescription, HasBankAccount, PaymentEmailAddress, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType FROM [fetest].[dbo].[ThirdParties] WHERE ThirdPartyKey = @PrimaryKey";
        public const string thirdpartiesdatamodel_delete = "DELETE FROM [fetest].[dbo].[ThirdParties] WHERE ThirdPartyKey = @PrimaryKey";
        public const string thirdpartiesdatamodel_deletewhere = "DELETE FROM [fetest].[dbo].[ThirdParties] WHERE";
        public const string thirdpartiesdatamodel_insert = "INSERT INTO [fetest].[dbo].[ThirdParties] (ThirdPartyKey, Id, LegalEntityKey, TradingName, Contact, GeneralStatusKey, GenericKey, GenericKeyTypeKey, GenericKeyTypeDescription, HasBankAccount, PaymentEmailAddress, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType) VALUES(@ThirdPartyKey, @Id, @LegalEntityKey, @TradingName, @Contact, @GeneralStatusKey, @GenericKey, @GenericKeyTypeKey, @GenericKeyTypeDescription, @HasBankAccount, @PaymentEmailAddress, @BankAccountKey, @BankName, @BranchCode, @BranchName, @AccountName, @AccountNumber, @AccountType); ";
        public const string thirdpartiesdatamodel_update = "UPDATE [fetest].[dbo].[ThirdParties] SET ThirdPartyKey = @ThirdPartyKey, Id = @Id, LegalEntityKey = @LegalEntityKey, TradingName = @TradingName, Contact = @Contact, GeneralStatusKey = @GeneralStatusKey, GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, GenericKeyTypeDescription = @GenericKeyTypeDescription, HasBankAccount = @HasBankAccount, PaymentEmailAddress = @PaymentEmailAddress, BankAccountKey = @BankAccountKey, BankName = @BankName, BranchCode = @BranchCode, BranchName = @BranchName, AccountName = @AccountName, AccountNumber = @AccountNumber, AccountType = @AccountType WHERE ThirdPartyKey = @ThirdPartyKey";



    }
}