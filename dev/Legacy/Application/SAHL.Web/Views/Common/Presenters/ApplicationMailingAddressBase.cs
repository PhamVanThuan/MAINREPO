using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using NHibernate.Mapping;
using System.Collections;
using System.Collections.Generic;



namespace SAHL.Web.Views.Common.Presenters
{
    public class ApplicationMailingAddressBase : SAHLCommonBasePresenter<IAccountMailingAddress>
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
        protected IApplicationRepository appRepo;
        /// <summary>
        /// Account 
        /// </summary>
        protected SAHL.Common.BusinessModel.Interfaces.IApplication application;
         /// <summary>
        /// List of Mailing Address
        /// </summary>
        protected IList<string> mailingAddressLst;

        private ILegalEntityRepository legalEntityRepository;
        
        public ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (legalEntityRepository == null)
                {
                    legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                }
                return legalEntityRepository;
            }
        }

        public ApplicationMailingAddressBase(IAccountMailingAddress view, SAHLCommonBaseController controller)
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

            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            application = appRepo.GetApplicationByKey(_genericKey);

            mailingAddressLst = new List<string>();

            for (int i = 0; i < application.ApplicationMailingAddresses.Count; i++)
            {
                mailingAddressLst.Add(application.ApplicationMailingAddresses[i].Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma));
            }
        }
    }
}
