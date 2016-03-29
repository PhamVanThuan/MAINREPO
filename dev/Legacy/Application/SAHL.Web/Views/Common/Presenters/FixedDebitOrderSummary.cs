using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// FixedDebitOrder Summary
    /// </summary>
    public class FixedDebitOrderSummary : SAHLCommonBasePresenter<IFixedDebitOrderSummary>
    {
        /// <summary>
        /// account interface
        /// </summary>
        protected IAccount account;
        /// <summary>
        /// MortgageLoan Interface for Variable portion of Loan
        /// </summary>
        protected IMortgageLoan _mlVar;
        /// <summary>
        /// List of Financial Service Type of Variable Loan
        /// </summary>
        protected IReadOnlyEventList<IFinancialService> _mlVarLst;
        /// <summary>
        /// List of Financial Service Type of Fixed Loan
        /// </summary>
        protected IReadOnlyEventList<IFinancialService> _mlFixedLst;
        /// <summary>
        /// MortgageLoan Interface for Fixed portion of Loan
        /// </summary>
        protected IMortgageLoan _mlFixed;
        /// <summary>
        /// Account Repository
        /// </summary>
        protected IAccountRepository _accRepo;
        /// <summary>
        /// FutureDated Change repository
        /// </summary>
        protected IFutureDatedChangeRepository _futureDatedRepo;
        /// <summary>
        /// List of FutureDatedChange records
        /// </summary>
        protected IList<IFutureDatedChange> _futureDatedChangeLst;
        /// <summary>
        /// List of Accounts
        /// </summary>
        protected IEventList<IAccount> accountList;

        private double amortisingInstallmentFixed;
        private bool IsProspectIntOnly;
        /// <summary>
        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;

        /// <summary>
        /// Used by Test
        /// </summary>
        public IAccount accountVal
        {
            set
            {
                account = value;
            }
        }

        /// <summary>
        /// Used by Test
        /// </summary>
        public IList<IFutureDatedChange> futureDCLst
        {
            set
            {
                _futureDatedChangeLst = value;
            }
        }

        /// <summary>
        /// Constructor for Fixed Debit Order Summary
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FixedDebitOrderSummary(IFixedDebitOrderSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }
        /// <summary>
        /// OnViewInitialised event - retrieve require data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;
            // 1542440 - Super Lo with Int Only

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;                   
            
            _futureDatedRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
            _futureDatedChangeLst = _futureDatedRepo.GetFutureDatedChangesByGenericKey(Convert.ToInt32(_node.GenericKey),(int)FutureDatedChangeTypes.FixedDebitOrder);
             
            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            account = _accRepo.GetAccountByKey(Convert.ToInt32(_node.GenericKey));

            _mlVarLst = account.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open });
            if (_mlVarLst.Count > 0)
                _mlVar = _mlVarLst[0] as IMortgageLoan;

            _mlFixedLst = account.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Open });
            if (_mlFixedLst.Count > 0 && _mlFixedLst !=null)
                _mlFixed = _mlFixedLst[0] as IMortgageLoan;

            if (_mlVar != null)
                IsProspectIntOnly = _mlVar.HasInterestOnly();

             if (IsProspectIntOnly)
             {
                 double amortisingInstallmentVar = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(_mlVar.CurrentBalance, _mlVar.InterestRate, _mlVar.RemainingInstallments, false);
                 if (_mlFixed != null)
                     amortisingInstallmentFixed = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(_mlFixed.CurrentBalance, _mlFixed.InterestRate, _mlFixed.RemainingInstallments, false);
                 else
                     amortisingInstallmentFixed = 0;
                 double totalAmortisingInstallment = amortisingInstallmentVar + amortisingInstallmentFixed;
                 _view.BindInterestOnlyData(totalAmortisingInstallment);
             }

             accountList = account.GetNonProspectRelatedAccounts();
             accountList.Insert(_view.Messages, 0, account);

             IRegent regent = _accRepo.GetRegent(account.Key, (int)SAHL.Common.Globals.RegentStatus.NewBusiness);
             if (regent != null)
                 accountList.Add(_view.Messages, regent);

             _view.selectedFirstRow = false;
             _view.BindFutureDatedDOGrid(_futureDatedChangeLst);
             _view.ShowInterestOnly = IsProspectIntOnly;
             _view.BindAccountSummaryGrid(accountList);
             _view.BindFixedDebitOrderData(account);
             CheckRules();

        }
        /// <summary>
        /// Set visibility of Controls on PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.ShowButtons = false;
            _view.ShowUpdateableControl = false;
        }

        public void CheckRules()
        {
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(_view.Messages, "NaedoDebitOrderPendingWarning", account);
        }

    }
}
