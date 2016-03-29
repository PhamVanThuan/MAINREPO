using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PreDebtCounsellingAccountDetails : SAHLCommonBasePresenter<IPreDebtCounsellingAccountDetails>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;
        protected IDebtCounselling _debtCounselling;

        private int _debtCounsellingKey;
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

        private IHOCRepository _hocRepository;
        public IHOCRepository HOCRepository
        {
            get
            {
                if (_hocRepository == null)
                    _hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();

                return _hocRepository;
            }
        }

        private ILifeRepository _lifeRepository;
        public ILifeRepository LifeRepository
        {
            get
            {
                if (_lifeRepository == null)
                    _lifeRepository = RepositoryFactory.GetRepository<ILifeRepository>();

                return _lifeRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PreDebtCounsellingAccountDetails(IPreDebtCounsellingAccountDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

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

            // Get the Instance Node   
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

            switch (GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                    //Generic Key is Debt Counselling Key
                    _debtCounsellingKey = GenericKey;
                    break;
                default:
                    break;
            }

            if (_debtCounsellingKey > 0)
                _debtCounselling = DebtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);

            var accountSnapShot = DebtCounsellingRepo.GetDebtCounsellingSnapShot(_debtCounselling.Key);

            double preDCInstalment = 0, linkRate = 0, marketRate = 0, interestRate = 0;
            int term = 0;

            if (_debtCounselling.DebtCounsellingStatus.Key == (int)DebtCounsellingStatuses.Open)
            {
                DebtCounsellingRepo.GetPostDebtCounsellingMortgageLoanInstallment(_debtCounsellingKey, out preDCInstalment, out linkRate, out marketRate, out interestRate, out term);
                _view.Set_DebtCounsellingCancelledHeading_InnerText = "If Debt Counselling was to be cancelled today " + DateTime.Now.ToString(SAHL.Common.Constants.DateFormat);
                _view.Set_DebtCounsellingInfo_Visibility = true;
            }
            else
            {
                _view.Set_DebtCounsellingCancelledHeading_InnerText = "This account is not actively in debt counselling.";
                _view.Set_DebtCounsellingInfo_Visibility = false;
            }

            double lifePolicyInstallment = 0;
            double hocPolicyInstallment = 0;

            IMortgageLoanAccount mortgageLoanAccount = null;
            if (accountSnapShot != null && accountSnapShot.Account != null)
                mortgageLoanAccount = accountSnapShot.Account as IMortgageLoanAccount;

            if (mortgageLoanAccount != null)
            {
                if (mortgageLoanAccount.LifePolicyAccount != null)
                {
                    if (mortgageLoanAccount.LifePolicyAccount.AccountStatus.Key == (int)GeneralStatuses.Active)
                    {
                        lifePolicyInstallment = LifeRepository.GetMonthlyPremium(mortgageLoanAccount.LifePolicyAccount.Key);
                    }
                }

                if (mortgageLoanAccount.HOCAccount != null)
                {
                    if (mortgageLoanAccount.HOCAccount.AccountStatus.Key == (int)GeneralStatuses.Active)
                    {
                        hocPolicyInstallment = HOCRepository.GetMonthlyPremium(mortgageLoanAccount.HOCAccount.Key);
                    }
                }
            }

            _view.BindSnapShotAccount(accountSnapShot, preDCInstalment, linkRate, marketRate, interestRate, term, lifePolicyInstallment, hocPolicyInstallment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }
    }
}