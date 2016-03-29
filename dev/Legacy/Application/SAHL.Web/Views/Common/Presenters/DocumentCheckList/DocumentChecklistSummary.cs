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
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Common.UI;

using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Threading;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;


namespace SAHL.Web.Views.Common.Presenters.DocumentCheckList
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentChecklistSummary : SAHLCommonBasePresenter<IDocumentChecklist>
    {
        protected CBOMenuNode _node;
        int _genericKey;
        int _genericKeyTypeKey;
        IList<IApplicationDocument> _doclist;
        IDocumentCheckListService _dcs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DocumentChecklistSummary(IDocumentChecklist view, SAHLCommonBaseController controller)
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

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _genericKey = Convert.ToInt32(_node.GenericKey);
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            if (_genericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                _dcs = ServiceFactory.GetService<IDocumentCheckListService>();
                _doclist = _dcs.GetList(_genericKey);
                _view.SetViewOnly = true;
                _view.HideControls = false;
                if (_doclist.Count > 0)
                {
                    _view.BindDocumentList(_doclist);
                }
            }
        }
    }
}
