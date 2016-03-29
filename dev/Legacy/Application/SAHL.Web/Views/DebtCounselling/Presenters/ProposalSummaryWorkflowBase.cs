using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using System.Security.Principal;
using SAHL.Common.Logging;
using System.Reflection;
using SAHL.Web.Controls;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class ProposalSummaryWorkflowBase : ProposalSummaryBase
	{
		public ProposalSummaryWorkflowBase(IProposalSummary view, SAHLCommonBaseController controller)
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

			_view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
			_view.OnAddButtonClicked += new EventHandler(_view_OnAddButtonClicked);
			_view.OnUpdateButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnUpdateButtonClicked);
			_view.OnCopyToDraftButtonClicked += new KeyChangedEventHandler(_view_OnCopyToDraftButtonClicked);
			_view.OnCreateCounterProposalClicked += new KeyChangedEventHandler(_view_OnCreateCounterProposalClicked);
			_view.OnSetActiveButtonClicked += new KeyChangedEventHandler(_view_OnSetActiveButtonClicked);
			_view.OnDeleteButtonClicked += new KeyChangedEventHandler(_view_OnDeleteButtonClicked);
			_view.ReasonsClicked += new KeyChangedEventHandler(OnReasonsClicked);
		}

		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);
			if (!_view.ShouldRunPage)
				return;

			//setup visible buttons
			//_view.ShowCancelButton = true;
			_view.ShowAddButton = true;
			_view.ShowUpdateButton = true;
			_view.ShowSetActiveButton = true;
			_view.ShowCopyToDraftButton = true;

			_view.ShowDeleteButton = true;

			_view.ShowPrintButton = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnCancelButtonClicked(object sender, EventArgs e)
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

		void _view_OnUpdateButtonClicked(object sender, KeyChangedEventArgs e)
		{
			base.SelectedProposal = base.GetSelectedProposalByKey(Convert.ToInt32(e.Key));

			base.ValidateSelection(ButtonClicked.Update);

			if (_view.IsValid)
			{
				GlobalCacheData.Remove(ViewConstants.ProposalKey);
				GlobalCacheData.Add(ViewConstants.ProposalKey, base.SelectedProposal.Key, LifeTimes);
				GlobalCacheData.Remove(ViewConstants.DebtCounsellingKey);
				GlobalCacheData.Add(ViewConstants.DebtCounsellingKey, base.GenericKey, LifeTimes);
				_view.Navigator.Navigate("Update");
			}
		}

		void _view_OnAddButtonClicked(object sender, EventArgs e)
		{
			base.SelectedProposal = null;

			ValidateSelection(ButtonClicked.Add);

			if (_view.IsValid)
			{
				GlobalCacheData.Remove(ViewConstants.ProposalKey);
				GlobalCacheData.Remove(ViewConstants.DebtCounsellingKey);
				GlobalCacheData.Add(ViewConstants.DebtCounsellingKey, base.GenericKey, LifeTimes);
				_view.Navigator.Navigate("Add");
			}
		}

		void _view_OnSetActiveButtonClicked(object sender, KeyChangedEventArgs e)
		{
			base.SelectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

			ValidateSelection(ButtonClicked.SetActive);

			//TODO: Run my ProposalItemsDatesOverlap rule here
			if (base.SelectedProposal != null)
			{
				IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
				svcRule.ExecuteRule(_view.Messages, "ProposalItemsDatesOverlap", base.SelectedProposal);
				if (base.SelectedProposal.ProposalType.Key == (int)ProposalTypes.CounterProposal)
				{
					svcRule.ExecuteRule(_view.Messages, "DebtCounsellingActiveCounterProposalRequiresReason", base.SelectedProposal);
				}
			}

			if (_view.IsValid)
			{
				TransactionScope txn = new TransactionScope();

				try
				{
					DebtCounsellingRepo.SetProposalToActive(base.SelectedProposal, this.CurrentADUser);

					txn.VoteCommit();

					base.BindProposals();
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
		}



		void _view_OnDeleteButtonClicked(object sender, KeyChangedEventArgs e)
		{
			base.SelectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

			ValidateSelection(ButtonClicked.Delete);

			if (_view.IsValid)
			{
				TransactionScope txn = new TransactionScope();

				try
				{
					DebtCounsellingRepo.DeleteProposal(base.SelectedProposal);
					//Only do this once the Proposal has been deleted
					if (base.SelectedProposal.ActiveReason != null)
					{
						var reasonRepository = RepositoryFactory.GetRepository<SAHL.Common.BusinessModel.Interfaces.Repositories.IReasonRepository>();
						reasonRepository.DeleteReason(base.SelectedProposal.ActiveReason);
					}
					txn.VoteCommit();
					BindProposals();

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


		}

		void _view_OnCopyToDraftButtonClicked(object sender, KeyChangedEventArgs e)
		{
			base.SelectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

			ValidateSelection(ButtonClicked.Copy);

			if (_view.IsValid)
			{
				TransactionScope txn = new TransactionScope();

				try
				{
					DebtCounsellingRepo.CopyProposalToDraft(base.SelectedProposal, this.CurrentADUser);

					txn.VoteCommit();

					BindProposals();
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
		}

		void _view_OnCreateCounterProposalClicked(object sender, KeyChangedEventArgs e)
		{
			base.SelectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

			ValidateSelection(ButtonClicked.Create);

			if (_view.IsValid)
			{
				TransactionScope txn = new TransactionScope();

				try
				{
					DebtCounsellingRepo.CopyProposalToDraft(base.SelectedProposal, this.CurrentADUser, ProposalTypes.CounterProposal);

					txn.VoteCommit();

					BindProposals();
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
		}

		/// <summary>
		/// On Reasons Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnReasonsClicked(object sender, KeyChangedEventArgs e)
		{
			base.SelectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

			base.ValidateSelection(ButtonClicked.Reasons);

			if (_view.IsValid)
			{
				var objectLifeTime = new List<ICacheObjectLifeTime>();
				GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, objectLifeTime);
				GlobalCacheData.Add(ViewConstants.GenericKey, base.SelectedProposal.Key, objectLifeTime);
				GlobalCacheData.Add(ViewConstants.ReasonHistoryTitle, "Proposal", objectLifeTime);
				GlobalCacheData.Add(ViewConstants.CancelView, _view.ViewName, objectLifeTime);
				GlobalCacheData.Add(ViewConstants.ReasonTypeKey, (int)ReasonTypes.CounterProposals, objectLifeTime);
				Navigator.Navigate("ActiveCounterProposalReasons");
			}
		}
	}
}