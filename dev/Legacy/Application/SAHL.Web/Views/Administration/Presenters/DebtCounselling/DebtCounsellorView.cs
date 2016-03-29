﻿using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
    /// <summary>
    /// 
    /// </summary>
    public class DebtCounsellorView : DebtCounsellorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebtCounsellorView(IExternalOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #region Events

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

            // Set DragDrop Functionality
            // If the Add, Update & Delete buttons are disabled then this will be set to false
            _view.AllowNodeDragging = true;

            _view.SearchServiceMethod = SAHL.Web.AJAX.WebServiceUrls.GetDebtCounsellorByNCRRegistrationNumber;
            _view.AllowSearch = true;
            // Set "Add to CBO" button visibility
            _view.AllowAddToCBO = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;
        }

        #endregion

    }
}
