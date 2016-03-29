using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationInOrder : SAHLCommonBasePresenter<IWorkFlowConfirm>
    {
        CBOMenuNode _node;
        int _applicationKey = 0;

        public ApplicationInOrder(IWorkFlowConfirm view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) 
                return;

            base._view.OnYesButtonClicked += new EventHandler(_view_OnYesButtonClicked);
            base._view.OnNoButtonClicked += new EventHandler(_view_OnNoButtonClicked);

            base._view.ShowControls(true);

            IX2Info info = X2Service.GetX2Info(_view.CurrentPrincipal);

            if (info != null && !String.IsNullOrEmpty(info.ActivityName))
                _view.TitleText = String.Format("Confirm {0}", info.ActivityName);
            else
                _view.TitleText = "Confirm Activity";

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null && _node.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                _applicationKey = Convert.ToInt32(_node.GenericKey);
        }

        void _view_OnNoButtonClicked(object sender, EventArgs e)
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        void _view_OnYesButtonClicked(object sender, EventArgs e)
        {
            bool ret = false;

            if (_applicationKey > 0)
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                TransactionScope txn = new TransactionScope();
                try
                {
                    ret = appRepo.IsApplicationInOrder(_applicationKey);
                    if (ret)
                        txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                    {
                        this.X2Service.CancelActivity(_view.CurrentPrincipal);
                        throw;
                    }
                }
                finally
                {
                    txn.Dispose();
                }

                if (_view.IsValid && ret)
                {
                    try
                    {
                        SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                        this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                        this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                    }
                    catch (Exception)
                    {
                        // we must cancel the activity here, otherwise if the user navigates to another node and 
                        // tries to perform a workflow action, X2 may try to perform the action on the wrong 
                        // activity
                        if (_view.IsValid)
                        {
                            this.X2Service.CancelActivity(_view.CurrentPrincipal);
                            throw;
                        }
                    }
                }
            }
        }
    }
}