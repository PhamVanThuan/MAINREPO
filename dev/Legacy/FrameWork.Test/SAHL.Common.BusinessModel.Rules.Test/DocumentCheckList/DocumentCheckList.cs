using System;
using System.Data;
using System.Configuration;
using System.Web;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Rules.DocumentCheckList;
using SAHL.Common.BusinessModel.Rules.Test.LegalEntity;
using System.Text;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Rules.Test.DocumentCheckList
{

    [TestFixture]
    public class DocumentCheckList : LegalEntityBase
    {
        //
        IApplicationMortgageLoan _application;
        Dictionary<int, string> _genericList;
        IDocumentSetConfig _docSetConfig;
        IApplicationRole _appRoleMA;
        IApplicationRoleTypeGroup _appRoleTypeGroup;
        IGeneralStatus _genStatus;
        IApplicationRoleType _appRoleTypeMA;
        ILegalEntityNaturalPerson _LeNP;
        ILegalEntityCloseCorporation _LeCC;
        ILegalEntityCompany _LeComp;
        ILegalEntityTrust _LeTrust;
        ICitizenType _citizenType;
        IDocumentType _docType;
        IGenericKeyType _genKeyType;
        IEmployment _employment;
        IEmploymentType _employmentType;
        IEmploymentStatus _empStatus;
        Stack<IApplicationRole> _appRoleStack;
        IEventList<IEmployment> _empList;
        IMaritalStatus _maritalStatus;
        IApplicationType _applicationType;
        IEventList<IProperty> _propertyList;
        IOccupancyType _occupancyType;
        IProperty _property;
        IAccountSequence _reservedAccount;
        //
        IApplicationProductVariableLoan _applicationProductVariableLoan;
        IApplicationInformationVariableLoan _vli;
        //
        int _maritalStatusKey = (int)MaritalStatuses.MarriedAnteNuptualContract;
        int _citizenTypeKey = (int)CitizenTypes.SACitizen;
        int _ageNextBirthday = 60;
        int _employmentTypeKey = (int)EmploymentTypes.Salaried;
        int _applicationTypeKey = (int)OfferTypes.SwitchLoan;
        double _loanAgreementAmount = Convert.ToDouble(1600500.00);
        int _leTypeKey = (int)LeTypes.LENP;
        int _applicationKey = 1;
        int _legalEntityKey = 1;
        int _occupancyTypeKey = (int)OccupancyTypes.InvestmentProperty;
        int _reservedAccountKey = 1;
        //
        public enum LeTypes
        {
            LENP = 1,
            LECC = 2,
            LETrust = 3,
            LEComp =4
        }
        //
        List<Reason_DAO> ReasonsToDeleted;
        //
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            Messages = new DomainMessageCollection();
        }

        [TearDown]
        public new void TearDown()
        {
            DeleteReasons();
            base.TearDown();

        }

        
        [Test]
        public void ValidateTest()
        {
            using (new TransactionScope(TransactionMode.New))
            {
                DocumentCheckListValidate rule = new DocumentCheckListValidate();
                IApplicationRepository _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplication _application = _appRepo.GetApplicationByKey(Convert.ToInt32(701828)); // TODO: Hardcoded Key!!! What the.. 
                ExecuteRule(rule, 0, _application);
            }
        }

        [Test]
        public void LegalEntitySAIDTest()
        {
            DocumentCheckListRequiredLegalEntitySAID rule = new DocumentCheckListRequiredLegalEntitySAID();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count,0);
        }

        [Test]
        public void LegalEntitySalarySliporLetterofAppointmentorIRP5Test()
        {
            DocumentCheckListRequiredLegalEntitySalarySliporLetterofAppointmentorIRP5 rule = new DocumentCheckListRequiredLegalEntitySalarySliporLetterofAppointmentorIRP5();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityMarriageCertificateTest()
        {
            DocumentCheckListRequiredLegalEntityMarriageCertificate rule = new DocumentCheckListRequiredLegalEntityMarriageCertificate();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.AreEqual(1, _genericList.Count);
        }

        [Test]
        public void LegalEntityANCContractTest()
        {
            DocumentCheckListRequiredLegalEntityANCContract rule = new DocumentCheckListRequiredLegalEntityANCContract();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityDivorceAgreementTest()
        {
            DocumentCheckListRequiredLegalEntityDivorceAgreement rule = new DocumentCheckListRequiredLegalEntityDivorceAgreement();
            SetUpDefault();
            _maritalStatusKey = (int)MaritalStatuses.Divorced;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityPassportTest()
        {
            DocumentCheckListRequiredLegalEntityPassport rule = new DocumentCheckListRequiredLegalEntityPassport();
            SetUpDefault();
            _citizenTypeKey = (int)CitizenTypes.Foreigner;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationIncomeAndExpenditureTest()
        {
            DocumentCheckListRequiredApplicationIncomeAndExpenditure rule = new DocumentCheckListRequiredApplicationIncomeAndExpenditure();
            SetUpDefault(); 
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityAssetsAndLiabilitiesTest()
        {
            DocumentCheckListRequiredLegalEntityAssetsAndLiabilities rule = new DocumentCheckListRequiredLegalEntityAssetsAndLiabilities();
            SetUpDefault(); 
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }




        [Test]
        public void LegalEntityLatestComplete3MonthBankStatementsPersonalTest()
        {
            DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsPersonal rule = new DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsPersonal();
            SetUpDefault(); 
            _employmentTypeKey = (int)EmploymentTypes.Salaried;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityLatestComplete6MonthBankStatementsPersonalTest()
        {
            DocumentCheckListRequiredLegalEntityLatestComplete6MonthBankStatementsPersonal rule = new DocumentCheckListRequiredLegalEntityLatestComplete6MonthBankStatementsPersonal();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityAnnualFinancialStatementsPast2YearsTest()
        {
            DocumentCheckListRequiredLegalEntityAnnualFinancialStatementsPast2Years rule = new DocumentCheckListRequiredLegalEntityAnnualFinancialStatementsPast2Years();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityManagementAccountsNotOlder2MonthsTest()
        {
            DocumentCheckListRequiredLegalEntityManagementAccountsNotOlder2Months rule = new DocumentCheckListRequiredLegalEntityManagementAccountsNotOlder2Months();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }


        [Test]
        public void LegalEntityLatestComplete3MonthBankStatementsBusinessTest()
        {
            DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsBusiness rule = new DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsBusiness();
            SetUpDefault(); 
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityFullGeneralTest()
        {
            DocumentCheckListRequiredLegalEntityFullGeneral rule = new DocumentCheckListRequiredLegalEntityFullGeneral();
            SetUpDefault(); 
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityFinancialStatementsTest()
        {
            DocumentCheckListRequiredLegalEntityFinancialStatements rule = new DocumentCheckListRequiredLegalEntityFinancialStatements();
            SetUpDefault();
            _applicationTypeKey = (int)OfferTypes.NewPurchaseLoan;
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityAccountantLetterorIT34Test()
        {
            DocumentCheckListRequiredLegalEntityAccountantLetterorIT34 rule = new DocumentCheckListRequiredLegalEntityAccountantLetterorIT34();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }



        [Test]
        public void LegalEntityOriginalOrAmendedCertCopyFoundingStatementTest()
        {
            DocumentCheckListRequiredLegalEntityOriginalOrAmendedCertCopyFoundingStatement rule = new DocumentCheckListRequiredLegalEntityOriginalOrAmendedCertCopyFoundingStatement();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            _leTypeKey = (int)LeTypes.LECC;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }



        [Test]
        public void LegalEntityDeedTrustOrLetterAuthorityMasterSupremeCourtTest()
        {
            DocumentCheckListRequiredLegalEntityDeedTrustOrLetterAuthorityMasterSupremeCourt rule = new DocumentCheckListRequiredLegalEntityDeedTrustOrLetterAuthorityMasterSupremeCourt();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            _leTypeKey = (int)LeTypes.LETrust;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityCertOfIncorporationTest()
        {
            DocumentCheckListRequiredLegalEntityCertOfIncorporation rule = new DocumentCheckListRequiredLegalEntityCertOfIncorporation();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            _leTypeKey = (int)LeTypes.LEComp;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityMemoAndArticlesofAssocTest()
        {
            DocumentCheckListRequiredLegalEntityMemoAndArticlesofAssoc rule = new DocumentCheckListRequiredLegalEntityMemoAndArticlesofAssoc();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            _leTypeKey = (int)LeTypes.LEComp;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void CertToCommenceBusinessTest()
        {
            DocumentCheckListCertToCommenceBusiness rule = new DocumentCheckListCertToCommenceBusiness();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            _leTypeKey = (int)LeTypes.LEComp;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityCopyOfChangeofNameTest()
        {
            DocumentCheckListRequiredLegalEntityCopyOfChangeofName rule = new DocumentCheckListRequiredLegalEntityCopyOfChangeofName();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            _leTypeKey = (int)LeTypes.LEComp;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void LegalEntityITCJudgementLetterOfSettlementTest()
        {
            DocumentCheckListRequiredLegalEntityITCJudgementLetterOfSettlement rule = new DocumentCheckListRequiredLegalEntityITCJudgementLetterOfSettlement();
            SetUpDefault();
            List<int> _reasonList = new List<int>();
            _reasonList.Add((int)ReasonDescriptions.ITCJudgement);
            _reasonList.Add((int)ReasonDescriptions.ITCDefault);
            CreateReasons(_reasonList, _applicationKey, (int)GenericKeyTypes.Offer);
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
        }

        [Test]
        public void LegalEntityITCNoticeCertificateOfRehabilitationTest()
        {
            DocumentCheckListRequiredLegalEntityITCNoticeCertificateOfRehabilitation rule = new DocumentCheckListRequiredLegalEntityITCNoticeCertificateOfRehabilitation();
            SetUpDefault();
            List<int> _reasonList = new List<int>();
            _reasonList.Add((int)ReasonDescriptions.ITCNotices);
            CreateReasons(_reasonList, _applicationKey, (int)GenericKeyTypes.Offer);
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
        }

        [Test]
        public void LegalEntityITCArrearsProofOfPaymentTest()
        {
            DocumentCheckListRequiredLegalEntityITCArrearsProofOfPayment rule = new DocumentCheckListRequiredLegalEntityITCArrearsProofOfPayment();
            SetUpDefault();
            List<int> _reasonList = new List<int>();
            _reasonList.Add((int)ReasonDescriptions.ITCPaymentProfileArrears);
            CreateReasons(_reasonList, _applicationKey, (int)GenericKeyTypes.Offer);
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
        }

        [Test]
        public void ApplicationRatesStatementTest()
        {
            DocumentCheckListRequiredApplicationRatesStatement rule = new DocumentCheckListRequiredApplicationRatesStatement();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationUtilityStatement()
        {
            DocumentCheckListRequiredApplicationUtilityStatement rule = new DocumentCheckListRequiredApplicationUtilityStatement();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationLatestcomplete3monthbondstatementsTest()
        {
            DocumentCheckListRequiredApplicationLatestcomplete3monthbondstatements rule = new DocumentCheckListRequiredApplicationLatestcomplete3monthbondstatements();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationLatestcomplete12monthbondstatementsTest()
        {
            DocumentCheckListRequiredApplicationLatestcomplete12monthbondstatements rule = new DocumentCheckListRequiredApplicationLatestcomplete12monthbondstatements();
            SetUpDefault();
            _employmentTypeKey = (int)EmploymentTypes.SelfEmployed;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationSaleAgreementTest()
        {
            DocumentCheckListRequiredApplicationSaleAgreement rule = new DocumentCheckListRequiredApplicationSaleAgreement();
            SetUpDefault();
            _applicationTypeKey = (int)OfferTypes.NewPurchaseLoan;
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);

        }

        [Test]
        public void ApplicationLeaseAgreementsTest()
        {
            DocumentCheckListRequiredApplicationLeaseAgreements rule = new DocumentCheckListRequiredApplicationLeaseAgreements();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationScheduleOfRentalsTest()
        {
            DocumentCheckListRequiredApplicationScheduleOfRentals rule = new DocumentCheckListRequiredApplicationScheduleOfRentals();
            SetUpDefault();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationDeedsInterdictAndAttachmentProofOfRemovalTest()
        {
            DocumentCheckListRequiredApplicationDeedsInterdictAndAttachmentProofOfRemoval rule = new DocumentCheckListRequiredApplicationDeedsInterdictAndAttachmentProofOfRemoval();
            SetUpDefault();
            DocCheckListHelper();
            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsInterdictAttachment);
            CreateReasons(reasons, _applicationKey, (int)GenericKeyTypes.Offer);
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationDeedExtentLetterOfMotivationTest()
        {
            DocumentCheckListRequiredApplicationDeedExtentLetterOfMotivation rule = new DocumentCheckListRequiredApplicationDeedExtentLetterOfMotivation();
            SetUpDefault();
            DocCheckListHelper();
            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsExtent);
            CreateReasons(reasons, _applicationKey, (int)GenericKeyTypes.Offer);
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationDeedOwnershipSaleAgreementsTest()
        {
            DocumentCheckListRequiredApplicationDeedOwnershipSaleAgreements rule = new DocumentCheckListRequiredApplicationDeedOwnershipSaleAgreements();
            SetUpDefault();
            DocCheckListHelper();
            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsOwnership);
            CreateReasons(reasons, _applicationKey, (int)GenericKeyTypes.Offer);
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        [Test]
        public void ApplicationDeedOfGrantProofOfChangeFromAttorneysTest()
        {
            DocumentCheckListRequiredApplicationDeedOfGrantProofOfChangeFromAttorneys rule = new DocumentCheckListRequiredApplicationDeedOfGrantProofOfChangeFromAttorneys();
            SetUpDefault();
            DocCheckListHelper();
            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsBondGrantRights);
            CreateReasons(reasons, _applicationKey, (int)GenericKeyTypes.Offer);
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
            Assert.Greater(_genericList.Count, 0);
        }

        #region Helper Tests and Methods

        [Test]
        public void AddDocumentTrueTest()
        {
            DocumentCheckListAddDocumentTrue rule = new DocumentCheckListAddDocumentTrue();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
        }

        [Test]
        public void AddDocumentFalseTest()
        {
            DocumentCheckListAddDocumentFalse rule = new DocumentCheckListAddDocumentFalse();
            DocCheckListHelper();
            ExecuteRule(rule, 0, _docSetConfig, _genericList, _application);
        }

        [Test]
        public void ITCCheckFailedTest()
        {

            // Just test the method to make sure there are no coding errors.
            /*
           Application_DAO _firstApp = Application_DAO.FindFirst();
           IApplication _app = new SAHL.Common.BusinessModel.Application(_firstApp);
           DCLHelper.ITCCheckFailed(_app.ReservedAccount.Key, _app.ApplicationRoles[0].LegalEntity.Key, "BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07");
           */
            int _AccountKey = 2231926;
            int _LegalEntityKey = 92133;

            //string description = "BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07/TUBureau:DefaultAmount";
            List<String> descriptions = new List<string>();
            descriptions.Add("BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07");
            descriptions.Add("BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07/TUBureau:DefaultAmount");
            descriptions.Add("BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1/TUBureau:DefaultAmount");

            // We need to make sure that all query formats work
            foreach (string itcCheck in descriptions)
            {
                DCLHelper.ITCCheckFailed(_AccountKey, _LegalEntityKey, itcCheck);
            }
        }

        [Test]
        public void ITCPaymentProfileTestTest()
        {

            // Just test the method to make sure there are no coding errors.
            /*
           Application_DAO _firstApp = Application_DAO.FindFirst();
           IApplication _app = new SAHL.Common.BusinessModel.Application(_firstApp);
           DCLHelper.ITCCheckFailed(_app.ReservedAccount.Key, _app.ApplicationRoles[0].LegalEntity.Key, "BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07");
           */
            int _AccountKey = 2311956;
            int _LegalEntityKey = 549036;
            DCLHelper.ITCPaymentProfileTest(_LegalEntityKey, _AccountKey);
        }


        private void DocCheckListHelper()
        {
            _application = _mockery.StrictMock<IApplicationMortgageLoan>();
            _genericList = new Dictionary<int, string>();
            _docSetConfig = _mockery.StrictMock<IDocumentSetConfig>();
            _appRoleMA = _mockery.StrictMock<IApplicationRole>();
            _genStatus = _mockery.StrictMock<IGeneralStatus>();
            _appRoleTypeMA = _mockery.StrictMock<IApplicationRoleType>();
            _LeNP = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            _LeCC = _mockery.StrictMock<ILegalEntityCloseCorporation>();
            _LeComp = _mockery.StrictMock<ILegalEntityCompany>();
            _LeTrust = _mockery.StrictMock<ILegalEntityTrust>();
            _citizenType = _mockery.StrictMock<ICitizenType>();
            _docType = _mockery.StrictMock<IDocumentType>();
            _genKeyType = _mockery.StrictMock<IGenericKeyType>();
            _employment = _mockery.StrictMock<IEmployment>();
            _employmentType = _mockery.StrictMock<IEmploymentType>();
            _empStatus = _mockery.StrictMock<IEmploymentStatus>();
            _maritalStatus = _mockery.StrictMock<IMaritalStatus>();
            _applicationType = _mockery.StrictMock<IApplicationType>();
            _applicationProductVariableLoan = _mockery.StrictMock<IApplicationProductVariableLoan>();
            _vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            _property = _mockery.StrictMock<IProperty>();
            _propertyList = new EventList<IProperty>();
            _occupancyType = _mockery.StrictMock<IOccupancyType>();
            _appRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            _reservedAccount = _mockery.StrictMock<IAccountSequence>();
            //
            SetupResult.For(_reservedAccount.Key).Return(_reservedAccountKey);
            //
            SetupResult.For(_appRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);
            //
            SetupResult.For(_occupancyType.Key).Return(_occupancyTypeKey);
            //
            SetupResult.For(_property.OccupancyType).Return(_occupancyType);
            //
            _propertyList.Add(null, _property);
            //
            SetupResult.For(_applicationType.Key).Return(_applicationTypeKey);
            //
            _appRoleStack = new Stack<IApplicationRole>();
            _empList = new EventList<IEmployment>();
            //
            SetupResult.For(_maritalStatus.Key).Return(_maritalStatusKey);
            //
            SetupResult.For(_empStatus.Key).Return((int)EmploymentStatuses.Current);
            //
            SetupResult.For(_employmentType.Key).Return(_employmentTypeKey);
            //
            SetupResult.For(_employment.EmploymentType).Return(_employmentType);
            //
            SetupResult.For(_employment.EmploymentStatus).Return(_empStatus);
            _empList.Add(null, _employment);
            //
            SetupResult.For(_vli.LoanAgreementAmount).Return(_loanAgreementAmount);
            SetupResult.For(_vli.EmploymentType).Return(_employmentType);
            //
            SetupResult.For(_applicationProductVariableLoan.VariableLoanInformation).Return(_vli);
            //
            SetupResult.For(_genKeyType.Key).Return((int)GenericKeyTypes.LegalEntity);
            SetupResult.For(_genKeyType.Description).Return(GenericKeyTypes.LegalEntity.ToString());
            //
            SetupResult.For(_docType.GenericKeyType).Return(_genKeyType);
            SetupResult.For(_docType.Description).Return("description");
            //
            SetupResult.For(_appRoleTypeMA.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(_appRoleTypeMA.Description).Return(OfferRoleTypes.MainApplicant.ToString());
            SetupResult.For(_appRoleTypeMA.ApplicationRoleTypeGroup).Return(_appRoleTypeGroup);
            //
            SetupResult.For(_genStatus.Key).Return((int)GeneralStatuses.Active);
            //
            SetupResult.For(_citizenType.Key).Return(_citizenTypeKey);
            //
            SetupResult.For(_appRoleMA.ApplicationRoleType).Return(_appRoleTypeMA);
            SetupResult.For(_appRoleMA.GeneralStatus).Return(_genStatus);
            //
            SetupResult.For(_LeNP.CitizenType).Return(_citizenType);
            SetupResult.For(_LeNP.DisplayName).Return("DisplayName");
            SetupResult.For(_LeNP.Key).Return(_legalEntityKey);
            SetupResult.For(_LeNP.Employment).Return(_empList);
            SetupResult.For(_LeNP.MaritalStatus).Return(_maritalStatus);
            SetupResult.For(_LeNP.AgeNextBirthday).Return(_ageNextBirthday);
            //
            SetupResult.For(_LeCC.DisplayName).Return("DisplayName");
            SetupResult.For(_LeCC.Key).Return(_legalEntityKey);
            SetupResult.For(_LeCC.Employment).Return(_empList);
            //
            SetupResult.For(_LeComp.DisplayName).Return("DisplayName");
            SetupResult.For(_LeComp.Key).Return(_legalEntityKey);
            SetupResult.For(_LeComp.Employment).Return(_empList);
            //
            SetupResult.For(_LeTrust.DisplayName).Return("DisplayName");
            SetupResult.For(_LeTrust.Key).Return(_legalEntityKey);
            SetupResult.For(_LeTrust.Employment).Return(_empList);
            //
            SetupResult.For(_docSetConfig.DocumentType).Return(_docType);
            //
            switch (_leTypeKey)
            {
                case (int)LeTypes.LENP:
                    SetupResult.For(_appRoleMA.LegalEntity).Return(_LeNP);
                    break;
                case (int)LeTypes.LECC:
                    SetupResult.For(_appRoleMA.LegalEntity).Return(_LeCC);
                    break;
                case (int)LeTypes.LETrust:
                    SetupResult.For(_appRoleMA.LegalEntity).Return(_LeTrust);
                    break;
                case (int)LeTypes.LEComp:
                    SetupResult.For(_appRoleMA.LegalEntity).Return(_LeComp);
                    break;
            }
            //
            _appRoleStack.Push(_appRoleMA);
            //
            IReadOnlyEventList<IApplicationRole> appRoles = new ReadOnlyEventList<IApplicationRole>(_appRoleStack);
            //
            SetupResult.For(_application.ApplicationRoles).Return(appRoles);
            //
            SetupResult.For(_application.Key).Return(_applicationKey);
            //
            SetupResult.For(_application.CurrentProduct).Return(_applicationProductVariableLoan);
            //
            SetupResult.For(_application.ApplicationType).Return(_applicationType);
            //
            SetupResult.For(_application.Property).Return(_property);
            //
            SetupResult.For(_application.ReservedAccount).Return(_reservedAccount);
            //
        }

        private void SetUpDefault()
        {
            _maritalStatusKey = (int)MaritalStatuses.MarriedAnteNuptualContract;
            _citizenTypeKey = (int)CitizenTypes.SACitizen;
            _ageNextBirthday = 60;
            _employmentTypeKey = (int)EmploymentTypes.Salaried;
            _applicationTypeKey = (int)OfferTypes.SwitchLoan;
            _loanAgreementAmount = Convert.ToDouble(1600500.00);
            _leTypeKey = (int)LeTypes.LENP;
            _applicationKey = 1;
            _legalEntityKey = 1;
            _reservedAccountKey = 1;
        }

        private void CreateReasons(List<int> reasons, int GenericKey, int GenericKeyTypeKey)
        {
            ReasonsToDeleted = new List<Reason_DAO>();

            foreach (int reasonDescKey in reasons)
            {
                string HQL = "select rd from ReasonDefinition_DAO rd where rd.ReasonDescription.Key = ? and rd.ReasonType.GenericKeyType.Key = ?";
                SimpleQuery<ReasonDefinition_DAO> q = new SimpleQuery<ReasonDefinition_DAO>(HQL, reasonDescKey, GenericKeyTypeKey);
                ReasonDefinition_DAO[] res = q.Execute();

                if (res.Length == 0)
                    Assert.Fail();

                using (new SessionScope())
                {
                    Reason_DAO _reason = new Reason_DAO();
                    _reason.GenericKey = GenericKey;
                    _reason.ReasonDefinition = res[0];
                    _reason.Comment = "test reason";
                    _reason.SaveAndFlush();
                    ReasonsToDeleted.Add(_reason);
                }
            }
        }

        private void DeleteReasons()
        {
            if (ReasonsToDeleted != null && ReasonsToDeleted.Count > 0)
            {
                foreach (Reason_DAO _reason in ReasonsToDeleted)
                {
                    using (new SessionScope())
                    {
                        _reason.DeleteAndFlush();
                    }
                }
                ReasonsToDeleted = new List<Reason_DAO>();
            }
        }

        #endregion
    }
}
