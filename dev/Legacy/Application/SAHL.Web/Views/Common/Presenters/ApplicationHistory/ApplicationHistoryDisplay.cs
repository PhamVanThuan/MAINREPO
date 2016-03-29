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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;


namespace SAHL.Web.Views.Common.Presenters.ApplicationHistory
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationHistoryDisplay : SAHLCommonBasePresenter<IApplicationHistory>
    {
        private CBOMenuNode _node;
        private IApplication _application;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationHistoryDisplay(IApplicationHistory view, SAHLCommonBaseController controller)
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
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // get the cbo node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            IApplicationRepository appRep = RepositoryFactory.GetRepository<IApplicationRepository>();
            _application = appRep.GetApplicationByKey(_node.GenericKey);
            //(136116);

            if (_view.Messages.ErrorMessages.Count == 0)
            {
                _view.application = _application;                

                _view.BindGrid();
            }
            else
            {
                // todo : handle error
            }
        }
    }
}
