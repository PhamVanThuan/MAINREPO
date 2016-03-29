using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.ActiveRecord;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DebtCounsellingDetailsUpdate : DebtCounsellingDetailsBase
    {
         /// <summary>
		/// Constructor for CourtDetailsAdd
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public DebtCounsellingDetailsUpdate(IDebtCounsellingDetails view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}


          /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // call the base presenter
            base.OnViewInitialised(sender, e);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
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

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
              if (_view.IsValid == false)
                return;

            TransactionScope txn = new TransactionScope();
            try
            {
                _debtCounselling.ReferenceNumber = _view.ReferenceNumber;
                DebtCounsellingRepo.SaveDebtCounselling(_debtCounselling);

                txn.VoteCommit();

                // comnplete activity and navigate
                CompleteActivityAndNavigate();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
          
        }

        private void CancelActivity()
        {
            if (!NavigateNoWorkFlow())
            {
                base.X2Service.CancelActivity(_view.CurrentPrincipal);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        private void CompleteActivityAndNavigate()
        {
            if (!NavigateNoWorkFlow())
            {
                base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        /// <summary>
        /// If there is a cache view to navigate to, navigate there
        /// This means we are not in WorkFlow, so no Cancel or Complete activity should be called
        /// </summary>
        /// <returns></returns>
        private bool NavigateNoWorkFlow()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigate = GlobalCacheData[ViewConstants.NavigateTo] as string;
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                Navigator.Navigate(navigate);
                return true;
            }
            return false;
        }
    }
}