using BuildingBlocks.Services.Contracts;
using Common.Extensions;
using ObjectMaps.Pages;
using System;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationLoanDetailsApprove : ApplicationLoanDetailsApproveControls
    {
        private readonly IWatiNService watinService;

        public ApplicationLoanDetailsApprove()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Update the SPV by select an SPV by SPVKey from the SPV drop down list
        /// </summary>
        /// <param name="spvKey">16 = Blue banner Account, 17 = Main Street 65 (Pty) Ltd.</param>
        public void UpdateSPVName(int spvKey)
        {
            base.ddlSPV.Option(Find.ByValue(spvKey.ToString())).Select();
        }

        /// <summary>
        /// Update the Cash Deposit amount for a New Purchase application.  The method will split the Cash Deposit amount into Rands and Cents at the decimal
        /// point and enter the amount in the relevant Rands and Cents fields
        /// </summary>
        /// <param name="cashDeposit">#0.00</param>
        public void UpdateNewPurchaseCashOut(decimal cashDeposit)
        {
            watinService.PopulateRandCentsFields(base.txtNewPurchaseCashDeposit_txtRands, base.txtNewPurchaseCashDeposit_txtCents, cashDeposit);
        }

        /// <summary>
        /// Update the Cash Out amount for a Switch application.  The method will split the Cash Out amount into Rands and Cents at the decimal
        /// point and enter the amount in the relevant Rands and Cents fields
        /// </summary>
        /// <param name="cashOut">#0.00</param>
        public void UpdateSwitchCashOut(decimal cashOut)
        {
            watinService.PopulateRandCentsFields(base.txtSwitchCashOut_txtRands, base.txtSwitchCashOut_txtCents, cashOut);
        }

        /// <summary>
        /// Update the Cash Out amount for a Switch application.  The method will split the Cash Out amount into Rands and Cents at the decimal
        /// point and enter the amount in the relevant Rands and Cents fields
        /// </summary>
        /// <param name="cashOut">#0.00</param>
        public void UpdateRefinanceCashOut(decimal cashOut)
        {
            watinService.PopulateRandCentsFields(base.txtRefinanceCashOut_txtRands, base.txtRefinanceCashOut_txtCents, cashOut);
        }

        /// <summary>
        /// Apply or Update a Discounted Link Rate.  The difference between the current Discounted Link Rate and the NewDiscountValue is calculated.
        /// Thereafter the number of button clicks it requires to set the new discount are calculated and executed
        /// </summary>
        /// <param name="newDiscountValue">#0.0</param>
        public void ApplyDiscountedLinkRate(decimal newDiscountValue)
        {
            //Set the discounted link rate to Checked
            base.chkDiscountedLinkRate.Checked = true;
            //we need to get the current discount in order to apply the new one correctly.
            decimal currentDiscount = Convert.ToDecimal(base.txtDiscount.Value);
            //the difference between the new required value and the current one is the no. of times we click
            decimal difference = newDiscountValue - currentDiscount;
            int numClicks = Convert.ToInt32(Convert.ToDouble(Math.Abs(difference)) / 0.1);
            //need to repeat this a number of times to apply the correct discount/surcharge
            for (int i = 0; i < numClicks; i++)
            {
                if (currentDiscount > newDiscountValue)
                {
                    base.btnDiscount_bDown.Click();
                }
                else
                {
                    base.btnDiscount_bUp.Click();
                }
            }
        }

        /// <summary>
        /// Update the Quick Cash Details section for a Switch or Refinance application.  The method will split the Total Amount Approved and Upfront Payment Approved
        /// amounts into Rands and Cents at the decimal point and enter the amount in the relevant Rands and Cents fields
        /// </summary>
        /// <param name="totalAmountApproved">#0.00</param>
        /// <param name="upfrontPaymentApproved">#0.00</param>
        public void UpdateQuickCashDetails(double totalAmountApproved, double upfrontPaymentApproved)
        {
            watinService.PopulateRandCentsFields(base.txtQuickCashDetails__txtTotalAmountApproved_txtRands,
                base.txtQuickCashDetails__txtTotalAmountApproved_txtCents, (decimal)totalAmountApproved);
            watinService.PopulateRandCentsFields(base.txtQuickCashDetails__txtUpfrontPaymentApproved_txtRands,
                base.txtQuickCashDetails__txtUpfrontPaymentApproved_txtCents, (decimal)upfrontPaymentApproved);
        }

        /// <summary>
        /// Updates the Total Amount Approved and Upfront Payment Approved values to 0 and clicks the Decline Quick Cash button
        /// </summary>
        public void QCDecline()
        {
            UpdateQuickCashDetails(0, 0);
            base.btnQuickCashDetails_ctl17.Click();
        }

        /// <summary>
        /// Recalculates and Saves the Application
        /// </summary>
        public void RecalcAndSave()
        {
            //recalculate
            base.btnRecalc.Click();
            //save
            base.btnUpdate.Click();
        }

        /// <summary>
        /// Recalculates the application only
        /// </summary>
        public void RecalculateApplication()
        {
            //recalculate
            base.btnRecalc.Click();
        }

        /// <summary>
        /// Saves the application only
        /// </summary>
        public void SaveApplication()
        {
            //save
            base.btnUpdate.Click();
        }

        /// <summary>
        /// Get the Maximum Quick Cash value as displayed on the Application Loan Details Approve screen for Switch and Refinance offers and convert it from a string to double
        /// </summary>
        /// <returns>#0.00</returns>
        public double GetMaximumQC()
        {
            string sMaxQC = base.spanQuickCashDetails__txtMaximumQuickCash.Text.CleanCurrencyString(false);
            double dMaxQC = Convert.ToDouble(sMaxQC);
            return dMaxQC;
        }

        /// <summary>
        /// Checks if Quick Cash has already been declined.  If the Total Amount Approved field is disabled we assume QC has been declined and return true.
        /// If it is enabled, we assume QC has not been declined and return false
        /// </summary>
        /// <returns>QC declined = true, QC not declined = false</returns>
        public bool IsQCDeclined()
        {
            if (base.txtQuickCashDetails__txtTotalAmountApproved_txtRands.Enabled) return false;
            else return true;
        }

        /// <summary>
        /// Ticks the Quick Pay Loan checkbox.
        /// </summary>
        public void ApplyQuickPay()
        {
            //Tick the Quick pay Loan checkbox
            base.chkQuickPayLoan.Checked = true;
        }
    }
}