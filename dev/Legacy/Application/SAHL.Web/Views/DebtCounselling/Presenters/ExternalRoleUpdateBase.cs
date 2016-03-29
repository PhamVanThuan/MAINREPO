using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class ExternalRoleUpdateBase : SAHLCommonBasePresenter<IExternalRoleUpdate>
	{
		#region Properties

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

		private ILegalEntityRepository _leRepo;
		public ILegalEntityRepository LERepo
		{
			get
			{
				if (_leRepo == null)
					_leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

				return _leRepo;
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
		/// Constructor for ExternalRoleUpdateBase
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ExternalRoleUpdateBase(IExternalRoleUpdate view, SAHLCommonBaseController controller)
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
			if (!_view.ShouldRunPage)
				return;

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
			_view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
		}

		protected override void OnViewLoaded(object sender, EventArgs e)
		{
			base.OnViewLoaded(sender, e);
			if (!_view.ShouldRunPage)
				return;

			List<BindableExternalRole> erList = new List<BindableExternalRole>();

			foreach (IDebtCounselling dc in DebtCounselling.DebtCounsellingGroup.DebtCounsellingCases)
			{
				if (dc.DebtCounsellingStatus.Key == (int)DebtCounsellingStatuses.Open)
                    erList.Add(new BindableExternalRole(dc, _view.RoleType));
			}

			_view.BindExternalRoleGrid(erList);
		}

		protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
		{
			using (TransactionScope txn = new TransactionScope())
			{
				try
				{
					Dictionary<int, bool> selectedGridItems = _view.GetCheckedItems;
					foreach (KeyValuePair<int, bool> kv in selectedGridItems)
					{
						if (kv.Value)
						{
							// Insert External Role
							int DebtCounsellingKey = kv.Key;
							ExternalRoleTypes externalRoleType = _view.RoleType;
							ILegalEntity le = _view.NewLegalEntity;
							GenericKeyTypes GenericKeyType = (GenericKeyTypes)Enum.Parse(typeof(GenericKeyTypes), _genericKeyTypeKey.ToString());
							LERepo.InsertExternalRole(externalRoleType, DebtCounsellingKey, GenericKeyType, le.Key, true);

							StageDefinitionStageDefinitionGroups? stageDefinitionGroup = null;
							switch (_view.RoleType)
							{
								case ExternalRoleTypes.DebtCounsellor:
									stageDefinitionGroup = StageDefinitionStageDefinitionGroups.ChangeofDebtCounselor;
									break;
								case ExternalRoleTypes.PaymentDistributionAgent:
									stageDefinitionGroup = StageDefinitionStageDefinitionGroups.ChangeOfPaymentDistributionAgent;
									break;
							}

							if (stageDefinitionGroup.HasValue)
							{
								SDRepo.SaveStageTransition(DebtCounsellingKey, stageDefinitionGroup.Value, string.Empty, _view.CurrentPrincipal.Identity.Name);
							}
						}
					}
					txn.VoteCommit();
				}
				catch (Exception)
				{
					txn.VoteRollBack();
					if (_view.IsValid)
						throw;
				}
			}

			_view.Navigator.Navigate("ExternalRole");
		}
	}
}