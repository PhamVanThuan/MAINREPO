using System;
using System.Linq;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class CourtDetailsAdd : CourtDetailsBase
	{

		/// <summary>
		/// Constructor for CourtDetailsAdd
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public CourtDetailsAdd(ICourtDetails view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
			_view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
			_view.OnCommentEditorSubmitButtonClicked += new EventHandler(_view_OnCommentEditorSubmitButtonClicked);

			// bind the hearing types
			_view.BindHearingTypes(base.LookupRepo.HearingTypes);

			// set the mainatenance panel visible
			_view.ShowMaintenancePanel = true;
		}

		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);
			if (!_view.ShouldRunPage)
				return;
		}

		void _view_OnCancelButtonClicked(object sender, EventArgs e)
		{
			CancelActivity();
		}

		void _view_OnSubmitButtonClicked(object sender, EventArgs e)
		{
			ValidateScreenInput();

			if (_view.IsValid == false)
				return;

			TransactionScope txn = new TransactionScope();
			try
			{
				IDebtCounselling debtCounselling = base.DebtCounsellingRepo.GetDebtCounsellingByKey(base.GenericKey);
				IHearingDetail hearingDetail = base.DebtCounsellingRepo.CreateEmptyHearingDetail();
				hearingDetail.DebtCounselling = debtCounselling;
				hearingDetail.HearingType = base.DebtCounsellingRepo.GetHearingTypeByKey(_view.SelectedHearingTypeKey);
				hearingDetail.HearingAppearanceType = base.DebtCounsellingRepo.GetHearingAppearanceTypeByKey(_view.SelectedHearingAppearanceTypeKey);
				if (_view.SelectedCourtKey > 0)
					hearingDetail.Court = base.DebtCounsellingRepo.GetCourtByKey(_view.SelectedCourtKey);
				hearingDetail.CaseNumber = _view.CaseNumber;
				hearingDetail.HearingDate = _view.HearingDate.Value;
				hearingDetail.GeneralStatus = base.LookupRepo.GeneralStatuses[GeneralStatuses.Active];
				hearingDetail.Comment = _view.Comments;

				debtCounselling.HearingDetails.Add(new DomainMessageCollection(), hearingDetail);

				base.DebtCounsellingRepo.SaveDebtCounselling(debtCounselling);

				txn.VoteCommit();

				// comnplete activity and navigate
				CompleteActivityAndNavigate();
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

		private void ValidateScreenInput()
		{
			string errorMessage = "";

			if (_view.SelectedHearingTypeKey <= 0)
			{
				errorMessage = "Hearing Type must be selected.";
				_view.Messages.Add(new Error(errorMessage, errorMessage));
			}

			if (_view.SelectedHearingAppearanceTypeKey <= 0)
			{
				errorMessage = "Appearance Type must be selected.";
				_view.Messages.Add(new Error(errorMessage, errorMessage));
			}

			if (_view.SelectedHearingTypeKey == (int)SAHL.Common.Globals.HearingTypes.Court)
			{
				if (_view.SelectedCourtKey <= 0)
				{
					errorMessage = "Court must be selected.";
					_view.Messages.Add(new Error(errorMessage, errorMessage));
				}

				if (String.IsNullOrEmpty(_view.CaseNumber))
				{
					errorMessage = "Case Number must be entered.";
					_view.Messages.Add(new Error(errorMessage, errorMessage));
				}
				else
				{
					// if there are previous records we need to check the case number is the same
					if (base._hearingDetails != null && base._hearingDetails.Count > 0)
					{
						IHearingDetail detail = _hearingDetails
							.Where(x => x.HearingType.Key == (int)SAHL.Common.Globals.HearingTypes.Court && !string.IsNullOrEmpty(x.CaseNumber))
							.OrderByDescending(x => x.HearingDate)
							.FirstOrDefault();

						if (detail != null && _view.CaseNumber != detail.CaseNumber)
						{
							errorMessage = "Case Number must be the same as on previous Hearing Detail records : " + detail.CaseNumber;
							_view.Messages.Add(new Warning(errorMessage, errorMessage));
						}
					}
				}
			}

			if (_view.HearingDate.HasValue == false)
			{
				errorMessage = "Hearing Date must be entered.";
				_view.Messages.Add(new Error(errorMessage, errorMessage));
			}

		}

		private void CancelActivity()
		{
			base.X2Service.CancelActivity(_view.CurrentPrincipal);
			base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
		}

		private void CompleteActivityAndNavigate()
		{
            // pass the hearing type key to workflow engine
            int hearingAppearanceTypeKey = _view.SelectedHearingAppearanceTypeKey;
            base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false, hearingAppearanceTypeKey);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

		protected void _view_OnCommentEditorSubmitButtonClicked(object sender, EventArgs e)
		{
			if (_view.HearingDetailKey > 0)
			{
				IHearingDetail selectedHearingDetail = DebtCounsellingRepo.GetHearingDetailByKey(_view.HearingDetailKey);
				selectedHearingDetail.Comment = string.IsNullOrEmpty(_view.CommentEditor) ? null : _view.CommentEditor.Trim();
				base.DebtCounsellingRepo.SaveHearingDetail(selectedHearingDetail);
			}
		}
	}
}