using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Web.Views.Recoveries.Interfaces;
using SAHL.Common.Web.UI;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Recoveries
{
    public partial class RecoveriesProposal : SAHLCommonBaseView, IRecoveriesProposal
    {
        IRecoveriesRepository _recoveriesRepo;

        #region Properties

        public bool AddButtonVisible
        {
            set
            {
                AddButton.Visible = value;
            }
        }

        public bool CancelButtonVisible
        {
            set
            {
                CancelButton.Visible = value;
            }
        }

        public bool AddPanelVisible
        {
            set
            {
                pnlAdd.Visible = value;
            }
        }

        public double ShortfallAmount
        {
            get
            {
                if (txtShortfallAmount.Amount.HasValue)
                    return txtShortfallAmount.Amount.Value;
                else
                    return 0;
            }
        }

        public double RepaymentAmount
        {
            get
            {
                if (txtRepaymentAmount.Amount.HasValue)
                    return txtRepaymentAmount.Amount.Value;
                else
                    return 0;
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return dteStartDate.Date;
            }
        }

        public bool AOD
        {
            get
            {
                return chkAOD.Checked;
            }
        }

        #endregion

        #region Events

        public event EventHandler OnAddButtonClicked;
        public event EventHandler OnCancelButtonClicked;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            if (_recoveriesRepo == null)
                _recoveriesRepo = RepositoryFactory.GetRepository<IRecoveriesRepository>();

            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

            txtShortfallAmount.Amount = 0;
            txtRepaymentAmount.Amount = 0;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (OnAddButtonClicked != null)
                OnAddButtonClicked(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        public void BindRecoveriesProposals(List<SAHL.Common.BusinessModel.Interfaces.IRecoveriesProposal> proposalList)
        {
            gvRecoveriesProposal.DataSource = proposalList;
            gvRecoveriesProposal.DataBind();
        }

        protected void gvRecoveriesProposal_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e != null)
            {
                SAHL.Common.BusinessModel.Interfaces.IRecoveriesProposal recoveriesProposal = _recoveriesRepo.GetRecoveriesProposalByKey(Convert.ToInt32(e.GetValue("Key")));
                switch (e.DataColumn.Caption)
                {
                    case "Status":
                        e.Cell.Text = recoveriesProposal.GeneralStatus.Description;
                        break;
                    case "User":
                        e.Cell.Text = recoveriesProposal.ADUser.ADUserName;
                        break;
                    case "AOD":
                        e.Cell.Text = recoveriesProposal.AcknowledgementOfDebt == null ? "False" : Convert.ToString(recoveriesProposal.AcknowledgementOfDebt.Value);
                        break;
                    case "Start Date":
                        e.Cell.Text = recoveriesProposal.StartDate.ToString(SAHL.Common.Constants.DateFormat);
                        break;
                    case "Shortfall Amount":
                        e.Cell.Text = recoveriesProposal.ShortfallAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                        break;
                    case "Repayment Amount":
                        e.Cell.Text = recoveriesProposal.RepaymentAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                        break;
                }
            }
        }
    }
}