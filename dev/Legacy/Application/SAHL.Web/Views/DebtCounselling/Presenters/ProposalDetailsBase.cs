using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class ProposalDetailsBase : SAHLCommonBasePresenter<IProposalDetails>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;

        private IDebtCounselling _debtCounselling;

        public IDebtCounselling DebtCounselling
        {
            get { return _debtCounselling; }
            set { _debtCounselling = value; }
        }

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

        /// <summary>
        ///
        /// </summary>
        private IProposal _selectedProposal;

        public IProposal SelectedProposal
        {
            get
            {
                return _selectedProposal;
            }

            set
            {
                _selectedProposal = value;

                //bit of a nasty hack setting this here....
                //this is to avoid having to duplicate some severe web.config uip configuration
                if (_selectedProposal.ProposalType != null)
                    _view.ShowProposals = (ProposalTypeDisplays)_selectedProposal.ProposalType.Key;
            }
        }

        /// <summary>
        /// Gets the <see cref="IADUser"/> for the current principal.
        /// </summary>
        protected IADUser CurrentADUser
        {
            get
            {
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                return secRepo.GetADUserByPrincipal(_view.CurrentPrincipal); ;
            }
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

        private IAccountRepository _accountRepo;

        public IAccountRepository AccountRepo
        {
            get
            {
                if (_accountRepo == null)
                    _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accountRepo;
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

        private IHOCRepository _HOCRepository;

        public IHOCRepository HOCRepository
        {
            get
            {
                if (_HOCRepository == null)
                    _HOCRepository = RepositoryFactory.GetRepository<IHOCRepository>();

                return _HOCRepository;
            }
        }

        private IMemoRepository _memoRepository;

        public IMemoRepository MemoRepository
        {
            get
            {
                if (_memoRepository == null)
                {
                    _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
                }
                return _memoRepository;
            }
        }

        private IList<ICacheObjectLifeTime> _lifeTimes;

        public IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                    _lifeTimes = new List<ICacheObjectLifeTime>();

                return _lifeTimes;
            }
        }

        public ProposalDetailsBase(IProposalDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.BindMarketRates(GetMarketRatesToBind());

            IDictionary<int, string> inclusiveExclusive = new Dictionary<int, string>();
            inclusiveExclusive.Add(SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey, SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc);
            inclusiveExclusive.Add(SAHL.Common.Constants.Proposals.HOCLifeExclusiveKey, SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc);
            _view.BindHOCAndLife(inclusiveExclusive);

            _view.BindProposalHeader(_selectedProposal);

            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            BindProposal(_selectedProposal, false);

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                _instanceNode = _node as InstanceNode;
                _genericKey = _instanceNode.GenericKey;
                _genericKeyTypeKey = _instanceNode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;
            }

            DebtCounselling = DebtCounsellingRepo.GetDebtCounsellingByKey(_genericKey);

            double hocMonthlyPremium, lifePolicyMonthlyPremium, lifeBalance;
            GetLifeAndHOCPremiums(DebtCounselling.Account, out hocMonthlyPremium, out lifePolicyMonthlyPremium, out lifeBalance);

            _view.BindAccountSummary(DebtCounselling.Account, hocMonthlyPremium, lifePolicyMonthlyPremium, lifeBalance);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.RenderProposalGraph(_selectedProposal.Key);

            IDebtCounsellingRepository DebtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            IControlRepository controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
            if (_selectedProposal.Key > 0)
            {
                _view.RenderAmortisationSchedule(_selectedProposal.Key);
            }
            if (_view.ShowProposals == ProposalTypeDisplays.CounterProposal)
            {
                List<IProposal> proposals = DebtCounsellingRepo.GetProposalsByType(DebtCounselling.Key, ProposalTypes.Proposal);
                IProposal activeProposal = proposals.Find((possibleProposal) => { return possibleProposal.ProposalStatus.Key == (int)ProposalStatuses.Active; });
                if (activeProposal != null)
                {
                    _view.RenderCounterProposalGraph(activeProposal.Key);
                    _view.SetProposalRemainingTerm(activeProposal);
                }
            }
            else
            {
                List<IProposal> proposals = DebtCounsellingRepo.GetProposalsByType(DebtCounselling.Key, ProposalTypes.CounterProposal);
                IProposal activeProposal = proposals.Find((possibleProposal) => { return possibleProposal.ProposalStatus.Key == (int)ProposalStatuses.Active; });
                if (activeProposal != null)
                {
                    _view.RenderCounterProposalGraph(activeProposal.Key);
                    _view.SetProposalRemainingTerm(activeProposal);
                }
            }
        }

        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ProposalKey);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }

        protected void BindProposal(IProposal proposal, bool rebindHeader)
        {
            if (rebindHeader)
                _view.BindProposalHeader(proposal);

            if (_view.ShowProposals == ProposalTypeDisplays.CounterProposal && proposal.Memo != null)
            {
                _view.CounterProposalNote = proposal.Memo.Description;
            }

            _view.BindProposalItemsGrid(proposal.ProposalItems);
        }

        private static IDictionary<int, string> GetMarketRatesToBind()
        {
            IDictionary<int, string> marketRates = new Dictionary<int, string>();

            marketRates.Add(SAHL.Common.Constants.Proposals.FixedMarketRateKey, SAHL.Common.Constants.Proposals.FixedMarketRateDesc);

            return marketRates;
        }

        internal void GetLifeAndHOCPremiums(IAccount account, out double hocMonthlyPremium, out double lifePolicyMonthlyPremium, out double lifeBalance)
        {
            lifePolicyMonthlyPremium = 0D;
            hocMonthlyPremium = 0D;
            lifeBalance = 0D;

            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;

            if (mortgageLoanAccount != null)
            {
                if (mortgageLoanAccount.LifePolicyAccount != null)
                {
                    if (mortgageLoanAccount.LifePolicyAccount.AccountStatus.Key == (int)GeneralStatuses.Active)
                    {
                        lifePolicyMonthlyPremium = LifeRepository.GetMonthlyPremium(mortgageLoanAccount.LifePolicyAccount.Key);
                        lifeBalance = mortgageLoanAccount.LifePolicyAccount.LifePolicy.FinancialService.Balance.Amount;
                    }
                }

                if (mortgageLoanAccount.HOCAccount != null)
                {
                    if (mortgageLoanAccount.HOCAccount.AccountStatus.Key == (int)GeneralStatuses.Active)
                    {
                        hocMonthlyPremium = HOCRepository.GetMonthlyPremium(mortgageLoanAccount.HOCAccount.Key);
                    }
                }
            }
        }

        internal double GetPostDebtCounsellingMortgageLoanInstallment(int _debtCounsellingKey, IMortgageLoan mortgageLoan)
        {
            double postDCInstalment = 0, linkRate = 0, marketRate = 0, interestRate = 0;
            int term = 0;
            
            DebtCounsellingRepo.GetPostDebtCounsellingMortgageLoanInstallment(_debtCounsellingKey, out postDCInstalment, out linkRate, out marketRate, out interestRate, out term);
            if (postDCInstalment == 0)
            {
                postDCInstalment = mortgageLoan.Payment;
            }
            return postDCInstalment;
        }
    }
}