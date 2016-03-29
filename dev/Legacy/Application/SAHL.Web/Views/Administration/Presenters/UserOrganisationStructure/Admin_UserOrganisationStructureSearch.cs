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
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.UserOrganisationStructure
{
    public class Admin_UserOrganisationStructureSearch : Admin_UserOrganisationStructureBase
    {

        public Admin_UserOrganisationStructureSearch(IAdmin_UserOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {   }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnRowItemSelected += new KeyChangedEventHandler(_view_OnRowItemSelected);
            _view.ADUserSearchButtonClicked += new KeyChangedEventHandler(_view_ADUserSearchButtonClicked);
            _view.OnViewADUserHistClicked += new KeyChangedEventHandler(_view_OnViewADUserHistClicked);
            _view.ADUserResultsGridPageIndexChanged += new KeyChangedEventHandler(_view_ADUserResultsGridPageIndexChanged);
            _view.OnAddButtonClicked += new KeyChangedEventHandler(_view_OnAddButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(_view_OnRemoveButtonClicked);

            if (_view.IsPostBack)
            {
                _view.SetUpADUserResultsGridSelect();
                ADUserSearch();
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (!_view.IsPostBack)
            {
                _view.SetUpADUserResultsGridSelect();
            }

            SetUpView();
        }

        protected void _view_ADUserSearchButtonClicked(object sender, KeyChangedEventArgs e)
        {
            this.GlobalCacheData.Remove(SelectedADUser);
            this.GlobalCacheData.Remove(SearchADUserText);
            this.GlobalCacheData.Remove(ADUserResultsGridPageIndex);
            this.GlobalCacheData.Remove(ADUserResultsGridFocusedRowIndex);

            if (!string.IsNullOrEmpty(_view.ADUserName.Trim()))
            {
                if (ADUserSearch())
                {
                    _view.ClearADUserResultsGrid();
                    _view.ADUserResultsGridButtonsVisible = true;
                }
                this.GlobalCacheData.Add(SearchADUserText, _view.ADUserName.Trim(), LifeTimes);
            }
            else
            {
                string errMsg = "Please enter an aduser name or partial text.";
                _view.Messages.Add(new Error(errMsg, errMsg));
            }
        }

        protected void _view_OnRowItemSelected(object sender, KeyChangedEventArgs e)
        {
            if (this.GlobalCacheData.ContainsKey(SelectedADUser))
                this.GlobalCacheData[SelectedADUser] = e.Key;
            else
                this.GlobalCacheData.Add(SelectedADUser, e.Key, LifeTimes);

            _view.ADUserResultsGridClear();
        }

        protected void _view_OnSelectADUserClicked(object sender, KeyChangedEventArgs e)
        {
            if (this.GlobalCacheData.ContainsKey(SelectedADUser))
                this.GlobalCacheData[SelectedADUser] = e.Key;
            else
                this.GlobalCacheData.Add(SelectedADUser, e.Key, LifeTimes);

            _view.ADUserResultsGridClear();
        }

        protected void _view_ADUserResultsGridPageIndexChanged(object sender, KeyChangedEventArgs e)
        {
            ADUserSearch();
        }

        #region Navigation Events

        protected void _view_OnAddButtonClicked(object sender, KeyChangedEventArgs e)
        {
            SetNavigateAwayValues(e);
            _view.Navigator.Navigate("Add");
        }

        protected void _view_OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            SetNavigateAwayValues(e);
            _view.Navigator.Navigate("Remove");
        }

        protected void _view_OnViewADUserHistClicked(object sender, KeyChangedEventArgs e)
        {
            SetNavigateAwayValues(e);
            _view.Navigator.Navigate("History");
        }

        #endregion

        #region Helper Methods

        private void SetNavigateAwayValues(KeyChangedEventArgs e)
        {
            if (this.GlobalCacheData.ContainsKey(SelectedADUser))
                this.GlobalCacheData[SelectedADUser] = e.Key;
            else
                this.GlobalCacheData.Add(SelectedADUser, e.Key, LifeTimes);

            if (this.GlobalCacheData.ContainsKey(ADUserResultsGridPageIndex))
                this.GlobalCacheData[ADUserResultsGridPageIndex] = _view.ADUserResultsGridPageIndex;
            else
                this.GlobalCacheData.Add(ADUserResultsGridPageIndex, _view.ADUserResultsGridPageIndex, LifeTimes);

            if (this.GlobalCacheData.ContainsKey(ADUserResultsGridFocusedRowIndex))
                this.GlobalCacheData[ADUserResultsGridFocusedRowIndex] = _view.ADUserResultsGridFocusedRowIndex;
            else
                this.GlobalCacheData.Add(ADUserResultsGridFocusedRowIndex, _view.ADUserResultsGridFocusedRowIndex, LifeTimes);

            if (this.GlobalCacheData.ContainsKey(DoRebind))
                this.GlobalCacheData[DoRebind] = true;
            else
                this.GlobalCacheData.Add(DoRebind, true, LifeTimes);
        }

        private void SetUpView()
        {
            _view.LabelHeadingText = "Please select the correct ADUser.";
            _view.SearchViewCustomSetUp();

            if (!_view.IsPostBack)
            {
                if (this.GlobalCacheData.ContainsKey(DoRebind))
                {
                    this.GlobalCacheData.Remove(SelectedADUser);
                    this.GlobalCacheData.Remove(SelectedNodes);
                    this.GlobalCacheData.Remove(SelectedCompany);
                    this.GlobalCacheData.Remove(ADUserResultsGridPageIndex);
                    this.GlobalCacheData.Remove(ADUserResultsGridFocusedRowIndex);
                    this.GlobalCacheData.Remove(DoRebind);
                }
                else 
                {
                    this.GlobalCacheData.Remove(SelectedADUser);
                    this.GlobalCacheData.Remove(SelectedNodes);
                    this.GlobalCacheData.Remove(SearchADUserText);
                    this.GlobalCacheData.Remove(SelectedCompany);
                    this.GlobalCacheData.Remove(ADUserResultsGridPageIndex);
                    this.GlobalCacheData.Remove(ADUserResultsGridFocusedRowIndex);
                    this.GlobalCacheData.Remove(DoRebind);

                }
            }

            if (ADUserSearch())
            {
                _view.ClearADUserResultsGrid();
                _view.ADUserResultsGridButtonsVisible = true;
            }
        }

        #endregion
    }
}
