using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Administration.Presenters
{
	/// <summary>
	/// 
	/// </summary>
	public class ViewMyProfileDetails : SAHL.Web.Views.Common.Presenters.LegalEntityDetails.LegalEntityDetailsUpdateBase
	{
		public ViewMyProfileDetails(ILegalEntityDetails view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
			LoadLegalEntityFromLogin();
			BindLegalEntity();
		}

		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);

			if (!_view.ShouldRunPage) return;

			_view.SetUpdateMyDetails = true;
			_view.RoleTypeVisible = false;
			_view.IncomeContributorVisible = false;
			_view.CitizenTypeVisible = false;
			_view.DateOfBirthVisible = false;
			_view.PassportNumberVisible = false;
			_view.TaxNumberVisible = false;
			_view.GenderVisible = false;
			_view.MaritalStatusVisible = false;
			_view.StatusVisible = false;
			_view.PopulationGroupVisible = false;
			_view.InsurableInterestVisible = false;
			_view.LimitedUpdate = true;
			_view.PanelMarketingOptionPanelVisible = false;
			_view.LockedUpdateControlsVisible = false;
			_view.UpdateControlsVisible = false;
			_view.SubmitButtonVisible = false;
			_view.CancelButtonVisible = false;
		}
	}
}
