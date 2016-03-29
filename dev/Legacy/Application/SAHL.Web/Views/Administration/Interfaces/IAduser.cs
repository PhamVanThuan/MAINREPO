using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAduser : IViewBase
    {
        /// <summary>
        /// Sets whether the Add / UPdate button is visible
        /// </summary>
        bool VisibleSubmit { set; }
        /// <summary>
        /// Sets whether or not the Add / Update area is visible
        /// </summary>
        bool VisibleMaint { set; }     
        /// <summary>
        /// 
        /// </summary>
        bool UserExistsInDatabase { set; }
        /// <summary>
        /// 
        /// </summary>
        string SelectedUserName { get; set; }

        /// <summary>
        /// binds a single user to the update area to be updated.
        /// </summary>
        /// <param name="ActiveDirectoryUser"></param>
        /// <param name="ADUser"></param>
        void BindAdUser(ActiveDirectoryUserBindableObject ActiveDirectoryUser, IADUser ADUser);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generalStatus"></param>
        void BindStatusDropDown(ICollection<IGeneralStatus> generalStatus);
        /// <summary>
        /// Fires when an AdUser is selected in the ajax search control
        /// </summary>
        event KeyChangedEventHandler OnAdUserSelected;
        /// <summary>
        /// Fires when an AdUser is Added / Updates and the submit button is clicked.
        /// </summary>
        event EventHandler OnSubmitClick;

        /// <summary>
        /// 
        /// </summary>
        string AdUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string FirstName { get; }
        /// <summary>
        /// 
        /// </summary>
        string Surname { get; }
        /// <summary>
        /// 
        /// </summary>
        string CellPhoneNumber { get; }
        /// <summary>
        /// 
        /// </summary>
        string EMail { get; }
        /// <summary>
        /// 
        /// </summary>
        int GeneralStatusKey { get; }
        
    }
    /// <summary>
    /// 
    /// </summary>
    public class ADUserBind
    {
        /// <summary>
        /// 
        /// </summary>
        public int _AdUserKey;
        /// <summary>
        /// 
        /// </summary>
        public int AdUserKey { get { return _AdUserKey; } }
        /// <summary>
        /// 
        /// </summary>
        public string _ADUserName;
        /// <summary>
        /// 
        /// </summary>
        public string ADUserName { get { return _ADUserName; } }
        /// <summary>
        /// 
        /// </summary>
        public string _FirstName;
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get { return _FirstName; } }
        /// <summary>
        /// 
        /// </summary>
        public string _Surname;
        /// <summary>
        /// 
        /// </summary>
        public string Surname { get { return _Surname; } }
        /// <summary>
        /// 
        /// </summary>
        public string _CellPhoneNumber;
        /// <summary>
        /// 
        /// </summary>
        public string CellPhoneNumber { get { return _CellPhoneNumber; } }
        /// <summary>
        /// 
        /// </summary>
        public string _EMail;
        /// <summary>
        /// 
        /// </summary>
        public string EMail { get { return _EMail; } }
        /// <summary>
        /// 
        /// </summary>
        public ADUserBind(IADUser a)
        {
            _AdUserKey = a.Key;
            _ADUserName = a.ADUserName;
            _FirstName = a.LegalEntity.FirstNames;
            _Surname = a.LegalEntity.Surname;
            _CellPhoneNumber = a.LegalEntity.CellPhoneNumber;
            _EMail = a.LegalEntity.EmailAddress;
        }
    }
}
