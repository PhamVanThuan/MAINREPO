using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IAssignUser : IViewBase
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
        event KeyChangedEventHandler onSelectedUserChanged;

        /// <summary>
        ///
        /// </summary>
        /// <param name="users"></param>
        void BindUsers(IEventList<IADUser> users);
    }
}