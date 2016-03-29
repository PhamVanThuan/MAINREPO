using BuildingBlocks.Services.Contracts;
using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class RecoveriesProposalCapture : RecoveriesProposalCaptureControls
    {
        private IWatiNService watinService;

        public RecoveriesProposalCapture()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Enters the Shortfall Amount.
        /// </summary>
        /// <param name="value">Amount</param>
        public void EnterShortfallAmount(decimal value)
        {
            watinService.PopulateRandCentsFields(base.ShortfallAmountRands, base.ShortfallAmountCents, value);
        }

        /// <summary>
        /// Enters the Repayment Amount.
        /// </summary>
        /// <param name="value">Amount</param>
        public void EnterRepaymentAmount(decimal value)
        {
            watinService.PopulateRandCentsFields(base.RepaymentAmountRands, base.RepaymentAmountCents, value);
        }

        /// <summary>
        /// Enters the Start Date.
        /// </summary>
        /// <param name="date">Start Date</param>
        public void EnterStartDate(DateTime date)
        {
            base.StartDate.Value = date.ToString(Formats.DateFormat);
        }

        /// <summary>
        /// Captures a Recoveries Proposal with a zero  Shortfall Amount.
        /// </summary>
        public void EnterZeroShortfallAmount()
        {
            base.ShortfallAmountRands.Clear();
            base.ShortfallAmountCents.Clear();
            base.RepaymentAmountCents.Clear();
            base.RepaymentAmountRands.Clear();
            base.StartDate.Clear();
            EnterShortfallAmount(0.00M);
            EnterRepaymentAmount(1500);
            EnterStartDate(DateTime.Now);
            ClickAdd();
        }

        /// <summary>
        /// Captures a Recoveries Proposal with a zero Repayment Amount.
        /// </summary>
        public void EnterZeroRepaymentAmount()
        {
            base.ShortfallAmountRands.Clear();
            base.ShortfallAmountCents.Clear();
            base.RepaymentAmountCents.Clear();
            base.RepaymentAmountRands.Clear();
            base.StartDate.Clear();
            EnterShortfallAmount(45000);
            EnterRepaymentAmount(0.00M);
            EnterStartDate(DateTime.Now);
            ClickAdd();
        }

        /// <summary>
        /// Capture a Recoveries Proposal with an invalid Start Date.
        /// </summary>
        public void EnterInvalidStartDate()
        {
            base.ShortfallAmountRands.Clear();
            base.ShortfallAmountCents.Clear();
            base.RepaymentAmountCents.Clear();
            base.RepaymentAmountRands.Clear();
            base.StartDate.Clear();
            EnterShortfallAmount(45000);
            EnterRepaymentAmount(1250);
            ClickAdd();
        }

        /// <summary>
        /// Captures the required details for a valid Recoveries Proposal.
        /// </summary>
        /// <param name="shortfallAmount">Shortfall Amount</param>
        /// <param name="repaymentAmount">Repayment Amount</param>
        /// <param name="date">Start Date</param>
        /// <param name="AOD">Acknowledgement of Debt</param>
        public void CaptureRecoveriesProposalDetails(decimal shortfallAmount, decimal repaymentAmount, DateTime date, bool AOD)
        {
            EnterShortfallAmount(shortfallAmount);
            EnterRepaymentAmount(repaymentAmount);
            EnterStartDate(date);
            base.AcknowledgementOfDebt.Checked = AOD;
            ClickAdd();
        }

        /// <summary>
        /// Clicks the Add button.
        /// </summary>
        public void ClickAdd()
        {
            base.Add.Click();
        }

        /// <summary>
        /// Clicks the Cancel button.
        /// </summary>
        public void ClickCancel()
        {
            base.Cancel.Click();
        }
    }
}