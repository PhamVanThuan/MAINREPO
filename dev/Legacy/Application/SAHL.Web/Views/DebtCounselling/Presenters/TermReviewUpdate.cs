using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class TermReviewUpdate : PaymentReceivedBase
	{
		public TermReviewUpdate(IPayment view, SAHLCommonBaseController controller)
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
            _view.ReadOnly = false;

			_view.DisplayUpdatePanel = false;
			_view.DisplayUpdateReviewDatePanel = true;

			_view.OnUpdateButtonClicked += new EventHandler(OnUpdateButtonClick);

			_view.BindForPaymentUpdate(DebtCounselling, LoadDebitOrderDetails(DebtCounselling.Account.FinancialServices[0]));
        }

		/// <summary>
		/// On Update Button Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnUpdateButtonClick(object sender, EventArgs e)
		{
			//Before commiting, let's ensure that the rules run and that the view is valid after that
			RunBusinessRules();

			if (!View.IsValid)
			{
				return;
			}

			string message = string.Empty;
			IX2Service svc = ServiceFactory.GetService<IX2Service>();
			TransactionScope txn = new TransactionScope();
			try
			{
				//Save the debt Counselling Case
				DCRepo.SaveDebtCounselling(_view.DebtCounselling);

				//Save the Term Review Date on the Accepted Proposal
                DCRepo.SaveProposal(_view.DebtCounselling.AcceptedActiveProposal);

				txn.VoteCommit();
			}
			catch (Exception)
			{
				txn.VoteRollBack();
				if (_view.IsValid)
				{
					svc.CancelActivity(_view.CurrentPrincipal);
					throw;
				}
			}
			finally
			{
				txn.Dispose();
			}

			if (_view.IsValid)
			{
				svc.CompleteActivity(_view.CurrentPrincipal, null, false, message);
				svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
			}
		}

		/// <summary>
		/// Run Business Rules
		/// </summary>
		private void RunBusinessRules()
		{
			SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
			IRuleService rsvc = ServiceFactory.GetService<IRuleService>();
			rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingTermReviewDateMandatory", DebtCounselling);
			rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingMaximumReviewDateInMonths", DebtCounselling);
			rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingMinimumReviewDate", DebtCounselling);
		}
	}
}