using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Interfaces
{
	/// <summary>
	/// Attorney Interface
	/// </summary>
    public interface IInternetLeadUsers : IViewBase
    {
        //event EventHandler btnRefreshClick;
        event EventHandler btnUpdateClick;
        event EventHandler btnCancelClick;

        event EventHandler btnAddClick;
        event EventHandler btnRemoveClick;

        event KeyChangedEventHandler lstActiveUsersSelectedIndexChanged;
        event KeyChangedEventHandler lstInactiveUsersSelectedIndexChanged;

	    void PopulatelstActiveUsers(DataTable activeusers);
	    void PopulatelstInactiveUsers(DataTable inactiveusers);

        int InactiveUsersIndex { get; set;}
        int ActiveUsersIndex { get; set;}


    }
}
