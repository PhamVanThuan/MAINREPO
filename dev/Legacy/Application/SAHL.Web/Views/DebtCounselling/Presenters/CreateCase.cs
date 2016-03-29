using System;
using System.Collections.Generic;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.X2.BusinessModel.Interfaces;
using System.Linq;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class CreateCase : SAHLCommonBasePresenter<ICreateCase>
	{
		protected List<ICacheObjectLifeTime> _lifeTimes;
		protected IADUser _adUser;

		private ILegalEntityRepository _legalEntityRepository;
		private IAccountRepository _accountRepository;
		private IDebtCounsellingRepository _dcRepo;
		//private IReasonRepository _reasonRepo;
		private ILookupRepository _lkRepo;
		//private IList<IReason> _reasons;
		private Int64 _instanceID = -1;

		#region Properties

		private ILegalEntity DebtCounsellor { get; set; }

		private IDebtCounsellingStatus OpenDCStatus
		{
			get
			{
                return LKRepo.DebtCounsellingStatuses[DebtCounsellingStatuses.Open];
			}
		}

		protected List<ICacheObjectLifeTime> LifeTimes
		{
			get
			{
				if (_lifeTimes == null)
				{
					List<string> views = new List<string>();
					//views.Add("UpdateRateOverrides");
					_lifeTimes = new List<ICacheObjectLifeTime>();
					_lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
				}
				return _lifeTimes;
			}
		}
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

		public ILegalEntityRepository LegalEntityRepository
		{
			get
			{
				return _legalEntityRepository;
			}
		}
		public IAccountRepository AccountRepository
		{
			get
			{
				return _accountRepository;
			}
		}

		public IDebtCounsellingRepository DCRepo
		{
			get
			{
				if (_dcRepo == null)
					_dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

				return _dcRepo;
			}
		}
        //public IReasonRepository ReasonRepo
        //{
        //    get
        //    {
        //        if (_reasonRepo == null)
        //            _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

        //        return _reasonRepo;
        //    }
        //}
		public ILookupRepository LKRepo
		{
			get
			{
				if (_lkRepo == null)
					_lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

				return _lkRepo;
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
        #endregion

		/// <summary>
		/// Constructor for ProposalDetailsBase
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public CreateCase(ICreateCase view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		/// <summary>
		/// OnView Initialised event - retrieve data for use by presenters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage) return;

			_accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
			_legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

			_view.CreateCaseClick += new EventHandler<EventArgs>(OnCreateClick);
			_view.LegalEntityIDNumberSelected += new KeyChangedEventHandler(OnLegalEntityIDNumberSelected);
			_view.PersonOfInterestClick += new KeyChangedEventHandler(OnPersonOfInterestClicked);
			_view.CancelClick += new EventHandler<EventArgs>(OnCanceClick);

			//Update the Selected Accounts
			if (_view.TreeViewAccount != null && PrivateCacheData.ContainsKey(ViewConstants.LegalEntityList))
			{
				UpdateSelectedAccounts(_view.TreeViewAccount, (List<AccountForView>)PrivateCacheData[ViewConstants.LegalEntityList]);
			}

			if (PrivateCacheData.ContainsKey(ViewConstants.LegalEntityList))
			{
				_view.UpdateDisplay(((IList<AccountForView>)PrivateCacheData[ViewConstants.LegalEntityList]));
			}

			if (GlobalCacheData.ContainsKey(ViewConstants.DebtCounsellorLegalEntityKey))
			{
				DebtCounsellor = LegalEntityRepository.GetLegalEntityByKey((int)GlobalCacheData[ViewConstants.DebtCounsellorLegalEntityKey]);
				_view.ShowDebtCounsellor(DebtCounsellor);
			}
			else
			{
				_view.Messages.Add(new Error("No Debt Counsellor has been selected for this case.", "No Debt Counsellor has been selected for this case."));
			}

            if (!_view.IsPostBack)
                _view.Date17pt1 = DateTime.Now;
		}

		/// <summary>
		/// On Cancel Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnCanceClick(object sender, EventArgs e)
		{
			Navigator.Navigate("Cancel");
		}

		/// <summary>
		/// On Search Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCreateClick(object sender, EventArgs e)
		{
            if (!_view.Date17pt1.HasValue)
                _view.Messages.Add(new Error("The 17.1 Date is required.", "The 17.1 Date is required."));
            else if (_view.Date17pt1.Value > DateTime.Now)
                _view.Messages.Add(new Error("The 17.1 Date cannot be in the future.", "The 17.1 Date cannot be in the future."));

            //Do nothing if there are no accounts to create cases for
			if (!PrivateCacheData.ContainsKey(ViewConstants.LegalEntityList))
			{
				return;
			}

            List<AccountForView> afvList = PrivateCacheData[ViewConstants.LegalEntityList] as List<AccountForView>;
            //check if any work is required, else tell the user
            bool createCase = false;
            foreach (AccountForView afv in afvList)
                if (afv.HasFlaggedForDebtCounsellingEntities)
                    createCase = true;

            if (!createCase)
            {
                string errMsg = "No items selected to create a Debt Counseling case.";
                _view.Messages.Add(new Error(errMsg, errMsg));
                return;
            }

            //let the user know what to fix if necessary.
            if (!_view.IsValid) return;

			//this will need to happen inside of a transaction, so we commit/fail all
			using (TransactionScope txn = new TransactionScope())
			{
				try
				{
                    IDebtCounsellingGroup dcGroup = DCRepo.CreateEmptyDebtCounsellingGroup();
					dcGroup.CreatedDate = DateTime.Now;

                    foreach (AccountForView afv in afvList)
					{
						if (!afv.HasFlaggedForDebtCounsellingEntities)
						{
							continue;
						}

						//1. Add the new record to the 2AM table to reserve the generic key to send to WorkFlow

						//a. Create new empty dc, and populate it
						IDebtCounselling dc = DCRepo.CreateEmptyDebtCounselling();
						dc.Account = AccountRepository.GetAccountByKey(afv.Key);
						dc.DebtCounsellingStatus = OpenDCStatus;
						//setup the bidirectional reference for these BM objects

                        //Save the Reference Number if one has been entered
                        if (!string.IsNullOrEmpty(_view.ReferenceNumber))
                        {
                            dc.ReferenceNumber = _view.ReferenceNumber;
                        }

						//Debt Counselling Group
						//Get the possible related debt counselling groups to this case
						IList<IDebtCounsellingGroup> groups = DCRepo.GetRelatedDebtCounsellingGroupForLegalEntities((from le in afv.LegalEntities where le.FlaggedForDebtCounselling select le.Key).ToList<int>());
						if (groups != null && groups.Count == 1)
						{
							dcGroup = groups[0];
						}


						dc.DebtCounsellingGroup = dcGroup;
						dcGroup.DebtCounsellingCases.Add(null, dc);

						//save this to get the DB to reserve the dc Key
						DCRepo.SaveDebtCounsellingGroup(dcGroup);

						//dc should now have a key, so we can do the roles, reasons and WF creates

						//2. Add the roles.

						//a. Client roles
						foreach (LegalEntityForView lefv in afv.LegalEntities)
						{
							if (lefv.FlaggedForDebtCounselling == true)
							{
                                LegalEntityRepository.InsertExternalRole(ExternalRoleTypes.Client, dc.Key, GenericKeyTypes.DebtCounselling2AM, lefv.Key,false);
							}
						}

						//b. Debt counsellor role
                        LegalEntityRepository.InsertExternalRole(ExternalRoleTypes.DebtCounsellor, dc.Key, GenericKeyTypes.DebtCounselling2AM, DebtCounsellor.Key,true);

						//3. Initiator Reasons
                        SDRepo.SaveStageTransition(dc.Key, StageDefinitionStageDefinitionGroups.Received17pt1, _view.Date17pt1.Value, "", _view.CurrentPrincipal.Identity.Name);
                        //foreach (IReason r in _reasons)
                        //{
                        //    //we have to create a new reason from the reasons in the list
                        //    //issue: If we set the Generic Key on the reason, the reason will be created and referenced in the _reasons
                        //    //The next time, there won't be a creation of a reason, but an update of the current one.

                        //    //set the Generic Key now that we have it
                        //    IReason reasonToCreate = ReasonRepo.CreateEmptyReason();
                        //    reasonToCreate.StageTransition = r.StageTransition;
                        //    reasonToCreate.Comment = r.Comment;
                        //    reasonToCreate.ReasonDefinition = r.ReasonDefinition;
                        //    reasonToCreate.GenericKey = dc.Key;

                        //    //save reason
                        //    ReasonRepo.SaveReason(reasonToCreate);
                        //}

						//4. Create the WF Case
                        CreateWorkflow(dc.Key, afv.Key, afv.ProductKey);
					}
					if (_view.IsValid)
					{
						txn.VoteCommit();
						RedirectToX2Worklist();
					}
				}
				catch (Exception)
				{
					if (_view.IsValid)
						throw;
					txn.VoteRollBack();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dcKey"></param>
		/// <param name="accKey"></param>
        /// <param name="productKey"></param>
        private void CreateWorkflow(int dcKey, int accKey, int productKey)
		{
			bool created = false;
			try
			{
				// once we have an application create a workflow case
				Dictionary<string, string> Inputs = new Dictionary<string, string>();
				Inputs.Add("DebtCounsellingKey", dcKey.ToString());
				Inputs.Add("AccountKey", accKey.ToString());
                Inputs.Add("ProductKey", productKey.ToString());

				IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
				if (XI == null || String.IsNullOrEmpty(XI.SessionID))
					X2Service.LogIn(_view.CurrentPrincipal);

				SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
				X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.DebtCounselling, SAHL.Common.Constants.WorkFlowActivityName.CreateDebtCounsellingCase, Inputs, false);
				created = true;
                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, spc.IgnoreWarnings, null);
				_instanceID = XI.InstanceID;
			}
			catch (Exception)
			{
				if (created)
					X2Service.CancelActivity(_view.CurrentPrincipal);

				if (_view.IsValid)
					throw;
			}
		}

		private void RedirectToX2Worklist()
		{
			IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
			IState state = x2Repo.GetStateByName(SAHL.Common.Constants.WorkFlowName.DebtCounselling, SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling, SAHL.Common.Constants.DebtCounsellingWorkFlowStates.ReviewNotification);

			// Add the Generic Key to the global cache for our redirect view to use
			GlobalCacheData.Remove(ViewConstants.StateID);
			GlobalCacheData.Add(ViewConstants.StateID, state.ID, new List<ICacheObjectLifeTime>());

			// Add the instanceID to the global cache for our redirect view to use
			GlobalCacheData.Remove(ViewConstants.InstanceID);
			GlobalCacheData.Add(ViewConstants.InstanceID, _instanceID, new List<ICacheObjectLifeTime>());

			// navigate to the workflow redirect view
			Navigator.Navigate("X2InstanceRedirect");
		}

		/// <summary>
		/// On Person of Interest Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnPersonOfInterestClicked(object sender, KeyChangedEventArgs e)
		{
			//Add the Legal Entity to the List
			IList<AccountForView> accounts = (IList<AccountForView>)PrivateCacheData[ViewConstants.LegalEntityList];
			foreach (AccountForView account in accounts)
			{
				foreach (LegalEntityForView legalEntity in account.LegalEntities)
				{
					if (e.Key.ToString() == legalEntity.Key.ToString())
					{
						legalEntity.FlaggedForDebtCounselling = false;
						legalEntity.IsInteresting = false;
					}
				}
			}

			((List<AccountForView>)accounts).RemoveAll((account) => account.IsInteresting == false);

			PrivateCacheData[ViewConstants.LegalEntityList] = accounts;

			//Add the Legal Entity to the List
			_view.UpdateDisplay(((IList<AccountForView>)PrivateCacheData[ViewConstants.LegalEntityList]));
		}

		/// <summary>
		/// On Legal Entity ID Number Selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnLegalEntityIDNumberSelected(object sender, KeyChangedEventArgs e)
		{
			ILegalEntity legalEntity = LegalEntityRepository.GetLegalEntityByKey(Convert.ToInt32(e.Key));

			AddLegalEntityToCache(legalEntity);

			//Add the Legal Entity to the List
			_view.UpdateDisplay(((IList<AccountForView>)PrivateCacheData[ViewConstants.LegalEntityList]));
		}

		/// <summary>
		/// Add Legal Entity To Cache
		/// </summary>
		/// <param name="legalEntityToAdd"></param>
		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Neccesary casts")]
		private void AddLegalEntityToCache(ILegalEntity legalEntityToAdd)
		{
			List<AccountForView> accounts;
			if (legalEntityToAdd.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
			{
				ILegalEntityNaturalPerson legalEntityNaturalPerson = (ILegalEntityNaturalPerson)legalEntityToAdd;
				if (PrivateCacheData.ContainsKey(ViewConstants.LegalEntityList))
				{
					accounts = ((List<AccountForView>)PrivateCacheData[ViewConstants.LegalEntityList]);
				}
				else
				{
					accounts = new List<AccountForView>();
				}

				//Foreach of the Roles the legal entity plays, get those roles and account that the person plays a role on
				foreach (IRole accountRole in legalEntityToAdd.Roles)
				{
                    //if (accountRole.Account != null && accountRole.Account is IMortgageLoanAccount &&
                    //    accountRole.Account.OriginationSource.Key != (int)OriginationSources.RCS)
                    if (accountRole.Account != null && (accountRole.Account is IMortgageLoanAccount  || accountRole.Account is IAccountPersonalLoan))
					{
						if (accountRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
							(accountRole.Account.AccountStatus.Key == (int)AccountStatuses.Open ||
							 accountRole.Account.AccountStatus.Key == (int)AccountStatuses.Dormant))
						{
							CreateAccountForView(accountRole.Account, legalEntityNaturalPerson, accounts);
						}
					}
				}
				PrivateCacheData[ViewConstants.LegalEntityList] = accounts;
			}
		}

		/// <summary>
		/// Update Selected Accounts
		/// </summary>
		/// <param name="treeView"></param>
		/// <param name="accounts"></param>
		private static void UpdateSelectedAccounts(SAHLTreeView treeView, List<AccountForView> accounts)
		{
			//First, let's reset all the legal entities to not interesting
			foreach (AccountForView accountForView in accounts)
			{
				foreach (LegalEntityForView legalEntityForView in accountForView.LegalEntities)
				{
					accountForView.FlagLegalEntityForDebtCounselling(legalEntityForView.Key, false);
				}
			}

			//Now, let's set only the selected legal entities to be selected
			foreach (string valuePath in treeView.CheckedValues)
			{
				if (valuePath.Contains("-"))
				{
					int accountKey = int.Parse(valuePath.Split('-')[0]);
					int legalEntityKey = int.Parse(valuePath.Split('-')[1]);
					AccountForView accountForViewToUpdate = accounts.Find(account => account.Key == accountKey);
					if (accountForViewToUpdate == null)
					{
						continue;
					}
					//Get the legal entity key from the node
					accountForViewToUpdate.FlagLegalEntityForDebtCounselling(legalEntityKey, true);
				}
			}
		}

		/// <summary>
		/// Create Account For View
		/// </summary>
		/// <param name="account"></param>
		///	<param name="personOfInterest"></param>
		/// <param name="accounts"></param>
		/// <returns></returns>
		private static AccountForView CreateAccountForView(IAccount account, ILegalEntityNaturalPerson personOfInterest, List<AccountForView> accounts)
		{
			AccountForView accountForView = accounts.Find((accountToFind) => accountToFind.Key == account.Key);
			if (accountForView == null)
			{
				accountForView = new AccountForView() { Key = account.Key, ProductDescription = account.Product.Description, ProductKey = account.Product.Key, IsUnderDebtCounselling = account.UnderDebtCounselling, OriginationSourceKey = account.OriginationSource.Key};
				accounts.Add(accountForView);
			}

			//Check the account Roles of the people in the account
			//Find the Main Applicant's as well as the Suretor
			//Add them to the list of people that we are going to show on the tree view
			foreach (IRole accountRole in account.Roles)
			{
				if (accountRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
				{
					LegalEntityForView person = new LegalEntityForView()
					{
						DisplayName = accountRole.LegalEntity.DisplayName,
						Key = accountRole.LegalEntity.Key,
						IDNumber = accountRole.LegalEntity.LegalNumber,
						PassportNumber = String.Empty,
						LegalEntityType = (LegalEntityTypes)accountRole.LegalEntity.LegalEntityType.Key,
						IsUnderDebtCounselling = accountRole.UnderDebtCounselling(true),
                        RoleTypeDescription = accountRole.RoleType.Description
					};
					//Determine whether this is the person of interest,
					//if so, let's indicate it
					if (person.Key == personOfInterest.Key)
					{
						person.IsInteresting = true;
						//Since this is the first time the person has been added to the list, flag him/her for debt counselling
						person.FlaggedForDebtCounselling = true;
					}
					accountForView.AddLegalEntity(person);
				}
			}

			return accountForView;
		}
	}

	/// <summary>
	/// Legal Entity for the View
	/// </summary>
	public class LegalEntityForView
	{
		public int Key { get; set; }
		public string DisplayName { get; set; }
		public string IDNumber { get; set; }
		public string PassportNumber { get; set; }
		public bool IsInteresting { get; set; }
		public bool FlaggedForDebtCounselling { get; set; }
		public LegalEntityTypes LegalEntityType { get; set; }
		public bool IsUnderDebtCounselling { get; set; }
        public string RoleTypeDescription { get; set; }
	}

	/// <summary>
	/// Account for the View
	/// </summary>
	public class AccountForView
	{
		private List<LegalEntityForView> _legalEntities;

		public bool IsUnderDebtCounselling { get; set; }
		public int Key { get; set; }

        public int ProductKey { get; set; }
		public string ProductDescription { get; set; }

        public int OriginationSourceKey { get; set; }
        public List<LegalEntityForView> LegalEntities
		{
			get
			{
				return _legalEntities;
			}
		}

		public bool IsInteresting
		{
			get
			{
				if (_legalEntities.Find(legalEntity => legalEntity.IsInteresting == true) != null)
				{
					return true;
				}
				return false;
			}
		}

		public bool HasFlaggedForDebtCounsellingEntities
		{
			get
			{
				if (_legalEntities.Find(legalEntity => legalEntity.FlaggedForDebtCounselling == true) != null)
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Initializes a new Account For View
		/// </summary>
		public AccountForView()
		{
			if (_legalEntities == null)
			{
				_legalEntities = new List<LegalEntityForView>();
			}
		}


		/// <summary>
		/// Add Legal Entity
		/// </summary>
		/// <param name="legalEntity"></param>
		public void AddLegalEntity(LegalEntityForView legalEntity)
		{
			if (_legalEntities == null)
			{
				_legalEntities = new List<LegalEntityForView>();
			}
			LegalEntityForView legalEntityToUpdate = _legalEntities.Find(le => le.Key == legalEntity.Key);
			if (legalEntityToUpdate == null)
			{
				_legalEntities.Add(legalEntity);
			}
			else
			{
				if (!legalEntityToUpdate.IsInteresting)
				{
					legalEntityToUpdate.IsInteresting = legalEntity.IsInteresting;
				}
				if (!legalEntityToUpdate.FlaggedForDebtCounselling)
				{
					legalEntityToUpdate.FlaggedForDebtCounselling = legalEntity.FlaggedForDebtCounselling;
				}
			}
		}

		/// <summary>
		/// Update Legal Entity Interesting
		/// </summary>
		/// <param name="legalEntityKey"></param>
		/// <param name="flaggedForDebtCounselling"></param>
		public void FlagLegalEntityForDebtCounselling(int legalEntityKey, bool flaggedForDebtCounselling)
		{
			LegalEntityForView legalEntityToUpdate = _legalEntities.Find((legalEntity) => legalEntity.Key == legalEntityKey);
			if (legalEntityToUpdate != null)
			{
				legalEntityToUpdate.FlaggedForDebtCounselling = flaggedForDebtCounselling;
			}
		}

		/// <summary>
		/// Remove Legal Entity
		/// </summary>
		/// <param name="legalEntityToRemove"></param>
		public void RemoveLegalEntity(LegalEntityForView legalEntityToRemove)
		{
			_legalEntities.Remove(legalEntityToRemove);
		}
	}
}