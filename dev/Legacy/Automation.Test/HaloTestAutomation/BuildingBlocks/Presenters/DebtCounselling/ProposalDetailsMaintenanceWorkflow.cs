using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Threading;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    /// <summary>
    /// Maintian Proposal Details.  Add or Remove Proposal Details via the Proposal Details Maintenance Workflow view
    /// </summary>
    public class ProposalDetailsMaintenanceWorkflow : ProposalDetailsMaintenanceWorkflowControls
    {
        private readonly IWatiNService watinService;
        private readonly ICommonService commonService;

        public ProposalDetailsMaintenanceWorkflow()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Add an entry to the Proposal Details
        /// </summary>
        /// <param name="proposalDetails"></param>
        /// <param name="usePeriods"></param>
        /// <param name="buttonLabel">The button to click</param>
        public void AddProposalEntry(Automation.DataModels.ProposalItem proposalDetails, ButtonTypeEnum buttonLabel, bool usePeriods)
        {
            if (!string.IsNullOrEmpty(proposalDetails.HOC) && (base.HOCInclusive.SelectedItem != proposalDetails.HOC))
            {
                base.HOCInclusive.Option(proposalDetails.HOC).Select();
                Thread.Sleep(1000);
            }
            if (!string.IsNullOrEmpty(proposalDetails.Life) && (base.LifeInclusive.SelectedItem != proposalDetails.Life))
            {
                base.LifeInclusive.Option(proposalDetails.Life).Select();
                Thread.Sleep(1000);
            }
            if (proposalDetails.MonthlyServiceFee)
            {
                base.ServiceFee.Checked = true;
            }
            if (proposalDetails.StartDate != DateTime.MinValue && base.StartDate.Enabled)
                base.StartDate.Value = proposalDetails.StartDate.ToString(Formats.DateFormat);
            //if we are not capturing using periods then we can set the end date
            if (!usePeriods)
            {
                if (proposalDetails.EndDate != DateTime.MinValue)
                {
                    base.EndDate.Clear();
                    Thread.Sleep(1500);
                    base.EndDate.Value = proposalDetails.EndDate.ToString(Formats.DateFormat);
                    Thread.Sleep(1500);
                    base.EndDate.FireEvent("onchange");
                }
            }
            else //we are capturing using the periods
            {
                //if this is enabled we are on the initial line capture so set it to 1
                if (base.StartPeriod.Enabled)
                {
                    base.StartPeriod.Value = "1";
                }
                //set the end period
                base.EndPeriod.Value = proposalDetails.EndPeriod.ToString();
                base.EndDate.Value = proposalDetails.EndDate.ToString(Formats.DateFormat);
            }

            if (!string.IsNullOrEmpty(proposalDetails.MarketRate))
                base.MarketRate.Option(proposalDetails.MarketRate).Select();
            if (proposalDetails.InterestRate != -1)
            {
                int interestRateInteger;
                int interestRateDecimal;
                commonService.SplitRandsCents(out interestRateInteger, out interestRateDecimal, proposalDetails.InterestRate);
                base.InterestRateRands.TypeText(interestRateInteger.ToString());
                base.InterestRateCents.TypeText(interestRateDecimal.ToString());
            }
            if (proposalDetails.Amount != -1)
            {
                int amountRands;
                int amountCents;
                commonService.SplitRandsCents(out amountRands, out amountCents, proposalDetails.Amount);
                base.AmountRands.TypeText(amountRands.ToString());
                base.AmountCents.TypeText(amountCents.ToString());
            }
            if (proposalDetails.AdditionalAmount != -1)
            {
                int additionalAmountRands;
                int additionalAmountCents;
                commonService.SplitRandsCents(out additionalAmountRands, out additionalAmountCents, proposalDetails.AdditionalAmount);
                base.AdditionalAmountRands.TypeText(additionalAmountRands.ToString());
                base.AdditionalAmountCents.TypeText(additionalAmountCents.ToString());
            }

            ClickButton(buttonLabel);
        }

        /// <summary>
        /// Click the chosen button.  If the selected button does not exist, no button will be clicked
        /// </summary>
        /// <param name="buttonLabel">The button to click</param>
        public void ClickButton(ButtonTypeEnum buttonLabel)
        {
            switch (buttonLabel)
            {
                case ButtonTypeEnum.Add:
                    base.Add.Click();
                    break;

                case ButtonTypeEnum.Done:
                case ButtonTypeEnum.Back:
                    base.Cancel.Click();
                    break;

                case ButtonTypeEnum.Remove:
                    watinService.HandleConfirmationPopup(base.Remove);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Remove an entry from the Proposal Details
        /// </summary>
        /// <param name="startDate">Start Date used to identify the Proposal Details entry to be selected</param>
        /// <param name="endDate">End Date used to identify the Proposal Details entry to be selected</param>
        /// <param name="buttonLabel">The button to click</param>
        public void RemoveEntry(DateTime startDate, DateTime endDate, ButtonTypeEnum buttonLabel)
        {
            base.ProposalItemsTableRow(startDate, endDate).Click();
            ClickButton(buttonLabel);
        }
    }
}