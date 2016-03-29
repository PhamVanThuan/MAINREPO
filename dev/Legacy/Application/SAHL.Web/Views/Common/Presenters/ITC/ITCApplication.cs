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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class ITCApplication : ITCBase
    {
        private SAHL.Common.BusinessModel.Interfaces.IApplication _application;

        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ITCApplication(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            //_view.OnDoEnquiryButtonClicked += new EventHandler(_view_OnDoEnquiryButtonClicked);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            // get the application object
            _application = appRepo.GetApplicationByKey(base.GenericKey);

            base.AccountSequence = _application.ReservedAccount;

            IReadOnlyEventList<ILegalEntityNaturalPerson> lst = null;

            // get the legal entity roles off the application
            int[] roles =
                {
                    (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant,
                    (int)SAHL.Common.Globals.OfferRoleTypes.Suretor,
                    (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant,
                    (int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor
                };

            lst = _application.GetNaturalPersonLegalEntitiesByRoleType(_view.Messages, roles);

            if (lst != null && lst.Count > 0)
            {
                base.ListLE = new List<ILegalEntityNaturalPerson>(lst);
            }

            // call the base to do the rest of the processing
            base.OnViewInitialised(sender, e);
        }
    }
}