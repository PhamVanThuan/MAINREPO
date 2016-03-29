using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class PremiumQuoteAdmin : PremiumQuoteBase
    {
        private ILifePolicy _lifePolicy;
        private IAccountLifePolicy _lifePolicyAccount;
        private SAHL.Common.BusinessModel.Interfaces.IAccount _loanAccount;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PremiumQuoteAdmin(IPremiumQuote view, SAHLCommonBaseController controller)
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

            _view.OnCalculateButtonClicked += new EventHandler(OnCalculateButtonClicked);

            // Get the Life Policy Account object
            _lifePolicyAccount = AccountRepo.GetAccountByKey(base.GenericKey) as IAccountLifePolicy;

            // Get the Life Policy Object
            _lifePolicy = _lifePolicyAccount.LifePolicy;

            // Get the Loan Account Object
            _loanAccount = _lifePolicyAccount.ParentMortgageLoan;

            // Get the HOC Financial Service Object
            HOCAccount = _loanAccount.GetRelatedAccountByType(AccountTypes.HOC, _loanAccount.RelatedChildAccounts) as IAccountHOC;
            if (HOCAccount != null)
                HocFS = HOCAccount.HOC;

            _view.ShowWorkFlowHeader = false;

            // Select the Policy Type
            _view.PolicyTypeSelectedValue = _lifePolicy.LifePolicyType.Key;

            // Bind the Premium Details
            _view.BindPremiumDetails(_lifePolicy, _lifePolicy.MonthlyPremium, _loanAccount, HocFS);

            // Bind Assured lives
            IReadOnlyEventList<ILegalEntity> lstLE = _lifePolicyAccount.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife });
            if (PrivateCacheData.ContainsKey("LegalEntityList") == false)
            {
                _view.lstLegalEntities = new List<ILegalEntity>();
                foreach (ILegalEntity LE in lstLE)
                {
                    _view.lstLegalEntities.Add(LE);
                }
                PrivateCacheData.Add("LegalEntityList", _view.lstLegalEntities);
            }
            else
            {
                _view.lstLegalEntities = (List<ILegalEntity>)PrivateCacheData["LegalEntityList"];
            }

            _view.BindAssuredLivesGrid();
        }

        private void OnCalculateButtonClicked(object sender, EventArgs e)
        {
            bool success = base.CalculatePremiums(_lifePolicy.FinancialService.Account.Key);

            if (success)
            {
                // populate fields with values returned from calculation
                _lifePolicy.CurrentSumAssured = base.CurrentSumAssured;
                _lifePolicy.DeathBenefitPremium = base.DeathBenefitPremium;
                _lifePolicy.InstallmentProtectionPremium = base.IPBenefitPremium;
                _lifePolicy.YearlyPremium = base.AnnualPremium;
                //_lifePolicy.MonthlyPremium = base.MonthlyInstalment;

                // Bind the Premium Details
                _view.BindPremiumDetails(_lifePolicy, base.MonthlyInstalment, _loanAccount, HocFS);
            }
        }
    }
}