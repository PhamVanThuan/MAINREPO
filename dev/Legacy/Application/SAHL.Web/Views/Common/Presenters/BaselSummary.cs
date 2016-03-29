using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Data;

namespace SAHL.Web.Views.Common.Presenters
{
	public class BaselSummary : SAHLCommonBasePresenter<IBaselSummary>
	{
		/// <summary>
		/// CBO Node
		/// </summary>
		private CBONode _node;

		private IAccountRepository _accountRepository;

		/// <summary>
		/// Initializes a new instance of the Basel Summary
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public BaselSummary(IBaselSummary view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
			_accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
		}

		/// <summary>
		/// On View Initalized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			_node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);

			if(!_view.ShouldRunPage)
			{
				return;
			}

			if(_accountRepository != null)
			{
				DataSet behaviouralScoresDataSet = _accountRepository.GetBehaviouralScore(_node.GenericKey);
				if (behaviouralScoresDataSet != null)
				{
					if (behaviouralScoresDataSet.Tables.Count > 0)
					{
						_view.BindBehaviouralScores(behaviouralScoresDataSet.Tables[0]);
					}
				}
			}
		}

		/// <summary>
		/// On View PreRender
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);
		}
	}
}