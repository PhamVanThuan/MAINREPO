using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Web.Views.Common;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class ConsultantDecline : SAHLCommonBaseView, IConsultantDecline
    {
        IDebtCounsellingRepository _debtCounsellingRepo;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        private bool _cancelButtonVisible, _submitButtonVisible;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

            if (_debtCounsellingRepo == null)
                _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            btnSubmit.Visible = _submitButtonVisible;
            btnCancel.Visible = _cancelButtonVisible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }


        public void BindProposals(List<IProposal> proposals)
        {
            //setup columns
            gvProposals.Columns.Clear();

            // key columns
            gvProposals.AddGridColumn("Key", "", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);

            // visible columns
            gvProposals.AddGridColumn("", "Type", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gvProposals.AddGridColumn("", "Status", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gvProposals.AddGridColumn("CreateDate", "Date Captured", 20, GridFormatType.GridDateTime, "dd/MM/yyyy HH:mm:ss", HorizontalAlign.Left, true, true);
            gvProposals.AddGridColumn("", "Captured By", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gvProposals.AddGridColumn("", "HOC", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gvProposals.AddGridColumn("", "Life", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gvProposals.AddGridColumn("", "Accepted", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);

            gvProposals.DataSource = proposals;
            gvProposals.DataBind();

        }

        protected void gvProposals_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            switch (e.DataColumn.Caption)
            {
                case "Type":
                case "Status":
                case "Captured By":
                case "HOC":
                case "Life":
                case "Accepted":
                    var proposal = _debtCounsellingRepo.GetProposalByKey(Convert.ToInt32(e.GetValue("Key")));
                    if (e.DataColumn.Caption == "Type")
                        e.Cell.Text = proposal.ProposalType.Description;
                    if (e.DataColumn.Caption == "Status")
                        e.Cell.Text = proposal.ProposalStatus.Description;
                    else if (e.DataColumn.Caption == "Captured By")
                        e.Cell.Text = proposal.ADUser.ADUserName;
                    else if (e.DataColumn.Caption == "HOC")
                        e.Cell.Text = proposal.HOCInclusive.Value == true ? SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc : SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc;
                    else if (e.DataColumn.Caption == "Life")
                        e.Cell.Text = proposal.LifeInclusive.Value == true ? SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc : SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc;
                    else if (e.DataColumn.Caption == "Accepted")
                    {
                        if (proposal.Accepted != null)
                        {
                            if (Convert.ToBoolean(proposal.Accepted.Value) == true)
                                e.Cell.Text = "Yes";
                            else
                                e.Cell.Text = "No";
                        }
                        else
                            e.Cell.Text = "No";
                    }

                    break;
                default:
                    break;
            }
        }

        public bool SubmitButtonVisible
        {
            set { _submitButtonVisible = value; }
        }

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible = value; }
        }

        public int SelectedProposalKey
        {
            get
            {
                // get the selected key from the grid
                int selectedProposalKey = gvProposals.SelectedKeyValue !=null ? Convert.ToInt32(gvProposals.SelectedKeyValue) : -1;

                return selectedProposalKey;
            }
        }
    }
}