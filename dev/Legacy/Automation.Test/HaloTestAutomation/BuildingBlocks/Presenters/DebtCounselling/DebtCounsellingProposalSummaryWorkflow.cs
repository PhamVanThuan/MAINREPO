using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Threading;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    /// <summary>
    /// Summary of Debt Counselling Proposals.  Select a Proposal and choose an action to perform on that proposal
    /// </summary>
    public class DebtCounsellingProposalSummaryWorkflow : DebtCounsellingProposalSummaryWorkflowControls
    {
        private readonly IWatiNService watinService;

        public DebtCounsellingProposalSummaryWorkflow()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Click the chosen button.  If the selected button does not exist, no button will be clicked
        /// </summary>
        /// <param name="b">WatiN.Core.TestBrowser object</param>
        /// <param name="buttonLabel">The button to click</param>
        public void ClickButton(ButtonTypeEnum buttonLabel)
        {
            Thread.Sleep(1500);
            switch (buttonLabel)
            {
                case ButtonTypeEnum.Add:
                    base.btnAdd.Click();
                    break;

                case ButtonTypeEnum.Update:
                    base.btnUpdate.Click();
                    break;

                case ButtonTypeEnum.View:
                    base.btnView.Click();
                    break;

                case ButtonTypeEnum.CopytoDraft:
                    base.btnCopyToDraft.Click();
                    break;

                case ButtonTypeEnum.SetActive:
                    watinService.HandleConfirmationPopup(base.btnSetActive);
                    break;

                case ButtonTypeEnum.Delete:
                    base.btnDelete.Click();
                    base.Document.Page<BasePage>().DomainWarningClickYes();
                    break;

                case ButtonTypeEnum.Reasons:
                    base.btnReasons.Click();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Identify a Proposal by 'ProposalType' and 'ProposalStatus' from the list of proposals in the 'Debt Counselling Proposals' grid and select it
        /// </summary>
        /// <param name="b">WatiN.Core.TestBrowser object</param>
        /// <param name="type">Proposal Type identifier</param>
        /// <param name="status">Proposal Status identifier</param>
        /// <param name="buttonLabel">The button to click</param>
        public void SelectProposal(string type, string status, ButtonTypeEnum buttonLabel)
        {
            base.gridProposalSummaryDXMainTableRow(type, status).Click();
            ClickButton(buttonLabel);
        }

        /// <summary>
        /// Identify a Proposal by 'ProposalType', 'ProposalStatus' and 'CreateDate' from the list of proposals in the 'Debt Counselling Proposals' grid and select it
        /// </summary>
        /// <param name="b">WatiN.Core.TestBrowser object</param>
        /// <param name="type">Proposal Type identifier</param>
        /// <param name="status">Proposal Status identifier</param>
        /// <param name="dateCaptured">Date Capture identifier</param>
        /// <param name="buttonLabel">The button to click</param>
        public void SelectProposal(string type, string status, DateTime dateCaptured, ButtonTypeEnum buttonLabel)
        {
            base.gridProposalSummaryDXMainTableRow(type, status, dateCaptured).Click();
            ClickButton(buttonLabel);
        }

        public bool CheckEntryExists(string type, string status)
        {
            return base.gridProposalSummaryDXMainTableRowExists(type, status);
        }

        public void ClickCreateCounterProposal()
        {
            base.CreateCounterProposal.Click();
        }
    }
}