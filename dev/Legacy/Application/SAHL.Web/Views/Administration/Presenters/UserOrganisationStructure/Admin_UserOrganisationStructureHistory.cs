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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using System.Threading;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.UserOrganisationStructure
{
    public class Admin_UserOrganisationStructureHistory : Admin_UserOrganisationStructureBase
    {
        public Admin_UserOrganisationStructureHistory(IAdmin_UserOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new KeyChangedEventHandler(_view_OnCancelButtonClicked);
            _view.ADUserResultsGridPageIndexChanged += new KeyChangedEventHandler(_view_ADUserResultsGridPageIndexChanged);

            _view.ADUserResultsGridTitle = "User Organisation Structure History";
            _view.SetUpUserOrgHistoryGridView();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ADUserSearchVisible = false;
            _view.CompanyListVisble = false;
            _view.OrgStructVisible = false;
            _view.UserSummaryGridVisible = false;
            if (this.GlobalCacheData.ContainsKey(DoRebind))
            {
                _view.SubmitButtonVisible = true;
                _view.SubmitButtonText = "Back";
            }
            else
            {
                _view.CancelButtonVisible = true;
            }
            
            BindUserOrgHistoryGrid();
        }

        protected void _view_ADUserResultsGridPageIndexChanged(object sender, KeyChangedEventArgs e)
        {
            BindUserOrgHistoryGrid();
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
        }

        #region Helper Methods

        private void BindUserOrgHistoryGrid()
        {
            int adUserKey = Convert.ToInt32(this.GlobalCacheData[SelectedADUser]);
            DataTable dt = _osRepo.GetUserOrganisationStructureHistory(adUserKey);
            _view.BindADUserResultsGrid(dt);
        }

        #endregion
    }
}
