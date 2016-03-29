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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;


namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILegalAttorney : IViewBase
    {
        /// <summary>
        /// Hide Attorney Panel
        /// </summary>
        void HideAttorneyPanel();
        /// <summary>
        /// Hide User Panel
        /// </summary>
        void HideRegistrationUserPanel();
        /// <summary>
        /// Bind Registration Users
        /// </summary>
        /// <param name="adUserLst"></param>
        void BindRegistrationUsers(IEventList<IADUser> adUserLst);

        void BindDeedsOffice(IEventList<IDeedsOffice> deedsOffice);

        /// <summary>
        /// Get selected AD User
        /// </summary>
        int GetADUserSelected {get;}
        /// <summary>
        /// Get selected AD User
        /// </summary>
        void SetADUserSelected (int adUserKey);
        /// <summary>
        /// Get Selected Attorney
        /// </summary>
        int GetSetAttorneySelected { get; set;}
        /// <summary>
        /// Bind Registration Attorneys
        /// </summary>
        /// <param name="attorneys"></param>
        void BindRegistrationAttorneys(IList<IAttorney> attorneys);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deedOfficeKey"></param>
        void SetAttorneyDeedsOffice(int deedOfficeKey);

        #region EventHandlers
        /// <summary>
        /// On Update button clicked
        /// </summary>
        event EventHandler OnUpdateButtonClicked;

        event KeyChangedEventHandler OnDeedsOfficeSelectedIndexChanged;

        #endregion

    }
}
