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
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.CacheData;



namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CallBackHold : SAHLCommonBasePresenter<ICallBackHold>
    {
        private IApplicationRepository _applicationRepo;
        private IEventList<ICallback> _callBacks;

        private CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CallBackHold(SAHL.Web.Views.Life.Interfaces.ICallBackHold view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

      
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _callBacks = _applicationRepo.GetCallBacksByApplicationKey((int)_node.GenericKey, false);

            _view.BindCallBackGrid(_callBacks);
        }

    }
}
