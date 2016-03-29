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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ReplacePolicy"></param>
    public delegate void YesNoEventHandler(bool ReplacePolicy);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="AssurerKey"></param>
    /// <param name="policyNo"></param>
    public delegate void NextButtonEventHandler(int AssurerKey,string policyNo);
    /// <summary>
    /// 
    /// </summary>
    public interface IRPAR : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnNTUButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnConsideringButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnNextButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnRBNYesNoSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnDDLAssurerSelectedIndexChanged;

        /// <summary>
        /// Sets whether the Replace Policy controls is visible.
        /// </summary>
        bool ReplacePolicyControlsVisible { get; set; }
        
        /// <summary>
        /// Sets whether the Other Insurer textbox is visible
        /// </summary>
        bool OtherInsurerVisible { set; get;}

        /// <summary>
        /// Get/Set whether all the Checkboxes have been checked
        /// </summary>
        bool AllOptionsChecked { get; set;}

        /// <summary>
        /// 
        /// </summary>
        string OtherInsurerName { get;set;}

        /// <summary>
        /// 
        /// </summary>
        int RPARInsurerKey { get;set;}

        /// <summary>
        /// 
        /// </summary>
        string RPARInsurer { get;set;}

        /// <summary>
        /// 
        /// </summary>
        string RPARPolicyNumber { get;set;}

        /// <summary>
        /// Binds the Replace Policy Conditions to the appropriate controls
        /// </summary>
        /// <param name="conditions">A collection of ITextStatements</param>
        /// <param name="rparDone">Boolean to specify if the Declarations screen has been completed</param>
        void BindReplacePolicyConditions(IReadOnlyEventList<ITextStatement> conditions, bool rparDone);

        /// <summary>
        /// Binds the insurers to the drop-down.
        /// </summary>
        /// <param name="insurers"></param>
        void BindInsurers(IDictionary<string, string> insurers);
    }
}
