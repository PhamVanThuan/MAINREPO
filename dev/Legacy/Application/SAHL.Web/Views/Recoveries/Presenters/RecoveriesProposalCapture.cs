using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Recoveries.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Recoveries.Presenters
{
    public class RecoveriesProposalCapture : RecoveriesProposalBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RecoveriesProposalCapture(SAHL.Web.Views.Recoveries.Interfaces.IRecoveriesProposal view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnAddButtonClicked += new EventHandler(OnAddButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.AddPanelVisible = true;
            _view.AddButtonVisible = true;
            _view.CancelButtonVisible = true;
        }

        private void ValidateRecoveryProposal()
        {
            if (_view.ShortfallAmount <= 0)
            {
                string error = "Please enter a Shortfall Amount greater than 0.";
                _view.Messages.Add(new Error(error, error));
            }
            if (_view.RepaymentAmount <= 0)
            {
                string error = "Please enter a Repayment Amount greater than 0.";
                _view.Messages.Add(new Error(error, error));
            }
            if (_view.StartDate == null)
            {
                string error = "Please enter a Start Date.";
                _view.Messages.Add(new Error(error, error));
            }
        }

        /// <summary>
        /// On Add Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAddButtonClicked(object sender, EventArgs e)
        {
            ValidateRecoveryProposal();
            if (!View.IsValid)
            {
                return;
            }

            string message = string.Empty;
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            TransactionScope txn = new TransactionScope();
            try
            {

                //Set all current active recoveries proposals to inactive
                foreach (SAHL.Common.BusinessModel.Interfaces.IRecoveriesProposal currRecoveriesProposal in proposalList)
                {
                    IGeneralStatus inactiveGeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];
                    currRecoveriesProposal.GeneralStatus = inactiveGeneralStatus;

                    _recoveriesRepo.SaveRecoveriesProposal(currRecoveriesProposal);
                }

                //Save the new recovery proposal
                SAHL.Common.BusinessModel.Interfaces.IRecoveriesProposal recoveriesProposal = _recoveriesRepo.CreateEmptyRecoveriesProposal();
                recoveriesProposal.ShortfallAmount = _view.ShortfallAmount;
                recoveriesProposal.RepaymentAmount = _view.RepaymentAmount;
                recoveriesProposal.StartDate = Convert.ToDateTime(_view.StartDate);
                recoveriesProposal.AcknowledgementOfDebt = _view.AOD;
                recoveriesProposal.Account = _debtCounselling.Account;
                recoveriesProposal.CreateDate = DateTime.Now;
                IGeneralStatus activeGeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Active];
                recoveriesProposal.GeneralStatus = activeGeneralStatus;
                recoveriesProposal.ADUser = CurrentADUser;

                _recoveriesRepo.SaveRecoveriesProposal(recoveriesProposal);

                txn.VoteCommit();

                //Rebind the data
                BindRecoveriesProposals(_accountKey);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                {
                    svc.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }
            finally
            {
                txn.Dispose();
            }

            if (_view.IsValid)
            {
                svc.CompleteActivity(_view.CurrentPrincipal, null, false, message);
                svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        /// <summary>
        /// On Cancel Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            svc.CancelActivity(_view.CurrentPrincipal);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}