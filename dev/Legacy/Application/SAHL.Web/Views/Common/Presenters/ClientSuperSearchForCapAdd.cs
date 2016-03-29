using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// must only allow searching for Natural Persons
    /// must enable 'Cancel' and 'New Assured Life' Buttons
    /// </summary>
    public class ClientSuperSearchForCapAdd : ClientSuperSearch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ClientSuperSearchForCapAdd(IClientSuperSearch view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage) return;

            // setup the buttons
            _view.CancelButtonVisible = true;
            _view.CreateNewClientButtonVisible = false;
            _view.CreateNewClientButtonText = "Select";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        protected override void OnClientSelectedClicked(object sender, ClientSuperSearchSelectedEventArgs ea)
        {

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IDomainMessageCollection dmc = spc.DomainMessages;
            IAccount _account = null;

            // clear the legalentity out of global cache if exists
            if (GlobalCacheData.ContainsKey("CapAccountKey"))
                GlobalCacheData.Remove("CapAccountKey");

             ILegalEntity selectedLegalEntity = LegalEntityRepo.GetLegalEntityByKey(ea.LegalEntityKey);
             for (int i = 0; i < selectedLegalEntity.Roles.Count; i++)
             {
                 if (selectedLegalEntity.Roles[i].Account.Product.Key == Convert.ToInt32(Products.VariableLoan) ||
                     selectedLegalEntity.Roles[i].Account.Product.Key == Convert.ToInt32(Products.VariFixLoan) ||
                     selectedLegalEntity.Roles[i].Account.Product.Key == Convert.ToInt32(Products.NewVariableLoan) ||
                     selectedLegalEntity.Roles[i].Account.Product.Key == Convert.ToInt32(Products.SuperLo))
                 {
                     _account = selectedLegalEntity.Roles[i].Account;
                     GlobalCacheData.Add("CapAccountKey", selectedLegalEntity.Roles[i].Account.Key, new List<ICacheObjectLifeTime>());
                     break;
                 }
             }

             if (_account != null)
             {
                 IRuleService svc = ServiceFactory.GetService<IRuleService>();
                 List<string> rulesToRun = new List<string>();
                 rulesToRun.Add("ApplicationCAP2QualifyStatus");
                 rulesToRun.Add("ApplicationCap2QualifyUnderCancel");
                 rulesToRun.Add("ApplicationCap2CurrentBalance");
                 rulesToRun.Add("ApplicationCap2CapTypeConfig");
                 svc.ExecuteRuleSet(spc.DomainMessages, rulesToRun, _account);
             }
             
            // This message will need to be displayed when tere LE Roles but Account was filtered out
             if (selectedLegalEntity.Roles.Count > 0 && _account == null)
             {
                 string errorMessage = string.Format("CAP2 Offer Create only allowed for Products - {0}, {1}, {2} & {3}",
                 Products.VariableLoan.ToString(), Products.VariFixLoan.ToString(), Products.NewVariableLoan.ToString(),Products.SuperLo.ToString());
                 dmc.Add(new Error(errorMessage, errorMessage));
             }

             if (_view.Messages.Count == 0)
                base.Navigator.Navigate("EntitySelected");
        }
    }
}
