using System;
using System.Web.UI.WebControls;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.IO;
using System.Web.UI;
using System.Web;

namespace SAHL.Web.Views.DebtCounselling
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProposalSummary : SAHLCommonBaseView, IProposalSummary
    {
        private IDebtCounsellingRepository debtCounsellingRepository;
		private IControlRepository controlRepository;
        protected byte[] ammorisationSchedulePDF = null;

        #region IProposalSummary Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        public event EventHandler OnAddButtonClicked;
		public event KeyChangedEventHandler OnDeleteButtonClicked;
        public event KeyChangedEventHandler OnUpdateButtonClicked;
        public event KeyChangedEventHandler OnViewButtonClicked;
        public event KeyChangedEventHandler OnPrintButtonClicked;
        public event KeyChangedEventHandler OnCopyToDraftButtonClicked;
        public event KeyChangedEventHandler OnCreateCounterProposalClicked;
        public event KeyChangedEventHandler OnSetActiveButtonClicked;
        public event KeyChangedEventHandler ReasonsClicked;
		public event KeyChangedEventHandler OnViewAmortisationScheduleClicked;

        /// <summary>
        /// Controls visibility of Cancel Button
        /// </summary>
        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Add Button
        /// </summary>
        public bool ShowAddButton
        {
            set
            {
                btnAdd.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Update Button
        /// </summary>
        public bool ShowUpdateButton
        {
            set
            {
                btnUpdate.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Print Button
        /// </summary>
        public bool ShowPrintButton
        {
            set
            {
                btnPrint.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of View Button
        /// </summary>
        public bool ShowViewButton
        {
            set
            {
                btnView.Visible = value;
            }
        }

		/// <summary>
		/// Controls visibility of View Button
		/// </summary>
		public bool ShowDeleteButton
		{
			set
			{
				btnDelete.Visible = value;
			}
		}

        private ProposalTypeDisplays _showProposals;
        public ProposalTypeDisplays ShowProposals
        {
            get
            {
                if (_showProposals == null)
                    return ProposalTypeDisplays.All;

                return _showProposals;
            }
            set
            {
                _showProposals = value;
            }
        }

        public bool ShowCopyToDraftButton
        {
            set
            {
                btnCopyToDraft.Visible = value;
            }
        }

        public bool ShowCreateCounterProposalButton
        {
            set
            {
                btnCreateCounterProposal.Visible = value;
            }
        }

        public bool ShowSetActiveButton
        {
            set
            {
                btnSetActive.Visible = value;
            }
        }

        public bool ShowReasonsButton
        {
            set
            {
                btnReasons.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstDebtCounsellingProposalSummary"></param>
        public void BindProposalSummaryGrid(List<IProposal> lstDebtCounsellingProposalSummary)
        {
            gridProposalSummary.Settings.ShowGroupPanel = false;
            gridProposalSummary.Settings.ShowTitlePanel = true;
            gridProposalSummary.PostBackType = GridPostBackType.NoneWithClientSelect;
            gridProposalSummary.AutoGenerateColumns = false;
            gridProposalSummary.KeyFieldName = "Key";

            gridProposalSummary.SettingsText.Title = "Debt Counselling Proposals";
            gridProposalSummary.SettingsText.EmptyDataRow = "No Debt Counselling Proposals";

            //setup columns
            gridProposalSummary.Columns.Clear();

            // key columns
            gridProposalSummary.AddGridColumn("Key", "", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);

            // visible columns
            gridProposalSummary.AddGridColumn("", "Type", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gridProposalSummary.AddGridColumn("", "Status", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gridProposalSummary.AddGridColumn("CreateDate", "Date Captured", 20, GridFormatType.GridDateTime, "dd/MM/yyyy HH:mm:ss", HorizontalAlign.Left, true, true);
            gridProposalSummary.AddGridColumn("", "Captured By", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gridProposalSummary.AddGridColumn("", "HOC", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gridProposalSummary.AddGridColumn("", "Life", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gridProposalSummary.AddGridColumn("", "Accepted", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
                     
            gridProposalSummary.DataSource = lstDebtCounsellingProposalSummary;
            gridProposalSummary.DataBind();
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        
            gridProposalSummary.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(gridProposalSummary_HtmlDataCellPrepared);

			if (debtCounsellingRepository == null)
			{
				debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
			}
			if (controlRepository == null)
			{
				controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
			}
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            btnSetActive.Attributes.Add("onclick", "return confirm('Are you sure you want to set this proposal to Active ?. Any existing active proposals will be set to Inactive');");
        }

        protected void gridProposalSummary_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            switch (e.DataColumn.Caption)
            {
                case "Type":
                case "Status":
                case "Captured By":
                case "HOC":
                case "Life":
                case "Accepted":
                    var proposal = debtCounsellingRepository.GetProposalByKey(Convert.ToInt32(e.GetValue("Key")));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            OnAddButtonClicked(sender, e);
        }

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);
			OnDeleteButtonClicked(sender, keyChangedEventArgs);
			
		}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            OnUpdateButtonClicked(sender, keyChangedEventArgs);
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            OnViewButtonClicked(sender, keyChangedEventArgs);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            OnPrintButtonClicked(sender, keyChangedEventArgs);
        }

        protected void btnCopyToDraft_Click(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            OnCopyToDraftButtonClicked(sender, keyChangedEventArgs);
        }

        protected void btnCreateCounterProposal_Click(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            OnCreateCounterProposalClicked(sender, keyChangedEventArgs);
        }

        protected void btnSetActive_Click(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            OnSetActiveButtonClicked(sender, keyChangedEventArgs);
        }

		protected void OnViewAmortisationScheduleClick(object sender, EventArgs e)
		{
			KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

			if (OnViewAmortisationScheduleClicked != null)
			{
				OnViewAmortisationScheduleClicked(sender, keyChangedEventArgs);
			}
		}

        /// <summary>
        /// On Reasons Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnReasonsClick(object sender, EventArgs e)
        {
            // get the selected ProposalKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gridProposalSummary.SelectedKeyValue);

            if (ReasonsClicked != null)
            {
                ReasonsClicked(sender, keyChangedEventArgs);
            }
        }

		public void SendPDFToClient(string pdfFilePath)
		{
			Response.Clear();
			Response.ClearHeaders();
			Response.ClearContent();

			Response.ContentType = "application/pdf";
			Response.AppendHeader("Content-Disposition", "attachment; filename=" + pdfFilePath);
			Response.WriteFile(pdfFilePath);
			Response.Flush();
			Response.End();
		}

        public byte[] AmmorisationSchedulePDF 
        {
            get { return ammorisationSchedulePDF; }
            set { ammorisationSchedulePDF = value; } 
        }
    }
}
