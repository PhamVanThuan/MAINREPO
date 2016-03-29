using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Security;
using System;
using SAHL.Common.BusinessModel.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;



namespace SAHL.Web.Views.Common.Presenters
{
	/// <summary>
	/// Presenter - ReOpenAccount
	/// </summary>
	public class ReOpenAccount : SAHLCommonBasePresenter<IReOpenAccount>
	{
		protected CBOMenuNode _node;
		private IMortgageLoan mlFixed;
		private IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
		private IAccount account;
		private int genericKey;
		private IReadOnlyEventList<IFinancialService> fsVarLst;
		private IReadOnlyEventList<IFinancialService> fsFixedLst;
		private IMortgageLoan mlVar;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Waiting on Backend Changes trac 19412")]
		private IFinancialService fsVar;
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Waiting on Backend Changes trac 19412")]
		private IFinancialService fsFixed;


		/// <summary>
		/// Constructor for ReOpen Account Presenter
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ReOpenAccount(IReOpenAccount view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}
		/// <summary>
		/// OnInitialise event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage) return;

			//_cboCurrentNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
			//_cboAccountNode = _cboCurrentNode.GetNodeByType(GenericKeyTypes.Account) as CBOMenuNode;
			//(int)_cboAccountNode.GenericKey -- to get AccountKey

			_node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

			if (_node == null)
				throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
			else
				genericKey = Convert.ToInt32(_node.GenericKey);

			account = accRepo.GetAccountByKey(genericKey);

			fsVarLst = account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Closed });
			fsFixedLst = account.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Closed });

			for (int i = 0; i < fsVarLst.Count; i++)
			{
				mlVar = fsVarLst[i] as IMortgageLoan;
				if (Convert.ToDateTime(fsVarLst[i].Account.CloseDate).Date == Convert.ToDateTime(mlVar.CloseDate).Date)
				{
					mlVar = fsVarLst[i] as IMortgageLoan;
					fsVar = fsVarLst[i];
				}
			}

			if (fsFixedLst != null)
			{
				for (int i = 0; i < fsFixedLst.Count; i++)
				{
					mlFixed = fsFixedLst[i] as IMortgageLoan;
					if (Convert.ToDateTime(fsFixedLst[i].Account.CloseDate).Date == Convert.ToDateTime(mlFixed.CloseDate).Date)
					{
						mlFixed = fsFixedLst[i] as IMortgageLoan;
						fsFixed = fsFixedLst[i];
					}
				}
			}

			_view.SubmitButtonClicked += new EventHandler(_view_SubmitButtonClicked);
			_view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
		}

		void _view_CancelButtonClicked(object sender, EventArgs e)
		{
			Navigator.Navigate("LoanSummary");
		}

        
		void _view_SubmitButtonClicked(object sender, EventArgs e)
		{
            TransactionScope txn = new TransactionScope();

            try
            {
                accRepo.ReOpenAccount(genericKey, _view.CurrentPrincipal.Identity.Name);
                txn.VoteCommit();
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

			_view.Navigator.Navigate("LoanSummary");

		}
	}

}
