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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class ITCLegalEntity : ITCBase
    {
        private SAHL.Common.BusinessModel.Interfaces.ILegalEntity _legalEntity;

        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ITCLegalEntity(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            //_view.OnDoEnquiryButtonClicked += new EventHandler(_view_OnDoEnquiryButtonClicked);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // get the parent node
            CBONode parentNode = base.Node.ParentNode;

            switch (parentNode.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    SAHL.Common.BusinessModel.Interfaces.IAccount account = accRepo.GetAccountByKey(parentNode.GenericKey);
                    base.AccountSequence = accRepo.GetAccountSequenceByKey(account.Key);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication application = appRepo.GetApplicationByKey(parentNode.GenericKey);
                    base.AccountSequence = application.ReservedAccount;
                    break;
                default:
                    break;
            }

            // get the legalentity object
            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _legalEntity = legalEntityRepo.GetLegalEntityByKey(base.GenericKey);

            // add the legal to the collection
            if (_legalEntity is ILegalEntityNaturalPerson)
                base.ListLE.Add(_legalEntity as ILegalEntityNaturalPerson);

            // call the base to do the rest of the processing
            base.OnViewInitialised(sender, e);
        }
    }
}