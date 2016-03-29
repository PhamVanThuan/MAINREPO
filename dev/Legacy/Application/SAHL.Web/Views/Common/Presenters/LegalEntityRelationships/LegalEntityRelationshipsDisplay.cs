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
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.CacheData;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityRelationships
{
    public class LegalEntityRelationshipsDisplay : LegalEntityRelationshipsBase
    {
        private CBONodeSetType _currentNodeSet;

        public LegalEntityRelationshipsDisplay(ILegalEntityRelationships view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _currentNodeSet = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal).CurrentNodeSetType;

            _view.GridPostBackType = GridPostBackType.DoubleClickWithClientSelect;

            // if we are in workflow then disable grid selection as we dont want to add to cbo
            if (_currentNodeSet == CBONodeSetType.X2)
                _view.GridPostBackType = GridPostBackType.NoneWithClientSelect;


            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) 
                return;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.CancelButtonVisible = false;
            _view.SubmitButtonVisible = false;
            _view.ActionTableVisible = false;
            _view.LegalEntityInfoTableVisible = false;

            // if we are in workflow then dont display the "Add to CBO" button
            if (_currentNodeSet == CBONodeSetType.X2)
                _view.AddToCBOButtonVisible = false;
        }
    }
}
