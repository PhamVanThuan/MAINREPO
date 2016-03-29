using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Recoveries.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Recoveries.Presenters
{
    public class RecoveriesProposalBase : SAHLCommonBasePresenter<SAHL.Web.Views.Recoveries.Interfaces.IRecoveriesProposal>
    {
        private CBOMenuNode _node;
        public InstanceNode _instanceNode;
        private int _debtCounsellingKey, _genericKeyTypeKey;
        private IDebtCounsellingRepository _debtCounsellingRepo;
        protected IRecoveriesRepository _recoveriesRepo;
        protected IDebtCounselling _debtCounselling;
        protected ILookupRepository _lookups;
        protected IADUser _adUser;
        protected int _accountKey;
        protected List<SAHL.Common.BusinessModel.Interfaces.IRecoveriesProposal> proposalList;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RecoveriesProposalBase(SAHL.Web.Views.Recoveries.Interfaces.IRecoveriesProposal view, SAHLCommonBaseController controller)
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

            _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            _recoveriesRepo = RepositoryFactory.GetRepository<IRecoveriesRepository>();
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            _view.AddPanelVisible = false;
            _view.AddButtonVisible = false;
            _view.CancelButtonVisible = false;

            // get the cbo node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            //set the debt counsellingy key
            if (_node is InstanceNode)
            {
                _instanceNode = _node as InstanceNode;
                _genericKeyTypeKey = _instanceNode.GenericKeyTypeKey;

                // Get values from the Debt Counselling Data 
                _debtCounsellingKey = Convert.ToInt32(_instanceNode.X2Data["DebtCounsellingKey"]);
            }
            else
            {
                _genericKeyTypeKey = _node.GenericKeyTypeKey;

                switch (_genericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                        _debtCounsellingKey = _node.GenericKey;
                        break;
                    default:
                        break;
                }
            }

            if (_debtCounsellingKey > 0)
            {
                _debtCounselling = _debtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);
                if (_debtCounselling != null)
                {
                    if (_debtCounselling.Account != null)
                    {
                        _accountKey = _debtCounselling.Account.Key;
                        BindRecoveriesProposals(_accountKey);
                    }
                }
            }
        }


        protected void BindRecoveriesProposals(int AccountKey)
        {
             proposalList = _recoveriesRepo.GetRecoveriesProposalsByAccountKey(AccountKey);
            _view.BindRecoveriesProposals(proposalList);
        }

        /// <summary>
        /// Gets the <see cref="IADUser"/> for the current principal.
        /// </summary>
        protected IADUser CurrentADUser
        {
            get
            {
                if (_adUser == null)
                {
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    _adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);
                }
                return _adUser;
            }
        }
    }
}