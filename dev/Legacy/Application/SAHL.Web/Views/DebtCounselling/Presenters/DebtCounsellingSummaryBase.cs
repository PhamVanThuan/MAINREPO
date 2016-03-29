using System;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	/// <summary>
	/// 
	/// </summary>
	public class DebtCounsellingSummaryBase : SAHLCommonBasePresenter<IDebtCounsellingSummary>
	{
		
		private CBOMenuNode _node;
		public InstanceNode _instanceNode;
		private int _debtCounsellingKey, _genericKeyTypeKey;
		private ILookupRepository _lookupRepo;
		private IDebtCounsellingRepository _debtCounsellingRepo;
		private IDebtCounselling _debtCounselling;

		private string _eStageName, _eFolderID;
		public IADUser _eADUser;

		public IDebtCounselling DebtCounselling
		{
			get
			{
				return _debtCounselling;
			}
		}

		public string eStageName
		{
			get
			{
				return _eStageName;
			}
			set
			{
				_eStageName = value;
			}
		}

		public IADUser eADUser
		{
			get
			{
				return _eADUser;
			}
			set
			{
				_eADUser = value;
			}
		}

		public string eFolderID
		{
			get
			{
				return _eFolderID;
			}
			set
			{
				_eFolderID = value;
			}
		}
	


		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public DebtCounsellingSummaryBase(IDebtCounsellingSummary view, SAHLCommonBaseController controller)
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

			_lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
			_debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

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
				_debtCounselling = _debtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);

			//Run the rule
			IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
			svcRule.ExecuteRule(_view.Messages, "DebtCounsellingLatestTransitionIsCourtOrderWithAppeal", _debtCounselling);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.DisplayEworkCaseDetails = false;
			//_view.BindControls(_debtCounselling);

			if (!String.IsNullOrEmpty(_eStageName))
			{
				_view.DisplayEworkCaseDetails = true;
				_view.BindEworkCaseDetails(_eStageName, _eADUser);
			}
		}

		protected override void OnViewLoaded(object sender, EventArgs e)
		{
			base.OnViewLoaded(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.BindControls(_debtCounselling);

			if (_debtCounselling != null && _debtCounselling.Account != null)
			{
				SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
				//run some rules: 
				List<string> rulesToRun = new List<string>();
				rulesToRun.Add("MultipleDebtCounsellingCasesForAccount");
				rulesToRun.Add("MultipleDebtCounsellingGroupsForLegalEntity");
				RuleServ.ExecuteRuleSet(spc.DomainMessages, rulesToRun, _debtCounselling.Account.Key, _view.CurrentPrincipal.Identity.Name);
			}
		}

		private IRuleService _ruleServ;
		protected IRuleService RuleServ
		{
			get
			{
				if (_ruleServ == null)
					_ruleServ = ServiceFactory.GetService<IRuleService>();

				return _ruleServ;
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
		}

		protected IDebtCounsellingRepository DebtCounsellingRepo
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
