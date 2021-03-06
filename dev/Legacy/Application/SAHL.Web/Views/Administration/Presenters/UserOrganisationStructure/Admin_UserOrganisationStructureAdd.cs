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
    public class Admin_UserOrganisationStructureAdd : Admin_UserOrganisationStructureBase
    {
        public Admin_UserOrganisationStructureAdd(IAdmin_UserOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnSelectedCompanyChanged += new KeyChangedEventHandler(_view_OnSelectedCompanyChanged);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new KeyChangedEventHandler(_view_OnCancelButtonClicked);

            if (_view.IsPostBack)
            {
                BindCompanyList();
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (!_view.IsPostBack)
            {
                BindCompanyList();
            }

            SetUpView();
        }

        #region Events

        protected void _view_OnSelectedCompanyChanged(object sender, KeyChangedEventArgs e)
        {
            if (this.GlobalCacheData.ContainsKey(SelectedCompany))
                this.GlobalCacheData[SelectedCompany] = _view.CompanySelectedValue;
            else
                this.GlobalCacheData.Add(SelectedCompany, _view.CompanySelectedValue, LifeTimes);
            this.GlobalCacheData.Remove(SelectedNodes);
        }

        protected void _view_OnSubmitButtonClicked(object sender, KeyChangedEventArgs e)
        {
            BindTreeViewAdd();
            Dictionary<int, bool> selection = GetUserOrgStructAddList;

            if (_view.SelectedNodes.Count == 0)
            {
                string errorMessage = "At least one user designation must be ticked in the tree view before proceeding.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
            else if (_view.SelectedNodes.Count > 1)
            {
                string errorMessage = "Please tick only one user designation before proceeding.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.IsValid)
            {
                if (this.GlobalCacheData.ContainsKey(SelectedNodes))
                    this.GlobalCacheData[SelectedNodes] = selection;
                else
                    this.GlobalCacheData.Add(SelectedNodes, selection, LifeTimes);

                _view.Navigator.Navigate("Next");
            }
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
        }

        #endregion

        #region Helper Methods

        private void SetUpView()
        {
            if (!_view.IsPostBack && this.GlobalCacheData.ContainsKey(SelectedCompany))
            {
                _view.CompanySelectedValue = this.GlobalCacheData[SelectedCompany].ToString();
            }

            _view.LabelHeadingText = "Please select the correct Company, then untick the designations for addition.";
            _view.ADUserSearchResultsVisible = false;
            _view.ADUserSearchVisible = false;
            _view.CompanyListVisble = true;
            _view.ClearOrganisationStructure();
            BindTreeViewAdd();
            SetUpTreeViewAdd();
        }

        private void BindTreeViewAdd()
        {
            int organisationStructureKey = Convert.ToInt32(_view.CompanySelectedValue);
            List<IBindableTreeItem> treeViewList = new List<IBindableTreeItem>();
            IOrganisationStructure orgStruct = _osRepo.GetOrganisationStructureForKey(organisationStructureKey);
            UserOrganisationStructure.BindOrganisationStructure recursiveConstruct = new UserOrganisationStructure.BindOrganisationStructure(orgStruct);
            treeViewList.Add(recursiveConstruct);
            _view.OrgStructVisible = true;
            _view.BindOrganisationStructure(treeViewList);
        }

        private void SetUpTreeViewAdd()
        {
            Dictionary<int, bool> dictSelectedNodes = null;
            if (this.GlobalCacheData.ContainsKey(SelectedNodes))
            {
                dictSelectedNodes = this.GlobalCacheData[SelectedNodes] as Dictionary<int, bool>;
            }
            else
            {
                dictSelectedNodes = new Dictionary<int, bool>();
                IList<IOrganisationStructure> orgStructLst = GetCurrentUserOrgStructList;
                foreach (IOrganisationStructure os in orgStructLst)
                {
                    dictSelectedNodes.Add(os.Key, true);
                }
            }
            _view.CheckAndExpandNodes(dictSelectedNodes);
        }

        private Dictionary<int, bool> GetUserOrgStructAddList
        {
            get
            {
                Dictionary<int, bool> dictSelectedNodes = _view.SelectedNodes;
                foreach (IOrganisationStructure os in GetCurrentUserOrgStructList)
                {
                    dictSelectedNodes.Add(os.Key, true);
                }
                return dictSelectedNodes;
            }
        }

        #endregion
    }
}
