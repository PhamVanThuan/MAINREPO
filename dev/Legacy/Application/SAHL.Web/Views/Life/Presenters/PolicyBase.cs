using System;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Web.UI.Events;
using System.Linq;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PolicyBase : SAHLCommonBasePresenter<IPolicy>
    {
        private IAccountLifePolicy _accountLifePolicy;
        private SAHL.Common.BusinessModel.Interfaces.IMortgageLoanAccount _loanAccount;
        private IReadOnlyEventList<ILegalEntity> _lstLegalEntities;
        private IList<ICacheObjectLifeTime> _lifeTimes;
        private IMortgageLoan _mortgageLoanVariable;
        private IMortgageLoan _mortgageLoanFixed;
        private IHOC _hoc;
        private IAccountHOC _hocAccount;
        private ILifeRepository _lifeRepo;
        private IApplicationRepository _applicationRepo;
        private IAccountRepository _accountRepo;
        private IStageDefinitionRepository _stageDefinitionRepo;
        private ILookupRepository _lookupRepo;
        private ILegalEntityRepository _legalEntityRepo;
        private int _accountKey;
        private string _contactNumber;
        private int stageDefinitionGroupKey = -1;
        private IADUser _adUser;


        /// <summary>
        /// 
        /// </summary>
        public string ContactNumber
        {
            set { _contactNumber = value; }
        }
	
        /// <summary>
        /// 
        /// </summary>
        public IAccountLifePolicy AccountLifePolicy
        {
            get { return _accountLifePolicy; }
            set { _accountLifePolicy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyEventList<ILegalEntity> lstLegalEntities
        {
            get { return _lstLegalEntities; }
            set { _lstLegalEntities = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AccountKey
        {
            get { return _accountKey; }
            set { _accountKey = value; }
        }
	
        /// <summary>
        /// 
        /// </summary>
        protected IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                return _lifeTimes;
            }
            set
            {
                _lifeTimes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILifeRepository LifeRepo
        {
            get
            {
                if (_lifeRepo == null)
                    _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

                return _lifeRepo;
            }
            set
            {
                _lifeRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IApplicationRepository ApplicationRepo
        {
            get
            {
                if (_applicationRepo == null)
                    _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicationRepo;
            }
            set
            {
                _applicationRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IAccountRepository AccountRepo
        {
            get
            {
                if (_accountRepo == null)
                    _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                
                return _accountRepo;
            }
            set
            {
                _accountRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IStageDefinitionRepository StageDefinitionRepo
        {
            get
            {
                if (_stageDefinitionRepo == null)
                    _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                
                return _stageDefinitionRepo;
            }
            set
            {
                _stageDefinitionRepo = value;
            }
        }

        protected ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                if (_legalEntityRepo == null)
                    _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _legalEntityRepo;
            }
            set
            {
                _legalEntityRepo = value;
            }
        }

        protected ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
            set
            {
                _lookupRepo = value;
            }
        }

        /// <summary>
        /// Gets
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PolicyBase(IPolicy view, SAHLCommonBaseController controller)
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

            _lifeTimes = new List<ICacheObjectLifeTime>();

            GlobalCacheData.Remove(ViewConstants.NavigateTo);
            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, _lifeTimes);

            _view.OnAddLifeButtonClicked += new EventHandler(OnAddLifeButtonClicked);
            _view.OnRemoveLifeButtonClicked += new KeyChangedEventHandler(OnRemoveLifeButtonClicked);
            _view.OnPremiumCalculatorButtonClicked += new EventHandler(OnPremiumCalculatorButtonClicked);

            // load the data
            LoadData();

            // Check if Life is condition of loan
            bool lifeIsConditionOfLoan = LifeRepo.IsLifeConditionOfLoan(_loanAccount.Key);

            // Bind the Policy Details
            _view.BindPolicyDetails(_accountLifePolicy, _loanAccount, _mortgageLoanVariable, _mortgageLoanFixed, _hoc, lifeIsConditionOfLoan, _contactNumber);

            // Bind Assured lives
            _lstLegalEntities = _accountLifePolicy.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife });

            _view.BindAssuredLivesGrid(_lstLegalEntities);

            // Bind Contact Person
            int contactPersonKey = -1;
            if (_accountLifePolicy.LifePolicy == null)
                contactPersonKey = _accountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity == null ? -1 : _accountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity.Key;
            else
                contactPersonKey = _accountLifePolicy.LifePolicy.PolicyHolderLE == null ? -1 : _accountLifePolicy.LifePolicy.PolicyHolderLE.Key;

            _view.BindContactPersons(_lstLegalEntities, contactPersonKey);
        }

        private void LoadData()
        {
            // Get the Life Policy Account object
            _accountLifePolicy = AccountRepo.GetAccountByKey(_accountKey) as IAccountLifePolicy;

            if (_accountLifePolicy.LifePolicy == null)
                stageDefinitionGroupKey = Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination);
            else
                stageDefinitionGroupKey = Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeAdmin);

            // Get the Loan Account Object
            _loanAccount = _accountLifePolicy.ParentMortgageLoan as IMortgageLoanAccount;

            // get the variable portion, we will always have this around takeon which happens before a life policy is created
            _mortgageLoanVariable = _loanAccount.SecuredMortgageLoan;
            // see if we have a fixed portion
            IAccountVariFixLoan varifixLoanAccount = _loanAccount as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
                _mortgageLoanFixed = varifixLoanAccount.FixedSecuredMortgageLoan;

            // Get the Loan Application Object
            //_applicationMortgageLoan = _loanAccount.CurrentMortgageLoanApplication;

            // Get the HOC Object
            _hocAccount = _loanAccount.GetRelatedAccountByType(AccountTypes.HOC, _loanAccount.RelatedChildAccounts) as IAccountHOC;
            if (_hocAccount != null)
                _hoc = _hocAccount.HOC;

            // Get the Loan Pipeline Status
            _view.LoanPipelineStatus = LifeRepo.GetLoanPipelineStatus(_loanAccount.Key);
            _view.ManualLifePolicyPaymentVisible = _loanAccount.Details.Any(x => x.DetailType.Key == (int)DetailTypes.ManualLifePolicyPayment);
        }

        void OnAddLifeButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.SelectedLifeAccountKey);
            GlobalCacheData.Add(ViewConstants.SelectedLifeAccountKey, _accountKey, new List<ICacheObjectLifeTime>());

            // Navigate to the required view
            _view.Navigator.Navigate("AddLife");
        }

        void OnRemoveLifeButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                // get the selected LegalEntity        
                ILegalEntity le = LegalEntityRepo.GetLegalEntityByKey(Convert.ToInt32(e.Key));
                // remove the insurable interest
                ILifeInsurableInterest lifeInsurableInterest = le.GetInsurableInterest(_accountLifePolicy.Key);
                if (lifeInsurableInterest != null)
                    _accountLifePolicy.LifeInsurableInterests.Remove(_view.Messages, lifeInsurableInterest);

                // remove the role from the account
                _accountLifePolicy.RemoveRolesForLegalEntity(_view.Messages, le, new int[] { Convert.ToInt32(SAHL.Common.Globals.RoleTypes.AssuredLife) });

                // save the account
                _accountRepo.SaveAccount(_accountLifePolicy);

                // write the stage transition  record
                int genericKey = -1;
                string comments = le.GetLegalName(LegalNameFormat.Full);
                if (_accountLifePolicy.LifePolicy != null)
                {
                    genericKey = _accountLifePolicy.Key;
                    comments += " removed from policy";
                }
                else
                {
                    genericKey = _accountLifePolicy.CurrentLifeApplication.Key;
                    comments += " removed from application";
                }
                StageDefinitionRepo.SaveStageTransition(genericKey, stageDefinitionGroupKey, SAHL.Common.Constants.StageDefinitionConstants.RemoveAssuredLife, comments, CurrentADUser);

                // Recalculate Premiums 
                LifeRepo.RecalculateSALifePremium(_accountLifePolicy, true);

                txn.VoteCommit();

                // reload the data to exclude the removed legalentity
                _view.Navigator.Navigate(_view.ViewName);
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

        void OnPremiumCalculatorButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Calculator");
        }
    }
}
