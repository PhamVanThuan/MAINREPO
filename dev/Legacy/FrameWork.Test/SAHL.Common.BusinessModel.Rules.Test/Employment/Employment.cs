using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Rules.Employment;

namespace SAHL.Common.BusinessModel.Rules.Test.Employment
{
    [TestFixture]
    public class Employment : RuleBase
    {
        //protected IEmploymentType employmentType = null;
        //protected IEmployer employer = null;
        private IEmployment _employment = null;
        //protected IEmployerBusinessType empbus = null;
        //protected IEmploymentSector empsec = null;
        //protected IRemunerationType renum = null;
        //protected IEmploymentStatus empStat = null;
        //protected ILegalEntity le = null;

        [NUnit.Framework.SetUp]
        public override void Setup()
        {
            base.Setup();
            _employment = _mockery.StrictMock<IEmployment>();
        }

        [TearDown]
        public override void TearDown()
        {
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentConfirmedIncomeMinimum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentConfirmedIncomeMinimum()
        {
            EmploymentConfirmedIncomeMinimum rule = new EmploymentConfirmedIncomeMinimum();
            IEmploymentUnemployed employmentUnemployed = _mockery.StrictMock<IEmploymentUnemployed>();
            IEmploymentSalaried employmentSalaried = _mockery.StrictMock<IEmploymentSalaried>();

            // unemployed object in - should pass
            ExecuteRule(rule, 0, employmentUnemployed);

            // salaried object not requiring extended info with confirmed income of 0 but with status of previous - should pass
            EmploymentConfirmedIncomeMinimumHelper(ref employmentSalaried, EmploymentStatuses.Previous, true);
            SetupResult.For(employmentSalaried.ConfirmedIncome).Return(new double?(0D).Value);
            ExecuteRule(rule, 0, employmentSalaried);

            // unconfirmed employment object in - should pass even though value is 0
            EmploymentConfirmedIncomeMinimumHelper(ref employmentSalaried, EmploymentStatuses.Current, false);
            SetupResult.For(employmentSalaried.ConfirmedIncome).Return(new double?(0D).Value);
            ExecuteRule(rule, 0, employmentSalaried);

            // salaried object not requiring extended info with confirmed income of 0 - should fail
            EmploymentConfirmedIncomeMinimumHelper(ref employmentSalaried, EmploymentStatuses.Current, true);
            SetupResult.For(employmentSalaried.ConfirmedIncome).Return(new double?(0D).Value);
            ExecuteRule(rule, 1, employmentSalaried);

            // salaried object requiring extended info with confirmed income of 0 - should fail
            EmploymentConfirmedIncomeMinimumHelper(ref employmentSalaried, EmploymentStatuses.Current, true);
            SetupResult.For(employmentSalaried.ConfirmedIncome).Return(new double?(0D).Value);
            ExecuteRule(rule, 1, employmentSalaried);

            // salaried object not requiring extended info with confirmed income of 1 - should pass
            EmploymentConfirmedIncomeMinimumHelper(ref employmentSalaried, EmploymentStatuses.Current, true);
            SetupResult.For(employmentSalaried.ConfirmedIncome).Return(new double?(1D).Value);
            ExecuteRule(rule, 0, employmentSalaried);

            // salaried object requiring extended info with confirmed income of 1 - should pass
            EmploymentConfirmedIncomeMinimumHelper(ref employmentSalaried, EmploymentStatuses.Current, true);
            SetupResult.For(employmentSalaried.ConfirmedIncome).Return(new double?(1D).Value);
            ExecuteRule(rule, 0, employmentSalaried);

        }

        private void EmploymentConfirmedIncomeMinimumHelper(ref IEmploymentSalaried employment, EmploymentStatuses employmentStatus, bool isConfirmed)
        {
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();

            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(empStatus.Key).Return((int)employmentStatus);
            SetupResult.For(employment.IsConfirmed).Return(isConfirmed);


        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentCurrentStatusEndDateInvalid"/> rule.
        /// </summary>
        [Test]
        public void EmploymentCurrentStatusEndDateInvalid()
        {
            EmploymentCurrentStatusEndDateInvalid rule = new EmploymentCurrentStatusEndDateInvalid();
            IEmploymentStatus _employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            // use null employment status - should pass
            Expect.Call(_employment.EmploymentStatus).Return(null);
            ExecuteRule(rule, 0, _employment);

            // use previous employment status - should pass
            Expect.Call(_employment.EmploymentStatus).Return(_employmentStatus).Repeat.Twice();
            Expect.Call(_employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            ExecuteRule(rule, 0, _employment);

            // use current employment status and unpopulated end date - should pass
            Expect.Call(_employment.EmploymentStatus).Return(_employmentStatus).Repeat.Twice();
            Expect.Call(_employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime?());
            ExecuteRule(rule, 0, _employment);

            // use previous employment status and populated end date - should fail
            Expect.Call(_employment.EmploymentStatus).Return(_employmentStatus).Repeat.Twice();
            Expect.Call(_employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime?(DateTime.Now));
            ExecuteRule(rule, 1, _employment);
        }
        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentStatusAddCurrent"/> rule.
        /// </summary>
        [Test]
        public void EmploymentStatusAddCurrent()
        {
            EmploymentStatusAddCurrent rule = new EmploymentStatusAddCurrent();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            // key 0, status null - should fail
            Expect.Call(_employment.Key).Return(0);
            Expect.Call(_employment.EmploymentStatus).Return(null);
            ExecuteRule(rule, 1, _employment);

            // key 0, status previous - should fail
            Expect.Call(_employment.Key).Return(0);
            Expect.Call(_employment.EmploymentStatus).Return(employmentStatus).Repeat.Twice();
            Expect.Call(employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            ExecuteRule(rule, 1, _employment);

            // key 0, status current - should pass
            Expect.Call(_employment.Key).Return(0);
            Expect.Call(_employment.EmploymentStatus).Return(employmentStatus).Repeat.Twice();
            Expect.Call(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            ExecuteRule(rule, 0, _employment);

            // key 1, status previous - should pass
            Expect.Call(_employment.Key).Return(1);
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentMonthlyIncomeMinimum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentMonthlyIncomeMinimum()
        {
            EmploymentMonthlyIncomeMinimum rule = new EmploymentMonthlyIncomeMinimum();

            IEmploymentSalaried employmentSalaried = _mockery.StrictMock<IEmploymentSalaried>();

            // unemployed object in - should pass
            IEmploymentUnemployed employmentUnemployed = _mockery.StrictMock<IEmploymentUnemployed>();
            ExecuteRule(rule, 0, employmentUnemployed);

            // salaried object requiring extended info with 0 basic income set but with previous employment - should pass
            EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses.Previous, ref employmentSalaried);
            SetupResult.For(employmentSalaried.MonthlyIncome).Return(0D);
            ExecuteRule(rule, 0, employmentSalaried);


            // salaried object not requiring extended info with no basic income set - should fail
            EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses.Current, ref employmentSalaried);
            SetupResult.For(employmentSalaried.MonthlyIncome).Return(0D);
            ExecuteRule(rule, 1, employmentSalaried);

            // salaried object not requiring extended info with 0 basic income set - should fail
            EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses.Current, ref employmentSalaried);
            SetupResult.For(employmentSalaried.MonthlyIncome).Return(0D);
            ExecuteRule(rule, 1, employmentSalaried);

            // salaried object requiring extended info with 0 basic income set - should fail
            EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses.Current, ref employmentSalaried);
            SetupResult.For(employmentSalaried.MonthlyIncome).Return(0D);
            ExecuteRule(rule, 1, employmentSalaried);

            // salaried object not requiring extended info with basic income > 0 set - should pass
            EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses.Current, ref employmentSalaried);
            SetupResult.For(employmentSalaried.MonthlyIncome).Return(1D);
            ExecuteRule(rule, 0, employmentSalaried);

            // salaried object requiring extended info with basic income > 0 - should fail
            EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses.Current, ref employmentSalaried);
            SetupResult.For(employmentSalaried.MonthlyIncome).Return(1D);
            ExecuteRule(rule, 0, employmentSalaried);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentEmployerMandatory"/> rule.
        /// </summary>
        [Test]
        public void EmploymentEmployerMandatory()
        {
            EmploymentEmployerMandatory rule = new EmploymentEmployerMandatory();
            IEmploymentUnemployed employmentUnemployed = _mockery.StrictMock<IEmploymentUnemployed>();
            IRemunerationType remunerationType = _mockery.StrictMock<IRemunerationType>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            IEmployer employer = _mockery.StrictMock<IEmployer>();

            // unemployed - should pass
            ExecuteRule(rule, 0, employmentUnemployed);

            // previous employment status - should pass
            SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            ExecuteRule(rule, 0, _employment);

            // null remuneration object - should pass
            SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            Expect.Call(_employment.RemunerationType).Return(null);
            ExecuteRule(rule, 0, _employment);

            // run through all the enumeration types - only some remuneration types should pass when employer is null
            Array remunTypes = Enum.GetValues(typeof(RemunerationTypes));
            for (int i = 0; i < remunTypes.Length; i++)
            {
                int remunTypeKey = (int)remunTypes.GetValue(i);
                Expect.Call(_employment.RemunerationType).Return(remunerationType).Repeat.Twice();
                Expect.Call(remunerationType.Key).Return(remunTypeKey);
                Expect.Call(_employment.Employer).Return(null).Repeat.Any();
                int expectedMessages = 1;
                switch ((RemunerationTypes)remunTypeKey)
                {
                    case RemunerationTypes.RentalIncome:
                    case RemunerationTypes.InvestmentIncome:
                    case RemunerationTypes.Pension:
                    case RemunerationTypes.Maintenance:
                    case RemunerationTypes.Unknown:
                        expectedMessages = 0;
                        break;

                }
                SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
                SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
                ExecuteRule(rule, expectedMessages, _employment);

                // if expected message count is > 0, then a not null employer should result in a pass
                if (expectedMessages > 0)
                {
                    SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
                    SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
                    Expect.Call(_employment.RemunerationType).Return(remunerationType).Repeat.Twice();
                    Expect.Call(remunerationType.Key).Return(remunTypeKey);
                    Expect.Call(_employment.Employer).Return(employer).Repeat.Any();
                    ExecuteRule(rule, 0, _employment);
                }
            }
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentStartDateMinimum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentStartDateMinimum()
        {
            EmploymentStartDateMinimum rule = new EmploymentStartDateMinimum();

            // use null - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(new DateTime?());
            ExecuteRule(rule, 0, _employment);

            // use start date 01/01/1900 but with status of previous - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Previous);
            SetupResult.For(_employment.EmploymentStartDate).Return(new DateTime(1900, 1, 1));
            ExecuteRule(rule, 0, _employment);

            // use start date 01/01/1900 - should fail
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(new DateTime(1900, 1, 1)).Repeat.Times(3);
            ExecuteRule(rule, 1, _employment);

            // use start date 02/01/1900 - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(new DateTime(1900, 1, 2)).Repeat.Times(3);
            ExecuteRule(rule, 0, _employment);

            // use start date 01/02/1900 - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(new DateTime(1900, 2, 1)).Repeat.Times(3);
            ExecuteRule(rule, 0, _employment);

        }


        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentStartDateMaximum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentStartDateMaximum()
        {
            EmploymentStartDateMaximum rule = new EmploymentStartDateMaximum();

            // use today but with status of previous - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Previous);
            SetupResult.For(_employment.EmploymentStartDate).Return(DateTime.Today);
            ExecuteRule(rule, 0, _employment);

            // use tomorrow - should fail
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(DateTime.Today.AddDays(1)).Repeat.Twice();
            ExecuteRule(rule, 1, _employment);

            // use today - should fail
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(DateTime.Today).Repeat.Twice();
            ExecuteRule(rule, 1, _employment);

            // use yesterday - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            Expect.Call(_employment.EmploymentStartDate).Return(DateTime.Today.AddDays(-1)).Repeat.Twice();
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentStartDateMaximum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentLegalEntityCompanyRemunerationTypes()
        {
            EmploymentLegalEntityCompanyRemunerationTypes rule = new EmploymentLegalEntityCompanyRemunerationTypes();
            ILegalEntityCloseCorporation closeCorporation = _mockery.StrictMock<ILegalEntityCloseCorporation>();
            ILegalEntityTrust trust = _mockery.StrictMock<ILegalEntityTrust>();
            ILegalEntityCompany company = _mockery.StrictMock<ILegalEntityCompany>();
            IRemunerationType remunerationType = _mockery.StrictMock<IRemunerationType>();
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            // run through all the enumeration types - only some remuneration types should pass when employer is null
            Array remunTypes = Enum.GetValues(typeof(RemunerationTypes));
            for (int i = 0; i < remunTypes.Length; i++)
            {
                int remunTypeKey = (int)remunTypes.GetValue(i);

                int expectedMessages = 1;
                switch ((RemunerationTypes)remunTypeKey)
                {
                    case RemunerationTypes.BusinessProfits:
                        expectedMessages = 0;
                        break;
                }

                SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
                SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
                Expect.Call(_employment.LegalEntity).Return(closeCorporation).Repeat.Any();
                Expect.Call(_employment.RemunerationType).Return(remunerationType).Repeat.Any();
                Expect.Call(remunerationType.Key).Return(remunTypeKey);
                ExecuteRule(rule, expectedMessages, _employment);

                SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
                SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
                Expect.Call(_employment.LegalEntity).Return(company).Repeat.Any();
                Expect.Call(_employment.RemunerationType).Return(remunerationType).Repeat.Any();
                Expect.Call(remunerationType.Key).Return(remunTypeKey);
                ExecuteRule(rule, expectedMessages, _employment);

                SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
                SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);
                Expect.Call(_employment.LegalEntity).Return(trust).Repeat.Any();
                Expect.Call(_employment.RemunerationType).Return(remunerationType).Repeat.Any();
                Expect.Call(remunerationType.Key).Return(remunTypeKey);
                ExecuteRule(rule, expectedMessages, _employment);
            }

            // pass through a bad remuneration type but with status set to previous - this should pass
            SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            SetupResult.For(_employment.LegalEntity).Return(trust);
            SetupResult.For(_employment.RemunerationType).Return(remunerationType);
            SetupResult.For(remunerationType.Key).Return((int)RemunerationTypes.Salaried);
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentSupportedRemunerationTypes"/> rule.
        /// </summary>
        [Test]
        public void EmploymentSupportedRemunerationTypes()
        {
            EmploymentSupportedRemunerationTypes rule = new EmploymentSupportedRemunerationTypes();
            IRemunerationType remunType = _mockery.StrictMock<IRemunerationType>();
            IEmploymentType empType = _mockery.StrictMock<IEmploymentType>();
            List<RemunerationTypes> supportedList = new List<RemunerationTypes>();

            // use existing employment with nothing else - should pass as the method exits immediately
            SetupResult.For(_employment.Key).Return(1);
            ExecuteRule(rule, 0, _employment);

            // use new employment with no remuneration type - should pass as the method exits immediately
            SetupResult.For(_employment.Key).Return(0);
            SetupResult.For(_employment.RemunerationType).Return(null);
            ExecuteRule(rule, 0, _employment);

            // use a supported remuneration type - should pass
            supportedList.Add(RemunerationTypes.BasicAndCommission);
            SetupResult.For(_employment.Key).Return(0);
            SetupResult.For(_employment.RemunerationType).Return(remunType);
            SetupResult.For(_employment.SupportedRemunerationTypes).Return(new ReadOnlyEventList<RemunerationTypes>(supportedList));
            SetupResult.For(remunType.Key).Return((int)RemunerationTypes.BasicAndCommission);
            ExecuteRule(rule, 0, _employment);
            supportedList.Clear();

            // use an supported remuneration type - should fail
            supportedList.Add(RemunerationTypes.Salaried);
            SetupResult.For(_employment.Key).Return(0);
            SetupResult.For(_employment.RemunerationType).Return(remunType);
            SetupResult.For(_employment.SupportedRemunerationTypes).Return(new ReadOnlyEventList<RemunerationTypes>(supportedList));
            SetupResult.For(remunType.Key).Return((int)RemunerationTypes.BasicAndCommission);
            SetupResult.For(remunType.Description).Return("Test");
            SetupResult.For(_employment.EmploymentType).Return(empType);
            SetupResult.For(empType.Description).Return("Test");
            ExecuteRule(rule, 1, _employment);
            supportedList.Clear();
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentMandatoryConfirmedBy"/> rule.
        /// </summary>
        [Test]
        public void EmploymentMandatoryConfirmedBy()
        {
            EmploymentMandatoryConfirmedBy rule = new EmploymentMandatoryConfirmedBy();

            // use confirmed = false - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(false);
            ExecuteRule(rule, 0, _employment);

            // use confirmed = true and confirmedby = null - should fail
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedBy).Return(null);
            ExecuteRule(rule, 1, _employment);

            // use confirmed = true and confirmedby = null but with a staus of previous - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Previous);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedBy).Return(null);
            ExecuteRule(rule, 0, _employment);

            // use confirmed = true and confirmedby = "" - should fail
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedBy).Return("");
            ExecuteRule(rule, 1, _employment);

            // use confirmed = true and confirmedby = "test" - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedBy).Return("test");
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentMandatoryConfirmedDate"/> rule.
        /// </summary>
        [Test]
        public void EmploymentMandatoryConfirmedDate()
        {
            EmploymentMandatoryConfirmedDate rule = new EmploymentMandatoryConfirmedDate();

            // use confirmed = false - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(false);
            ExecuteRule(rule, 0, _employment);

            // use confirmed = true and confirmeddate = null - should fail
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedDate).Return(new DateTime?());
            ExecuteRule(rule, 1, _employment);

            // use confirmed = true and confirmeddate = null but a status of previous - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Previous);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedDate).Return(new DateTime?());
            ExecuteRule(rule, 0, _employment);

            // use confirmed = true and confirmeddate = DateTime.Now - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.IsConfirmed).Return(true);
            SetupResult.For(_employment.ConfirmedDate).Return(new DateTime?(DateTime.Now));
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentEndDateMinimum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentEndDateMinimum()
        {
            EmploymentEndDateMinimum rule = new EmploymentEndDateMinimum();
            DateTime dtTest = new DateTime(1900, 1, 1);

            // use null end date - should pass
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime?());
            ExecuteRule(rule, 0, _employment);

            // use null start date - should pass
            Expect.Call(_employment.EmploymentEndDate).Return(dtTest);
            Expect.Call(_employment.EmploymentStartDate).Return(new DateTime?());
            ExecuteRule(rule, 0, _employment);

            // use end date 01/01/1900 - should fail
            Expect.Call(_employment.EmploymentStartDate).Return(DateTime.Now.AddMonths(-2));
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime(1900, 1, 1)).Repeat.Twice();
            ExecuteRule(rule, 1, _employment);

            // use end date before start date - should fail
            Expect.Call(_employment.EmploymentEndDate).Return(DateTime.Now.AddYears(-1)).Repeat.Times(3);
            Expect.Call(_employment.EmploymentStartDate).Return(DateTime.Now.AddMonths(-2)).Repeat.Twice();
            ExecuteRule(rule, 1, _employment);

            // use end date after start date - should fail
            Expect.Call(_employment.EmploymentEndDate).Return(DateTime.Now.AddYears(-1)).Repeat.Times(3);
            Expect.Call(_employment.EmploymentStartDate).Return(DateTime.Now.AddYears(-2)).Repeat.Twice();
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentEndDateMaximum"/> rule.
        /// </summary>
        [Test]
        public void EmploymentEndDateMaximum()
        {
            EmploymentEndDateMaximum rule = new EmploymentEndDateMaximum();

            // use null end date - should pass
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime?());
            ExecuteRule(rule, 0, _employment);

            // use end date after today - should fail
            Expect.Call(_employment.EmploymentEndDate).Return(DateTime.Now.AddDays(1)).Repeat.Twice();
            ExecuteRule(rule, 1, _employment);

            // use end date on today - should fail
            Expect.Call(_employment.EmploymentEndDate).Return(DateTime.Now).Repeat.Twice();
            ExecuteRule(rule, 0, _employment);

        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentPreviousStatusEndDateMandatory"/> rule.
        /// </summary>
        [Test]
        public void EmploymentPreviousStatusEndDateMandatory()
        {
            EmploymentPreviousStatusEndDateMandatory rule = new EmploymentPreviousStatusEndDateMandatory();
            IEmploymentStatus _employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            // use null employment status - should pass
            Expect.Call(_employment.EmploymentStatus).Return(null);
            ExecuteRule(rule, 0, _employment);

            // use current employment status - should pass
            Expect.Call(_employment.EmploymentStatus).Return(_employmentStatus).Repeat.Twice();
            Expect.Call(_employmentStatus.Key).Return((int)EmploymentStatuses.Current);
            ExecuteRule(rule, 0, _employment);

            // use previous employment status and populated end date - should pass
            Expect.Call(_employment.EmploymentStatus).Return(_employmentStatus).Repeat.Twice();
            Expect.Call(_employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime?(DateTime.Now));
            ExecuteRule(rule, 0, _employment);

            // use previous employment status and unpopulated end date - should fail
            Expect.Call(_employment.EmploymentStatus).Return(_employmentStatus).Repeat.Twice();
            Expect.Call(_employmentStatus.Key).Return((int)EmploymentStatuses.Previous);
            Expect.Call(_employment.EmploymentEndDate).Return(new DateTime?());
            ExecuteRule(rule, 1, _employment);
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Employment.EmploymentPreviousPTICheck"/> rule.
        /// </summary>
        [Test]
        public void EmploymentPreviousPTICheck()
        {
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();

            // current employment - rule will pass
            EmploymentPreviousPTICheckHelper(EmploymentStatuses.Current, legalEntity, 0, 0);

            // previous employment but no legal entity - rule will pass
            EmploymentPreviousPTICheckHelper(EmploymentStatuses.Previous, null, 0, 0);

            // execute the actual rule part - no affected accounts so we shouldn't receive any errors
            EmploymentPreviousPTICheckHelper(EmploymentStatuses.Previous, legalEntity, 0, 0);

            // execute the actual rule part - an account is affected so the rule should fail
            EmploymentPreviousPTICheckHelper(EmploymentStatuses.Previous, legalEntity, 1, 1);

        }

        /// <summary>
        /// Tests the <see cref="EmploymentRemunerationCommissionMandatory"/> rule.
        /// </summary>
        [Test]
        public void EmploymentRemunerationCommissionMandatory()
        {
            EmploymentRemunerationCommissionMandatory rule = new EmploymentRemunerationCommissionMandatory();
            IRemunerationType remunerationType = _mockery.StrictMock<IRemunerationType>();
            IExtendedEmployment extendedEmployment = _mockery.StrictMock<IExtendedEmployment>();

            // use null employment remuneration - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.RemunerationType).Return(null);
            ExecuteRule(rule, 0, _employment);

            // run through all the remuneration types with commission of 0 - rule should only fail for BasicAndCommission
            foreach (int remunTypeKey in Enum.GetValues(typeof(RemunerationTypes)))
            {
                BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
                SetupResult.For(_employment.RemunerationType).Return(remunerationType);
                SetupResult.For(remunerationType.Key).Return(remunTypeKey);
                SetupResult.For(remunerationType.Description).Return(" ").IgnoreArguments();
                SetupResult.For(_employment.ExtendedEmployment).Return(extendedEmployment);
                SetupResult.For(extendedEmployment.Commission).Return(new double?(0D));
                ExecuteRule(rule, (remunTypeKey == (int)RemunerationTypes.BasicAndCommission ? 1 : 0), _employment);
            }

            // set commission of 0 for BasicAndCommission but with status of Previous - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Previous);
            SetupResult.For(_employment.RemunerationType).Return(remunerationType);
            SetupResult.For(remunerationType.Key).Return((int)RemunerationTypes.BasicAndCommission);
            SetupResult.For(remunerationType.Description).Return(" ").IgnoreArguments();
            SetupResult.For(_employment.ExtendedEmployment).Return(extendedEmployment);
            SetupResult.For(extendedEmployment.Commission).Return(new double?(0D));
            ExecuteRule(rule, 0, _employment);

            // test basic and commission but with a value - should pass
            BasicEmploymentHelper(ref _employment, EmploymentStatuses.Current);
            SetupResult.For(_employment.RemunerationType).Return(remunerationType);
            SetupResult.For(remunerationType.Key).Return((int)RemunerationTypes.BasicAndCommission);
            SetupResult.For(_employment.ExtendedEmployment).Return(extendedEmployment);
            SetupResult.For(extendedEmployment.Commission).Return(new double?(10D));
            ExecuteRule(rule, 0, _employment);

        }

        #region EmploymentPreviousValuesCannotChange

        [Test]
        public void EmploymentPreviousValuesCannotChangeTestPass()
        {
            EmploymentPreviousValuesCannotChange rule = new EmploymentPreviousValuesCannotChange();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void EmploymentPreviousValuesCannotChangeTestFail()
        {
            EmploymentPreviousValuesCannotChange rule = new EmploymentPreviousValuesCannotChange();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Previous;
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region ExistingConfirmedEmployment

        [Test]
        public void ExistingConfirmedEmploymentTestPass()
        {
            ExistingConfirmedEmployment rule = new ExistingConfirmedEmployment();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedIncome).Return(confirmedIncome);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void ExistingConfirmedEmploymentTestFail()
        {
            ExistingConfirmedEmployment rule = new ExistingConfirmedEmployment();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(false);
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(false);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedIncome).Return(confirmedIncome);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region EmploymentVerificationProcessMinimum

        [Test]
        public void EmploymentVerificationProcessMinimumTestPass()
        {
            EmploymentVerificationProcessMinimum rule = new EmploymentVerificationProcessMinimum();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            List<int> vpList = new List<int>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            vpList.Add(1);
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedIncome).Return(confirmedIncome);
            ExecuteRule(rule, 0, employment, vpList);
        }

        [Test]
        public void EmploymentVerificationProcessMinimumTestFail()
        {
            EmploymentVerificationProcessMinimum rule = new EmploymentVerificationProcessMinimum();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            List<int> vpList = new List<int>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedIncome).Return(confirmedIncome);
            ExecuteRule(rule, 1, employment, vpList);
        }

        #endregion

        #region EmploymentConfirmationSourceMandatory

        [Test]
        public void EmploymentConfirmationSourceMandatoryTestPass()
        {
            EmploymentConfirmationSourceMandatory rule = new EmploymentConfirmationSourceMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            IEmploymentConfirmationSource empConfirmSource = _mockery.StrictMock<IEmploymentConfirmationSource>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(confirmedIncome);
            SetupResult.For(employment.EmploymentConfirmationSource).Return(empConfirmSource);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void EmploymentConfirmationSourceMandatoryTestFail()
        {
            EmploymentConfirmationSourceMandatory rule = new EmploymentConfirmationSourceMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            IEmploymentConfirmationSource empConfirmSource = null;
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double ConfirmedBasicIncome = 1.00;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(ConfirmedBasicIncome);
            SetupResult.For(employment.EmploymentConfirmationSource).Return(empConfirmSource);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region EmploymentDepartmentMandatory

        [Test]
        public void EmploymentDepartmentMandatoryTestPass()
        {
            EmploymentDepartmentMandatory rule = new EmploymentDepartmentMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            string department = "test";
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(confirmedIncome);
            SetupResult.For(employment.Department).Return(department);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void EmploymentDepartmentMandatoryTestFail()
        {
            EmploymentDepartmentMandatory rule = new EmploymentDepartmentMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double ConfirmedBasicIncome = 1.00;
            string department = string.Empty;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(ConfirmedBasicIncome);
            SetupResult.For(employment.Department).Return(department);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region EmploymentContactPhoneMandatory

        [Test]
        public void EmploymentContactPhoneMandatoryTestPass()
        {
            EmploymentContactPhoneMandatory rule = new EmploymentContactPhoneMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            string ContactPhoneCode = "1";
            string ContactPhoneNumber = "1";
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(confirmedIncome);
            SetupResult.For(employment.ContactPhoneCode).Return(ContactPhoneCode);
            SetupResult.For(employment.ContactPhoneNumber).Return(ContactPhoneNumber);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void EmploymentContactPhoneMandatoryTestFail()
        {
            EmploymentContactPhoneMandatory rule = new EmploymentContactPhoneMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double ConfirmedBasicIncome = 1.00;
            string ContactPhoneCode = string.Empty;
            string ContactPhoneNumber = string.Empty;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(ConfirmedBasicIncome);
            SetupResult.For(employment.ContactPhoneCode).Return(ContactPhoneCode);
            SetupResult.For(employment.ContactPhoneNumber).Return(ContactPhoneNumber);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region EmploymentContactPersonMandatory

        [Test]
        public void EmploymentContactPersonMandatoryTestPass()
        {
            EmploymentContactPersonMandatory rule = new EmploymentContactPersonMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double confirmedIncome = 1.00;
            string ContactPerson = "test";
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(confirmedIncome);
            SetupResult.For(employment.ContactPerson).Return(ContactPerson);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void EmploymentContactPersonMandatoryTestFail()
        {
            EmploymentContactPersonMandatory rule = new EmploymentContactPersonMandatory();
            IEmployment employment = _mockery.StrictMock<IEmployment>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            int employmentKey = 1;
            double ConfirmedBasicIncome = 1.00;
            string ContactPerson = string.Empty;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.Key).Return(employmentKey);
            SetupResult.For(employment.ConfirmedBasicIncome).Return(ConfirmedBasicIncome);
            SetupResult.For(employment.ContactPerson).Return(ContactPerson);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region EmploymentSubsidisedSetEmploymentToPrevious

        [Test]
        public void EmploymentSubsidisedSetEmploymentToPreviousTestPass()
        {
            EmploymentSubsidisedSetEmploymentToPrevious rule = new EmploymentSubsidisedSetEmploymentToPrevious();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int origEmploymentStatusKey = (int)EmploymentStatuses.Current;
            int employmentStatusKey = (int)EmploymentStatuses.Current;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            ExecuteRule(rule, 0, employment, origEmploymentStatusKey);
        }

        [Test]
        public void EmploymentSubsidisedSetEmploymentToPreviousTestFail()
        {
            EmploymentSubsidisedSetEmploymentToPrevious rule = new EmploymentSubsidisedSetEmploymentToPrevious();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int origEmploymentStatusKey = (int)EmploymentStatuses.Current;
            int employmentStatusKey = (int)EmploymentStatuses.Previous;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            ExecuteRule(rule, 1, employment, origEmploymentStatusKey);
        }

        #endregion

        #region EmploymentConfirmedIncomeMandatory

        [Test]
        public void EmploymentConfirmedIncomeMandatoryTestPass()
        {
            EmploymentConfirmedIncomeMandatory rule = new EmploymentConfirmedIncomeMandatory();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            //
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(true);
            ExecuteRule(rule, 0, employment);
        }

        [Test]
        public void EmploymentConfirmedIncomeMandatoryTestFail()
        {
            EmploymentConfirmedIncomeMandatory rule = new EmploymentConfirmedIncomeMandatory();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            //
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(null);
            ExecuteRule(rule, 1, employment);
        }

        #endregion

        #region EmploymentConfirmedSetBackToNo

        [Test]
        public void EmploymentConfirmedSetBackToNoTestPass()
        {
            EmploymentConfirmedSetBackToNo rule = new EmploymentConfirmedSetBackToNo();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            //
            int origConfirmedEmploymentFlag = (int)SAHL.Common.Globals.ConfirmedEmployment.Yes;
            double ConfirmedIncome = 1.00;
            //
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.ConfirmedIncome).Return(ConfirmedIncome);
            ExecuteRule(rule, 0, employment, origConfirmedEmploymentFlag);
        }

        [Test]
        public void EmploymentConfirmedSetBackToNoTestFail()
        {
            EmploymentConfirmedSetBackToNo rule = new EmploymentConfirmedSetBackToNo();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            //
            int origConfirmedEmploymentFlag = (int)SAHL.Common.Globals.ConfirmedEmployment.Yes;
            double ConfirmedIncome = 1.00;
            //
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(false);
            SetupResult.For(employment.ConfirmedIncome).Return(ConfirmedIncome);
            ExecuteRule(rule, 1, employment, origConfirmedEmploymentFlag);
        }

        #endregion

        #region EmploymentPreviousConfirmedIncomeCannotChange

        [Test]
        public void EmploymentPreviousConfirmedIncomeCannotChangeTestPass()
        {
            EmploymentPreviousConfirmedIncomeCannotChange rule = new EmploymentPreviousConfirmedIncomeCannotChange();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int origEmploymentStatusKey = (int)EmploymentStatuses.Current;
            double ConfirmedIncome = 1.00;
            double origConfirmedIncome = 1.00;
            int employmentStatusKey = (int)EmploymentStatuses.Previous;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.ConfirmedIncome).Return(ConfirmedIncome);
            ExecuteRule(rule, 0, employment, origEmploymentStatusKey, origConfirmedIncome);
        }

        [Test]
        public void EmploymentPreviousConfirmedIncomeCannotChangeTestFail()
        {
            EmploymentPreviousConfirmedIncomeCannotChange rule = new EmploymentPreviousConfirmedIncomeCannotChange();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            //
            int origEmploymentStatusKey = (int)EmploymentStatuses.Current;
            double ConfirmedIncome = 1.00;
            double origConfirmedIncome = 2.00;
            int employmentStatusKey = (int)EmploymentStatuses.Previous;
            //
            SetupResult.For(empStatus.Key).Return(employmentStatusKey);
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(employment.ConfirmedEmploymentFlag).Return(true);
            SetupResult.For(employment.ConfirmedIncome).Return(ConfirmedIncome);
            ExecuteRule(rule, 1, employment, origEmploymentStatusKey, origConfirmedIncome);
        }

        #endregion

        #region Helper Methods

        private void EmploymentMonthlyIncomeMinimumHelper(EmploymentStatuses empStatus, ref IEmploymentSalaried empSalaried)
        {
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            SetupResult.For(empSalaried.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)empStatus);
        }

        /// <summary>
        /// Helper method for the EmploymentPreviousPTICheck test.
        /// </summary>
        /// <param name="empStatus"></param>
        /// <param name="legalEntity"></param>
        /// <param name="affectedAccountCount"></param>
        /// <param name="expectedMessageCount"></param>
        private void EmploymentPreviousPTICheckHelper(EmploymentStatuses empStatus, ILegalEntity legalEntity, int affectedAccountCount, int expectedMessageCount)
        {
            EmploymentPreviousPTICheck rule = new EmploymentPreviousPTICheck();

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IEmploymentRepository empRepo = _mockery.StrictMock<IEmploymentRepository>();
            MockCache.Add((typeof(IEmploymentRepository)).ToString(), empRepo);

            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();

            SetupResult.For(_employment.LegalEntity).Return(legalEntity);
            SetupResult.For(_employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employmentStatus.Key).Return((int)empStatus);

            List<IAccount> affectedAccounts = new List<IAccount>();
            for (int i = 0; i < affectedAccountCount; i++)
            {
                IAccount acc = _mockery.StrictMock<IAccount>();
                SetupResult.For(acc.Key).Return(1);
                affectedAccounts.Add(acc);
            }
            SetupResult.For(empRepo.GetAccountsForPTI(_employment)).IgnoreArguments().Return(affectedAccounts);

            ExecuteRule(rule, expectedMessageCount, _employment);

        }

        private void BasicEmploymentHelper(ref IEmployment employment, EmploymentStatuses employmentStatus)
        {
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employment.EmploymentStatus).Return(empStatus);
            SetupResult.For(empStatus.Key).Return((int)employmentStatus);
        }


        #endregion

        #region EmploymentConfirmedSetYesSave

        [Test]
        public void EmploymentConfirmedSetYesSavePass()
        {
            EmploymentConfirmedSetYesSave rule = new EmploymentConfirmedSetYesSave();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            //
            int origConfirmedEmploymentFlag = (int)SAHL.Common.Globals.ConfirmedEmployment.Yes;
            double ConfirmedIncome = 1.00;
            //
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(true);
            //SetupResult.For(employment.ConfirmedIncomeFlag.Value).Return(true);
            SetupResult.For(employment.ConfirmedIncome).Return(ConfirmedIncome);
            ExecuteRule(rule, 0, employment, origConfirmedEmploymentFlag);
        }

        [Test]
        public void EmploymentConfirmedSetYesSaveFail()
        {
            EmploymentConfirmedSetYesSave rule = new EmploymentConfirmedSetYesSave();
            IEmploymentSubsidised employment = _mockery.StrictMock<IEmploymentSubsidised>();
            //
            int origConfirmedEmploymentFlag = (int)SAHL.Common.Globals.ConfirmedEmployment.Yes;
            double ConfirmedIncome = 0.00;
            //
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(true);
            //SetupResult.For(employment.ConfirmedIncomeFlag.Value).Return(true);
            SetupResult.For(employment.ConfirmedIncome).Return(ConfirmedIncome);
            ExecuteRule(rule, 1, employment, origConfirmedEmploymentFlag);
        }

        #endregion

    }
}
