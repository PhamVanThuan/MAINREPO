using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.DebitOrderDetails;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class DebitOrderDetailTests : TestBase<DebitOrderDetailsView>
    {
        #region PrivateVariables

        private Automation.DataModels.Account account;
        private int legalEntityKey;
        private Automation.DataModels.LegalEntityBankAccount legalEntityBankAccount;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //we need an account
            account = GetAccountBasedOnCurrentDebitOrderDay(1);
            //navigate
            legalEntityBankAccount = Service<ILegalEntityService>().InsertLegalEntityBankAccount(legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().VariableLoanNode(account.AccountKey);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
        }

        #endregion SetupTearDown

        #region Validation

        /// <summary>
        /// When adding a debit order, the payment type field is required.
        /// </summary>
        [Test]
        public void PaymentTypeIsMandatory()
        {
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = null,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.None,
                ChangeDate = DateTime.Now.AddDays(+1)
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Financial Service Payment Type is a mandatory field");
        }

        /// <summary>
        /// When adding a debit order payment, the bank account must be selected.
        /// </summary>
        [Test]
        public void WhenAddingDebitOrderPayment_BankAccountIsMandatory()
        {
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = null,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = DateTime.Now.AddDays(+1)
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Payment Type : Debit Order - Bank Account details must be captured");
        }

        /// <summary>
        /// When adding a debit order the debit order day is a required field.
        /// </summary>
        [Test]
        public void DebitOrderDayIsMandatory()
        {
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = null,
                DebitOrderDay = null,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.None,
                ChangeDate = DateTime.Now.AddDays(+1)
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("Debit Order Day is a mandatory field");
        }

        /// <summary>
        /// Ensures that the bank account is in the select list on the screen. We are not checking that the entire contents, just that it contains the
        /// legal entity bank account that was added.
        /// </summary>
        [Test]
        public void ValidateBankAccountSelectList()
        {
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.SelectPaymentType(FinancialServicePaymentTypeEnum.DebitOrderPayment);
            //check that our bank account is in the select list
            base.View.AssertBankAccountDropdownContainsBankAccountKey(legalEntityBankAccount.BankAccountKey);
        }

        #endregion Validation

        #region Rules

        /// <summary>
        /// When capturing a debit order the effective date must be greater than or equal to todays date
        /// </summary>
        [Test]
        public void EffectiveDateGreaterThanOrEqualToTodaysDate()
        {
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = DateTime.Now.AddDays(-1)
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Effective Date must be greater than or equal to today's date");
        }

        /// <summary>
        /// The effective date captured must be a business day
        /// </summary>
        [Test]
        public void EffectiveDateMustBeABusinessDay()
        {
            var effectiveDate = Service<ICommonService>().GetDateWithBusinessDayCheck(false, true);
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = effectiveDate
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Effective Date must be a business day");
        }

        /// <summary>
        /// The day prior to the effective day must also be a business day when capturing a debit order.
        /// </summary>
        [Test]
        public void MustHaveOneBusinessDayPriorToEffectiveDate()
        {
            var effectiveDate = Service<ICommonService>().GetDateWithBusinessDayCheck(true, false);
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = effectiveDate
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Effective Date must have one business day prior to Effective Date");
        }

        /// <summary>
        /// A debit order cannot be added with an effective day that is the same as the current debit order day.
        /// </summary>
        [Test]
        public void EffectiveDateCannotBeEqualToCurrentDebitOrderDay()
        {
            //get the financial service bank accounts
            var fsba = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey);
            int debitOrderDay = (from f in fsba
                                 where f.GeneralStatusKey == (int)GeneralStatusEnum.Active
                                 select f.DebitOrderDay.Value).FirstOrDefault();
            //we meed to build up a date
            var effectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, debitOrderDay);
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = effectiveDate
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "The Effective Date cannot be the same as the Debit Order Day of the current Debit Order");
        }

        /// <summary>
        /// The effective date of the debit order cannot be more than 6 months in the future.
        /// </summary>
        [Test]
        public void EffectiveDateCannotBeGreaterThanSixMonthsInTheFuture()
        {
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = DateTime.Now.AddDays(+1).AddMonths(6)
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Effective Date cannot be more than 6 months from today's date");
        }

        /// <summary>
        /// You are not allowed to have more than one future dated change in a single month.
        /// </summary>
        [Test]
        public void CannotHaveMoreThanOneFutureDatedChangeInASingleMonth()
        {
            var fsb = AddValidDebitOrderPayment(NodeTypeEnum.Add);
            //add again
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(fsb);
            //message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "There can not be more than 1 change of Debit Order details scheduled for same month");
        }

        /// <summary>
        /// The new debit order that is setup cannot result in more than 1 debit order being collected in a month.
        /// </summary>
        [Test]
        public void NewDebitOrderResultsInMoreThanOnePayment()
        {
            int currentDebitOrderDay = 1;
            var firstOfNextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, 1);
            var nextBusinessDayAfterfirstOfNextMonth = Service<ICommonService>().GetNextBusinessDay(firstOfNextMonth, true).AddDays(+1);
            var newAccountForTest = GetAccountBasedOnCurrentDebitOrderDay(currentDebitOrderDay);
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = nextBusinessDayAfterfirstOfNextMonth.Day + 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = nextBusinessDayAfterfirstOfNextMonth
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            //try and add it, should work
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "A debit order with the selected Debit Order Day and Effective Date will result in more than one payment in a single month");
        }

        /// <summary>
        /// This test will try to create a future dated change, that if it was allowed, would result in the account skipping a payment. A rule
        /// should be running that should not allow the future dated change to be created.
        /// </summary>
        [Test]
        public void NewDebitOrderResultsInSkippedPayment()
        {
            int currentDebitOrderDay = 28;
            int newDebitOrderDay = 1;
            var firstOfNextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, 1);
            var nextBusinessDayAfterfirstOfNextMonth = Service<ICommonService>().GetNextBusinessDay(firstOfNextMonth, true).AddDays(+1);
            var newAccountForTest = GetAccountBasedOnCurrentDebitOrderDay(currentDebitOrderDay);
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = newDebitOrderDay,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = nextBusinessDayAfterfirstOfNextMonth
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            //try and add it, should work
            base.View.AddDebitOrderDetails(financialServiceBankAccount);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "A debit order with the selected Debit Order Day and Effective Date will result in a skipped payment");
        }

        #endregion Rules

        #region DebitOrderDetailsAdd

        /// <summary>
        /// Test ensures that a new debit order can be added as a future dated change request
        /// </summary>
        [Test]
        public void AddDebitOrderPayment_CreatesFutureDatedChange()
        {
            var fsb = AddValidDebitOrderPayment(NodeTypeEnum.Add);
            //assert that it has been added
            var futureDatedChange = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(fsb.FinancialServiceKey);
            //check that the detail matches
            var futureDatedChangeDetails = (from f in futureDatedChange
                                            where f.FutureDatedChangeType == FutureDatedChangeTypeEnum.FixedDebitOrder
                                            select f)
                                    .SelectMany(x => x.FutureDatedChangeDetails)
                                    .Where(
                                            b =>
                                            b.Value == fsb.DebitOrderDay.ToString() &&
                                            b.TableName == "FinancialServiceBankAccount" &&
                                            b.ColumnName == "DebitOrderDay"
                                           );
            Assert.That(futureDatedChangeDetails != null, "No matching future dated change details were found.");
        }

        #endregion DebitOrderDetailsAdd

        #region DebitOrderDetailsUpdate

        /// <summary>
        /// This test will select the current active debit order using the Update Manual Debit order screen and update it to use a different bank account. This
        /// should result in a new future dated change being created and the existing financial service bank account record should not be changed.
        /// </summary>
        [Test]
        public void UpdateCurrentDebitOrder_CreatesFutureDatedChange()
        {
            //we need the financial service bank accounts
            var financialServiceBankAccount = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Active).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            //select it
            base.View.SelectRow(financialServiceBankAccount.FinancialServiceBankAccountKey);
            //select the new bank account
            base.View.SelectBankAccount(legalEntityBankAccount.BankAccountKey);
            var effectiveDate = DateTime.Now.DayOfWeek == DayOfWeek.Friday ? DateTime.Now : DateTime.Now.AddDays(+1);
            base.View.PopulateEffectiveDate(effectiveDate);
            base.View.ClickUpdate();
            var futureDatedChange = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(financialServiceBankAccount.FinancialServiceKey);
            //check that the detail matches
            Assert.That(futureDatedChange != null, string.Format("No future dated change was found for financialService: {0}",
                financialServiceBankAccount.FinancialServiceKey));
            //check for inactive FSB record
            var inactiveFSB = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Inactive && x.BankAccountKey == legalEntityBankAccount.BankAccountKey)
                .FirstOrDefault();
            Assert.That(inactiveFSB != null, "An inactive FinancialServiceBankAccount record was not added for the Future Dated Change");
            //check that the active on one is not affected
            var activeFSB = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Active && x.BankAccountKey == legalEntityBankAccount.BankAccountKey)
                .FirstOrDefault();
            Assert.That(activeFSB == null, "The active FinancialServiceBankAccount was incorrectly updated to use the new bank account.");
        }

        /// <summary>
        /// This test will create a future dated change and then try and update it to use a different bank account. It expects the future dated change
        /// details to contain a reference to the updated bank account. This test will currently fail, see trac ticket:
        /// #20512: BUG: Updating a Future Dated Debit Order to a New Bank Account
        /// </summary>
        [Ignore]
        [Test]
        public void UpdateFutureDatedChange_UpdatesExistingFutureDatedChangeDetails()
        {
            //add a future dated change
            var fsb = AddValidDebitOrderPayment(NodeTypeEnum.Add);
            var futureDatedChange = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(fsb.FinancialServiceKey).FirstOrDefault();
            //go to the update screen
            //update it to have a different bank account
            legalEntityBankAccount = Service<ILegalEntityService>().InsertLegalEntityBankAccount(legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            //select it
            base.View.SelectRow(futureDatedChange.FutureDatedChangeKey);
            //select the new bank account
            base.View.SelectBankAccount(legalEntityBankAccount.BankAccountKey);
            base.View.ClickUpdate();
            //get the future dated change
            var futureDatedChangeDetails = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(fsb.FinancialServiceKey)
                .SelectMany(x => x.FutureDatedChangeDetails)
                .Where(
                        b =>
                        b.ColumnName == "BankAccountKey" &&
                        b.TableName == "FinancialServiceBankAccount" &&
                        b.Value == legalEntityBankAccount.BankAccountKey.ToString()
                      )
                .FirstOrDefault();
            Assert.That(futureDatedChangeDetails != null, "The future dated change details were not update to reference the new bank account.");
        }

        #endregion DebitOrderDetailsUpdate

        #region DebitOrderDetailsDelete

        /// <summary>
        /// This test will add a valid future dated change and then delete it, ensuring that the future dated change is removed.
        /// </summary>
        [Test]
        public void DeleteFutureDatedChangeDebitOrder()
        {
            var fsb = AddValidDebitOrderPayment(NodeTypeEnum.Add);
            //check for future dated change
            var futureDatedChange = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(fsb.FinancialServiceKey).FirstOrDefault();
            //go to delete screen
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Delete);
            //select it and delete it
            base.View.SelectRow(futureDatedChange.FutureDatedChangeKey);
            base.View.ClickDelete();
            //there should be no more future dated changes
            futureDatedChange = Service<IFutureDatedChangeService>().GetFutureDatedChangeByIdentifierReference(fsb.FinancialServiceKey).FirstOrDefault();
            Assert.That(futureDatedChange == null, "The future dated change was not deleted as expected.");
        }

        /// <summary>
        /// Test ensures that the active debit order cannot be deleted.
        /// </summary>
        [Test]
        public void CannotDeleteActiveDebitOrder()
        {
            //we need the financial service bank accounts
            var financialServiceBankAccount = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Active).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Delete);
            //select it
            base.View.SelectRow(financialServiceBankAccount.FinancialServiceBankAccountKey);
            base.View.ClickDelete();
            //warning message
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot delete the active debit order");
        }

        #endregion DebitOrderDetailsDelete

        #region EmploymentSalaryDate

        [Test]
        public void when_adding_any_OTHER_type_than_debit_order_payment_should_NOT_display_employment_salary_payment_dates()
        {
            var leRoles = Service<ILegalEntityService>().GetLegalEntityRoles(account.AccountKey);
            var employments = this.SetupEmploymentForLegalEntities(leRoles);

            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.SelectPaymentType(FinancialServicePaymentTypeEnum.DirectPayment);
            base.View.AssertSalaryPaymentDayNotDisplayed();
            base.View.SelectPaymentType(FinancialServicePaymentTypeEnum.SubsidyPayment);
            base.View.AssertSalaryPaymentDayNotDisplayed();
        }

        [Test]
        public void when_adding_debit_order_payment_should_display_employment_salary_payment_dates()
        {
            var leRoles = Service<ILegalEntityService>().GetLegalEntityRoles(account.AccountKey);
            var employments = this.SetupEmploymentForLegalEntities(leRoles);

            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.SelectPaymentType(FinancialServicePaymentTypeEnum.DebitOrderPayment);
            base.View.AssertSalaryPaymentDays(leRoles, employments);
        }

        [Test]
        public void when_adding_debit_order_payment_should_NOT_display_employment_salary_payment_dates()
        {
            var leRoles = Service<ILegalEntityService>().GetLegalEntityRoles(account.AccountKey);
            this.UpdateEmploymentSalaryDatesToNULL(leRoles);

            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.SelectPaymentType(FinancialServicePaymentTypeEnum.DebitOrderPayment);
            base.View.AssertSalaryPaymentDayNotDisplayed();
        }

        [Test]
        public void when_updating_debit_order_payment_should_NOT_display_employment_salary_payment_dates()
        {
            var leRoles = Service<ILegalEntityService>().GetLegalEntityRoles(account.AccountKey);
            this.UpdateEmploymentSalaryDatesToNULL(leRoles);

            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            base.View.AssertSalaryPaymentDayNotDisplayed();
        }

        [Test]
        public void when_updating_debit_order_payment_should_display_employment_salary_payment_dates()
        {
            var leRoles = Service<ILegalEntityService>().GetLegalEntityRoles(account.AccountKey);
            var employments = this.SetupEmploymentForLegalEntities(leRoles);

            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            base.View.AssertSalaryPaymentDays(leRoles, employments);
        }

        #endregion EmploymentSalaryDate

        #region IsNaedoCompliant

        [Test]
        public void when_adding_a_new_debit_order_set_naedo_compliant_to_true()
        {
            var fsb = AddValidDebitOrderPayment(NodeTypeEnum.Add);
            var accountKey = Service<IAccountService>().GetAccountKeyByFinancialServiceKey(fsb.FinancialServiceKey);
            var newfsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(accountKey)
                         .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Inactive && x.BankAccountKey == fsb.BankAccountKey)
                         .FirstOrDefault();
            Assert.True(newfsb.IsNaedoCompliant, "The is naedo compliant flag was not set to true.");
        }

        [Test]
        public void when_adding_a_new_subsidy_payment_set_naedo_compliant_to_false()
        {
            var effectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, 3);
            if (DateTime.Now.Month == 12)
            {
                effectiveDate = effectiveDate.AddYears(1);
            }
            var nextBusinessDayAfterEffectiveDate = Service<ICommonService>().GetNextBusinessDay(effectiveDate);
            if (nextBusinessDayAfterEffectiveDate.DayOfWeek == DayOfWeek.Monday)
                nextBusinessDayAfterEffectiveDate = nextBusinessDayAfterEffectiveDate.AddDays(1);
            var financialServiceKey = (from a in account.FinancialServices
                                       where a.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                       select a.FinancialServiceKey).FirstOrDefault();
            var fsb = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                DebitOrderDay = 2,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.SubsidyPayment,
                ChangeDate = nextBusinessDayAfterEffectiveDate,
                FinancialServiceKey = financialServiceKey
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(fsb, NodeTypeEnum.Add);
            var newfsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                         .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Inactive && x.BankAccountKey == null
                         && x.ChangeDate == DateTime.Today)
                         .FirstOrDefault();
            Assert.False(newfsb.IsNaedoCompliant, "The is naedo compliant flag was incorrectly set to true.");
        }

        [Test]
        public void when_adding_a_new_direct_payment_set_naedo_compliant_to_false()
        {
            var effectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, 3);
            if (DateTime.Now.Month == 12)
            {
                effectiveDate = effectiveDate.AddYears(1);
            }
            var nextBusinessDayAfterEffectiveDate = Service<ICommonService>().GetNextBusinessDay(effectiveDate);
            if (nextBusinessDayAfterEffectiveDate.DayOfWeek == DayOfWeek.Monday)
                nextBusinessDayAfterEffectiveDate = nextBusinessDayAfterEffectiveDate.AddDays(1);
            var financialServiceKey = (from a in account.FinancialServices
                                       where a.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                       select a.FinancialServiceKey).FirstOrDefault();
            var fsb = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                DebitOrderDay = 2,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DirectPayment,
                ChangeDate = nextBusinessDayAfterEffectiveDate,
                FinancialServiceKey = financialServiceKey
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.View.AddDebitOrderDetails(fsb, NodeTypeEnum.Add);
            var newfsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                         .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Inactive && x.BankAccountKey == null
                         && x.ChangeDate == DateTime.Today)
                         .FirstOrDefault();
            Assert.False(newfsb.IsNaedoCompliant, "The is naedo compliant flag was incorrectly set to true.");
        }

        [Test]
        public void when_updating_debit_order_day_of_the_current_active_debit_order_set_naedo_compliant_to_false()
        {
            var fsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Active).FirstOrDefault();
            int debitOrderDay = ((int)fsb.DebitOrderDay + 1);
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            base.View.SelectRow(fsb.FinancialServiceBankAccountKey);
            var effectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, debitOrderDay + 2);
            if (DateTime.Now.Month == 12)
            {
                effectiveDate = effectiveDate.AddYears(1);
            }
            var nextBusinessDayAfterEffectiveDate = Service<ICommonService>().GetNextBusinessDay(effectiveDate);
            if (nextBusinessDayAfterEffectiveDate.DayOfWeek == DayOfWeek.Monday)
                nextBusinessDayAfterEffectiveDate = nextBusinessDayAfterEffectiveDate.AddDays(1);
            base.View.PopulateDebitOrderDay(debitOrderDay);
            base.View.PopulateEffectiveDate(nextBusinessDayAfterEffectiveDate);
            base.View.ClickUpdate();
            var newfsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                         .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Inactive && x.BankAccountKey == fsb.BankAccountKey
                         && x.DebitOrderDay == debitOrderDay && x.ChangeDate == DateTime.Today)
                         .FirstOrDefault();
            Assert.False(newfsb.IsNaedoCompliant, "The is naedo compliant flag was incorrectly set to true.");
        }

        [Test]
        public void when_updating_debit_order_bank_account_of_the_current_active_debit_order_set_naedo_compliant_to_true()
        {
            var fsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Active).FirstOrDefault();
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            base.View.SelectRow(fsb.FinancialServiceBankAccountKey);
            base.View.SelectBankAccount(legalEntityBankAccount.BankAccountKey);
            var effectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, (int)fsb.DebitOrderDay + 2);
            if (DateTime.Now.Month == 12)
            {
                effectiveDate = effectiveDate.AddYears(1);
            }
            var nextBusinessDayAfterEffectiveDate = Service<ICommonService>().GetNextBusinessDay(effectiveDate);
            if (nextBusinessDayAfterEffectiveDate.DayOfWeek == DayOfWeek.Monday)
                nextBusinessDayAfterEffectiveDate = nextBusinessDayAfterEffectiveDate.AddDays(1);
            base.View.PopulateEffectiveDate(effectiveDate);
            base.View.ClickUpdate();
            var newfsb = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey)
                         .Where(x => x.GeneralStatusKey == (int)GeneralStatusEnum.Inactive && x.BankAccountKey == legalEntityBankAccount.BankAccountKey
                         && x.ChangeDate == DateTime.Today)
                         .FirstOrDefault();
            Assert.True(newfsb.IsNaedoCompliant, "The is naedo compliant flag was not set to true.");
        }

        [Test]
        public void when_account_is_in_an_active_neado_tracking_period_display_the_required_warning()
        {
            var DOtran = Service<IDebitOrdersService>().GetAccountWithinNaedoTrackingPeriod().FirstOrDefault();
            Service<IDebitOrdersService>().UpdateAccountOnDOTransactionRecord(account.AccountKey, DOtran.DOTransactionKey);

            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");

            base.Browser.Navigate<LoanServicingCBO>().VariableLoanNode(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.View);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Update);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(NodeTypeEnum.Delete);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");

            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.View);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Update);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");
            base.Browser.Navigate<LoanServicingCBO>().ManualDebitOrders(NodeTypeEnum.Delete);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The NAEDO Debit Order for this account is in the tracking period.");

            Service<IDebitOrdersService>().UpdateAccountOnDOTransactionRecord(DOtran.AccountKey, DOtran.DOTransactionKey);
        }

        #endregion IsNaedoCompliant

        #region Helpers

        /// <summary>
        /// Gets a new account for a test based on the current debit order day provided.
        /// </summary>
        /// <param name="currentDebitOrderDay"></param>
        /// <returns></returns>
        private Automation.DataModels.Account GetAccountBasedOnCurrentDebitOrderDay(int currentDebitOrderDay)
        {
            var financialServiceBankAccounts = Service<IDebitOrdersService>().GetDebitOrderFinancialServiceBankAccount(currentDebitOrderDay, false).SelectRandom();
            var accountKey = Service<IAccountService>().GetAccountKeyByFinancialServiceKey(financialServiceBankAccounts.FinancialServiceKey);
            var accountForTest = Service<IAccountService>().GetAccountByKey(accountKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            //navigate
            legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountForTest.AccountKey);
            legalEntityBankAccount = Service<ILegalEntityService>().InsertLegalEntityBankAccount(legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().VariableLoanNode(accountForTest.AccountKey);
            return accountForTest;
        }

        /// <summary>
        /// Adds a valid future dated debit order payment
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Automation.DataModels.FinancialServiceBankAccountModel AddValidDebitOrderPayment(NodeTypeEnum node)
        {
            int currentDebitOrderDay = 1;
            var newAccountForTest = GetAccountBasedOnCurrentDebitOrderDay(currentDebitOrderDay);
            var effectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(+1).Month, currentDebitOrderDay + 2);
            if (DateTime.Now.Month == 12) //we need to increment the year
            {
                effectiveDate = effectiveDate.AddYears(1);
            }
            //we need the next business date
            var nextBusinessDayAfterEffectiveDate = Service<ICommonService>().GetNextBusinessDay(effectiveDate);

            //TO avoid the "Must have one business day prior to effect date" rule

            if (nextBusinessDayAfterEffectiveDate.DayOfWeek == DayOfWeek.Monday)
                nextBusinessDayAfterEffectiveDate = nextBusinessDayAfterEffectiveDate.AddDays(1);

            // we need the financial service
            var financialServiceKey = (from a in newAccountForTest.FinancialServices
                                       where a.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                       select a.FinancialServiceKey).FirstOrDefault();
            //build up the model
            var financialServiceBankAccount = new Automation.DataModels.FinancialServiceBankAccountModel
            {
                BankAccountKey = legalEntityBankAccount.BankAccountKey,
                DebitOrderDay = currentDebitOrderDay + 1,
                FinancialServicePaymentTypeKey = FinancialServicePaymentTypeEnum.DebitOrderPayment,
                ChangeDate = nextBusinessDayAfterEffectiveDate,
                FinancialServiceKey = financialServiceKey
            };
            base.Browser.Navigate<LoanServicingCBO>().DebitOrderDetails(node);
            //try and add it, should work
            base.View.AddDebitOrderDetails(financialServiceBankAccount, node);
            return financialServiceBankAccount;
        }

        private IEnumerable<Automation.DataModels.Employment> SetupEmploymentForLegalEntities(IEnumerable<Automation.DataModels.LegalEntityRole> legalentityRoles)
        {
            var legalEntityEmployments = new List<Automation.DataModels.Employment>();
            var randomNo = new Random();
            foreach (var leRole in legalentityRoles)
            {
                var emp = new Automation.DataModels.Employment();
                emp.EmployerKey = Service<IEmploymentService>().GetEmployer("A.H. BUILDERS").EmployerKey;
                emp.EmploymentTypeKey = EmploymentTypeEnum.Salaried;
                emp.RemunerationTypeKey = RemunerationTypeEnum.Salaried;
                emp.EmploymentStatusKey = EmploymentStatusEnum.Current;
                emp.LegalEntityKey = leRole.LegalEntityKey;

                var todaysDay = DateTime.Now.Day;
                var daysToSubstract = todaysDay - 1;
                //turn to negative number
                daysToSubstract = daysToSubstract * -1;

                emp.EmploymentStartDate = DateTime.Now.AddDays(daysToSubstract);
                emp.BasicIncome = 20560.00d;
                emp.ConfirmedBasicIncome = 20560.00d;

                emp.ContactPerson = "Peter";
                emp.ContactPhoneCode = "012";
                emp.ContactPhoneNumber = "1234567";
                emp.Department = "IT";

                emp.ConfirmedEmploymentFlag = true;
                emp.ConfirmedIncomeFlag = true;
                emp.EmploymentConfirmationSourceKey = EmploymentConfirmationSourceEnum.ElectronicYellowPagesDirectory;

                emp.SalaryPaymentDay = randomNo.Next(1, 31);
                legalEntityEmployments.Add(emp);
                Service<IEmploymentService>().UpdateAllEmploymentStatus(leRole.LegalEntityKey, EmploymentStatusEnum.Previous);
                Service<IEmploymentService>().InsertEmployment(emp);
            }
            return legalEntityEmployments;
        }

        private void UpdateEmploymentSalaryDatesToNULL(IEnumerable<Automation.DataModels.LegalEntityRole> legalentityRoles)
        {
            foreach (var leRole in legalentityRoles)
                Service<IEmploymentService>().UpdateSalaryPaymentDay(leRole.LegalEntityKey, null);
        }

        #endregion Helpers
    }
}