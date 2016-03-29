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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IAdmin_UserOrganisationStructure : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        int ADUserResultsGridPageIndex { get;set;}
        /// <summary>
        /// 
        /// </summary>
        int ADUserResultsGridFocusedRowIndex { get;set;}
        /// <summary>
        /// 
        /// </summary>
        bool OrgStructVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        string ADUserName { get;}
        /// <summary>
        /// 
        /// </summary>
        string CompanySelectedValue { get;set;}
        /// <summary>
        /// 
        /// </summary>
        string ADUserResultsGridTitle { set;}
        /// <summary>
        /// 
        /// </summary>
        string LabelHeadingText { set;}
        /// <summary>
        /// 
        /// </summary>
        bool UserSummaryGridVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ADUserSearchVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool CompanyListVisble { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ADUserSearchResultsVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        string SubmitButtonText { set;}
        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, bool> SelectedNodes { get;}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        void UserCheckedNodes(Dictionary<int, bool> nodes);
        /// <summary>
        /// 
        /// </summary>
        void ClearADUserResultsGrid();
        /// <summary>
        /// 
        /// </summary>
        bool CanAddNode { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ADUserResultsGridButtonsVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool CancelButtonVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        void SetUpADUserResultsGridView();
        /// <summary>
        /// 
        /// </summary>
        void SetUpADUserResultsGridSelect();
        /// <summary>
        /// 
        /// </summary>
        void SetUpUserOrgHistoryGridView();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADUsers"></param>
        void BindADUserResultsGrid(DataTable ADUsers);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyList"></param>
        void BindCompanyList(IEventList<IOrganisationStructure> companyList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgStructLst"></param>
        /// <param name="ht"></param>
        void BindOrganisationStructure(IList<IBindableTreeItem> orgStructLst);
        /// <summary>
        /// 
        /// </summary>
        void ClearOrganisationStructure();
        /// <summary>
        /// 
        /// </summary>
        void SetUpUserSummaryGrid();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        void BindUserSummaryGrid(DataTable dt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        void BindUserSummaryGridPostRowUpdate(DataTable dt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DictSelectedNodes"></param>
        void CheckAndExpandNodes(Dictionary<int, bool> DictSelectedNodes);
        /// <summary>
        /// 
        /// </summary>
        void SearchViewCustomSetUp();
        /// <summary>
        /// 
        /// </summary>
        void ADUserResultsGridClear();
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnRowItemSelected;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler ADUserSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnSelectedCompanyChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler UserSummaryGridRowUpdating;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnViewADUserHistClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler ADUserResultsGridPageIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnAddButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnRemoveButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnValidate;

		/// <summary>
		/// 
		/// </summary>
		/// 
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Not a read only property.")]
		Dictionary<string, string> RoleTypes { get;set;}
    }
}
