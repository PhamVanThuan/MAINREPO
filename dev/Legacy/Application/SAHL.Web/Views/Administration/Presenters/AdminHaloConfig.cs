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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class AdminHaloConfig : SAHLCommonBasePresenter<IHaloConfig>
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AdminHaloConfig(IHaloConfig view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
            _view.BindControlTableValues(lookUps.Controls);

        }


    }
}
