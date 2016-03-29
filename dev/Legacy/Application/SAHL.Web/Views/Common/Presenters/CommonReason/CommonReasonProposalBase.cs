using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{

    public class CommonReasonProposalBase : CommonReasonBase
    {
        private IProposal _activeProposal;
        private IDebtCounsellingRepository _debtCounsellingRepo;
        private IDebtCounselling _debtCounselling;

        protected IDebtCounsellingRepository DebtCounsellingRepo
        {
            get { return _debtCounsellingRepo; }
            set { _debtCounsellingRepo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IProposal ActiveProposal
        {
            get { return _activeProposal; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDebtCounselling DebtCounselling
        {
            get { return _debtCounselling; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonProposalBase(ICommonReason view, SAHLCommonBaseController controller)
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

            if (GlobalCacheData.ContainsKey(ViewConstants.GenericKey))
            {
                base.GenericKey = int.Parse(GlobalCacheData[ViewConstants.GenericKey].ToString());
            }
            else
            {
                // get the instance node
                InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;

                _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // get the debtcounsellingkey
                int debtCounsellingKey = node.GenericKey;
                // get debtcounselling object
                _debtCounselling = _debtCounsellingRepo.GetDebtCounsellingByKey(debtCounsellingKey);
                // get the 'Active' proposal
                _activeProposal = _debtCounselling.GetActiveProposal(ProposalTypes.Proposal);
                // set generic key to the active proposal key
                base.GenericKey = _activeProposal.Key;
            }
        }

        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // write away the reason data
            base._view_OnSubmitButtonClicked(sender, e);

            //_insertedReasonKey = base.InsertedReasonKeys[0];

            if (_view.Messages.Count > 0)
                return;

            CompleteActivityAndNavigate();
        }

        public override void CancelActivity()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.CancelView))
            {
                Navigator.Navigate(GlobalCacheData[ViewConstants.CancelView].ToString());
                GlobalCacheData.Remove(ViewConstants.CancelView);
            }
            else
            {
                base.X2Service.CancelActivity(_view.CurrentPrincipal);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        public override void CompleteActivityAndNavigate()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
            if (rsp.IsError == false)
            {
                if (base.sdsdgKeys.Count > 0)
                {
                    // Update the reason with the StageTransitionKey
                    UpdateReasonsWithStageTransitionKey();
                }

                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }
    }
}