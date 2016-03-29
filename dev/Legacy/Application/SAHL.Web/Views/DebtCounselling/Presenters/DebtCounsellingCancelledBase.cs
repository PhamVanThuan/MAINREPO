using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DebtCounsellingCancelledBase : SAHLCommonBasePresenter<IDebtCounsellingCancelled>
    {
        protected CBOMenuNode _node;
        protected int _genericKey;
        protected int _genericKeyTypeKey;
        protected int _configuredSDSDGKey;
        protected int _selectedReasonDefinitionKey;
        protected IReasonDefinition _selectedReasonDefinition;
        protected int _insertedReasonKey;
        protected IReasonRepository _reasonRepo;
        protected IStageDefinitionRepository _stageDefinitionRepo;
        protected IDebtCounsellingRepository _dcRepo;
        protected SAHLPrincipalCache spc;
        protected X2ServiceResponse x2Response = null;

        public DebtCounsellingCancelledBase(IDebtCounsellingCancelled view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            //set the generic key
            if (_node != null)
            {
                _genericKey = int.Parse(_node.GenericKey.ToString());
                _genericKeyTypeKey = int.Parse(_node.GenericKeyTypeKey.ToString());
            }

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            int reasontypekey = Convert.ToInt32(_view.ViewAttributes["reasontypekey"]);

            _configuredSDSDGKey = Convert.ToInt32(_view.ViewAttributes["stagedefinitionstagedefinitionkey"]);

            IReadOnlyEventList<IReasonDefinition> lstReasons = _reasonRepo.GetReasonDefinitionsByReasonTypeKey(reasontypekey);
            Reasons = new Dictionary<int, string>();
            foreach (IReasonDefinition ird in lstReasons)
            {
                Reasons.Add(ird.Key, ird.ReasonDescription.Description);
            }

            //Bind Linked Accounts to Grid
            DataTable dtLinkedAccounts = _dcRepo.GetRelatedDebtCounsellingAccounts(_genericKey);
            _view.BindLinkedAccountsGrid(dtLinkedAccounts);
        }

        protected Dictionary<int, string> Reasons { get; private set; }

        protected virtual void BindReasons()
        {
            _view.BindReasons(Reasons);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.CancelButtonVisible = true;
        }

        protected virtual void OnSubmitButtonClicked(object sender, EventArgs e)
        {

        }

        protected void CompleteActivityAndNavigate()
        {
            SAHLPrincipalCache principal = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            // pass the reason definition for the selected reason to x2
            Dictionary<string, string> x2Data = new Dictionary<string, string>();
            x2Data.Add("LatestReasonDefinitionKey", _selectedReasonDefinitionKey.ToString());

            x2Response = base.X2Service.CompleteActivity(_view.CurrentPrincipal, x2Data, principal.IgnoreWarnings);

            // update the reasons with the stage transition key
            if (!x2Response.IsError)
            {
                IStageTransition stageTransition = _stageDefinitionRepo.GetLastStageTransitionByGenericKeyAndSDSDGKey(_genericKey, _genericKeyTypeKey, _configuredSDSDGKey);
                IReason reason = _reasonRepo.GetReasonByKey(_insertedReasonKey);
                if (reason != null)
                {
                    reason.StageTransition = stageTransition;
                    _reasonRepo.SaveReason(reason);
                }
            }

            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        private void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}