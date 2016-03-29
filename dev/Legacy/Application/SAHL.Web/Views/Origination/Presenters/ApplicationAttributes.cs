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
using SAHL.Web.Views.Origination.Interfaces;

using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationAttributes : SAHLCommonBasePresenter<IApplicationAttributes>
    {
        CBOMenuNode _node;
        int offerKey;
      
        IApplicationRepository appRepo;

        public ApplicationAttributes(IApplicationAttributes view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            offerKey = _node.GenericKey;
           
            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationMortgageLoan appMortgageLoan = appRepo.GetApplicationByKey(offerKey) as IApplicationMortgageLoan;
            
            ILookupRepository _lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
                       
            IList<IApplicationAttributeType> applicationAttributes = appRepo.GetApplicationAttributeTypeByIsGeneric(true);
            IEventList<IApplicationAttributeType> appAttributes = new EventList<IApplicationAttributeType>(applicationAttributes);
            
            _view.PopulateAttributes(appAttributes);
            
            _view.PopulateMarketingSource(_lookUps.ApplicationSources);

            _view.BindApplication(appMortgageLoan);

            _view.ShowButtons = false;
            _view.showControlsForDisplay();

        }
    }
  
}
