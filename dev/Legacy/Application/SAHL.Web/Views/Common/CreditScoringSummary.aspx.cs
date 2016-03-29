using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common
{
    public partial class CreditScoringSummary : SAHLCommonBaseView, ICreditScoringSummary
    {
        private IITCRepository _itcRepo;
        private ILegalEntityRepository _leRepo;
        public event KeyChangedEventHandler OnScoreGridSelectedIndexChanged;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
 
            if (!this.ShouldRunPage) 
                return;
        }

        public void BindScoreGrid(IEventList<IApplicationCreditScore> scores)
        {
            ScoreGrid.Columns.Clear();
            ScoreGrid.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            ScoreGrid.AddGridBoundColumn("", "Date", Unit.Percentage(19), HorizontalAlign.Left, true);
            ScoreGrid.AddGridBoundColumn("", "User/Actioned By", Unit.Percentage(20), HorizontalAlign.Left, true);
            ScoreGrid.AddGridBoundColumn("", "Workflow State", Unit.Percentage(23), HorizontalAlign.Left, true);
            ScoreGrid.AddGridBoundColumn("", "Workflow Action", Unit.Percentage(23), HorizontalAlign.Left, true);
            ScoreGrid.AddGridBoundColumn("", "Decision", Unit.Percentage(15), HorizontalAlign.Left, true);

            ScoreGrid.DataSource = scores;
            ScoreGrid.DataBind();
        }
        protected void ScoreGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                IApplicationCreditScore acs = e.Row.DataItem as IApplicationCreditScore;
                cells[0].Text = acs.Key.ToString();
                cells[1].Text = acs.ScoreDate.ToString();

                if (acs.ApplicationITCCreditScores != null && acs.ApplicationITCCreditScores.Count > 0)
                    cells[2].Text = acs.ApplicationITCCreditScores[0].ITCCreditScore.ADUserName == null ? "" : acs.ApplicationITCCreditScores[0].ITCCreditScore.ADUserName;
                else
                    cells[2].Text = "";

                cells[3].Text = acs.CallingContext.CallingState;
                cells[4].Text = acs.CallingContext.CallingMethod;
                cells[5].Text = acs.ApplicationAggregateDecision.CreditScoreDecision.Description;
            }

        }

        public void BindApplicantGrid(IApplicationCreditScore acs)
        {
            ApplicantGrid.Columns.Clear();

            if (acs == null)
                return;

            tblCreditScoreValues.Visible = true;

            ApplicantGrid.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            ApplicantGrid.AddGridBoundColumn("", "Applicant Name", Unit.Percentage(30), HorizontalAlign.Left, true);
            ApplicantGrid.AddGridBoundColumn("", "Applicant Type", Unit.Percentage(14), HorizontalAlign.Left, true);
            ApplicantGrid.AddGridBoundColumn("", "Empirica Score", Unit.Percentage(14), HorizontalAlign.Left, true);
            ApplicantGrid.AddGridBoundColumn("", "SBC Score", Unit.Percentage(14), HorizontalAlign.Left, true);
            ApplicantGrid.AddGridBoundColumn("", "Risk Matrix Cell", Unit.Percentage(14), HorizontalAlign.Left, true);
            ApplicantGrid.AddGridBoundColumn("", "Applicant Decision", Unit.Percentage(14), HorizontalAlign.Left, true);

            ApplicantGrid.DataSource = acs.ApplicationITCCreditScores;
            ApplicantGrid.DataBind();

            if (acs.ApplicationITCCreditScores != null && acs.ApplicationITCCreditScores.Count > 0)
            {
                if (acs.ApplicationITCCreditScores[0].ITCCreditScore.RiskMatrixRevision != null)
                {
                    lblRiskMatrix.Text = string.Format("\t{0}", acs.ApplicationITCCreditScores[0].ITCCreditScore.RiskMatrixRevision.RiskMatrix.Description);
                    lblRiskMatrixVersion.Text = string.Format("\t{0} - {1}",acs.ApplicationITCCreditScores[0].ITCCreditScore.RiskMatrixRevision.Key, acs.ApplicationITCCreditScores[0].ITCCreditScore.RiskMatrixRevision.Description);                 
                }

                if (acs.ApplicationITCCreditScores[0].ITCCreditScore.ScoreCard != null)
                {
                    lblScoreCard.Text = string.Format("\t{0}", acs.ApplicationITCCreditScores[0].ITCCreditScore.ScoreCard.Description);
                }
            }
        }
        protected void ApplicantGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                IApplicationITCCreditScore aics = e.Row.DataItem as IApplicationITCCreditScore;

                SAHL.Common.BusinessModel.Interfaces.IITC itc = null;
                IITCArchive itcArchive = null;
                ILegalEntity le = null;

                if (_itcRepo==null)
                    _itcRepo = RepositoryFactory.GetRepository<IITCRepository>();

                if (_leRepo==null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                _itcRepo.GetITCOrArchiveITCByITCKey(aics.ITCCreditScore.ITCKey, out itc, out itcArchive);

                if (itc != null)
                    le = itc.LegalEntity;
                else if (itcArchive != null)
                    le = _leRepo.GetLegalEntityByKey(itcArchive.LegalEntityKey);
                else
                    le = aics.ITCCreditScore.LegalEntity;

                cells[0].Text = aics.Key.ToString();
                cells[1].Text = le.DisplayName;
                cells[2].Text = aics.PrimaryApplicant ? "Primary" : "Secondary";
                cells[3].Text = aics.ITCCreditScore.EmpiricaScore.HasValue ? aics.ITCCreditScore.EmpiricaScore.Value.ToString() : "-";
                cells[4].Text = aics.ITCCreditScore.SBCScore.HasValue ? aics.ITCCreditScore.SBCScore.Value.ToString() : "-";
                cells[5].Text = aics.ITCCreditScore.RiskMatrixCell != null ? aics.ITCCreditScore.RiskMatrixCell.Designation : "-";
                cells[6].Text = aics.CreditScoreDecision.Description;
            }
        }


        public void BindRuleGrid(IApplicationCreditScore acs)
        {
            RuleGrid.Columns.Clear();

            if (acs == null)
                return;

            RuleGrid.AddGridBoundColumn("", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            RuleGrid.AddGridBoundColumn("", "Applicant", Unit.Percentage(8), HorizontalAlign.Left, true);
            RuleGrid.AddGridBoundColumn("", "Reason", Unit.Percentage(22), HorizontalAlign.Left, true);
            RuleGrid.AddGridBoundColumn("", "Comment", Unit.Percentage(70), HorizontalAlign.Left, true);

            RuleGrid.DataSource = acs.ITCDecisionReasons;
            RuleGrid.DataBind();
        }
        protected void RuleGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                IITCDecisionReason reason = e.Row.DataItem as IITCDecisionReason;
                bool isPrimary = false;

                foreach (IApplicationITCCreditScore offerItcScore in reason.ApplicationCreditScore.ApplicationITCCreditScores)
                {
                    if (offerItcScore.PrimaryApplicant && offerItcScore.ITCCreditScore.Key == reason.ITCCreditScore.Key)
                    {
                        isPrimary = true;
                        break;
                    }
                }

                cells[0].Text = reason.Key.ToString();
                cells[1].Text = isPrimary ? "Primary" : "Secondary";
                cells[2].Text = reason.Reason.ReasonDefinition.ReasonDescription.Description;
                cells[3].Text = reason.Reason.Comment;
            }

        }

        protected void ScoreGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnScoreGridSelectedIndexChanged != null)
                OnScoreGridSelectedIndexChanged(sender, new KeyChangedEventArgs(ScoreGrid.SelectedIndex));
        }

        public void ShowGrids()
        {
            ScoreGrid.GridHeight = new Unit(150, UnitType.Pixel);
            ScoreGrid.PostBackType = GridPostBackType.SingleAndDoubleClick;
            ScoreGrid.Visible = true;

            ApplicantGrid.Visible = true;
            RuleGrid.Visible = true;
            lblRiskMatrixVersion.Visible = true;
            lblScoreCard.Visible = true;
        }
    }
}
