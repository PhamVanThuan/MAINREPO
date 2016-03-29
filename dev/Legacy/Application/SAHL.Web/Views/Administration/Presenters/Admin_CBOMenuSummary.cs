using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_CBOMenuSummary : SAHLCommonBasePresenter<IViewCBOSummary>
    {
        public Admin_CBOMenuSummary(SAHL.Web.Views.Administration.Interfaces.IViewCBOSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) return;

            _view.OnFinishClick += new EventHandler(_view_OnFinishClick);
            BindCBO();
        }

        void _view_OnFinishClick(object sender, EventArgs e)
        {
            ICBOMenu CBO = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            using (new TransactionScope())
            {
                repo.SaveCBO(CBO);
            }
        }

        protected void BindCBO()
        {
            ICBOMenu CBO = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            _view.BindCBO(CBO);
        }
    }
}
