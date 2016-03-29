using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.CreditProtectionPlan.Presenters
{
    public class LifePolicyClaimAdd : LifePolicyClaimBase
    {
        public LifePolicyClaimAdd(SAHL.Web.Views.CreditProtectionPlan.Interfaces.ILifePolicyClaim view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _view.BindClaimTypes(LookupRepository.ClaimTypes);
            _view.BindClaimStatuses(LookupRepository.ClaimStatuses);
            _view.LifePolicyClaimGrid_PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.None;
            _view.ButtonRow_visability = true;
            _view.SetupControls(false, true);

            if (lifePolicyClaims != null && lifePolicyClaims.Count > 0)
            {
                _view.BindLifePolicyClaimGrid(lifePolicyClaims);
            }

            _view.BindLifePolicyClaimFields(null, false, true);
        }

        public void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ValidateScreenInput();

            if (_view.IsValid == false)
                return;

            if (_view.LifePolicyClaimKey == 0)
            {
                using (TransactionScope txn = new TransactionScope())
                {
                    try
                    {
                        ILifePolicyClaim lifePolicyClaim = LifeRepository.CreateEmptyLifePolicyClaim();
                        lifePolicyClaim.FinancialService = financialService;
                        lifePolicyClaim.ClaimType = LifeRepository.GetClaimTypeByKey(_view.ClaimTypeKey);
                        lifePolicyClaim.ClaimStatus = LifeRepository.GetClaimStatusByKey(_view.ClaimStatusKey);
                        lifePolicyClaim.ClaimDate = _view.ClaimDate.Value;

                        LifeRepository.SaveLifePolicyClaim(lifePolicyClaim);

                        txn.VoteCommit();
                    }
                    catch (Exception)
                    {
                        txn.VoteRollBack();
                        if (_view.IsValid)
                            throw;
                    }
                }
            }

            Navigator.Navigate("LifePolicyClaimDisplay");
        }

        private void ValidateScreenInput()
        {
            string errorMessage = "";

            if (_view.ClaimTypeKey <= 0)
            {
                errorMessage = "Claim Type must be selected.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.ClaimStatusKey <= 0)
            {
                errorMessage = "Claim Status must be selected.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.ClaimDate.HasValue == false)
            {
                errorMessage = "Claim Date must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
            else if (_view.ClaimDate.Value > DateTime.Now)
            {
                errorMessage = "A future Claim Date must not be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }
    }
}