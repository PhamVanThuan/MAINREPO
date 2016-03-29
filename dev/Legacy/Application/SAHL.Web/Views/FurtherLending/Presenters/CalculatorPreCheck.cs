using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.FurtherLending.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class CalculatorPreCheck : SAHLCommonBasePresenter<ICalculatorPreCheck>
    {
        protected int _accountKey = -1;
        private CBONode _cboNode;
        private InstanceNode _instanceNode;
        private IApplicationRepository _applicationRepo;
        protected IApplication _app;
        protected IAccount _account;
        private IAccountRepository _accountRepo;
        private IList<IFinancialAdjustment> _financialAdjustments;

        private IApplicationRepository ApplicationRepo
        {
            get
            {
                if (_applicationRepo == null)
                    _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicationRepo;
            }
        }

        protected IAccountRepository AccountRepo
        {
            get
            {
                if (_accountRepo == null)
                    _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accountRepo;
            }
        }

        public CalculatorPreCheck(ICalculatorPreCheck view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnCancelButtonClicked += CancelButtonClicked;
            _view.OnNextButtonClicked += NextButtonClicked;

            if (!_view.ShouldRunPage) return;

            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);

            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            #region GetAccount

            // Get the AccountKey from the CBO
            if (_cboNode is InstanceNode)
            {
                _instanceNode = _cboNode as InstanceNode;
                _app = ApplicationRepo.GetApplicationByKey(Convert.ToInt32(_instanceNode.GenericKey));
                _accountKey = _app.Account.Key;
            }
            else
            {
                _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            }

            // Get the Account Object
            _account = AccountRepo.GetAccountByKey(_accountKey);

            //Make sure we have a Valid account
            if ((_account as IMortgageLoanAccount) == null)
            {
                throw new Exception("Not a Mortgage Loan Account!");
            }

            #endregion GetAccount

            #region Financial Service

            _financialAdjustments = new List<IFinancialAdjustment>();
            // Get the list of Rate Overrides for the parent account
            foreach (IFinancialService fs in _account.FinancialServices)
            {
                switch (fs.Account.AccountStatus.Key)
                {
                    case (int)SAHL.Common.Globals.AccountStatuses.Open:
                    case (int)SAHL.Common.Globals.AccountStatuses.Locked:
                    case (int)SAHL.Common.Globals.AccountStatuses.Dormant:
                        foreach (IFinancialAdjustment fa in fs.FinancialAdjustments)
                        {
                            _financialAdjustments.Add(fa);
                        }
                        break;

                    default:
                        break;
                }
            }

            // Sort the FADJ's
            _financialAdjustments = _financialAdjustments.OrderBy(x => x.FinancialAdjustmentStatus.Key).ThenBy(y => y.FromDate).ToList<IFinancialAdjustment>();

            // Bind the FADJ's

            #endregion Financial Service

            //Run rule checks: potentially no application e.g.: for create
            // so test directly on Account.
            IRuleService rules = ServiceFactory.GetService<IRuleService>();
            List<string> rulesToRun = new List<string>();
            //Check for any detail types
            rulesToRun.Add("AccountDetailTypeCheck");
            rulesToRun.Add("AccountDetailTypeWarning");
            rulesToRun.Add("ActiveSubsidyAndSalaryStopOrderConditionExistsWarning");
            rulesToRun.Add("LoanHas30YearTermAndRemainingInstalmentsCheck");

            rules.ExecuteRuleSet(_view.Messages, rulesToRun, new object[] { _account });
            _view.BindDisplay(_account, _financialAdjustments);
            _view.NextButtonVisible = !_view.Messages.HasErrorMessages;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
        }

        private void NextButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Next");
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }
    }
}