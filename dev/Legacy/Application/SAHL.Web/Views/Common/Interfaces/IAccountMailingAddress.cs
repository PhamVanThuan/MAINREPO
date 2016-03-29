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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Account Mailing Address Interface
    /// </summary>
    public interface IAccountMailingAddress : IViewBase
    {
        #region eventhandlers
        /// <summary>
        /// Event Handler for MailingAddress Drop Down selected index change
        /// </summary>
        event KeyChangedEventHandler OnddlMailingAddressSelectedIndexChanged;
        /// <summary>
        /// Event Handler for Cancel Button Clicked
        /// </summary>
        event EventHandler onCancelButtonClicked;
        /// <summary>
        /// Event Handler for Submit Button Clicked
        /// </summary>
        event EventHandler onSubmitButtonClicked;
        /// <summary>
        /// Event Handler for Audit Trail Button Clicked
        /// </summary>
        event EventHandler onAuditTrailButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnddlCorrespondenceMediumSelectedIndexChanged;

        #endregion 
        
        #region Methods
        /// <summary>
        /// Bind List of Mailing Addresses for Display
        /// </summary>
        /// <param name="mailingAddressLst"></param>
        void BindMailingAddressLstDisplay(IList<string> mailingAddressLst);
        /// <summary>
        /// Set up view controls for display
        /// </summary>
        void SetControlsForDisplay();
        /// <summary>
        /// Bind Display Fields
        /// </summary>
        /// <param name="mailingAddress"></param>
        void BindDisplayFields(IMailingAddress mailingAddress);
        /// <summary>
        /// Bind Display Fields for Application
        /// </summary>
        /// <param name="mailingAddress"></param>
        void BindDisplayFieldsForApplication(IApplicationMailingAddress mailingAddress);
        /// <summary>
        /// Set up view controls for Update
        /// </summary>
        void SetControlsForUpdate();
        /// <summary>
        /// Populate Mailing Address Drop Down
        /// </summary>
        /// <param name="accMailingAddress"></param>
        void PopulateMailingAddressDropDown(IDictionary<string,string> accMailingAddress);
        /// <summary>
        /// Bind Updateable Fields
        /// </summary>
        /// <param name="mailingAddress"></param>
        void BindUpdateableFields(IMailingAddress mailingAddress);
        /// <summary>
        /// Bind Updateable Fields for Application
        /// </summary>
        /// <param name="mailingAddress"></param>
        void BindUpdateableFieldsForApplication(IApplicationMailingAddress mailingAddress);
        /// <summary>
        /// Bind Mailing Address when selected index on drop down has changed
        /// </summary>
        /// <param name="leAddress"></param>
        void BindMailingAddressUpdate(string leAddress);
        /// <summary>
        /// Get Captured Mailing Address for Add and Update
        /// </summary>
        /// <param name="iMailingAddress"></param>
        /// <returns></returns>
        IMailingAddress GetCapturedMailingAddress(IMailingAddress iMailingAddress);
        /// <summary>
        /// Get captured application mailing address
        /// </summary>
        /// <param name="appMailingAddress"></param>
        /// <returns></returns>
        IApplicationMailingAddress GetCapturedApplicationMailingAddress(IApplicationMailingAddress appMailingAddress);
        /// <summary>
        /// Get the Address Key of the Selected Address
        /// </summary>
        int GetSelectedAddressKey { get;}
        /// <summary>
        /// Bind the Drop Downs 
        /// </summary>
        void BindLookUpsForUpdate();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LegalEntityList"></param>
        void BindEmailAddressDropDown(IList<ILegalEntity> LegalEntityList);
 
        #endregion

        #region Properties
        
        bool ShowUpdateButton { set; }

        string CorrespondenceLanguageKey { get;}

        string CorrespondenceMediumKey { get;}

        string OnlineStatementFormatKey { get;}

        string CorrespondenceMailAddressKey { get;}

        bool OnlineStatementRequired { get;}

        bool CorrespondenceMediumRowVisible { set; }

        bool CorrespondenceMailAddressRowVisible { set;}

        bool ReBind { set;}

        #endregion
    }
}
