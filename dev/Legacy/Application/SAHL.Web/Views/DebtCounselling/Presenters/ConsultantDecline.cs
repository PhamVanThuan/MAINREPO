using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Web.Views.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Data;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class ConsultantDecline : SAHLCommonBasePresenter<IConsultantDecline>
    {
        protected CBOMenuNode _node;
        protected int _selectedProposalKey;
        protected IDebtCounsellingRepository _dcRepo;
        protected X2ServiceResponse x2Response = null;
        protected List<IProposal> _proposals = new List<IProposal>();
        private int _genericKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConsultantDecline(IConsultantDecline view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) return;

            // Get the Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                InstanceNode instanceNode = _node as InstanceNode;
                _genericKey = instanceNode.GenericKey; // this will be the debtcounsellingkey
            }
            else
            {
                _genericKey = _node.GenericKey;
            }


            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            // get debtcounselling object
            IDebtCounselling debtCounselling = _dcRepo.GetDebtCounsellingByKey(_genericKey);
            if (debtCounselling != null)
            {
                foreach (IProposal proposal in debtCounselling.Proposals)
                {
                    // we do not want counter proposals or inactive propsals
                    if (proposal.ProposalType.Key==(int)ProposalTypes.Proposal && proposal.ProposalStatus.Key != (int)ProposalStatuses.Inactive)
                    {
                        _proposals.Add(proposal);
                    }
                }
            }
    
            // bind proposals
            _view.BindProposals(_proposals);

            // set button visibility
            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.CancelButtonVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            //Get the selected proposal
            _selectedProposalKey = _view.SelectedProposalKey;

            // perform screen validation
            if (ValidateScreenInput() == false)
                return;

            // navigate to the common reasons screen
            if (_view.IsValid)
            {
                var objectLifeTime = new List<ICacheObjectLifeTime>();
                GlobalCacheData.Add(ViewConstants.GenericKey, _selectedProposalKey, objectLifeTime);
                GlobalCacheData.Add(ViewConstants.CancelView, _view.ViewName, objectLifeTime);
                Navigator.Navigate("Submit");
            }
        }

        private bool ValidateScreenInput()
        {
            bool valid = true;
            string errorMessage = "";

            if (_selectedProposalKey <= 0)
            {
                errorMessage = "Must select a proposal.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            return valid;
        }

        public void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}