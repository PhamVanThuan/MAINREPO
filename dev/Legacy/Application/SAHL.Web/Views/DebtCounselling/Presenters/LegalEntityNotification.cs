using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class LegalEntityNotification : SAHLCommonBasePresenter<ILegalEntityNotification>
    {
        #region Properties

        protected IX2Service x2Svc = ServiceFactory.GetService<IX2Service>();
        protected IMessageService msgSvc = ServiceFactory.GetService<IMessageService>();
        protected SAHLPrincipalCache spc;
        protected IList<BindableLEReasonRole> leList = new List<BindableLEReasonRole>();

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

        public CBOMenuNode _node;
        public InstanceNode _instanceNode;

        private IDebtCounselling _debtCounselling;
        public IDebtCounselling DebtCounselling
        {
            get { return _debtCounselling; }
            set { _debtCounselling = value; }
        }

        private IDebtCounsellingRepository _dcRepo;
        public IDebtCounsellingRepository DCRepo
        {
            get
            {
                if (_dcRepo == null)
                    _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _dcRepo;
            }
        }

        private IReasonRepository _rRepo;
        public IReasonRepository ReasonRepo
        {
            get
            {
                if (_rRepo == null)
                    _rRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _rRepo;
            }
        }

        private ICommonRepository _cRepo;
        public ICommonRepository CRepo
        {
            get
            {
                if (_cRepo == null)
                    _cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

                return _cRepo;
            }
        }

        private IX2Repository _x2Repo;
        public IX2Repository X2Repo
        {
            get
            {
                if (_x2Repo == null)
                    _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

                return _x2Repo;
            }
        }

        #endregion

        /// <summary>
        /// Constructor for LegalEntityNotification
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityNotification(ILegalEntityNotification view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                _instanceNode = _node as InstanceNode;
                _genericKey = _instanceNode.GenericKey; // this will be the debtcounsellingkey
                _genericKeyTypeKey = _instanceNode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;
            }

            DebtCounselling = DCRepo.GetDebtCounsellingByKey(_genericKey);

            List<int> les = new List<int>();
            foreach (IRole r in DebtCounselling.Account.Roles)
            {
                les.Add(r.LegalEntity.Key);
            }

            IReadOnlyEventList<IReason> reasons = ReasonRepo.GetReasonByGenericKeyListAndReasonTypeGroupKey(les, (int)ReasonTypeGroups.LegalEntity);

            //build up a list of items to bind to the grid.
            foreach (IRole r in DebtCounselling.Account.Roles)
            {
                bool d = false;
                bool s = false;
                foreach (IReason rn in reasons)
                {
                    if (rn.GenericKey == r.LegalEntity.Key)
                    {
                        switch (rn.ReasonDefinition.ReasonDescription.Key)
                        {
                            case (int)ReasonDescriptions.NotificationofDeath:
                                d = true;
                                break;
                            case (int)ReasonDescriptions.NotificationofSequestration:
                                s = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                leList.Add(new BindableLEReasonRole(r, d, s));
            }
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.BindGrid(leList);
        }

        protected void CompleteAndNavigate()
        {
            x2Svc.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
            x2Svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            x2Svc.CancelActivity(_view.CurrentPrincipal);
            x2Svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        protected IADUser AssignedDCUser(out string workflowRoleTypeDescription)
        {
            //loock for a court consultant
            IADUser adu = DCRepo.GetActiveDebtCounsellingUser(GenericKey, WorkflowRoleTypes.DebtCounsellingCourtConsultantD);
            workflowRoleTypeDescription = "Debt Counselling Court Consultant";
            
            //no court consultant, try consultant
            if (adu == null)
            {
                adu = DCRepo.GetActiveDebtCounsellingUser(GenericKey, WorkflowRoleTypes.DebtCounsellingConsultantD);
                workflowRoleTypeDescription = "Debt Counselling Consultant";
            }

            //no consultant, try admin
            if (adu == null)
            {
                adu = DCRepo.GetActiveDebtCounsellingUser(GenericKey, WorkflowRoleTypes.DebtCounsellingConsultantD);
                workflowRoleTypeDescription = "Debt Counselling Administrator";
            }

            return adu;
        }

    }
}