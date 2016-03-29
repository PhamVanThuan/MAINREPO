using System;
using System.Linq;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.CreditProtectionPlan.Presenters
{
    public class CreditProtectionCreate : SAHLCommonBasePresenter<IWorkFlowConfirm>
    {
        private CBOMenuNode _node;
        private int _PersonalLoanAccountKey;

        public CreditProtectionCreate(IWorkFlowConfirm view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _PersonalLoanAccountKey = _node.GenericKey;

            base._view.OnYesButtonClicked += new EventHandler(_view_OnYesButtonClicked);
            base._view.OnNoButtonClicked += new EventHandler(_view_OnNoButtonClicked);

            base._view.ShowControls(true);

            _view.TitleText = "Confirm Create Credit Life Policy ";
        }

        #region Event Handlers

        /// <summary>
        /// Handles No button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnNoButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("No");
        }

        /// <summary>
        /// Handles Yes button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnYesButtonClicked(object sender, EventArgs e)
        {
            try
            {
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(_view.Messages, "SAHLCreditProtectionPlanExists", _PersonalLoanAccountKey);
                if (_view.IsValid)
                {
                    using (var transaction = new TransactionScope(OnDispose.Rollback))
                    {
                        ILifeRepository lifeRepository = RepositoryFactory.GetRepository<ILifeRepository>();
                        lifeRepository.CreateCreditLifeForPersonalLoan(_PersonalLoanAccountKey, _view.CurrentPrincipal.Identity.Name);

                        if (_view.IsValid)
                        {
                            transaction.VoteCommit();

                            CBOManager.RefreshCBOMenuNodeByURL(_view.CurrentPrincipal, "ClientDetails");
                            CBOMenuNode personalLoansNode = CBOManager.GetCBOMenuNodeByUrl(_view.CurrentPrincipal, "UnsecuredLoanSummary", SAHL.Common.CBONodeSetType.CBO);
                            CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, personalLoansNode, SAHL.Common.CBONodeSetType.CBO);
                            _view.Navigator.Navigate("Yes");
                        }
                    }
                }
            }
            catch
            {
                if (_view.IsValid)
                    throw;
            }
        }

        #endregion Event Handlers
    }
}