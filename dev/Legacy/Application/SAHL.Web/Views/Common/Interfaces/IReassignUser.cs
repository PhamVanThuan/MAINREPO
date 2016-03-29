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

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IReassignUser : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler onSelectedConsultantChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler onSelectedRoleChanged;

        /// <summary>
        /// 
        /// </summary>
        int SelectedConsultantKey { get;}
        /// <summary>
        /// 
        /// </summary>
        string SetDropDownText { set; }
        /// <summary>
        /// 
        /// </summary>
        string SetHeaderText { set; }
        /// <summary>
        /// 
        /// </summary>
        int SelectedRoleTypeKey { get;}
        /// <summary>
        /// 
        /// </summary>
        bool SetPostBackType { set;}
        /// <summary>
        /// 
        /// </summary>
        bool SetPostBackTypeRole { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ConsultantsRowVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool RoleVisible { set;}
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
        bool ShowGrid { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ShowCommentRow { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ShowCheckBoxRow { set;}
        /// <summary>
        /// 
        /// </summary>
        bool CheckBoxValue { get;}
        /// <summary>
        /// 
        /// </summary>
        string MemoDescription { get;}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        void BindConsultantList(IEventList<IADUser> users);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adUsers"></param>
        void BindUsers(IEventList<IADUser> adUsers);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agencies"></param>
        void BindAgencies(IEventList<IApplicationOriginator> agencies);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppRole"></param>
        /// <param name="appOriginators"></param>
        void BindSelectedApplicationRole(IApplicationRole AppRole,IEventList<IApplicationOriginator> appOriginators);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppRole"></param>
        /// <param name="adUsers"></param>
        void BindSelectedBranchAdmin(IApplicationRole AppRole, IEventList<IADUser> adUsers);
        /// <summary>
        /// 
        /// </summary>
        void ShowRolesDropDown();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appRoles"></param>
        void BindRoles(IEventList<IApplicationRoleType> appRoles);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        void BindSelectedUser(IADUser user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultants"></param>
        void BindConsultantsAsPerMandates(Dictionary<int, string> consultants);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        void BindSelectedUserByMandate(IADUser user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appRoles"></param>
        void BindApplicationRoles(IEventList<IApplicationRole> appRoles);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appRoleLst"></param>
        void BindGridApplicationRoles(DataTable appRoleLst);
    }


}
