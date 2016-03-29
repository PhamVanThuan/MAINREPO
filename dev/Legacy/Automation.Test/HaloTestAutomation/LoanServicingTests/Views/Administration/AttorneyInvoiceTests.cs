using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public sealed class AttorneyInvoiceTests : TestBase<AttorneyInvoice>
    {
        #region PrivateVar

        private int accountKey;
        private string invoicenumber;
        private string attorney;
        private decimal amount;
        private string comments;
        private decimal VATamount;
        private decimal total;
        private int attorneyKey;
        private DateTime invoiceDate;

        #endregion PrivateVar

        protected override void OnTestStart()
        {
            base.OnTestStart();
            var random = new Random();
            this.invoicenumber = string.Format(@"{0}Test!@#$#%", random.Next(0, 500));
            base.Browser.Refresh();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().AttorneyInvoice();
            this.accountKey = Service<IDebtCounsellingService>().GetRandomDebtCounsellingAccount(DebtCounsellingStatusEnum.Open);
        }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            this.amount = 2000;
            this.comments = "Test Comments 123 !@#$%$";
            this.VATamount = 0;
            this.total = (this.amount + this.VATamount);
            this.invoiceDate = DateTime.Now;
            //get attorneyKey
            var attorney = base.Service<IAttorneyService>().GetAttorney(true, false);
            this.attorney = attorney.LegalEntity.RegisteredName;
            this.attorneyKey = attorney.AttorneyKey;
            //login
            Service<IWatiNService>().CloseAllOpenIEBrowsers();
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
        }

        #region Tests

        /// <summary>
        /// Tests the validation for capturing an Attorney Invoice.
        /// </summary>
        [Test, Description("Tests the validation for capturing an Attorney Invoice.")]
        public void ValidationMessages()
        {
            try
            {
                base.View.PopulateFields(this.accountKey);
                base.View.Add();
                string[] expectedMessages
                    = new string[]
                {
                    "Please select an Attorney.",
                    "Please enter an Invoice Number.",
                    "An amount, not zero is required."
                };
                string messages = base.View.GetValidationMessages();
                foreach (string expectedMessage in expectedMessages)
                    if (!messages.Contains(expectedMessage))
                        Assert.Fail(String.Format("Expected Validation Message:{0}", expectedMessage));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Captures an Invoice
        /// </summary>
        [Test, Description("Captures an Invoice")]
        public void CaptureInvoice()
        {
            try
            {
                base.View.PopulateFields
                    (
                     this.accountKey,
                     this.invoicenumber,
                     this.attorney,
                     this.amount,
                     this.comments,
                    invoiceDate: this.invoiceDate);
                base.View.Add();

                string messages = base.View.GetValidationMessages();
                Assert.IsTrue(String.IsNullOrEmpty(messages), String.Format("The following error messages prevented capturing an invoice: {0}", messages));

                //Assert
                AttorneyAssertions.AssertAttorneyInvoice
                    (
                        this.accountKey,
                        this.invoicenumber,
                        this.attorneyKey,
                        this.amount,
                        this.comments,
                       expectedTotal: this.total,
                       invoiceDate: this.invoiceDate
                   );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Captures an invoice including a VAT value
        /// </summary>
        [Test, Description("Captures an invoice including a VAT value")]
        public void CaptureInvoiceIncludingVAT()
        {
            try
            {
                this.VATamount = 280;
                this.total = this.amount + this.VATamount;
                base.View.PopulateFields
                    (
                    this.accountKey,
                    this.invoicenumber,
                    this.attorney,
                    this.amount,
                    this.comments,
                    this.VATamount,
                    this.invoiceDate
                    );
                base.View.Add();
                string messages = base.View.GetValidationMessages();
                Assert.IsTrue(String.IsNullOrEmpty(messages), String.Format("The following error messages prevented capturing an invoice: {0}", messages));
                //Assert
                AttorneyAssertions.AssertAttorneyInvoice
                    (
                        this.accountKey,
                        this.invoicenumber,
                        this.attorneyKey,
                        this.amount,
                        this.comments,
                        this.VATamount,
                        this.total,
                        this.invoiceDate
                   );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Captures an invoice with an invoice date prior to today
        /// </summary>
        [Test, Description("Captures an invoice with an invoice date prior to today")]
        public void CaptureBackDatedInvoice()
        {
            try
            {
                this.invoiceDate = DateTime.Today.AddDays(-2);
                base.View.PopulateFields
                    (
                    this.accountKey,
                    this.invoicenumber,
                    this.attorney,
                    this.amount,
                    this.comments,
                    this.VATamount,
                    this.invoiceDate
                    );
                base.View.Add();
                string messages = base.View.GetValidationMessages();
                Assert.IsTrue(String.IsNullOrEmpty(messages), String.Format("The following error messages prevented capturing an invoice: {0}", messages));
                //Assert
                AttorneyAssertions.AssertAttorneyInvoice
                    (
                        this.accountKey,
                        this.invoicenumber,
                        this.attorneyKey,
                        this.amount,
                        this.comments,
                        this.VATamount,
                        this.total,
                        this.invoiceDate
                   );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Captures an invoice and then deletes it.
        /// </summary>
        [Test, Description("Captures an invoice and then deletes it.")]
        public void DeleteInvoice()
        {
            base.View.PopulateFields
                   (accountkey: this.accountKey,
                   invoicenumber: this.invoicenumber,
                   attorney: this.attorney,
                   amount: this.amount,
                   comments: this.comments,
                   invoiceDate: this.invoiceDate);

            base.View.Add();
            //Select Invoice Item and delete
            base.View.SelectInvoiceRecord(this.invoicenumber);
            //Delete
            base.View.ClickDelete();
            //Assert
            AttorneyAssertions.AssertAttorneyInvoiceDoesNotExist
                (
                    this.accountKey,
                    this.invoicenumber,
                    this.attorneyKey,
                    this.amount,
                    this.comments,
                    this.VATamount,
                    this.total,
                    this.invoiceDate
               );
        }

        #endregion Tests
    }
}