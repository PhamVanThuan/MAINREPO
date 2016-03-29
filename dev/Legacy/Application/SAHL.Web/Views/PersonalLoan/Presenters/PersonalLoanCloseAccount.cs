using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanCloseAccount : SAHLCommonBasePresenter<IWorkFlowConfirm>
    {
        public PersonalLoanCloseAccount(IWorkFlowConfirm view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        private CBOMenuNode _node;
        private int _AccountKey;

        #region Events

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _AccountKey = _node.GenericKey;

            base._view.OnYesButtonClicked += new EventHandler(_view_OnYesButtonClicked);
            base._view.OnNoButtonClicked += new EventHandler(_view_OnNoButtonClicked);

            base._view.ShowControls(true);

            _view.TitleText = "Confirm Close Personal Loan Account";
        }

        #endregion Events

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
            using (var transaction = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                    accRepository.ClosePersonalLoanAccount(_AccountKey, _view.CurrentPrincipal.Identity.Name);
                    transaction.VoteCommit();
                }
                catch (Exception)
                {
                    transaction.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }

            if (_view.IsValid)
            {
                CBOManager.RefreshCBOMenuNodeByURL(_view.CurrentPrincipal, "ClientDetails");
                CBOMenuNode personalLoansNode = CBOManager.GetCBOMenuNodeByUrl(_view.CurrentPrincipal, "UnsecuredLoanSummary", SAHL.Common.CBONodeSetType.CBO);
                CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, personalLoansNode, SAHL.Common.CBONodeSetType.CBO);
                _view.Navigator.Navigate("UnsecuredLoanSummary");
            }
        }

        #endregion Event Handlers
    }
}