using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class ExternalRole : SAHLCommonBasePresenter<IExternalRole>
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

		private IDebtCounsellingRepository _debtCounsellingRepo;
		public IDebtCounsellingRepository DebtCounsellingRepository
		{
			get
			{
				if (_debtCounsellingRepo == null)
					_debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

				return _debtCounsellingRepo;
			}
		}

		/// <summary>
		/// Constructor for CourtDetails
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ExternalRole(IExternalRole view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		/// <summary>
		/// On View Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage)
				return;

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

			_view.DebtCounsellingCase = DebtCounsellingRepository.GetDebtCounsellingByKey(_genericKey);
		}
	}
}