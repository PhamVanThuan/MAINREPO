using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PremiumQuoteWorkFlow : PremiumQuoteBase
    {
        private IApplicationLife _applicationLife;
        private SAHL.Common.BusinessModel.Interfaces.IAccount _loanAccount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PremiumQuoteWorkFlow(IPremiumQuote view, SAHLCommonBaseController controller)
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

            // Get the Life Offer object
            _applicationLife = ApplicationRepo.GetApplicationLifeByKey(base.GenericKey);

            // Get the Life Account object
            IAccountLifePolicy accountLifePolicy = _applicationLife.Account as IAccountLifePolicy;

            // Get the Loan Account Object
            _loanAccount = accountLifePolicy.ParentMortgageLoan; 

            // Get the HOC Financial Service object
            HOCAccount = _loanAccount.GetRelatedAccountByType(AccountTypes.HOC, _loanAccount.RelatedChildAccounts) as IAccountHOC;
            if (HOCAccount != null)
                HocFS = HOCAccount.HOC;

            _view.ShowWorkFlowHeader = true;

            // Select the Policy Type
            _view.PolicyTypeSelectedValue = _applicationLife.LifePolicyType.Key;

            // Bind the Premium Details
            _view.BindPremiumDetails(_applicationLife, _loanAccount, HocFS);

            // Bind Assured lives
            IReadOnlyEventList<ILegalEntity> lstLE = _applicationLife.Account.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife });
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

        void OnCalculateButtonClicked(object sender, EventArgs e)
        {
            bool success = base.CalculatePremiums(_applicationLife.Account.Key);

            if (success)
            {
                // populate fields with values returned from calculation
                _applicationLife.CurrentSumAssured = base.CurrentSumAssured;
                _applicationLife.DeathBenefitPremium = base.DeathBenefitPremium;
                _applicationLife.InstallmentProtectionPremium = base.IPBenefitPremium;
                _applicationLife.YearlyPremium = base.AnnualPremium;
                _applicationLife.MonthlyPremium = base.MonthlyInstalment;

                // Bind the Premium Details
                _view.BindPremiumDetails(_applicationLife, _loanAccount, HocFS);
            }
        }
    }
}
