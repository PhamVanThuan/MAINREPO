using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;

using SAHL.Common.UI;
namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Base presenter for Account Mailing Address
    /// </summary>
    public class AccountMailingAddressBase : SAHLCommonBasePresenter<IAccountMailingAddress>
    {
        /// <summary>
        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;
        /// <summary>
        /// Holds the value of the generic key from the CBO Node
        /// </summary>
        protected int _genericKey;
        /// <summary>
        /// Holds the value of the generic key type key from the CBO Node
        /// </summary>
        protected int _genericKeyTypeKey;
        /// <summary>
        /// Account Repository
        /// </summary>
        protected IAccountRepository accRepo;
        /// <summary>
        /// Account 
        /// </summary>
        protected IAccount account;
        /// <summary>
        /// EventList of Mailing Address
        /// </summary>
        protected IEventList<IMailingAddress> accMailingAddress;
        /// <summary>
        /// List of Mailing Address
        /// </summary>
        protected IList<string> mailingAddressLst;

        public AccountMailingAddressBase(IAccountMailingAddress view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node != null)
            {
                _genericKey = Convert.ToInt32(_node.GenericKey);
                _genericKeyTypeKey = Convert.ToInt32(_node.GenericKeyTypeKey);
            }
            accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            account = accRepo.GetAccountByKey(_genericKey);
            accMailingAddress = account.MailingAddresses;
            
            mailingAddressLst = new List<string>();
            for (int i = 0; i < accMailingAddress.Count; i++)
            {
                mailingAddressLst.Add(accMailingAddress[i].Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma));
            }
        }
    }
}
