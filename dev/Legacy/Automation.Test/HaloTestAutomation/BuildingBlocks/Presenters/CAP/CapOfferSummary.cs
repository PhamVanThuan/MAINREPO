using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.Cap
{
    /// <summary>
    /// The CAP 2 Offer Summary Screen
    /// </summary>
    public class CapOfferSummary : CAP2OfferSummaryControls
    {
        private readonly IAccountService accountService;
        private readonly IWatiNService watinService;
        private readonly ICAP2Service cap2Service;

        public CapOfferSummary()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            cap2Service = ServiceLocator.Instance.GetService<ICAP2Service>();
        }

        /// <summary>
        /// Either NTU's or Declines a CAP Offer
        /// </summary>
        /// <param name="type">Either "NTU" or "Decline"</param>
        public void DeclineOrNTUCAP2Application(string type)
        {
            string reason = (from r in cap2Service.GetCapNTUReasons(type) select r.Column("Description").GetValueAs<string>()).FirstOrDefault();
            //select the reason from the dropdown
            base.NTUReason.Option(reason).Select();
            CompleteCAP2Action();
        }

        public void CompleteCAP2Action()
        {
            watinService.HandleConfirmationPopup(base.SubmitButton);
        }

        /// <summary>
        /// This can be called to ignore the initial warnings on the Create CAP 2 offer screen and carry on with
        /// the creation of the case. For example, the "Client doesn't qualify..." warning is displayed before the
        /// "Existing CAP 2 Offer..." warning so we need to continue with the creation
        /// </summary>
        public void IgnoreWarningsAndContinue()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                watinService.HandleConfirmationPopup(base.SubmitButton);
            }
        }

        /// <summary>
        /// This method will first determine the employment type of the application (subsidised cannot choose the Debit Order option)
        /// and then sets the appropriate payment option and selects it from the dropdown.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        public string SelectCAPPaymentOption(int accountKey)
        {
            //loop through the results if we find a subsidised then we cannot select d/o option
            string employmentType = (from r in cap2Service.GetAccountEmploymentType(accountKey)
                                     where r.Column("description").Value == EmploymentType.SalariedWithDeductions
                                     select r.Column("description").Value).FirstOrDefault();
            //we need the current payment option
            int financialServicePaymentType = accountService.GetFinancialServicePaymentTypeByAccountKey(accountKey);
            //now get the payment option to select based on the employment type
            string selectedPaymentOption =
                employmentType == EmploymentType.SalariedWithDeductions || financialServicePaymentType == (int)FinancialServicePaymentTypeEnum.SubsidyPayment ||
                financialServicePaymentType == (int)FinancialServicePaymentTypeEnum.DirectPayment
                    ? CapPaymentOptions.LoanAccount : CapPaymentOptions.DebitOrderBankAccount;
            //now we can select the payment option
            base.PaymentOption.Option(selectedPaymentOption).Select();
            return selectedPaymentOption;
        }

        /// <summary>
        /// This will selected a CAP option from the CAP offer grid on the CAP Offer Summary screen
        /// </summary>
        /// <param name="CapOption">The CAP option to select</param>
        public void SelectCAPOptionFromGrid(string CapOption)
        {
            bool result = base.gridCAPOptionExists(CapOption);
            if (result)
            {
                base.gridCAPOptions(CapOption).Click();
            }
        }

        /// <summary>
        /// This building block continue when a warning message exists and then selects OK from the popup
        /// </summary>
        public void DomainWarningClickYes()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                watinService.HandleConfirmationPopup(base.divValidationSummaryBody.Buttons[0]);
            }
        }

        /// <summary>
        /// This building block will find the current payment option and then select the other option from the dropdown.
        /// The newly selected value is returned to the test for the assertion. Requires error handling still.
        /// </summary>
        public string UpdateCAPPaymentOption()
        {
            //get the current payment option
            string currentPaymentOption = base.PaymentOption.SelectedItem.ToString();
            string optionToSelect = currentPaymentOption == CapPaymentOptions.DebitOrderBankAccount ? CapPaymentOptions.LoanAccount :
                CapPaymentOptions.DebitOrderBankAccount;
            base.PaymentOption.Option(optionToSelect).Select();
            return optionToSelect;
        }

        /// <summary>
        /// This assertion checks that each of the warning messages for a CAP offer that does not meet the minimum balance
        /// requirement appears on the screen when trying to create to create a CAP offer
        /// </summary>
        /// <param name="capOptions">The options that where the CurrentBalance + CAP Fee is less than the Min Required</param>
        /// <param name="minAmount">The minium MortgageLoan.CurrentBalance required for CAP</param>
        public void AssertCAP2MinBalanceQualification(string[] capOptions, string minAmount)
        {
            bool resultMatch = false;
            base.divValidationSummaryBody.WaitUntilExists();

            foreach (string s in capOptions)
            {
                string warning = String.Format(@"The Total Balance + Cap Fee for each Cap Type is not greater than {0} for cap type {1}", minAmount, s);
                for (int i = 0; i < base.listErrorMessages.Count; i++)
                {
                    string listWarning = base.listErrorMessages.ElementAt(i).Text;

                    if (warning == listWarning)
                    {
                        Logger.LogAction(String.Format(@"Asserting that Validation Message: {0} is returned and displayed", warning));
                        resultMatch = true;
                    }
                }
                Assert.True(resultMatch, "Warning does not exist");
            }
        }

        /// <summary>
        /// Checks the CAP offer grid on the CAP Offer Summary screen and asserts that the expected CAP options exist in the
        /// grid to be selected.
        /// </summary>
        /// <param name="capOptions">An array of CAP options that should exist </param>
        public void AssertCAP2OptionsExist(string[] capOptions)
        {
            var capOption = (from cp in capOptions where base.gridCAPOptionExists(cp) == false select cp).FirstOrDefault();
            Assert.That(String.IsNullOrEmpty(capOption), string.Format("Cap Option {0} should exist", capOption));
        }

        /// <summary>
        /// Checks the CAP offer grid on the CAP Offer Summary screen and asserts that the expected CAP options do not exist in the
        /// grid to be selected. i.e. Promotion Client only gets a 3% option
        /// </summary>
        /// <param name="capOptions">An array of CAP options that should not exist</param>
        public void AssertCAP2OptionsDoNotExist(string[] capOptions)
        {
            var capOption = (from cp in capOptions where base.gridCAPOptionExists(cp) select cp).FirstOrDefault();
            Assert.That(String.IsNullOrEmpty(capOption), string.Format("Cap Option {0} should not exist", capOption));
        }

        /// <summary>
        /// Asserts that each of the warnings are displayed when a client does not qualify for a particular CAP
        /// option. The test should provide it with an array of options to check for and it asserts each exists.
        /// </summary>
        /// <param name="capOptions">Options passed as an array i.e {"1","2"}</param>
        public void AssertCAP2Qualification(int[] capOptions)
        {
            base.divValidationSummaryBody.WaitUntilExists();
            int capOption = (from cp in capOptions
                             where base.listErrorMessages.Exists(Find.ByText(
                             String.Format(@"Client does not qualify for {0}% Above Current Rate. Bond Registration Amount would be exceeded.",
                             cp))) == false
                             select cp).FirstOrDefault();
            Assert.That(capOption == 0, string.Format("The Validation Message was not found for Cap Option {0}", capOption));
        }

        /// <summary>
        /// Clicks the Add Offer Button
        /// </summary>
        public void ClickAddOffer()
        {
            watinService.HandleConfirmationPopup(base.AddOffer);
        }
    }
}