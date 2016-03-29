using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class ChangeOfCircumstance : SAHLCommonBasePresenter<IChangeOfCircumstance>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;

        private int _genericKey;
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        private int _genericKeyTypeKey;
        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }

        private ILookupRepository _lookupRepo;
        public ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        /// <summary>
        /// Constructor for ChangeOfCircumstance
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ChangeOfCircumstance(IChangeOfCircumstance view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            // Get the Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                InstanceNode instanceNode = _node as InstanceNode;
                _genericKey = instanceNode.GenericKey; // this will be the debtcounsellingkey
                _genericKeyTypeKey = instanceNode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;
            }
        }

        public void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        public void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                SDRepo.SaveStageTransition(_genericKey, StageDefinitionStageDefinitionGroups.Received17pt3, _view.Date.Value, _view.Comment, _view.CurrentPrincipal.Identity.Name);

                X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);

                if (!rsp.IsError && _view.IsValid)
                {
                    base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                    txn.VoteCommit();
                }
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

        private IStageDefinitionRepository _sdRepo;
        public IStageDefinitionRepository SDRepo
        {
            get
            {
                if (_sdRepo == null)
                    _sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                return _sdRepo;
            }
        }

        private IDebtCounsellingRepository _debtCounsellingRepo;
        public IDebtCounsellingRepository DebtCounsellingRepo
        {
            get
            {
                if (_debtCounsellingRepo == null)
                    _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _debtCounsellingRepo;
            }
        }
    }
}