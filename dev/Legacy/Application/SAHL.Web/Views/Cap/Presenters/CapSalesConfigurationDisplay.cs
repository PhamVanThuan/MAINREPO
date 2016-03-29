using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapSalesConfigurationDisplay : CapSalesConfigurationBase
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapSalesConfigurationDisplay(ICapSalesConfiguration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.CapSalesConfigGridPostBackType = GridPostBackType.SingleClick;
            _view.ShowDisplayControls();
            BindResetDateDropDown();
            BindGrid();
            BindLabels();

        }

        /// <summary>
        /// 
        /// </summary>
        private void BindLabels()
        {
            int selectedIndex = -1;
            if (!_view.IsPostBack)
            {
                if (_capConfigDetailList != null && _capConfigDetailList.Count > 0)
                    selectedIndex = 0;
            }
            else
            {
                selectedIndex = _view.CapSalesConfigGridSelectedIndex;
            }

            if (selectedIndex != -1 && _capConfigDetailList.Count > 0)
                _view.BindLabels(_capConfigDetailList[selectedIndex]);
        }
    }
}
