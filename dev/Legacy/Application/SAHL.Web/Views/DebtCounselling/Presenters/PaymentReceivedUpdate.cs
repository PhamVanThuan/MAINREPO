using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class PaymentReceivedUpdate : PaymentReceivedBase
	{
		public PaymentReceivedUpdate(IPayment view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			_view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
			_view.OnUpdateButtonClicked += new EventHandler(_view_OnUpdateButtonClicked);
			_view.ReadOnly = false;
			_view.DisplayUpdatePanel = true;

			_view.BindForPaymentUpdate(DebtCounselling, LoadDebitOrderDetails(DebtCounselling.Account.FinancialServices[0]));
		}

		/// <summary>
		/// On Cancel Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void _view_OnCancelButtonClicked(object sender, EventArgs e)
		{
			IX2Service svc = ServiceFactory.GetService<IX2Service>();
			svc.CancelActivity(_view.CurrentPrincipal);
			svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
		}

		/// <summary>
		/// On Update Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void _view_OnUpdateButtonClicked(object sender, EventArgs e)
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
			rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingPaymentReceivedDateMandatory", DebtCounselling);
			rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingPaymentReceivedAmountMandatory", DebtCounselling);
			rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingMaximumReviewDateInMonths", DebtCounselling);

			//Run the rule for the Expectancy first (if it exists)
			if (DebitOrderDetails != null)
			{
				//Expectancy
				if (DebitOrderDetails.Count == 2)
				{
					rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay", DebtCounselling.PaymentReceivedDate, new DateTime(DateTime.Now.Year, DateTime.Now.Month, Int32.Parse(DebitOrderDetails[1].DebitOrderDay)));
				}
				else
				{
					//Current
					rsvc.ExecuteRule(spc.DomainMessages, "DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay", DebtCounselling.PaymentReceivedDate, new DateTime(DateTime.Now.Year, DateTime.Now.Month, Int32.Parse(DebitOrderDetails[0].DebitOrderDay)));
				}
			}
		}
	}
}