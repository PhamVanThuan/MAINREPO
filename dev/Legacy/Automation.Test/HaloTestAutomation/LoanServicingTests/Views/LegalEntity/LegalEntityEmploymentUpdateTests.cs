using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.LegalEntity
{
    [TestFixture, RequiresSTA]
    public class LegalEntityEmploymentUpdateTests : TestBase<LegalEntityEmploymentDetails>
    {
        private int legalentityKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            var accountKey = Service<IAccountService>().GetLatestOpenAccountWithOneMainApplicantAndOneEmploymentRecord();
            var results = Service<ILegalEntityService>().GetLegalEntityNamesAndRoleByAccountKey(accountKey);
            legalentityKey = results.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
        }

        [Test]
        public void when_updating_employment_and_confirming_should_save_payment_salary_date()
        {
            Service<IEmploymentService>().UpdateAllEmploymentStatus(legalentityKey, EmploymentStatusEnum.Previous);
            var employment = this.CreateEmployment(legalentityKey);
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Update);
            base.View.BasicConfirmSalariedEmployment(base.Browser, 1, 20);
            EmploymentAssertions.AssertSalaryPaymentDay(employment.EmploymentKey, 20);
        }

        [Test]
        public void when_updating_employment_and_confirming_should_be_required_to_provide_payment_salary_date()
        {
            Service<IEmploymentService>().UpdateAllEmploymentStatus(legalentityKey, EmploymentStatusEnum.Previous);
            var employment = this.CreateEmployment(legalentityKey);
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Update);
            base.View.BasicConfirmSalariedEmployment(base.Browser, 1);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please capture salary payment day");
        }

        [Test]
        public void when_updating_employment_and_confirming_should_be_required_to_provide_payment_salary_date_between_1_and_31()
        {
            Service<IEmploymentService>().UpdateAllEmploymentStatus(legalentityKey, EmploymentStatusEnum.Previous);
            var employment = this.CreateEmployment(legalentityKey);
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Update);
            base.View.BasicConfirmSalariedEmployment(base.Browser, 1, salaryPaymentDay: 44);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Salary payment day must be between 1 and 31");
        }

        [Test]
        public void when_updating_employment_and_confirming_and_setting_union_member_to_true_should_save_union_member_as_true()
        {
            Service<IEmploymentService>().UpdateAllEmploymentStatus(legalentityKey, EmploymentStatusEnum.Previous);
            var employment = this.CreateEmployment(legalentityKey);
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Update);
            base.View.BasicConfirmSalariedEmployment(base.Browser, 1, salaryPaymentDay: 25);
            EmploymentAssertions.AssertUnionMember(employment.EmploymentKey, true);
        }

        private Automation.DataModels.Employment CreateEmployment(int legalentitykey)
        {
            var todaysDay = DateTime.Now.Day;
            var daysToSubstract = todaysDay - 1;
            //turn to negative number
            daysToSubstract = daysToSubstract * -1;

            var emp = new Automation.DataModels.Employment
                {
                    EmployerKey = Service<IEmploymentService>().GetEmployer("A.H. BUILDERS").EmployerKey,
                    EmploymentTypeKey = EmploymentTypeEnum.Salaried,
                    RemunerationTypeKey = RemunerationTypeEnum.Salaried,
                    EmploymentStatusKey = EmploymentStatusEnum.Current,
                    LegalEntityKey = legalentitykey,
                    EmploymentStartDate = DateTime.Now.AddDays(daysToSubstract),
                    BasicIncome = 20560.00d,
                    ConfirmedEmploymentFlag = false,
                    ConfirmedIncomeFlag = false,
                    EmploymentConfirmationSourceKey = EmploymentConfirmationSourceEnum.ElectronicYellowPagesDirectory,
                    SalaryPaymentDay = null
                };

            return Service<IEmploymentService>().InsertEmployment(emp);
        }
    }
}