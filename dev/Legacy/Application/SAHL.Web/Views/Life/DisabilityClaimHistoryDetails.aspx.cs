using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Web.Views.Life.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using SAHL.Web.Views.Life.ViewModels;
using System.Data;

namespace SAHL.Web.Views.Life
{
    public partial class DisabilityClaimHistoryDetails : SAHLCommonBaseView, IDisabilityClaimHistoryDetails
    {
        public event KeyChangedEventHandler DisabilityClaims_OnSelectedIndexChanged;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;
        }

        public void BindRepudiatedDecision(DateTime decisionDate, string decisionUser, IEnumerable<IReason> repudiatedReasons)
        {
            pnlDeclinedClaim.Visible = true;
            lblRepudiatedDecisionDate.Text = decisionDate.ToString(SAHL.Common.Constants.DateFormat); 
            lblRepudiatedBy.Text = decisionUser;
            BindRepudiatedReasonsGrid(repudiatedReasons);  
        }

        public void BindApprovedDecision(DateTime decisionDate, string decisionUser)
        {
            pnlAcceptedClaim.Visible = true;
            lblApprovedDecisionDate.Text = decisionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblApprovedBy.Text = decisionUser;
        }

        public void BindTerminatedDecision(DateTime decisionDate, string decisionUser, IEnumerable<IReason> terminatedReasons)
        {
            pnlTerminated.Visible = true;
            lblTerminatedDecisionDate.Text = decisionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblTerminatedBy.Text = decisionUser;
            BindTerminatedReasonsGrid(terminatedReasons);            
        }

        public void BindSettledDecision(DateTime decisionDate, string decisionUser)
        {
            pnlSettled.Visible = true;
            lblSettledDate.Text = decisionDate.ToString(SAHL.Common.Constants.DateFormat);
            lblSettledBy.Text = decisionUser;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disabilityClaim"></param>
        public void BindDisabilityClaim(DisabilityClaimDetailModel disabilityClaim)
        {
            pnlDeclinedClaim.Visible = false;
            pnlAcceptedClaim.Visible = false;
            pnlTerminated.Visible = false;
            pnlSettled.Visible = false;
            lblClaimant.Text = disabilityClaim.ClaimantLegalEntityDisplayName;
            lblDateClaimReceived.Text = disabilityClaim.DateClaimReceived.Date.ToString(SAHL.Common.Constants.DateFormat);
            lblLastDateWorked.Text = disabilityClaim.LastDateWorked.HasValue ? disabilityClaim.LastDateWorked.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblDateOfDiagnosis.Text = disabilityClaim.DateOfDiagnosis.HasValue ? disabilityClaim.DateOfDiagnosis.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblClaimantOccupation.Text = !string.IsNullOrWhiteSpace(disabilityClaim.ClaimantOccupation) ? disabilityClaim.ClaimantOccupation : "-";
            lblDisabilityType.Text = !string.IsNullOrWhiteSpace(disabilityClaim.DisabilityType) ? disabilityClaim.DisabilityType : "-";
            lblOtherDisabilityComments.Text = !string.IsNullOrWhiteSpace(disabilityClaim.OtherDisabilityComments) ? disabilityClaim.OtherDisabilityComments : "-";
            lblExpectedReturnToWorkDate.Text = disabilityClaim.ExpectedReturnToWorkDate.HasValue ? disabilityClaim.ExpectedReturnToWorkDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblClaimStatus.Text = disabilityClaim.DisabilityClaimStatus;
            lblPaymentStartDate.Text = disabilityClaim.PaymentStartDate.HasValue ? disabilityClaim.PaymentStartDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblNumberOfInstalmentsAuthorised.Text = disabilityClaim.NumberOfInstalmentsAuthorised.HasValue ? disabilityClaim.NumberOfInstalmentsAuthorised.Value.ToString() : "-";
            lblPaymentEndDate.Text = disabilityClaim.PaymentEndDate.HasValue ? disabilityClaim.PaymentEndDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";


            IEnumerable<IStageTransition> transitions = SDRepo.GetStageTransitionsByGenericKey(disabilityClaim.DisabilityClaimKey, (int)StageDefinitionGroups.DisabilityClaimWorkflow);
            
            foreach (var transition in transitions)
            {
                
                switch (transition.StageDefinitionStageDefinitionGroup.Key)
                {
                    case (int)StageDefinitionStageDefinitionGroups.DisabilityClaimApproved:
                        BindApprovedDecision(transition.TransitionDate, transition.ADUser.ADUserName);
                        break;
                    case (int)StageDefinitionStageDefinitionGroups.DisabilityClaimRepudiated:
                        IEnumerable<IReason> repudiatedReasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(disabilityClaim.DisabilityClaimKey, (int)ReasonTypes.DisabilityClaimRepudiate);
                        BindRepudiatedDecision(transition.TransitionDate, transition.ADUser.ADUserName, repudiatedReasons);
                        break;
                    case (int)StageDefinitionStageDefinitionGroups.DisabilityClaimSettled:
                        BindSettledDecision(transition.TransitionDate, transition.ADUser.ADUserName);
                        break;
                    case (int)StageDefinitionStageDefinitionGroups.DisabilityClaimTerminated:
                        IEnumerable<IReason> terminatedReasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(disabilityClaim.DisabilityClaimKey, (int)ReasonTypes.DisabilityClaimTerminate);
                        BindTerminatedDecision(transition.TransitionDate, transition.ADUser.ADUserName, terminatedReasons);
                        break;
                    default:
                        break;
                }
            } 

        }

        private IStageDefinitionRepository sdRepo;
        private IStageDefinitionRepository SDRepo
        {
            get
            {
                if (sdRepo == null)
                {
                    sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                }
                return sdRepo;
            }
        }

        private IReasonRepository reasonRepo;
        private IReasonRepository ReasonRepo
        {
            get
            {
                if (reasonRepo == null)
                {
                    reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
                }
                return reasonRepo;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listDisabilityClaimsHistory"></param>
        public void BindDisabilityClaimsHistoryGrid(IEnumerable<DisabilityClaimDetailModel> listDisabilityClaimsHistory)
        {
            this.listDisabilityClaimsHistory = new List<DisabilityClaimDetailModel>(listDisabilityClaimsHistory);

            gridDisabilityClaims.AutoGenerateColumns = false;
            gridDisabilityClaims.AddGridBoundColumn("ClaimantLegalEntityDisplayName", "Claimant", Unit.Percentage(20), HorizontalAlign.Left, true);
            gridDisabilityClaims.AddGridBoundColumn("DateClaimReceived", "Date Claim Received", Unit.Percentage(10), HorizontalAlign.Left, true);
            gridDisabilityClaims.AddGridBoundColumn("DisabilityType", "Disability", Unit.Percentage(20), HorizontalAlign.Left, true);
            gridDisabilityClaims.AddGridBoundColumn("DisabilityClaimStatus", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);

            gridDisabilityClaims.DataSource = listDisabilityClaimsHistory;
            gridDisabilityClaims.DataBind();

            if (this.listDisabilityClaimsHistory.Count > 0)
            {
                BindDisabilityClaim(this.listDisabilityClaimsHistory[0]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDisabilityClaims_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            DisabilityClaimDetailModel disabilityClaim = e.Row.DataItem as DisabilityClaimDetailModel;
            if ((e.Row.DataItem != null)&&(disabilityClaim!= null))
            {
                cells[0].Text = disabilityClaim.ClaimantLegalEntityDisplayName;
                cells[1].Text = disabilityClaim.DateClaimReceived.ToString(SAHL.Common.Constants.DateFormat);
                cells[2].Text = !string.IsNullOrWhiteSpace(disabilityClaim.DisabilityType) ? disabilityClaim.DisabilityType : "";
                cells[3].Text = disabilityClaim.DisabilityClaimStatus;
            }
        }

        public void BindTerminatedReasonsGrid(IEnumerable<IReason> terminatedReasons)
        {
            var reasons = terminatedReasons.Select(x => new ReasonViewModel { Reason = x.ReasonDefinition.ReasonDescription.Description, 
                                                                            Comment = x.Comment });

            gridTerminatedReasons.Visible = (reasons.Count() > 0);
            gridTerminatedReasons.Columns.Clear();
            gridTerminatedReasons.AddGridBoundColumn("Reason", "Reason", Unit.Percentage(25), HorizontalAlign.Left, true);
            gridTerminatedReasons.AddGridBoundColumn("Comment", "Comment", Unit.Percentage(75), HorizontalAlign.Left, true);
            gridTerminatedReasons.DataSource = reasons;
            gridTerminatedReasons.DataBind();            
        }

        public void BindRepudiatedReasonsGrid(IEnumerable<IReason> repudiatedReasons)
        {
            var reasons = repudiatedReasons.Select(x => new ReasonViewModel
            {
                Reason = x.ReasonDefinition.ReasonDescription.Description,
                Comment = x.Comment
            });

            gridRepudiatedReasons.Columns.Clear();
            gridRepudiatedReasons.AddGridBoundColumn("Reason", "Reason", Unit.Percentage(25), HorizontalAlign.Left, true);
            gridRepudiatedReasons.AddGridBoundColumn("Comment", "Comment", Unit.Percentage(75), HorizontalAlign.Left, true);
            gridRepudiatedReasons.DataSource = reasons;
            gridRepudiatedReasons.DataBind();   
        }

        /// <summary>
        /// Raised when the user selects a new row on the grid. Bubble up the event for presenters to handle (passing the index).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDisabilityClaims_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndex = gridDisabilityClaims.SelectedIndex;
            DisabilityClaims_OnSelectedIndexChanged(sender, new KeyChangedEventArgs(gridDisabilityClaims.SelectedIndex));
        }

        private List<DisabilityClaimDetailModel> listDisabilityClaimsHistory;

        public List<DisabilityClaimDetailModel> ListDisabilityClaimsHistory
        {
            get { return listDisabilityClaimsHistory; }
        }


        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
        }
    }
}