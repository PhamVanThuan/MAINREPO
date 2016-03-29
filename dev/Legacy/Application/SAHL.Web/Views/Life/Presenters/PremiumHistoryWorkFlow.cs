using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PremiumHistoryWorkFlow : PremiumHistoryBase
    {
        private IApplicationRepository _applicationRepository;
        private IApplication _application;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PremiumHistoryWorkFlow(IPremiumHistory view, SAHL.Common.Web.UI.SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // get the life application object
            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            _application = _applicationRepository.GetApplicationByKey(base.GenericKey);

            base.AccountKey = _application.Account.Key;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowLifeWorkFlowHeader = true;
            _view.ShowCancelButton = true;
        }
    }
}
