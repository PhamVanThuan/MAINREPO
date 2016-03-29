using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class LoanSummaryDisplay : LoanSummaryBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanSummaryDisplay(ILoanSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                int accountKey = -1;
                switch (cboNode.GenericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                        accountKey = cboNode.GenericKey;
                        break;

                    case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                        IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                        IApplication application = appRepo.GetApplicationByKey(cboNode.GenericKey);
                        accountKey = application.ReservedAccount.Key;
                        break;

                    default:
                        break;
                }

                if (accountKey > 0)
                {
                    IAccount account = _accRepository.GetAccountByKey(accountKey);
                    BindAccountToView(account);
                    CheckRules(account);
                }
            }
        }

        /// <summary>
        /// Run these RULES before loading the screen
        /// </summary>
        public void CheckRules(IAccount _account)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "AccountUnderForeClosure", _account);
            svc.ExecuteRule(spc.DomainMessages, "AccountDebtCounseling", _account);
            svc.ExecuteRule(spc.DomainMessages, "LegalEntitiesUnderDebtCounsellingForAccount", _account);
            svc.ExecuteRule(spc.DomainMessages, "ITCAccountApplicationDisputeIndicated", _account);
            svc.ExecuteRule(spc.DomainMessages, "CheckDisputes", _account);
            svc.ExecuteRule(spc.DomainMessages, "ProductVarifixOptInFlag", _account);
            svc.ExecuteRule(spc.DomainMessages, "AccountIsAlphaHousing", _account);
            svc.ExecuteRule(spc.DomainMessages, "NaedoDebitOrderPending", _account);
            svc.ExecuteRule(spc.DomainMessages, "ActiveSubsidyAndSalaryStopOrderConditionExistsError", _account);
        }
    }
}