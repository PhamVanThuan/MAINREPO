using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LoanCalculator : SAHLCommonBasePresenter<ILoanCalculator>
    {
        private CBONode _cboNode;
        private Int32 _accountKey;
        private IAccount _account;
        private IControlRepository _controlRepo;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanCalculator(ILoanCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);//, CBONodeSetType.CBO);
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _view.OnAmortisationScheduleClicked += new EventHandler(_view_OnAmortisationScheduleClicked);
        }

        private void _view_OnAmortisationScheduleClicked(object sender, EventArgs e)
        {
            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();

            GlobalCacheData.Remove(ViewConstants.AmortisationSchedule);
            GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.AmortisationSchedule, _view.CalcDict, lifeTimes);
            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, lifeTimes);

            Navigator.Navigate("AmortisationSchedule");
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            double capitalisedLife = 0;
            double splitV = 1;
            double splitF = 0;

            _controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            // Get the Account Object
            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _account = accountRepo.GetAccountByKey(_accountKey);

            if (_account.Product.Key == (int)Products.VariFixLoan || _account.Product.Key == (int)Products.SuperLo)
                _view.ShowLoanPercSplit = true;

            List<BindableFinancialService> ListFS = new List<BindableFinancialService>();

            //Get the variable ML
            IMortgageLoanAccount mla = _account as IMortgageLoanAccount;
            if (mla == null)
            {
                _view.Messages.Add(new Error("This is not a Loan Account.", "This is not a Loan Account."));
                return;
            }

            //Get any capitalised life
            capitalisedLife = mla.CapitalisedLife;
            //Get the variable ML
            IMortgageLoan _vML = mla.SecuredMortgageLoan;
            IMortgageLoan _fML = null;

            //Get the Fixed ML
            if ((_account as IAccountVariFixLoan) != null)
            {
                IAccountVariFixLoan _fAccount = _account as IAccountVariFixLoan;
                _fML = _fAccount.FixedSecuredMortgageLoan;
                // and calculate current split %
                // with dodgy split payments balances can end up being negative
                var fBal = _fML.CurrentBalance > 0 ? _fML.CurrentBalance : 0; 
                var vBal = _vML.CurrentBalance > 0 ? _vML.CurrentBalance : 0;
                //prevent divide by 0's
                if ((vBal + fBal) == 0) vBal = 1;

                splitV = Math.Round((vBal / (vBal + fBal)), 2);
                splitF = 1 - splitV;

                ListFS.Add(new BindableFinancialService(_fML, splitF, 0));
            }

            ListFS.Add(new BindableFinancialService(_vML, splitV, capitalisedLife));

            //set control table values
            IControl ctrl = _controlRepo.GetControlByDescription("BondHigh");
            _view.MaxBondAmount = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0);

            _view.BindFinancialServiceGrid(ListFS);

            _view.BindLinkRates(_vML.GetLoanAttributeBasedMargins());

            _view.BindMortgageLoans(_vML, _fML, splitV, capitalisedLife);

            _view.ChangedCalcType();
        }
    }
}