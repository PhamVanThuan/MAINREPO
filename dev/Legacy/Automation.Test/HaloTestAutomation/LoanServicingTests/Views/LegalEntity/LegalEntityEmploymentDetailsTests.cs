using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Views.LegalEntity
{
    /// <summary>
    ///
    /// </summary>
    [TestFixture, RequiresSTA]
    public class LegalEntityEmploymentDetailsTests : TestBase<LegalEntityEmploymentDetails>
    {
        private int accountKey;
        private string legalEntityName;
        private int legalentityKey;
        private QueryResults results;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            // open browser with test user
            base.Browser = new TestBrowser(TestUsers.FLSupervisor);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            // instantiate out db object
            // Get Latest Open Account With 1 MainApplicant and 1 Employment record
            accountKey = Service<IAccountService>().GetLatestOpenAccountWithOneMainApplicantAndOneEmploymentRecord();
            //remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //go to the legal entity menu
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            // search for the account
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);

            // get the legalentity details
            results = Service<ILegalEntityService>().GetLegalEntityNamesAndRoleByAccountKey(accountKey);
            legalentityKey = results.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
            legalEntityName = results.Rows(0).Column("Name").Value;

            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
        }

        #region LegalEntityEmploymentAdd Tests

        /// <summary>
        /// Check that the following fields are mandatory. a) Employment Type b) Remuneration Type c) Start Date
        /// </summary>
        [Test, Description("Check that the following fields are mandatory. a) Employment Type b) Remuneration Type c) Start Date")]
        public void _001_MandatoryFields()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = "ABSA",
                    MonthlyIncomeRands = 20000,
                    EmploymentType = string.Empty,
                    RemunerationType = string.Empty,
                    StartDate = string.Empty
                };
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employment type is a mandatory field.");

            // capture employment details with no remuneration type & start date
            employment.EmploymentType = EmploymentType.Salaried;
            employment.RemunerationType = string.Empty;
            employment.StartDate = string.Empty;
            base.View.AddEmploymentDetails(employment, true);
            // assert error messages
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employment Start Date is a mandatory field");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Remuneration Type is a mandatory field");
        }

        /// <summary>
        /// Check that 'Employer Name' and 'Monthly Income' fields are not mandatory if 'Employment Type' = 'Unemployed'
        /// </summary>
        [Test, Description("Check that 'Employer Name' and 'Monthly Income' fields are not mandatory if 'Employment Type' = 'Unemployed'")]
        public void _002_NonMandatoryFieldsEmploymentType()
        {
            var employment =
                    new Automation.DataModels.Employment
                    {
                        Employer = string.Empty,
                        MonthlyIncomeRands = 20000,
                        EmploymentType = EmploymentType.Unemployed,
                        RemunerationType = RemunerationType.Unknown,
                        StartDate = "01/01/2010"
                    };
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            Assert.IsTrue(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists() == false);
        }

        /// <summary>
        /// Check that 'Employer Name' is not mandatory if 'Remuneration Type' is
        /// a) Investment Income
        /// b) Maintenance
        /// c) Pension
        /// d) Rental Income
        /// </summary>
        [Test, Description("Check that 'Employer Name' is not mandatory if 'Remuneration Type' is a) Investment Income b) Maintenance c) Pension d) Rental Income")]
        public void _003_NonMandatoryFieldsRemunerationType()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = string.Empty,
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.SelfEmployed,
                    RemunerationType = RemunerationType.InvestmentIncome,
                    StartDate = "01/01/2010"
                };
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            Assert.IsTrue(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists() == false);

            // b) Maintenance
            employment.RemunerationType = RemunerationType.Maintenance;
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            Assert.IsTrue(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists() == false);

            // c) Pension
            employment.RemunerationType = RemunerationType.Pension;
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            Assert.IsTrue(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists() == false);

            // d) Rental Income
            employment.RemunerationType = RemunerationType.RentalIncome;
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            Assert.IsTrue(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists() == false);
        }

        /// <summary>
        /// Check that 'Employer Name' is mandatory for the following 'Employment Type' - 'Remuneration Type' combinations.
        /// a) Salaried - Salaried
        /// b) Salaried - Basic + Commission
        /// c) Salaried - Commission Only
        /// </summary>
        [Test, Description("Check that the following fields are mandatory. a) Employment Type b) Remuneration Type c) Start Date")]
        public void _004_EmployerNameMandatorySalaried()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = string.Empty,
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.Salaried,
                    RemunerationType = RemunerationType.Salaried,
                    StartDate = "01/01/2010"
                };
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
            // b) Salaried - Basic + Commission
            employment.RemunerationType = RemunerationType.BasicPlusCommission;
            base.View.SelectRemunerationType(employment);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
            // c) Salaried - Commission Only
            employment.RemunerationType = RemunerationType.CommissionOnly;
            base.View.SelectRemunerationType(employment);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
        }

        /// <summary>
        /// Check that 'Employer Name' is mandatory for the following 'Employment Type' - 'Remuneration Type' combinations.
        /// a) Self Employed - Commission Only
        /// b) Self Employed - Drawings
        /// c) Self Employed -Business Profits
        /// </summary>
        [Test, Description("Check that the following fields are mandatory. a) Employment Type b) Remuneration Type c) Start Date")]
        public void _004_EmployerNameMandatorySelfEmployed()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = string.Empty,
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.SelfEmployed,
                    RemunerationType = RemunerationType.CommissionOnly,
                    StartDate = "01/01/2010"
                };
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
            // b) Self Employed - Drawings
            employment.RemunerationType = RemunerationType.Drawings;
            base.View.SelectRemunerationType(employment);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
            // c) Self Employed - Business Profits
            employment.RemunerationType = RemunerationType.BusinessProfits;
            base.View.SelectRemunerationType(employment);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
        }

        /// <summary>
        /// Check that 'Employer Name' is mandatory for the following 'Employment Type' - 'Remuneration Type' combinations.
        /// a) Subsidised - Salaried
        /// b) Subsidised - Basic + Commission
        /// </summary>
        [Test, Description("Check that the following fields are mandatory. a) Employment Type b) Remuneration Type c) Start Date")]
        public void _004_EmployerNameMandatorySubsidised()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = string.Empty,
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.SalariedWithDeductions,
                    RemunerationType = RemunerationType.Salaried,
                    StartDate = "01/01/2010"
                };
            base.View.AddEmploymentDetails(employment, true);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
            // b Subsidised - Basic + Commission
            employment.RemunerationType = RemunerationType.BasicPlusCommission;
            base.View.SelectRemunerationType(employment);
            // assert error message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Employer is mandatory for the selected remuneration type");
        }

        /// <summary>
        /// Check that unconfirmed employment details are added on clicking the 'Add' button for the following 'Employment Type' - 'Remuneration Type' combinations.
        /// a) Salaried - Commission Only
        /// b) Self Employed - Commission Only
        /// c) Self Employed - Drawings
        /// d) Self Employed -Business Profits
        /// </summary>
        [Test, Description(@"Check that unconfirmed employment details added.
        a) Salaried - Commission Only, b) Self Employed - Commission Only, c) Self Employed - Drawings, d) Self Employed -Business Profits ")]
        public void _005_UnconfirmedEmploymentAdd()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = "ABSA",
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.Salaried,
                    RemunerationType = RemunerationType.CommissionOnly,
                    StartDate = "01/01/2010"
                };
            // count the number of recs that exist for this combination
            int previousCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.Salaried, RemunerationTypeEnum.CommissionOnly,
                EmploymentStatusEnum.Current).RowList.Count;
            // add the new record
            base.View.AddEmploymentDetails(employment, true);
            // count the number of recs that exist for this combination
            int newCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.Salaried, RemunerationTypeEnum.CommissionOnly,
                EmploymentStatusEnum.Current).RowList.Count;
            // assert that the number of records is now 1 more
            Assert.AreEqual(newCount, previousCount + 1);
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
            // b) Self Employed - Commission Only
            employment.EmploymentType = EmploymentType.SelfEmployed;
            employment.RemunerationType = RemunerationType.CommissionOnly;
            // count the number of recs that exist for this combination
            previousCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.SelfEmployed, RemunerationTypeEnum.CommissionOnly,
                EmploymentStatusEnum.Current).RowList.Count;
            // add the new record
            base.View.AddEmploymentDetails(employment, true);
            // count the number of recs that exist for this combination
            newCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.SelfEmployed, RemunerationTypeEnum.CommissionOnly,
                EmploymentStatusEnum.Current).RowList.Count;
            // assert that the number of records is now 1 more
            Assert.AreEqual(newCount, previousCount + 1);

            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);

            // c) Self Employed - Drawings
            employment.RemunerationType = RemunerationType.Drawings;
            // count the number of recs that exist for this combination
            previousCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.SelfEmployed, RemunerationTypeEnum.Drawings,
                EmploymentStatusEnum.Current).RowList.Count;

            // add the new record
            base.View.AddEmploymentDetails(employment, true);

            // count the number of recs that exist for this combination
            newCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.SelfEmployed, RemunerationTypeEnum.Drawings, EmploymentStatusEnum.Current).RowList.Count;

            // assert that the number of records is now 1 more
            Assert.AreEqual(newCount, previousCount + 1);

            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);

            // d) Self Employed - Business Profits
            employment.RemunerationType = RemunerationType.BusinessProfits;

            // count the number of recs that exist for this combination
            previousCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.SelfEmployed, RemunerationTypeEnum.BusinessProfits, EmploymentStatusEnum.Current).RowList.Count;

            // add the new record
            base.View.AddEmploymentDetails(employment, true);

            // count the number of recs that exist for this combination
            newCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.SelfEmployed, RemunerationTypeEnum.BusinessProfits, EmploymentStatusEnum.Current).RowList.Count;

            // assert that the number of records is now 1 more
            Assert.AreEqual(newCount, previousCount + 1);
        }

        /// <summary>
        /// Check that the label of the 'Add' button changes to 'Next' and that clicking the 'Next' button loads the LegalEntityExtendedAdd page for the following
        /// 'Remuneration Types'
        /// a) Salaried
        /// b) Basic + Commission
        /// </summary>
        [Test, Description("Check the functionality of the 'Next' button for the following 'Remuneration Types' - Salaried,  Basic + Commission")]
        public void _006_NextButtonClick()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = "ABSA",
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.Salaried,
                    RemunerationType = RemunerationType.Salaried,
                    StartDate = "01/01/2010"
                };
            // add the new record but do not click the 'next button'
            base.View.AddEmploymentDetails(employment, false);

            // assert the text on the next button
            base.View.AssertSaveButtonText("Next");
            // click the 'next button'
            base.View.ClickSave();
            // assert that we are now on the extended details page
            Assert.IsTrue(base.Browser.Frames[0].Url.Contains("EmploymentExtended"));

            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);

            // b) Basic + Commission
            employment.EmploymentType = EmploymentType.Salaried;
            employment.RemunerationType = RemunerationType.BasicPlusCommission;

            // add the new record but do not click the 'next button'
            base.View.AddEmploymentDetails(employment, false);

            // assert the text on the next button
            base.View.AssertSaveButtonText("Next");
            // click the 'next button'
            base.View.ClickSave();

            // assert that we are now on the extended details page
            Assert.IsTrue(base.Browser.Frames[0].Url.Contains("EmploymentExtended"));
        }

        /// <summary>
        /// Check that clicking the 'Cancel' button returns to the LegalEntityEmploymentDisplay screen without committing any data
        /// </summary>
        [Test, Description("Check that clicking the 'Cancel' button returns to the LegalEntityEmploymentDisplay? screen without committing any data ")]
        public void _007_CancelButtonClick()
        {
            var employment =
                new Automation.DataModels.Employment
                {
                    Employer = "ABSA",
                    MonthlyIncomeRands = 20000,
                    EmploymentType = EmploymentType.Salaried,
                    RemunerationType = RemunerationType.Salaried,
                    StartDate = "01/01/2010"
                };
            // count the number of recs that exist for this combination
            int previousCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.Salaried, RemunerationTypeEnum.Salaried, EmploymentStatusEnum.Current).RowList.Count;
            // enter the data but do not click 'save'
            base.View.AddEmploymentDetails(employment, false);
            // click the 'cancel' button
            base.View.ClickCancel();
            // count the number of recs that exist for this combination
            int newCount = Service<IEmploymentService>().GetEmploymentByCriteria(legalentityKey, EmploymentTypeEnum.Salaried, RemunerationTypeEnum.Salaried, EmploymentStatusEnum.Current).RowList.Count;
            // assert that the number of records are equal
            Assert.AreEqual(newCount, previousCount);
        }

        #endregion LegalEntityEmploymentAdd Tests

        #region LegalEntityEmploymentExtendedAdd Tests

        /// <summary>
        /// Checks that the Basic Income is manadatory if Remuneration Type is Salaried
        /// </summary>
        [Test, Description("Checks that the Basic Income is manadatory if Remuneration Type is Salaried")]
        public void _008_ExtendedAddMandatoryBasicIncomeSalaried()
        {
            base.View.AddEmploymentDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, Common.Constants.RemunerationType.Salaried, "01/01/2010", "0", true);
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Basic Income Amount must be greater than R 0.00");
        }

        /// <summary>
        /// Checks that the Basic Income is manadatory if Remuneration Type is Commission Only
        /// </summary>
        [Test, Description(" Checks that the Basic Income is manadatory if Remuneration Type is Commission Only")]
        public void _008_ExtendedAddMandatoryBasicIncomeCommissionOnly()
        {
            base.View.AddEmploymentDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, Common.Constants.RemunerationType.CommissionOnly, "01/01/2010", "0", true);
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Basic Income Amount must be greater than R 0.00");
        }

        /// <summary>
        /// Checks that the Basic Income is manadatory if Remuneration Type is Basic + Commission
        /// </summary>
        [Test, Description("Checks that the Basic Income is manadatory if Remuneration Type is Basic + Commission")]
        public void _008_ExtendedAddMandatoryBasicIncomeBasicPlusCommission()
        {
            base.View.AddEmploymentDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, Common.Constants.RemunerationType.BasicPlusCommission, "01/01/2010", "0", true);
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Basic Income Amount must be greater than R 0.00");
        }

        /// <summary>
        /// Checks that the Commission is manadatory if Remuneration Type is Basic + Commission
        /// </summary>
        [Test, Description("Checks that the Commission is manadatory if Remuneration Type is Basic + Commission")]
        public void _009_ExtendedAddMandatoryCommissionBasicPlusCommission()
        {
            base.View.AddEmploymentDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, Common.Constants.RemunerationType.BasicPlusCommission, "01/01/2010", "0", true);
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Commission is mandatory when the remuneration type is Basic + Commission");
        }

        /// <summary>
        /// Checks that the Variable Monthly Income, Gross Monhly Income, Dedcutions and Net Income are calculated correctly
        /// </summary>
        [Test, Description("Checks that the Variable Monthly Income, Gross Monhly Income, Dedcutions and Net Income are calculated correctly")]
        public void _010_ExtendedAddCheckCalculations()
        {
            base.View.AddEmploymentExtendedDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, "Current", Common.Constants.RemunerationType.BasicPlusCommission, "01/01/2010", "",
                "10000", 100, 100, 100, 100, 100, 100, 100, 100, 100, false);
            int VariableMontlyIncome = 500;
            int GrossMonthlyIncome = 10500;
            int Deductions = 400;
            int netIncome = GrossMonthlyIncome - Deductions;
            base.Browser.Page<LegalEntityEmploymentExtendedUpdate>().AssertDetails(VariableMontlyIncome, GrossMonthlyIncome, Deductions, netIncome);
        }

        /// <summary>
        /// Checks Employment Details added for Salaried Employment Type and Rumeration Type Salaried
        /// </summary>
        [Test, Description("Checks Employment Details added for Salaried Employment Type and Rumeneration Type Salaried")]
        public void _011_ExtendedAddCheckEmploymentDetailsSalariedSalaried()
        {
            //Get tthe Exisiting Row Count
            int iExistingRowCount = 0;
            QueryResults existingresults = Service<IEmploymentService>().GetEmploymentByGenericKey(Convert.ToInt32(legalentityKey), true, false);
            iExistingRowCount = existingresults.RowList.Count;

            //Add the new Employment Details
            int iNewRowCount = 0;
            base.View.AddEmploymentDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, Common.Constants.RemunerationType.Salaried, "01/01/2010", "10000", true);
            QueryResults newresults = Service<IEmploymentService>().GetEmploymentByGenericKey(legalentityKey, true, false);
            iNewRowCount = newresults.RowList.Count;

            Assert.AreEqual(iExistingRowCount + 1, iNewRowCount);
        }

        /// <summary>
        /// Checks Employment Details added for Salaried Employment Type and Remuneration Type Basic + Commission
        /// </summary>
        [Test, Description("Checks Employment Details added for Salaried Employment Type and Remuneration Type Basic + Commission")]
        public void _011_ExtendedAddCheckEmploymentDetailsSalariedBasicPlusComms()
        {
            //Get tthe Exisiting Row Count
            int iExistingRowCount = 0;
            QueryResults existingresults = Service<IEmploymentService>().GetEmploymentByGenericKey(Convert.ToInt32(legalentityKey), true, false);
            iExistingRowCount = existingresults.RowList.Count;

            //Add the new Employment Details
            int iNewRowCount = 0;
            base.View.AddEmploymentExtendedDetails("A.H. BUILDERS", Common.Constants.EmploymentType.Salaried, "Current", Common.Constants.RemunerationType.BasicPlusCommission, "01/01/2010", "",
                "10000", 1000, 100, 100, 100, 100, 100, 100, 100, 100, true);
            QueryResults newresults = Service<IEmploymentService>().GetEmploymentByGenericKey(Convert.ToInt32(legalentityKey), true, false);
            iNewRowCount = newresults.RowList.Count;

            Assert.AreEqual(iExistingRowCount + 1, iNewRowCount);
        }

        /// <summary>
        /// Checks that the LegalEntitySubsidyDetailsAdd screen is loaded when the next button is clicked for
        /// the Employment Type Subsidied and Remuneration Type Salaried
        /// </summary>
        [Test, Description(@"Checks that the LegalEntitySubsidyDetailsAdd screen is loaded when the next button is clicked for  the Employment Type Subsidied and Remuneration
                            Type Salaried")]
        public void _012_ExtendedAddCheckSubsidyScreenSubsidiedSalaried()
        {
            base.View.AddEmploymentExtendedDetails("A.H. BUILDERS", Common.Constants.EmploymentType.SalariedWithDeductions, "Current", Common.Constants.RemunerationType.Salaried, "01/01/2010", "", "10000",
                1000, 100, 100, 100, 100, 100, 100, 100, 100, true);
            Assert.IsTrue(base.Browser.Frames[0].Url.Contains("SubsidyDetails"));
        }

        /// <summary>
        /// Checks that the LegalEntitySubsidyDetailsAdd screen is loaded when the next button is clicked for
        /// the Employment Type Subsidied and Remuneration Type Basic + Commission
        /// </summary>
        [Test, Description(@"Checks that the LegalEntitySubsidyDetailsAdd screen is loaded when the next button is clicked for the Employment Type Subsidied
                        and Remuneration Type Basic + Commission")]
        public void _012_ExtendedAddCheckSubsidyScreenSubsidiedBasicPlusComms()
        {
            base.View.AddEmploymentExtendedDetails("A.H. BUILDERS", Common.Constants.EmploymentType.SalariedWithDeductions, "Current", Common.Constants.RemunerationType.BasicPlusCommission, "01/01/2010", "",
                "10000", 1000, 100, 100, 100, 100, 100, 100, 100, 100, true);
            Assert.IsTrue(base.Browser.Frames[0].Url.Contains("SubsidyDetails"));
        }

        /// <summary>
        /// Checks that clicking the Back button returns the user to the LegalEntityEmploymentAdd screen
        /// with all user selections maintained
        /// </summary>
        [Test, Description("Checks that clicking the Back button returns the user to the LegalEntityEmploymentAdd screen with all user selections maintained")]
        public void _013_ExtendedAddCheckBackButton()
        {
            string EmployerName = "A.H. BUILDERS";
            string EmploymentType = Common.Constants.EmploymentType.SalariedWithDeductions;
            string RemunerationType = Common.Constants.RemunerationType.BasicPlusCommission;
            string EmploymentStatus = "Current";
            string StartDate = "01/01/2010";
            string EndDate = "";
            base.View.AddEmploymentExtendedDetails(EmployerName, EmploymentType, EmploymentStatus,
                RemunerationType, StartDate, EndDate, "10000", 1000, 100, 100, 100, 100, 100, 100, 100, 100, false);

            base.Browser.Page<LegalEntityEmploymentExtendedUpdate>().ClickBack();
            base.View.AssertEmployerDetails(EmployerName, EmploymentType, EmploymentStatus, RemunerationType, StartDate);
        }

        /// <summary>
        /// Checks that clicking the Cancel button returns the user to the LegalEntityEmploymentAdd screen
        /// with all user selections removed and the default selections restored
        /// </summary>
        [Test, Description(@"Checks that clicking the Cancel button returns the user to the LegalEntityEmploymentAdd screen with all user selections removed
                        and the default selections restored")]
        public void _014_ExtendedAddCheckCancelButton()
        {
            string EmployerName = "A.H. BUILDERS";
            string EmploymentType = Common.Constants.EmploymentType.SalariedWithDeductions;
            string RemunerationType = Common.Constants.RemunerationType.BasicPlusCommission;
            string EmploymentStatus = "Current";
            string StartDate = "01/01/2010";
            string EndDate = "";
            base.View.AddEmploymentExtendedDetails(EmployerName, EmploymentType, EmploymentStatus, RemunerationType, StartDate,
                EndDate, "10000", 1000, 100, 100, 100, 100, 100, 100, 100, 100, false);
            base.Browser.Page<LegalEntityEmploymentExtendedUpdate>().ClickCancel();
            base.View.AssertEmployerControlsReset();
        }

        #endregion LegalEntityEmploymentExtendedAdd Tests

        #region LegalEntitySubsidyDetailsAdd Tests

        /// <summary>
        ///
        /// </summary>
        [Test, Description("Mandatory field test for the Subsidy view")]
        public void _015_SubsidyDetailsAddTest()
        {
            Automation.DataModels.Employment employment = Service<IEmploymentService>().GetSubsidisedEmployment();
            employment.StopOrderAmount = 0;
            employment.SalaryNumber = string.Empty;
            employment.SubsidyProvider = string.Empty;
            base.View.AddEmploymentDetails(employment, true);
            // SubsidyProvider
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>
                ().AddSubsidyDetailsForAccount(Convert.ToInt32(accountKey), employment, Common.Enums.ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Subsidy Provider is a mandatory field");
            employment.SubsidyProvider = "SOEKOR";
            // SalaryNo
            employment.SalaryNumber = string.Empty;
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>
                ().AddSubsidyDetailsForAccount(Convert.ToInt32(accountKey), employment, Common.Enums.ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Salary Number is a mandatory field");
            employment.SalaryNumber = "1234567";
            employment.StopOrderAmount = 0;
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>
                ().AddSubsidyDetailsForAccount(Convert.ToInt32(accountKey), employment, Common.Enums.ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Stop Order Amount must be greater than R 0.01 and less than R 999 999 999.99");
            employment.StopOrderAmount = 5000;
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>
                ().AddSubsidyDetailsForAccount(Convert.ToInt32(accountKey), employment, Common.Enums.ButtonTypeEnum.Save);
        }

        /// <summary>
        /// Checks that clicking the Cancel button returns the user to the LegalEntityEmploymentAdd screen
        /// with all user selections removed and the default selections restored
        /// </summary>
        [Test, Description("Checks that clicking the Cancel button returns the user to the LegalEntityEmploymentAdd screen with all user selections removed and the default selections restored")]
        public void _016_SubsidyDetailsCheckCancelButton()
        {
            var employment = Service<IEmploymentService>().GetSubsidisedEmployment();
            employment.StopOrderAmount = 0;
            employment.SalaryNumber = string.Empty;
            employment.SubsidyProvider = string.Empty;
            base.View.AddEmploymentDetails(employment, true);
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>().ClickCancel();
            Assert.IsTrue(base.Browser.Frames[0].Url.Contains("Employment.aspx"));
            base.View.AssertEmploymentAddDetails(string.Empty, "- Please select -", "Current", "- Please select -", string.Empty);
        }

        /// <summary>
        /// Checks that clicking the Back button returns the user to the LegalEntityEmploymentSubsidy Details Screen
        /// with all user selections maintained
        /// </summary>
        [Test, Description("Checks that clicking the Back button returns the user to the LegalEntityEmploymentAdd screen with all user selections maintained")]
        public void _017_SubsidyDetailsCheckBackButton()
        {
            var employment = Service<IEmploymentService>().GetSubsidisedEmployment();
            employment.StopOrderAmount = 0;
            employment.SalaryNumber = string.Empty;
            employment.SubsidyProvider = string.Empty;
            base.View.AddEmploymentDetails(employment, true);
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>().ClickBack();
            Assert.IsTrue(base.Browser.Frames[0].Url.Contains("EmploymentExtended.aspx"));
        }

        #endregion LegalEntitySubsidyDetailsAdd Tests
    }
}