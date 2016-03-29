using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class LoanAdjustment : SAHLCommonBasePresenter<ISPVTermChangeRequest>
    {
        private IAccount _account;
        private IMortgageLoan _mortgageLoan;

        protected CBOMenuNode _node;
        protected InstanceNode _instanceNode;
        protected long _instanceID;

        protected IMortgageLoanRepository _mortgageLoanRepo;
        protected IFinancialServiceRepository _financialServiceRepo;
        protected IAccountRepository _accountRepository;

        //private DateTime? _openDate;
        private double? _currentBalance;
        private string _currentSPV;
        private int? _initialTerm;
        private int? _newTotalTerm;
        private int? _loanNumber;
        private double? _newInstallment;
        private double? _loanAmount;
        private string _requestedby;
        private string _newSPVDescription;
        private int? _currentTerm;
        private double? _ltv;
        private double? _currentPTI;
        private double? _newPTI;

        public LoanAdjustment(ISPVTermChangeRequest view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            SetValues();
            BindValues();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        private void SetValues()
        {
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();

            _instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;

            _instanceID = _instanceNode.InstanceID;
            _loanAmount = MortgageLoan.SumBondLoanAgreementAmounts();
            _loanNumber = Account.Key;
            //_openDate = Account.OpenDate;
            _currentBalance = MortgageLoan.CurrentBalance;
            _currentSPV = MortgageLoan.Account.SPV.Description.ToString();
            _initialTerm = MortgageLoan.InitialInstallments;
            _requestedby = _instanceNode.Instance.CreatorADUserName;
            _newTotalTerm = mlRepo.LookUpPendingTermChangeFromX2(_instanceID);
            _currentTerm = MortgageLoan.RemainingInstallments;

            if (MortgageLoan.HasInterestOnly())

                _newInstallment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInterestOnlyInstallment(MortgageLoan.CurrentBalance - (double)LifeBalance(), CapBalance(), Convert.ToDouble(MortgageLoan.InterestRate), Convert.ToDouble(MortgageLoan.ActiveMarketRate), Convert.ToDouble(MortgageLoan.ActiveMarketRate));
            else
                _newInstallment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(Convert.ToDouble(MortgageLoan.InterestRate), Convert.ToDouble(MortgageLoan.ActiveMarketRate), Convert.ToDouble(MortgageLoan.ActiveMarketRate), MortgageLoan, CapBalance(), (int)_newTotalTerm, 12);

            // SPV is not determined by 
            //if (svcRule.ExecuteRule(_view.Messages, "MortgageLoanSPVChangeCheck", MortgageLoan, (int)_instanceID) == 0)
            //{
            //    _newSPVDescription = mlRepo.GetNewSPVTermChange(MortgageLoan.Account.SPV.Key);
            //    _view.Messages.Remove(_view.Messages[_view.Messages.Count - 1]);
            //    //SPV description will stay the same so we don't diplay the new name
            //}
            //else
            //{
                
            //}

            _newSPVDescription = "-";
            double latestValuation = MortgageLoan.GetActiveValuationAmount();
            if (latestValuation != 0 && _currentBalance.HasValue)
                _ltv = _currentBalance.Value / latestValuation;

            double householdIncome = Account.GetHouseholdIncome();
            if (householdIncome != 0)
            {
                _currentPTI = Account.CalcAccountPTI;

                _newPTI = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(_newInstallment.HasValue ? _newInstallment.Value : 0, householdIncome);
            }
        }

        private void BindValues()
        {
            _view.CurrentBalance = _currentBalance;
            _view.InitialTerm = _initialTerm;
            _view.Requested = _requestedby;
            _view.LoanNumber = _loanNumber;
            _view.LoanAmount = _loanAmount;
            _view.CurrentSPV = _currentSPV;
            _view.NewTerm = _newTotalTerm;
            _view.NewInstallment = _newInstallment;
            _view.SPV = _newSPVDescription;
            _view.CurrentTerm = _currentTerm;
            _view.LTV = _ltv;
            _view.CurrentPTI = _currentPTI;
            _view.NewPTI = _newPTI;
        }

        public double CapBalance()
        {
            double capbalance;
            capbalance = 0;

            foreach (IFinancialAdjustment financialAdjustment in MortgageLoan.FinancialAdjustments)
            {
                if (financialAdjustment.FinancialAdjustmentType.Description == "CAP")
                    capbalance = Convert.ToDouble(financialAdjustment.FixedRateAdjustment.Rate);
            }
            return capbalance;
        }

        protected IMortgageLoanRepository MortgageLoanRepo
        {
            get
            {
                if (_mortgageLoanRepo == null)
                    _mortgageLoanRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                return _mortgageLoanRepo;
            }
        }

        protected Double? LifeBalance()
        {
            double? lifeBalance;
            lifeBalance = 0;
            IMortgageLoanAccount mortgageLoanAccount = Account as IMortgageLoanAccount;
            if (mortgageLoanAccount != null)
            {
                if (mortgageLoanAccount.LifePolicyAccount != null)
                {
                    ILifePolicy _lifePolicy = (ILifePolicy)mortgageLoanAccount.LifePolicyAccount.LifePolicy;
                    if (_lifePolicy != null)
                    {
                        return _lifePolicy.FinancialService.Balance.Amount;
                    }
                }
            }
            return lifeBalance;
        }

        protected IAccountRepository AccountRepo
        {
            get
            {
                if (_accountRepository == null)
                    _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                return _accountRepository;
            }
        }

        protected IMortgageLoan MortgageLoan
        {
            get
            {
                if (_mortgageLoan == null)
                {
                    int acctKey = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).GenericKey;
                    _mortgageLoan = MortgageLoanRepo.GetMortgageloanByAccountKey(acctKey);
                }
                return _mortgageLoan;
            }
            set
            {
                _mortgageLoan = value;
            }
        }

        protected IAccount Account
        {
            get
            {
                if (_account == null)
                {
                    int acctKey = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).GenericKey;
                    _account = AccountRepo.GetAccountByKey(acctKey);
                }
                return _account;
            }
            set
            {
                _account = value;
            }
        }
    }
}