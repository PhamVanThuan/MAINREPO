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
    public class LegalEntityApplications : LegalEntityApplicationsBase
    {
        private ILegalEntityRepository _legalEntityRepo;
        private CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityApplications(ILegalEntityApplications view, SAHLCommonBaseController controller)
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

            _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            // Find the Legal Entity node
            var legalEntityNode = _node.GetParentNodeByType(GenericKeyTypes.LegalEntity);

            // Get the legal entity
            ILegalEntity legalEntity = _legalEntityRepo.GetLegalEntityByKey(legalEntityNode.GenericKey);

            // Get the Applications for the LegalEntity
            base.ApplicationsList = new EventList<IApplication>();

            foreach (IApplicationRole applicationRole in legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
            {
                base.ApplicationsList.Add(_view.Messages, applicationRole.Application);
            }

            var clientRoles = _legalEntityRepo.GetExternalRoles(GenericKeyTypes.Offer, ExternalRoleTypes.Client, legalEntity.Key);

            foreach (IExternalRole r in clientRoles)
            {
                base.ApplicationsList.Add(_view.Messages, ApplicationRepository.GetApplicationByKey(r.GenericKey));
            }

            _view.GridHeading = "Legal Entity Applications";

            // call the base event that will handle the binding
            base.OnViewInitialised(sender, e);

        }

    }
}

