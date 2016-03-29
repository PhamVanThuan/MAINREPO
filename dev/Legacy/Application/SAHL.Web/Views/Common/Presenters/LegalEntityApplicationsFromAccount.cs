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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityApplicationsFromAccount : LegalEntityApplicationsBase
    {
        private IAccountRepository _accountRepo;
        private IFinancialServiceRepository _financialServceRepo;
        private CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityApplicationsFromAccount(ILegalEntityApplications view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage)
                return;

            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            int accountKey = -1;
            switch (_node.GenericKeyTypeKey)
            {
               case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                   accountKey = _node.GenericKey;
                   break;

               case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                   _financialServceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                   accountKey = _financialServceRepo.GetFinancialServiceByKey(_node.GenericKey).Account.Key;
                   break;

                default:
                    break;
            }
            // Get the account
            IAccount account = _accountRepo.GetAccountByKey(accountKey);

            // Get the Applications for the Account
            base.ApplicationsList = new EventList<IApplication>();

            foreach (IApplication application in account.Applications)
            {
                base.ApplicationsList.Add(_view.Messages, application);
            }

            _view.GridHeading = "Applications Summary";

            // call the base event that will handle the binding
            base.OnViewInitialised(sender, e);
        }
    }
}

