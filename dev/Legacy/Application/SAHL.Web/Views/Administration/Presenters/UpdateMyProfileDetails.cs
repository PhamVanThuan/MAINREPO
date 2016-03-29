using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Administration.Presenters
{
	/// <summary>
	/// 
	/// </summary>
	public class UpdateMyProfileDetails: SAHL.Web.Views.Common.Presenters.LegalEntityDetails.LegalEntityDetailsUpdateBase
	{
		public UpdateMyProfileDetails(ILegalEntityDetails view, SAHLCommonBaseController controller)
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
			_view.LockedUpdateControlsVisible = true;
			_view.UpdateControlsVisible = true;
			_view.SubmitButtonVisible = true;
			_view.CancelButtonVisible = true;
		}

		protected override void OnSubmitButtonClicked(object sender, EventArgs e)
		{
			// Get the details from the screen
			_view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);

			IRuleService RuleServ = ServiceFactory.GetService<IRuleService>();
			RuleServ.ExecuteRule(_view.Messages, "LegalEntityNaturalPersonMandatorySaluation", base.LegalEntity);
			RuleServ.ExecuteRule(_view.Messages, "LegalEntityMandatoryName", base.LegalEntity);  // First name and Surname
			RuleServ.ExecuteRule(_view.Messages, "LegalEntityNaturalPersonUpdateProfilePreferedName", base.LegalEntity); // PreferedName
			RuleServ.ExecuteRule(_view.Messages, "LegalEntityNaturalPersonUpdateProfileContactDetails", base.LegalEntity); // worke code & work number, cellphone, email

			if (_view.IsValid)
			{
				this.ExclusionSets.Add(RuleExclusionSets.LegalEntityUserProfile);
				TransactionScope ts = new TransactionScope();

				try
				{
					LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);
					ts.VoteCommit();
				}
				catch (Exception)
				{
					ts.VoteRollBack();
                    if (_view.IsValid)
                        throw;
				}
				finally
				{
					ts.Dispose();
				}
				if (this.ExclusionSets.Contains(RuleExclusionSets.LegalEntityUserProfile))
					this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityUserProfile);
			}

			if (_view.Messages.Count == 0)
				base.OnSubmitButtonClicked(sender, e);
		}
	}
}
