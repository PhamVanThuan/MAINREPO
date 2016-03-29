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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// Display presenter showing Seller details
    /// </summary>
    public class LegalEntityDetailsDisplaySeller : LegalEntityDetailsDisplay
    {
        private IApplicationRepository _applicationRepository;

        /// <summary>
        /// Presenter Constructor. Takes the <see cref="ILegalEntityDetails">view</see> and the <see cref="SAHLCommonBaseController">Controller</see> as parameters.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsDisplaySeller(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {


        }

        /// <summary>
        /// ViewInitialised event handler. Disables the InsurableInterestVisible fieldsm, which is being used by life.
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            if (!_view.ShouldRunPage)
                return;

            if (base.Node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
            {
                // get the application
                int applicationKey = base.Node.ParentNode.GenericKey;
                IApplication application = _applicationRepository.GetApplicationByKey(applicationKey);
                if (application != null)
                {
                    // get the role of the legalentity
                    foreach (IApplicationRole arole in application.ApplicationRoles)
                    {
                        if (arole.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.Seller
                        && arole.LegalEntity.Key == base.LegalEntity.Key)
                        {
                            _view.ApplicantRoleTypeKey = arole.ApplicationRoleType.Key;
                            break;
                        }
                    }
                }
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.DisplayRoleTypeVisible = true;
            _view.UpdateRoleTypeVisible = false;
        }
    }
}
