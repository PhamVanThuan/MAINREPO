using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Threading;

namespace BuildingBlocks
{
    /// <summary>
    /// Maintian Proposal Details.  Add or Remove Counter Proposal Details via the Counter Proposal Details Maintenance Workflow view
    /// </summary>
    public class CounterProposalDetailsMaintenanceWorkflow : CounterProposalDetailsMaintenanceWorkflowControls
    {
        private readonly IProposalService proposalService;
        private readonly IWatiNService watinService;
        private readonly ICommonService commonService;

        public CounterProposalDetailsMaintenanceWorkflow()
        {
            proposalService = ServiceLocator.Instance.GetService<IProposalService>();
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Remove an entry from the Counter Proposal Details
        /// </summary>
        /// <param name="b">WatiN.Core.TestBrowser object</param>
        /// <param name="startDate">Start Date used to identify the Proposal Details entry to be selected</param>
        /// <param name="endDate">End Date used to identify the Proposal Details entry to be selected</param>
        /// <param name="buttonLabel">The button to click</param>
        public void RemoveEntry(DateTime startDate, DateTime endDate, ButtonTypeEnum buttonLabel)
        {
            base.ProposalItemRow(startDate, endDate).Click();
            ClickButton(buttonLabel);
        }

        /// <summary>
        /// Adds a counter proposal entry
        /// </summary>
        /// <param name="b"></param>
        /// <param name="proposalItem"></param>
        /// <param name="buttonLabel"></param>
        /// <param name="usePeriods"></param>
        public void AddCounterProposalEntry(Automation.DataModels.ProposalItem proposalItem, ButtonTypeEnum buttonLabel, bool usePeriods)
        {
            if (!string.IsNullOrEmpty(proposalItem.HOC) && (base.HOC.SelectedItem != proposalItem.HOC))
            {
                base.HOC.Option(proposalItem.HOC).Select();
                Thread.Sleep(1000);
            }
            if (!string.IsNullOrEmpty(proposalItem.Life) && (base.Life.SelectedItem != proposalItem.Life))
            {
                base.Life.Option(proposalItem.Life).Select();
                Thread.Sleep(1000);
            }
            if (proposalItem.MonthlyServiceFee)
            {
                base.ServiceFee.Checked = true;
                Thread.Sleep(1000);
            }
            if (proposalItem.StartDate != DateTime.MinValue && base.StartDate.Enabled
                && base.StartDate.Value != proposalItem.StartDate.ToString(Formats.DateFormat))
                base.StartDate.Value = proposalItem.StartDate.ToString(Formats.DateFormat);
            //if we are not capturing using periods then we can set the end date
            if (!usePeriods)
            {
                if (proposalItem.EndDate != DateTime.MinValue && base.EndDate.Value != proposalItem.EndDate.ToString(Formats.DateFormat))
                {
                    base.EndDate.Clear();
                    Thread.Sleep(1500);
                    base.EndDate.Value = proposalItem.EndDate.ToString(Formats.DateFormat);
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

                if (proposalItem.EndDate != DateTime.MinValue)
                {
                    //set the end period
                    if (base.EndDate.Value != proposalItem.EndDate.ToString(Formats.DateFormat))
                        base.EndDate.Value = proposalItem.EndDate.ToString(Formats.DateFormat);
                    if (base.EndPeriod.Value != proposalItem.EndPeriod.ToString())
                        base.EndPeriod.Value = proposalItem.EndPeriod.ToString();
                }
            }
            if (proposalItem.Note != null)
            {
                if (base.CounterProposalNote.Text != proposalItem.Note)
                    base.linkCounterProposalNotes.Click();
                base.CounterProposalNote.TypeText(proposalItem.Note);
            }
            if (proposalItem.InterestRate != -1)
            {
                int interestRateInteger;
                int interestRateDecimal;
                commonService.SplitRandsCents(out interestRateInteger, out interestRateDecimal, proposalItem.InterestRate);
                if (base.InterestRate.Text != interestRateInteger.ToString())
                    base.InterestRate.TypeText(interestRateInteger.ToString());
                if (base.InterestRateDecimals.Text != interestRateDecimal.ToString())
                    base.InterestRateDecimals.TypeText(interestRateDecimal.ToString());
            }
            if (proposalItem.InstalmentPercent != -1)
            {
                int instalmentPercentRands;
                int instalmentPercentCents;
                commonService.SplitRandsCents(out instalmentPercentRands, out instalmentPercentCents, proposalItem.InstalmentPercent);
                if (base.InstalmentPercentage.Text != instalmentPercentRands.ToString())
                    base.InstalmentPercentage.TypeText(instalmentPercentRands.ToString());
                if (base.InstalmentPercentageDecimal.Text != instalmentPercentCents.ToString())
                    base.InstalmentPercentageDecimal.TypeText(instalmentPercentCents.ToString());

                base.InstalmentPercentageDecimal.FireEvent("onkeydown");
            }
            if (proposalItem.AnnualEscalation != -1)
            {
                int annualEscalationRands;
                int annualEscalationCents;
                commonService.SplitRandsCents(out annualEscalationRands, out annualEscalationCents, proposalItem.AnnualEscalation);
                if (base.AnnualEscalation.Text != annualEscalationRands.ToString())
                    base.AnnualEscalation.TypeText(annualEscalationRands.ToString());
                if (base.AnnualEscalationDecimal.Text != annualEscalationCents.ToString())
                    base.AnnualEscalationDecimal.TypeText(annualEscalationCents.ToString());
            }
            ClickButton(buttonLabel);
        }

        /// <summary>
        /// Removes all proposals when provided with their proposal types
        /// </summary>
        /// <param name="proposalType">Proposal Type to remove</param>
        /// <param name="debtCounsellingKey">debt counselling key</param>
        /// <param name="browser">Browser</param>
        public void RemoveAllProposalsByProposalType(TestBrowser browser, ProposalTypeEnum proposalType, int debtCounsellingKey)
        {
            Array proposalStatus = Enum.GetValues(typeof(ProposalStatusEnum));
            foreach (ProposalStatusEnum item in proposalStatus)
            {
                if (browser.Page<DebtCounsellingProposalSummaryWorkflow>().CheckEntryExists(proposalType.ToString(), item.ToString()))
                    proposalService.DeleteProposal(debtCounsellingKey, proposalType, item);
            }
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
                    base.btnAdd.Click();
                    break;

                case ButtonTypeEnum.Done:
                case ButtonTypeEnum.Back:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Remove:
                    watinService.HandleConfirmationPopup(base.btnRemove);
                    break;

                default:
                    break;
            }
        }
    }
}