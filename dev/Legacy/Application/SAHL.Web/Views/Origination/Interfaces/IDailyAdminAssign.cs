using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Data;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IDailyAdminAssign : IViewBase
    {
        event KeyChangedEventHandler onRoleTypeSelectedIndexChange;
        event KeyChangedEventHandler OnRowUpdating;
        event EventHandler onSubmitButtonClicked;
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        void BindGridPostRowUpdate(DataTable dt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generalStatus"></param>
        void SetUpUserStatusGrid(ICollection<IGeneralStatus> generalStatus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appRoleTypes"></param>
        void BindRoleTypes(Dictionary<string, string> RoleTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        void PopulateUsersInGrid(DataTable dt);
    }
}
