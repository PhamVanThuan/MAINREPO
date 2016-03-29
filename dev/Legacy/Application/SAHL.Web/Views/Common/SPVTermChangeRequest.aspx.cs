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
using Microsoft.ApplicationBlocks.UIProcess;

using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using System.Text;
using SAHL.Common.Collections;
using System.Drawing;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel;

namespace SAHL.Web.Views.Common
{
    public partial class SPVTermChange : SAHLCommonBaseView, ISPVTermChangeRequest
    {
        private double? _currentBalance;
        private string _spv;
        private int? _loanNumber;
        private double? _loanAmount;
        private string _currentSPV;
        private string _requested;
        private int? _initialTerm;
        private int? _newTerm;
        private double? _newInstallment;
        private int? _currentTerm;
        private double? _ltv;
        private double? _currentPTI;
        private double? _newPTI;

        public double? CurrentBalance
        {
            get { return _currentBalance; }
            set { _currentBalance = value; }
        }

        public double? NewInstallment
        {
            get { return _newInstallment; }
            set { _newInstallment = value; }
        }

        public string Requested
        {
            get { return _requested; }
            set { _requested = value; }
        }


        public double? LoanAmount
        {
            get { return _loanAmount; }
            set { _loanAmount = value; }
        }

        public int? LoanNumber
        {
            get { return _loanNumber; }
            set { _loanNumber = value; }
        }

        public string CurrentSPV
        {
            get { return _currentSPV; }
            set { _currentSPV = value; }
        }
        
        public string SPV
        {
            get { return _spv; }
            set { _spv = value; }
        }

        public int? InitialTerm
        {
            get { return _initialTerm; }
            set { _initialTerm = value; }
        }

        public int? NewTerm
        {
            get { return _newTerm; }
            set { _newTerm = value; }
        }

        public int? CurrentTerm
        {
            get { return _currentTerm; }
            set { _currentTerm = value; }
        }
	
        public double? LTV
        {
            get { return _ltv; }
            set { _ltv = value; }
        }

        public double? CurrentPTI
        {
            get { return _currentPTI; }
            set { _currentPTI = value; }
        }
        public double? NewPTI
        {
            get { return _newPTI; }
            set { _newPTI = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;
           
            BindControls();

        }

        public void BindControls()
        {
            lblLoanAmount.Text = _loanAmount.HasValue ? _loanAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "";
            lblCurrentSPV.Text = _currentSPV.ToString();
            lblInitialTerm.Text = _initialTerm.ToString();
            lblLoanNumber.Text = _loanNumber.ToString();
            lblNewSPV.Text = _spv.ToString();
            lblNewTotalTerm.Text = _newTerm.ToString();
            lblOutstandingBalance.Text = _currentBalance.HasValue ? _currentBalance.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "";
            lblRequestedBy.Text = _requested;
            lblNewInstallment.Text = _newInstallment.HasValue ? _newInstallment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "";
            lblCurrentTerm.Text = _currentTerm.ToString();
            lblLTV.Text = _ltv.HasValue ? _ltv.Value.ToString(SAHL.Common.Constants.RateFormat) : "";
            lblCurrentPTI.Text = _currentPTI.HasValue ? _currentPTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "";
            lblNewPTI.Text = _newPTI.HasValue ? _newPTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "";
        }
    }
}
